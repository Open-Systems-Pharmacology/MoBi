using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CloneSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IInteractionTasksForSimulation _interactionTasksForSimulation;
      private readonly ISimulationUpdateTask _simulationUpdateTask;

      public CloneSimulationUICommand(IInteractionTasksForSimulation interactionTasksForSimulation, ISimulationUpdateTask simulationUpdateTask)
      {
         _interactionTasksForSimulation = interactionTasksForSimulation;
         _simulationUpdateTask = simulationUpdateTask;
      }

      protected override void PerformExecute()
      {
         var clonedSimulation = _interactionTasksForSimulation.CloneSimulation(Subject);

         if (clonedSimulation == null) 
            return;
         
         _simulationUpdateTask.ConfigureSimulation(clonedSimulation);
      }
   }
}