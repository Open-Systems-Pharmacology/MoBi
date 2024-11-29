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
      private readonly IMoBiContext _context;

      public CloneSimulationUICommand(IInteractionTasksForSimulation interactionTasksForSimulation, ISimulationUpdateTask simulationUpdateTask, IMoBiContext context)
      {
         _interactionTasksForSimulation = interactionTasksForSimulation;
         _simulationUpdateTask = simulationUpdateTask;
         _context = context;
      }

      protected override void PerformExecute()
      {
         var clonedSimulation = _interactionTasksForSimulation.CloneSimulation(Subject);

         if (clonedSimulation == null) 
            return;

         _context.AddToHistory(_simulationUpdateTask.ConfigureSimulationAndAddToProject(clonedSimulation));
      }
   }
}