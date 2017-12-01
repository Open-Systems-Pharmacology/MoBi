using System;
using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Core.SBML;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Presentation.Services;

namespace MoBi.Presentation.Tasks
{
   public interface IProjectTask : OSPSuite.Presentation.Services.IProjectTask
   {
      bool Open();
      bool Save();
      bool SaveAs();
      void New(ReactionDimensionMode reactionDimensionMode);
      bool CloseProject();
      bool OpenSimulationAsProject();
      void StartWithSimulation(string fileName);
      void StartWithJournal(string journalFilePath);
      void LoadSimulationIntoProject();
      void OpenSBMLModel();
      SimulationTransfer LoadSimulationTransferDataFromFile(string fileName);
   }

   internal class ProjectTask : IProjectTask
   {
      private readonly IMoBiContext _context;
      private readonly IDialogCreator _dialogCreator;
      private readonly IMRUProvider _mruProvider;
      private readonly ISerializationTask _serializationTask;
      private readonly IHeavyWorkManager _heavyWorkManager;
      private readonly IMoBiSpatialStructureFactory _spatialStructureFactory;
      private readonly ISimulationSettingsFactory _simulationSettingsFactory;
      private readonly ISimulationLoader _simulationLoader;
      private readonly ISBMLTask _sbmlTask;
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

      public ProjectTask(IMoBiContext context, ISerializationTask serializationTask, IDialogCreator dialogCreator,
         IMRUProvider mruProvider, IMoBiSpatialStructureFactory spatialStructureFactory,
         IHeavyWorkManager heavyWorkManager, ISimulationSettingsFactory simulationSettingsFactory,
         ISimulationLoader simulationLoader, ISBMLTask sbmlTask, IReactionBuildingBlockFactory reactionBuildingBlockFactory)
      {
         _context = context;
         _simulationSettingsFactory = simulationSettingsFactory;
         _simulationLoader = simulationLoader;
         _sbmlTask = sbmlTask;
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
         _heavyWorkManager = heavyWorkManager;
         _spatialStructureFactory = spatialStructureFactory;
         _mruProvider = mruProvider;
         _serializationTask = serializationTask;
         _dialogCreator = dialogCreator;
      }

      public void OpenProjectFrom(string path)
      {
         bool readyForOpen = CloseProject();
         if (!readyForOpen)
            return;

         loadProjectIntoContext(path);
      }

      public bool OpenSimulationAsProject()
      {
         bool readyForOpen = CloseProject();
         if (!readyForOpen)
            return false;

         string fileName = _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(fileName))
            return false;

         StartWithSimulation(fileName);
         return true;
      }

      public void StartWithSimulation(string fileName)
      {
         _serializationTask.NewProject();
         loadSimulationFromFileInProject(fileName);
      }

      public void StartWithJournal(string journalFilePath)
      {
         New(ReactionDimensionMode.AmountBased);
         _serializationTask.LoadJournal(journalFilePath, showJournal: true);
      }

      private void loadSimulationFromFileInProject(string fileName)
      {
         var project = _context.CurrentProject;
         if (project == null) return;

         if (string.IsNullOrEmpty(fileName))
            return;

         SimulationTransfer simulationTransfer = null;

         _heavyWorkManager.Start(() => simulationTransfer = LoadSimulationTransferDataFromFile(fileName));
         if (simulationTransfer == null)
            return;

         _context.AddToHistory(addSimulationTransferToProject(simulationTransfer));
         loadJournalIfNotLoadedAlready(project, simulationTransfer.JournalPath);
         notifyProjectLoaded();
      }

      private void notifyProjectLoaded()
      {
         _context.PublishEvent(new ProjectCreatedEvent(_context.CurrentProject));
         _context.PublishEvent(new ProjectLoadedEvent(_context.CurrentProject));
      }

      public SimulationTransfer LoadSimulationTransferDataFromFile(string fileName)
      {
         try
         {
            return loadSimulationTransferFromFileUsingDimensionMode(fileName, ReactionDimensionMode.AmountBased);
         }
         catch (CannotConvertConcentrationToAmountException)
         {
            return loadSimulationTransferFromFileUsingDimensionMode(fileName, ReactionDimensionMode.ConcentrationBased);
         }
         catch (NotMatchingSerializationFileException)
         {
            return new SimulationTransfer
            {
               Simulation = _serializationTask.Load<MoBiSimulation>(fileName),
               AllObservedData = new List<DataRepository>(),
               PkmlVersion = ProjectVersions.V3_0_4
            };
         }
      }

      private SimulationTransfer loadSimulationTransferFromFileUsingDimensionMode(string fileName, ReactionDimensionMode dimensionMode)
      {
         _context.CurrentProject.ReactionDimensionMode = dimensionMode;
         return _serializationTask.Load<SimulationTransfer>(fileName);
      }

      private void loadJournalIfNotLoadedAlready(IMoBiProject project, string journalPath)
      {
         if (!string.IsNullOrEmpty(project.JournalPath))
            return;

         _serializationTask.LoadJournal(journalPath);
      }

