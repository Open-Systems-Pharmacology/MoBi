using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class ShowDifferencesUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IMoBiContext _context;
      private readonly IEditTasksForSimulation _simulationTasks;

      public ShowDifferencesUICommand(IMoBiContext context, IEditTasksForSimulation simulationTasks)
      {
         _context = context;
         _simulationTasks = simulationTasks;
      }

      protected override void PerformExecute()
      {
         _simulationTasks.Edit(Subject);
         _context.PublishEvent(new ShowSimulationChangesEvent(Subject));
      }
   }
}