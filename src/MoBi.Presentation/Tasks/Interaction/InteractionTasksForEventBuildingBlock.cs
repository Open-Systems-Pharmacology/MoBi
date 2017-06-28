using MoBi.Presentation.Services;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForEventBuildingBlock : InteractionTaskForCloneMergeBuildingBlock<IEventGroupBuildingBlock, IEventGroupBuilder>
   {
      public InteractionTasksForEventBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<IEventGroupBuildingBlock> editTask,
         IInteractionTasksForBuilder<IEventGroupBuilder> builderTask,
         IEventBuildingBlockMergeManager ignoreReplaceCloneMergeManager)
         : base(interactionTaskContext, editTask, builderTask, ignoreReplaceCloneMergeManager)
      {
      }
   }
}