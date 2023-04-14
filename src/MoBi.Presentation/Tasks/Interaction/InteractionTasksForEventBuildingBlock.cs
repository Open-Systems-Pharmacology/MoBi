using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForEventBuildingBlock : InteractionTaskForCloneMergeBuildingBlock<EventGroupBuildingBlock, EventGroupBuilder>
   {
      public InteractionTasksForEventBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<EventGroupBuildingBlock> editTask,
         IInteractionTasksForBuilder<EventGroupBuilder> builderTask,
         IEventBuildingBlockMergeManager ignoreReplaceCloneMergeManager)
         : base(interactionTaskContext, editTask, builderTask, ignoreReplaceCloneMergeManager)
      {
      }
   }
}