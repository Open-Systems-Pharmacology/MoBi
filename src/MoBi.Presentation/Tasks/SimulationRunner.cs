using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation.Tasks
{
   public class SimulationRunner : ISimulationRunner
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private ISimModelManager _simModelManager;
      private readonly IOutputSelectionsRetriever _outputSelectionsRetriever;
      private readonly ISimModelExporter _simModelExporter;
      private readonly IDataNamingService _dataNamingService;
      private IMoBiSimulation _simulation;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IDisplayUnitUpdater _displayUnitUpdater;
      private readonly IDisplayUnitRetriever _displayUnitRetriever;
      private readonly ISimModelSimulationFactory _simModelSimulationFactory;

      public SimulationRunner(IMoBiContext context, IMoBiApplicationController applicationController,
         IOutputSelectionsRetriever outputSelectionsRetriever, ISimModelExporter simModelExporter,
         IDataNamingService dataNamingService, ISimulationPersistableUpdater simulationPersistableUpdater,
         IDisplayUnitUpdater displayUnitUpdater, IDisplayUnitRetriever displayUnitRetriever, ISimModelSimulationFactory simModelSimulationFactory)
      {
         _context = context;
         _applicationController = applicationController;
         _outputSelectionsRetriever = outputSelectionsRetriever;
         _simModelExporter = simModelExporter;
         _dataNamingService = dataNamingService;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _displayUnitUpdater = displayUnitUpdater;
         _displayUnitRetriever = displayUnitRetriever;
         _simModelSimulationFactory = simModelSimulationFactory;
      }

      public void RunSimulation(IMoBiSimulation simulation, bool defineSettings = false)
      {
         addPersitableParametersToOutputSelection(simulation);
         if (settingsRequired(simulation, defineSettings))
         {
            var outputSelections = _outputSelectionsRetriever.OutputSelectionsFor(simulation);
            if (outputSelections == null) return;
            updateOutputSelectionInSimulation(simulation, outputSelections);
         }

         startSimulationRun(simulation);
      }

      private void addPersitableParametersToOutputSelection(IMoBiSimulation simulation)
      {
         if (simulation.Settings == null)
            return;

         var allPersistableParameters = simulation.Model.Root.GetAllChildren<IParameter>(x => x.Persistable);
         allPersistableParameters.Each(p => { simulation.OutputSelections.AddOutput(_outputSelectionsRetriever.SelectionFrom(p)); });
      }

      private void updateOutputSelectionInSimulation(IMoBiSimulation simulation, OutputSelections outputSelections)
      {
         if (!outputSelections.DiffersFrom(simulation.OutputSelections))
            return;

         //they are different. Issue an update command
         addCommand(new UpdateOutputSelectionsInSimulationCommand(outputSelections, simulation).Run(_context));
      }

      private bool settingsRequired(IMoBiSimulation simulation, bool defineSettings)
      {
         if (defineSettings)
            return true;

         if (simulation.Settings == null)
            return true;

         return !simulation.OutputSelections.HasSelection;
      }

      public void StopSimulation()
      {
         if (_simModelManager == null) return;
         _simModelManager.StopSimulation();
      }

      private void startSimulationRun(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _context.PublishEvent(new SimulationRunStartedEvent());
         _context.PublishEvent(new ProgressInitEvent(100, AppConstants.SimulationRun));
         _simModelManager = new SimModelManager(_simModelExporter, _simModelSimulationFactory,  createDataFactory());

         try
         {
            addEvents();
            updatePersistableFor(simulation);
            var results = _simModelManager.RunSimulation(_simulation);
            _simulation.HasChanged = true;
            showWarningsIfAny(results);

            if (results.Success)
            {
               _displayUnitUpdater.UpdateDisplayUnitsIn(results.Results);
               copyResultsToSimulation(results, _simulation);
            }

            addCommand(getSimulationResultLabel(results));
         }
         finally
         {
            removeEvents();
            _context.PublishEvent(new SimulationRunFinishedEvent(_simulation));
            _simulation = null;
         }
      }

      private void addCommand(ICommand command)
      {
         _context.HistoryManager.AddToHistory(command);
      }

      private void updatePersistableFor(IMoBiSimulation simulation)
      {
         _simulationPersistableUpdater.UpdatePersistableFromSettings(simulation);
      }

      private DataFactory createDataFactory()
      {
         return new DataFactory(_context.DimensionFactory, _dataNamingService, _context.ObjectPathFactory, _displayUnitRetriever);
      }

      private void showWarningsIfAny(SimulationRunResults results)
      {
         if (!results.Warnings.Any())
            return;

         using (var presenter = _applicationController.Start<ISolverMessagePresenter>())
         {
            presenter.Show(results.Warnings);
         }
      }

      private void copyResultsToSimulation(SimulationRunResults results, IMoBiSimulation simulation)
      {
         var data = results.Results;
         //Save HistoricData
         if (simulation.Results != null)
            simulation.HistoricResults.Add(simulation.Results);

         setMolecularWeight(simulation, data);
         simulation.Results = data;
      }

      private void setMolecularWeight(IMoBiSimulation simulation, IEnumerable<DataColumn> data)
      {
         data.Where(isConcentrationColumn).Each(c => setMolecularWeight(simulation, c));
      }

      private void setMolecularWeight(IMoBiSimulation simulation, DataColumn column)
      {
         var molecule = getMoleculeFromQuantityInfo(simulation, column.QuantityInfo);
         if (molecule == null) return;

         var mwPara = molecule.Parameters.FindByName(AppConstants.Parameters.MOLECULAR_WEIGHT);
         if (mwPara == null) return;

         column.DataInfo.MolWeight = mwPara.Value;
      }

      private IMoleculeBuilder getMoleculeFromQuantityInfo(IMoBiSimulation simulation, QuantityInfo quantityInfo)
      {
         var moleculeBuilder = simulation.BuildConfiguration.Molecules[quantityInfo.Name];
         if (moleculeBuilder == null)
         {
            var pathAsList = quantityInfo.Path.ToList();
            var moleculeNameIndex = pathAsList.Count - AppConstants.ObseverPathLengthDistance;
            moleculeBuilder = simulation.BuildConfiguration.Molecules[pathAsList[moleculeNameIndex]];
         }
         return moleculeBuilder;
      }

      private bool isConcentrationColumn(DataColumn column)
      {
         return column.Dimension.Equals(_context.DimensionFactory.Dimension(AppConstants.DimensionNames.MASS_CONCENTRATION)) ||
                column.Dimension.Equals(_context.DimensionFactory.Dimension(Constants.Dimension.MOLAR_CONCENTRATION));
      }

      private IInfoCommand getSimulationResultLabel(SimulationRunResults results)
      {
         var command = new OSPSuiteInfoCommand {Description = simulationLabelDescription(results)};
         if (results.Warnings.Any())
            command.Comment = AppConstants.Commands.SimulationLabelComment(results.Warnings.Count());

         return command;
      }

      private string simulationLabelDescription(SimulationRunResults results)
      {
         string terminationString;
         if (results.Success)
            terminationString = "successful";
         else
            terminationString = results.Warnings.Any() ? "failed" : "aborted";

         return $"simulation run {terminationString}";
      }

      private void onSimulationFinished(object sender, EventArgs eventArgs)
      {
         _context.PublishEvent(new ProgressDoneEvent());
      }

      private void onSimulationProgress(object sender, SimulationProgressEventArgs args)
      {
         _context.PublishEvent(new ProgressingEvent(args.Progress, args.Progress, AppConstants.SimulationRun));
      }

      private void removeEvents()
      {
         if (_simModelManager == null) return;
         _simModelManager.SimulationProgress -= onSimulationProgress;
         _simModelManager.Terminated -= onSimulationFinished;
      }

      private void addEvents()
      {
         if (_simModelManager == null) return;
         _simModelManager.SimulationProgress += onSimulationProgress;
         _simModelManager.Terminated += onSimulationFinished;
      }
   }
}