using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoBi.Assets;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Extensions;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
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
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IDisplayUnitUpdater _displayUnitUpdater;
      private readonly ISimModelManagerFactory _simModelManagerFactory;
      private readonly IKeyPathMapper _keyPathMapper;
      private readonly IEntityValidationTask _entityValidationTask;
      private readonly Dictionary<IMoBiSimulation, string> _originalSimulationNames = new Dictionary<IMoBiSimulation, string>();
      private readonly ConcurrentDictionary<IMoBiSimulation, CancellationTokenSource> _cancellationTokenSources = new ConcurrentDictionary<IMoBiSimulation, CancellationTokenSource>();


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

      public bool IsSimulationRunning(IMoBiSimulation simulation)
      {
         return _cancellationTokenSources.TryGetValue(simulation, out var cts) &&
                !cts.IsCancellationRequested;
      }

      public Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings = false)
      {
         return runSimulationAsync(simulation, defineSettings);
      }

      private async Task runSimulationAsync(IMoBiSimulation simulation, bool defineSettings)
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

         await startSimulationRunAsync(simulation);
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
         addCommand(new UpdateOutputSelectionsInSimulationCommand(outputSelections, simulation).RunCommand(_context));
      }

      private bool settingsRequired(IMoBiSimulation simulation, bool defineSettings)
      {
         if (defineSettings)
            return true;

         if (simulation.Settings == null)
            return true;

         return !simulation.OutputSelections.HasSelection;
      }

      public void StopSimulation(IMoBiSimulation simulation)
      {
         if (_cancellationTokenSources.TryRemove(simulation, out var cts))
         {
            cts.Cancel();
            cts.Dispose();
            _context.PublishEvent(new SimulationRunCanceledEvent());
         }
      }

      public void StopAllSimulations()
      {
         foreach (var simulation in _cancellationTokenSources.Keys.ToList())
         {
            if (_cancellationTokenSources.TryRemove(simulation, out var cts))
            {
               cts.Cancel();
               cts.Dispose();
            }
         }

         _context.PublishEvent(new SimulationRunCanceledEvent());
      }

      private async Task startSimulationRunAsync(IMoBiSimulation simulation)
      {
         var cts = new CancellationTokenSource();
         if (!_cancellationTokenSources.TryAdd(simulation, cts)) //this will prevent from running one that is already running
            return;
         _context.PublishEvent(new SimulationRunStartedEvent(simulation));
         setSimulationRunning(simulation, true);
         _context.PublishEvent(new ProgressInitEvent(100, AppConstants.SimulationRun));
         _simModelManager = _simModelManagerFactory.Create();

         try
         {
            addEvents();
            updatePersistableFor(simulation);

            var simulationRunResults = await _simModelManager.RunSimulationAsync(simulation, cts.Token);

            simulation.HasChanged = true;
            showWarningsIfAny(simulationRunResults);

            if (simulationRunResults.Success)
            {
               var results = simulationRunResults.Results;
               results.Name = getNewRepositoryName();
               _displayUnitUpdater.UpdateDisplayUnitsIn(results);
               copyResultsToSimulation(simulationRunResults, simulation);
            }

            addCommand(getSimulationResultLabel(simulationRunResults));
         }
         finally
         {
            if (_cancellationTokenSources.ContainsKey(simulation))
            {
               _cancellationTokenSources[simulation].Dispose();
               if (_cancellationTokenSources.TryRemove(simulation, out var ctsToDispose))
               {
                  ctsToDispose.Dispose();
               }
            }
            removeEvents();
            _context.PublishEvent(new SimulationRunFinishedEvent(simulation));
            setSimulationRunning(simulation, false);
         }
      }

      private void setSimulationRunning(IMoBiSimulation simulation, bool isRunning)
      {
         if (IsSimulationRunning(simulation))
         {
            if (!_originalSimulationNames.ContainsKey(simulation))
               _originalSimulationNames[simulation] = simulation.Name;
         }
         else if (_originalSimulationNames.TryGetValue(simulation, out var originalName))
         {
            simulation.Name = originalName;
            _originalSimulationNames.Remove(simulation);
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
         var moleculeName = _keyPathMapper.MoleculeNameFrom(column);
         if (string.IsNullOrEmpty(moleculeName))
            return;

         column.DataInfo.MolWeight = simulation.MolWeightFor(moleculeName);
      }

      private bool isConcentrationColumn(DataColumn column)
      {
         return column.Dimension.Name.IsOneOf(Constants.Dimension.MASS_CONCENTRATION, Constants.Dimension.MOLAR_CONCENTRATION);
      }

      private IInfoCommand getSimulationResultLabel(SimulationRunResults results)
      {
         var command = new OSPSuiteInfoCommand { Description = simulationLabelDescription(results) };
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
         _context.PublishEvent(new ProgressDoneWithMessageEvent(AppConstants.SimulationRun));
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