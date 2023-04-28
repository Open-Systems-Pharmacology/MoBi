using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForEventBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<EventGroupBuildingBlock, EventGroupBuilder>
   {
      public InteractionTasksForEventBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<EventGroupBuildingBlock> editTask,
         IInteractionTasksForBuilder<EventGroupBuilder> builderTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
      }
   }
}