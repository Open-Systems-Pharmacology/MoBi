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
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Services;

public interface ICoreSimulationRunner
{
   Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings = false);
   void StopSimulation(IMoBiSimulation simulation);
   void StopAllSimulations();
   bool IsSimulationRunning(IMoBiSimulation simulation);
}

public class CoreSimulationRunner : ICoreSimulationRunner
{
   private readonly IMoBiContext _context;
   private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
   private readonly IDisplayUnitUpdater _displayUnitUpdater;
   private readonly ISimModelManagerFactory _simModelManagerFactory;
   private readonly IKeyPathMapper _keyPathMapper;
   protected readonly ConcurrentDictionary<IMoBiSimulation, CancellationTokenSource> _cancellationTokenSources = new();
   private readonly IEntityValidationTask _entityValidationTask;
   private readonly IQuantitySelectionsRetriever _quantitySelectionsRetriever;
   private ISimModelManager _simModelManager;

   public CoreSimulationRunner(
      IMoBiContext context,
      ISimulationPersistableUpdater simulationPersistableUpdater,
      IDisplayUnitUpdater displayUnitUpdater,
      ISimModelManagerFactory simModelManagerFactory,
      IKeyPathMapper keyPathMapper,
      IEntityValidationTask entityValidationTask,
      IQuantitySelectionsRetriever quantitySelectionsRetriever)
   {
      _context = context;
      _quantitySelectionsRetriever = quantitySelectionsRetriever;
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

   public virtual Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings = false)
   {
      return RunSimulationAsync(simulation, defineSettings, createOutputs: null, showWarnings: null);
   }

   protected async Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings, Func<IMoBiSimulation, bool> createOutputs, Action<SimulationRunResults> showWarnings)
   {
      if (validate(simulation, defineSettings, createOutputs))
         return;

      await startSimulationRunAsync(simulation, showWarnings);
   }

   private bool validate(IMoBiSimulation simulation, bool defineSettings, Func<IMoBiSimulation, bool> createOutputs)
   {
      return !validateSimulation(simulation) || !validateSettings(simulation, defineSettings, createOutputs);
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

   private void addPersistableParametersToOutputSelection(IMoBiSimulation simulation)
   {
      _quantitySelectionsRetriever.UpdatePersistableOutputsIn(simulation);
   }

   private async Task startSimulationRunAsync(IMoBiSimulation simulation, Action<SimulationRunResults> resultsAction = null)
   {
      var cts = new CancellationTokenSource();
      if (!_cancellationTokenSources.TryAdd(simulation, cts)) //this will prevent from running one that is already running
         return;

      _context.PublishEvent(new SimulationRunStartedEvent(simulation));
      _context.PublishEvent(new ProgressInitEvent(100, AppConstants.SimulationRun));
      _simModelManager = _simModelManagerFactory.Create();

      try
      {
         addEvents();
         updatePersistableFor(simulation);

         var simulationRunResults = await _simModelManager.RunSimulationAsync(simulation, cts.Token);

         simulation.HasChanged = true;
         resultsAction?.Invoke(simulationRunResults);

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
      }
   }

   private bool validateSimulation(IMoBiSimulation simulation)
   {
      addPersistableParametersToOutputSelection(simulation);
      if (!_entityValidationTask.Validate(simulation))
         return false;

      return true;
   }

   private bool validateSettings(IMoBiSimulation simulation, bool defineSettings, Func<IMoBiSimulation, bool> createOutputs)
   {
      if (settingsRequired(simulation, defineSettings))
      {
         if (createOutputs == null || !createOutputs(simulation))
            return false;
      }

      return true;
   }

   private bool settingsRequired(IMoBiSimulation simulation, bool defineSettings)
   {
      if (defineSettings)
         return true;

      if (simulation.Settings == null)
         return true;

      return !simulation.OutputSelections.HasSelection;
   }

   protected void UpdateOutputSelectionInSimulation(IMoBiSimulation simulation, OutputSelections outputSelections)
   {
      if (!outputSelections.DiffersFrom(simulation.OutputSelections))
         return;

      //they are different. Issue an update command
      addCommand(new UpdateOutputSelectionsInSimulationCommand(outputSelections, simulation).RunCommand(_context));
   }
}