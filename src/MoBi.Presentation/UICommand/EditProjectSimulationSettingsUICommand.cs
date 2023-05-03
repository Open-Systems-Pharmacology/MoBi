using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   public class EditProjectSimulationSettingsUICommand : IUICommand
   {
      private readonly IInteractionTasksForSimulationSettings _interactionTask;
      private readonly IMoBiContext _context;

      public EditProjectSimulationSettingsUICommand(IInteractionTasksForSimulationSettings interactionTask, IMoBiContext context)
      {
         _interactionTask = interactionTask;
         _context = context;
      }

      public void Execute()
      {
         _interactionTask.Edit(_context.CurrentProject.SimulationSettings);
      }
   }
}