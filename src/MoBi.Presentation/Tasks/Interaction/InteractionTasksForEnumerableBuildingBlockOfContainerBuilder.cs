using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<TParent, TBuildingBlock, TBuilder>
      : InteractionTasksForEnumerableBuildingBlock<TParent, TBuildingBlock, TBuilder>
      where TBuilder : class, IContainer, IBuilder where TBuildingBlock : class, IBuildingBlock<TBuilder> where TParent : class, IObjectBase
   {
      protected InteractionTasksForEnumerableBuildingBlockOfContainerBuilder(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask, IInteractionTasksForBuilder<TBuilder> builderTask) : base(interactionTaskContext, editTask, builderTask)
      {
      }

      protected InteractionTasksForEnumerableBuildingBlockOfContainerBuilder(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask) : base(interactionTaskContext, editTask)
      {
      }
   }
}