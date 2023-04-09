using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadBuildingBlocksToModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForBuildingBlocks _interactionTasks;

      public LoadBuildingBlocksToModuleUICommand(IInteractionTasksForBuildingBlocks interactionTasksForModule, IMoBiContext context)
      {
         _interactionTasks = interactionTasksForModule;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.LoadBuildingBlocksToModule(Subject);
      }
   }
}