      public void LoadSimulationIntoProject()
      {
         var fileName = _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadSimulation, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         loadSimulationFromFileInProject(fileName);
      }

      public void OpenSBMLModel()
      {
         bool readyForOpen = CloseProject();
         if (!readyForOpen) return;

         string fileName = _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadSBMLProject, AppConstants.Filter.SBML_MODEL_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (fileName.IsNullOrEmpty()) return;

         _context.NewProject();
         _context.AddToHistory(_sbmlTask.ImportModelFromSBML(fileName, _context.CurrentProject));
         notifyProjectLoaded();
      }

      public bool Open()
      {
         bool readyForOpen = CloseProject();
         if (!readyForOpen)
            return false;

         string fileName = _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject, AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT);

         if (fileName.IsNullOrEmpty())
            return false;

         return loadProjectIntoContext(fileName);
      }

      public bool Save()
      {
         if (string.IsNullOrEmpty(_context.CurrentProject.FilePath))
            return SaveAs();

         return saveProject();
      }

      public bool SaveAs()
      {
         bool defaultNameIsUndefined = string.Equals(Constants.ProjectUndefined, _context.CurrentProject.Name);
         var defaultFileName = defaultNameIsUndefined ? string.Empty : _context.CurrentProject.Name;

         var newFilePath = _dialogCreator.AskForFileToSave(AppConstants.Dialog.AskForSaveProject,
            AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT, defaultFileName);

         if (string.IsNullOrEmpty(newFilePath))
            return false;

         _context.CurrentProject.FilePath = newFilePath;
         return saveProject();
      }

      private bool saveProject()
      {
         _serializationTask.SaveProject();
         _mruProvider.Add(_context.CurrentProject.FilePath);
         _context.PublishEvent(new ProjectSavedEvent(_context.CurrentProject));
         return true;
      }

      public void New(ReactionDimensionMode reactionDimensionMode)
      {
         if (!CloseProject()) return;
         _serializationTask.NewProject();
         _context.CurrentProject.ReactionDimensionMode = reactionDimensionMode;
         generateDefaultsInCurrentProject();
         _context.PublishEvent(new ProjectCreatedEvent(_context.CurrentProject));
         _context.PublishEvent(new ProjectLoadedEvent(_context.CurrentProject));
      }

      public bool CloseProject()
      {
         var shouldClose = true;
         if (_context.CurrentProject == null)
            return true;

         if (_context.CurrentProject.HasChanged)
            shouldClose = askForSaveProject();

         if (!shouldClose)
            return false;

         _context.PublishEvent(new ProjectClosingEvent());
         _serializationTask.CloseProject();
         _context.PublishEvent(new ProjectClosedEvent());

         return true;
      }

      private ICommand addSimulationTransferToProject(SimulationTransfer simulationTransfer)
      {
         return _simulationLoader.AddSimulationToProject(simulationTransfer);
      }

      // Check if TemplateBuildinhgBlock ID is set, and if not set it to th e used Building Block
      // Needed when importing from PK-Sim 

      private bool askForSaveProject()
      {
         switch (_dialogCreator.MessageBoxYesNoCancel(AppConstants.Dialog.DoYouWantToSaveTheCurrentProject))
         {
            case ViewResult.Yes:
               return Save();
            case ViewResult.No:
               return true;
            default:
               return false;
         }
      }

      private bool loadProjectIntoContext(string fileName)
      {
         _serializationTask.LoadProject(fileName);

         if (_context.CurrentProject == null)
            return false;

         _mruProvider.Add(fileName);

         notifyProjectLoaded();
         return true;
      }

      private void generateDefaultsInCurrentProject()
      {
         addDefault<IMoleculeBuildingBlock>(AppConstants.DefaultNames.MoleculeBuildingBlock);
         addDefault(AppConstants.DefaultNames.ReactionBuildingBlock, () => _reactionBuildingBlockFactory.Create());
         addDefault(AppConstants.DefaultNames.SpatialStructure, () => _spatialStructureFactory.CreateDefault(AppConstants.DefaultNames.SpatialStructure));
         addDefault<IPassiveTransportBuildingBlock>(AppConstants.DefaultNames.PassiveTransportBuildingBlock);
         addDefault<IEventGroupBuildingBlock>(AppConstants.DefaultNames.EventBuildingBlock);
         addDefault<IObserverBuildingBlock>(AppConstants.DefaultNames.ObserverBuildingBlock);
         addDefault(AppConstants.DefaultNames.SimulationSettings, _simulationSettingsFactory.CreateDefault);
      }

      private void addDefault<T>(string defaultName) where T : IBuildingBlock
      {
         addDefault(defaultName, _context.Create<T>);
      }

      private void addDefault<T>(string defaultName, Func<T> buildingBlockCreator) where T : IBuildingBlock
      {
         var project = _context.CurrentProject;
         var buildingBlock = buildingBlockCreator().WithName(defaultName);
         project.AddBuildingBlock(buildingBlock);
         _context.Register(buildingBlock);
      }
   }
}