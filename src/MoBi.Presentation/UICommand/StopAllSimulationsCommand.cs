using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.UICommand
{
    public class StopAllSimulationsCommand : IUICommand
   {
      private readonly ISimulationRunner _simulationRunnerTask;

      public StopAllSimulationsCommand(ISimulationRunner simulationRunnerTask)
      {
         _simulationRunnerTask = simulationRunnerTask;
      }

       public void Execute()
      {
         _simulationRunnerTask.StopSimulation();
      }
   }
}