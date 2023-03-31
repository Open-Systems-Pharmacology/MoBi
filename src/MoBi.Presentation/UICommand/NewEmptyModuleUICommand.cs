using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class NewEmptyModuleUICommand : ObjectUICommand<MoBiProject>
   {
      private readonly IInteractionTasksForModule _interactionTasksForModule;
      private readonly IMoBiContext _context;

      public NewEmptyModuleUICommand(IInteractionTasksForModule interactionTasksForModule, IMoBiContext context)
      {
         _interactionTasksForModule = interactionTasksForModule;
         _context = context;
      }
      
      protected override void PerformExecute()
      {
         _context.AddToHistory(_interactionTasksForModule.AddNew(_context.CurrentProject, null));
      }
   }
}