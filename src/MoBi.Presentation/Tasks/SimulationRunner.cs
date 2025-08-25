using System.Linq;
using System.Threading.Tasks;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation.Tasks
{
   public class SimulationRunner : CoreSimulationRunner, ISimulationRunner
   {
      private readonly IMoBiApplicationController _applicationController;
      private readonly IOutputSelectionsRetriever _outputSelectionsRetriever;

      public SimulationRunner(IMoBiContext context,
         IMoBiApplicationController applicationController,
         IOutputSelectionsRetriever outputSelectionsRetriever,
         ISimulationPersistableUpdater simulationPersistableUpdater,
         IDisplayUnitUpdater displayUnitUpdater,
         ISimModelManagerFactory simModelManagerFactory,
         IKeyPathMapper keyPathMapper,
         IEntityValidationTask entityValidationTask) : base(context, simulationPersistableUpdater, displayUnitUpdater, simModelManagerFactory, keyPathMapper, entityValidationTask, outputSelectionsRetriever)
      {
         _outputSelectionsRetriever = outputSelectionsRetriever;
         _applicationController = applicationController;
         _outputSelectionsRetriever = outputSelectionsRetriever;
      }

      public override Task RunSimulationAsync(IMoBiSimulation simulation, bool defineSettings = false)
      {
         return RunSimulationAsync(simulation, defineSettings, createOutputs: createNewSettings, showWarnings: showWarningsIfAny);
      }

      private bool createNewSettings(IMoBiSimulation simulation)
      {
         var outputSelections = _outputSelectionsRetriever.OutputSelectionsFor(simulation);
         if (outputSelections == null)
            return false;

         UpdateOutputSelectionInSimulation(simulation, outputSelections);
         return true;
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

      public bool IsSimulationIdle(IMoBiSimulation simulation)
      {
         return !_cancellationTokenSources.TryGetValue(simulation, out var cts)
                || cts.IsCancellationRequested;
      }

      public bool IsAnySimulationRunning()
      {
         return _cancellationTokenSources.Values.Any(cts => !cts.IsCancellationRequested);
      }
   }
}