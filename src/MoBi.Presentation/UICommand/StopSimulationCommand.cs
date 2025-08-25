using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.Tasks;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class StopSimulationCommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly ISimulationRunner _simulationRunnerTask;

      public StopSimulationCommand(ISimulationRunner simulationRunnerTask)
      {
         _simulationRunnerTask = simulationRunnerTask;
      }

      protected override void PerformExecute()
      {
         _simulationRunnerTask.StopSimulation(Subject);
      }
   }
}