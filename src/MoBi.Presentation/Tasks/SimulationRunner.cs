using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation.Tasks
{
   public class SimulationRunner : ISimulationRunner
   {
      private readonly IMoBiContext _context;
      private readonly IMoBiApplicationController _applicationController;
      private ISimModelManager _simModelManager;
      private readonly IOutputSelectionsRetriever _outputSelectionsRetriever;
      private IMoBiSimulation _simulation;
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IDisplayUnitUpdater _displayUnitUpdater;
      private readonly ISimModelManagerFactory _simModelManagerFactory;
      private readonly IKeyPathMapper _keyPathMapper;
      private readonly IEntityValidationTask _entityValidationTask;

      public SimulationRunner(IMoBiContext context, 
         IMoBiApplicationController applicationController,
         IOutputSelectionsRetriever outputSelectionsRetriever, 
         ISimulationPersistableUpdater simulationPersistableUpdater,
         IDisplayUnitUpdater displayUnitUpdater, 
         ISimModelManagerFactory simModelManagerFactory, 
         IKeyPathMapper keyPathMapper, 
         IEntityValidationTask entityValidationTask)
      {
         _context = context;
         _applicationController = applicationController;
         _outputSelectionsRetriever = outputSelectionsRetriever;
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _displayUnitUpdater = displayUnitUpdater;
         _simModelManagerFactory = simModelManagerFactory;
         _keyPathMapper = keyPathMapper;
         _entityValidationTask = entityValidationTask;
      }

      public void RunSimulation(IMoBiSimulation simulation, bool defineSettings = false)
      {
         addPersitableParametersToOutputSelection(simulation);
         if (!_entityValidationTask.Validate(simulation))
            return;

         if (settingsRequired(simulation, defineSettings))
         {
            var outputSelections = _outputSelectionsRetriever.OutputSelectionsFor(simulation);
            if (outputSelections == null)
               return;

            updateOutputSelectionInSimulation(simulation, outputSelections);
         }

         startSimulationRun(simulation);
      }

      private void addPersitableParametersToOutputSelection(IMoBiSimulation simulation)
      {
         _outputSelectionsRetriever.UpdatePersistableOutputsIn(simulation);
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
         _simModelManager?.StopSimulation();
      }

      private void startSimulationRun(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _context.PublishEvent(new SimulationRunStartedEvent());
         _context.PublishEvent(new ProgressInitEvent(100, AppConstants.SimulationRun));
         _simModelManager = _simModelManagerFactory.Create();

         try
         {
            addEvents();
            updatePersistableFor(simulation);
            var simulationRunResults = _simModelManager.RunSimulation(_simulation);
            _simulation.HasChanged = true;
            showWarningsIfAny(simulationRunResults);

            if (simulationRunResults.Success)
            {
               var results = simulationRunResults.Results;
               results.Name = getNewRepositoryName();
               _displayUnitUpdater.UpdateDisplayUnitsIn(results);
               copyResultsToSimulation(simulationRunResults, _simulation);
            }

            addCommand(getSimulationResultLabel(simulationRunResults));
         }
         finally
         {
            removeEvents();
            _context.PublishEvent(new SimulationRunFinishedEvent(_simulation));
            _simulation = null;
         }
      }

      private string getNewRepositoryName()
      {
         return AppConstants.ResultName + DateTime.Now.ToIsoFormat(withSeconds: true);
      }

      private void addCommand(ICommand command)
      {
         _context.AddToHistory(command);
      }

      private void updatePersistableFor(IMoBiSimulation simulation)
      {
         _simulationPersistableUpdater.UpdatePersistableFromSettings(simulation);
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
         var resultsResults = results.Results;
         if (simulation.ResultsDataRepository != null)
            simulation.HistoricResults.Add(simulation.ResultsDataRepository);

         setMolecularWeight(simulation, resultsResults);
         simulation.ResultsDataRepository = resultsResults;
      }

      private void setMolecularWeight(IMoBiSimulation simulation, IEnumerable<DataColumn> results)
      {
         results.Where(isConcentrationColumn).Each(c => setMolecularWeight(simulation, c));
      }

      private void setMolecularWeight(IMoBiSimulation simulation, DataColumn column)
      {
         var molecule = getMoleculeFor(simulation, column);

         var mwPara = molecule?.Parameter(AppConstants.Parameters.MOLECULAR_WEIGHT);
         if (mwPara == null) return;

         column.DataInfo.MolWeight = mwPara.Value;
      }

      private IMoleculeBuilder getMoleculeFor(IMoBiSimulation simulation, DataColumn dataColumn)
      {
         var moleculeName = _keyPathMapper.MoleculeNameFrom(dataColumn);
         if (string.IsNullOrEmpty(moleculeName))
            return null;

         return simulation.Configuration.Molecules.Select(x => x[moleculeName]).FirstOrDefault(x => x != null);
      }

      private bool isConcentrationColumn(DataColumn column)
      {
         return column.Dimension.Name.IsOneOf(Constants.Dimension.MASS_CONCENTRATION, Constants.Dimension.MOLAR_CONCENTRATION);
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