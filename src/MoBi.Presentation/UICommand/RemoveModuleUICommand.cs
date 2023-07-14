using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModule _interactionTasks;
      private readonly IMoBiContext _context;

      public RemoveModuleUICommand(IInteractionTasksForModule interactionTasksForModule, IMoBiContext context)
      {
         _interactionTasks = interactionTasksForModule;
         _context = context;
      }

      protected override void PerformExecute()
      {
         _context.AddToHistory(_interactionTasks.Remove(Subject, null, null));
      }
   }
}