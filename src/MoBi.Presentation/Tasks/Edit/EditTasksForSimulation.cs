using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks.Edit
{
   public interface IEditTasksForSimulation : IEditTaskFor<IMoBiSimulation>
   {
      void CreateReport(IModelCoreSimulation simulation);
      void ExportResultsToExcel(IMoBiSimulation simulation);
      void ExportResultsToExcel(IMoBiSimulation simulation, DataRepository dataRepository);
      void RenameResults(IMoBiSimulation simulation, DataRepository dataRepository);
      void ExportMatlabDifferentialSystem(IMoBiSimulation simulation);
      void ExportSimModelXml(IMoBiSimulation simulation);
      void CalculateScaleFactors(IMoBiSimulation simulation);
      void AddCommand(IMoBiCommand command);
   }

   public class EditTasksForSimulation : EditTasksForBuildingBlock<IMoBiSimulation>, IEditTasksForSimulation
   {
      private readonly ISimulationPersistor _simulationPersistor;
      private readonly IDialogCreator _dialogCreator;
      private readonly IForbiddenNamesRetriever _forbiddenNamesRetriver;
      private readonly IModelReportCreator _reportCreator;
      private readonly IDataRepositoryTask _dataRepositoryTask;
      private readonly ISimModelExporter _simModelExporter;
      private readonly IDimensionFactory _dimensionFactory;

      public EditTasksForSimulation(IInteractionTaskContext interactionTaskContext, ISimulationPersistor simulationPersistor, IDialogCreator dialogCreator,
         IForbiddenNamesRetriever forbiddenNamesRetriver, IDataRepositoryTask dataRepositoryTask,
         IModelReportCreator reportCreator, ISimModelExporter simModelExporter, IDimensionFactory dimensionFactory) : base(interactionTaskContext)
      {
         _simulationPersistor = simulationPersistor;
         _dialogCreator = dialogCreator;
         _forbiddenNamesRetriver = forbiddenNamesRetriver;
         _dataRepositoryTask = dataRepositoryTask;
         _reportCreator = reportCreator;
         _simModelExporter = simModelExporter;
         _dimensionFactory = dimensionFactory;
      }

      public void CreateReport(IModelCoreSimulation simulation)
      {
         var exportFile = _interactionTask.AskForFileToSave(AppConstants.Dialog.ExportSimulationModelToFileTitle, Constants.Filter.TEXT_FILE_FILTER, Constants.DirectoryKey.REPORT, simulation.Name);
         if (exportFile.IsNullOrEmpty()) return;
         using (var writer = new StreamWriter(exportFile))
         {
            writer.Write(_reportCreator.ModelReport(simulation.Model, true, true, true));
         }

         //now try to open the file
         FileHelper.TryOpenFile(exportFile);
      }

      public override void Edit(IMoBiSimulation simulation)
      {
         //simulation are not registered in repository anymore. We need to do it
         _interactionTaskContext.Context.Register(simulation);
         base.Edit(simulation);
      }

      public void ExportResultsToExcel(IMoBiSimulation simulation)
      {
         ExportResultsToExcel(simulation, simulation.Results);
      }

      public void ExportResultsToExcel(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         if (dataRepository == null) return;

         exportAllResults(simulation, dataRepository);
      }

      private void exportAllResults(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         var fileName = _interactionTask.AskForFileToSave(AppConstants.Dialog.ExportSimulationResultsToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, simulation.Name);
         if (string.IsNullOrEmpty(fileName)) return;

         exportDataRepository(fileName, dataRepository);
      }

      private void exportDataRepository(string fileName, IEnumerable<DataColumn> dataRepository)
      {
         var dataTables = _dataRepositoryTask.ToDataTable(dataRepository, new DataColumnExportOptions
         {
            ColumnNameRetriever = x => x.QuantityInfo.PathAsString,
            DimensionRetriever = _dimensionFactory.MergedDimensionFor
         });

         _dataRepositoryTask.ExportToExcel(dataTables, fileName, true);
      }

      public void RenameResults(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         string newName = _dialogCreator.AskForInput(AppConstants.Dialog.AskForNewName(dataRepository.Name), AppConstants.Captions.NewName,
            dataRepository.Name, allUsedResultsNameIn(simulation));

         if (string.IsNullOrEmpty(newName))
            return;

         AddCommand(new RenameSimulationResultsCommand(dataRepository, simulation, newName).Run(_context));
      }

      public void ExportMatlabDifferentialSystem(IMoBiSimulation simulation)
      {
         var exportFolder = _interactionTask.AskForFolder(AppConstants.Dialog.ExportSimulationMatlabODE, Constants.DirectoryKey.SIM_MODEL_XML);
         if (string.IsNullOrEmpty(exportFolder)) return;
         _simModelExporter.ExportODEForMatlab(simulation, exportFolder, MatlabFormulaExportMode.Formula);
      }

      public void CalculateScaleFactors(IMoBiSimulation simulation)
      {
         using (var presenter = _applicationController.Start<ICalculateScaleDivisorsPresenter>())
         {
            var command = presenter.CalculateScaleDivisorFor(simulation);
            if (command.IsEmpty())
               return;

            AddCommand(command);
         }
      }

      private IEnumerable<string> allUsedResultsNameIn(IMoBiSimulation simulation)
      {
         return simulation.HistoricResults.Select(x => x.Name).Union(new[] {simulation.Results.Name});
      }

      public void AddCommand(IMoBiCommand command)
      {
         _context.AddToHistory(command);
      }

      protected override IEnumerable<string> GetUnallowedNames(IMoBiSimulation simulation, IEnumerable<IObjectBase> existingObjectsInParent)
      {
         return _forbiddenNamesRetriver.For(simulation);
      }

      public override void Save(IMoBiSimulation simulation)
      {
         var fileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.Save, Constants.Filter.PKML_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, simulation.Name);
         if (fileName.IsNullOrEmpty()) return;
         _simulationPersistor.Save(new SimulationTransfer {Simulation = simulation}, fileName);
      }

      public void ExportSimModelXml(IMoBiSimulation simulation)
      {
         var fileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.Save, AppConstants.Filter.SIM_MODEL_FILE_FILTER, Constants.DirectoryKey.SIM_MODEL_XML, simulation.Name);
         if (fileName.IsNullOrEmpty()) return;
         _simModelExporter.Export(simulation, fileName);
      }
   }
}