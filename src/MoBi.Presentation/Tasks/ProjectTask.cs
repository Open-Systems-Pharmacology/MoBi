using System.IO;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Exceptions;
using MoBi.Core.Services;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Extensions;

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
      private readonly ISimulationLoader _simulationLoader;
      private readonly ISbmlTask _sbmlTask;

      public ProjectTask(
         IMoBiContext context,
         ISerializationTask serializationTask,
         IDialogCreator dialogCreator,
         IMRUProvider mruProvider,
         IHeavyWorkManager heavyWorkManager,
         ISimulationLoader simulationLoader,
         ISbmlTask sbmlTask
      )
      {
         _context = context;
         _simulationLoader = simulationLoader;
         _sbmlTask = sbmlTask;
         _heavyWorkManager = heavyWorkManager;
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
      }

      private SimulationTransfer loadSimulationTransferFromFileUsingDimensionMode(string fileName, ReactionDimensionMode dimensionMode)
      {
         _context.CurrentProject.ReactionDimensionMode = dimensionMode;
         return _serializationTask.Load<SimulationTransfer>(fileName);
      }

      private void loadJournalIfNotLoadedAlready(MoBiProject project, string journalPath)
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
         var readyForOpen = CloseProject();
         if (!readyForOpen)
            return;

         var fileName = _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadSBMLProject, AppConstants.Filter.SBML_MODEL_FILE_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (fileName.IsNullOrEmpty())
            return;

         var project = _serializationTask.NewProject();
         _context.AddToHistory(_sbmlTask.ImportModelFromSbml(fileName, project));
         notifyProjectLoaded();
      }

      public bool Open()
      {
         var readyForOpen = CloseProject();
         if (!readyForOpen)
            return false;

         var fileName = _dialogCreator.AskForFileToOpen(AppConstants.Dialog.LoadProject, AppConstants.Filter.MOBI_PROJECT_FILE_FILTER, Constants.DirectoryKey.PROJECT);

         if (fileName.IsNullOrEmpty())
            return false;

         return loadProjectIntoContext(fileName);
      }

      public bool Save()
      {
         var filePath = _context.CurrentProject.FilePath;
         if (string.IsNullOrEmpty(filePath))
            return SaveAs();

         var fileInfo = new FileInfo(filePath);
         if (fileInfo.Exists && fileInfo.IsReadOnly)
            return SaveAs();

         return saveProject();
      }

      public bool SaveAs()
      {
         var defaultNameIsUndefined = string.Equals(Constants.PROJECT_UNDEFINED, _context.CurrentProject.Name);
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
         if (!CloseProject())
            return;

         var project = _serializationTask.NewProject();
         project.ReactionDimensionMode = reactionDimensionMode;

         _context.PublishEvent(new ProjectCreatedEvent(project));
         _context.PublishEvent(new ProjectLoadedEvent(project));
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

      // Check if TemplateBuildingBlock ID is set, and if not set it to th e used Building Block
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
   }
}