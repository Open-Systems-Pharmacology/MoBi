using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForEnumerableBuildingBlock<TParent, TBuildingBlock, TBuilder> : InteractionTasksForBuildingBlock<TParent, TBuildingBlock>
      where TBuildingBlock : class, IBuildingBlock, IBuildingBlock<TBuilder>
      where TBuilder : class, IBuilder where TParent : class, IObjectBase
   {
      protected readonly IInteractionTasksForBuilder<TBuilder> _builderTask;

      protected InteractionTasksForEnumerableBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<TBuildingBlock> editTask,
         IInteractionTasksForBuilder<TBuilder> builderTask)
         : base(interactionTaskContext, editTask)
      {
         _builderTask = builderTask;
      }

      protected InteractionTasksForEnumerableBuildingBlock(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask) : this(interactionTaskContext, editTask, null)
      {
      }
   }
}