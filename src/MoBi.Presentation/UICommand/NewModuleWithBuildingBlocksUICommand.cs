using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class NewModuleWithBuildingBlocksUICommand : ObjectUICommand<MoBiProject>
   {
      private readonly IInteractionTasksForModule _interactionTasks;

      public NewModuleWithBuildingBlocksUICommand(IInteractionTasksForModule interactionTasksForModule, IMoBiContext context)
      {
         _interactionTasks = interactionTasksForModule;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.CreateNewModuleWithBuildingBlocks();
      }
   }
}