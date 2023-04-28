using OSPSuite.Utility.Extensions;
using MoBi.Core.Commands;
using MoBi.Core.Helper;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<TBuildingBlock, TBuilder> 
      : InteractionTasksForEnumerableBuildingBlock<TBuildingBlock, TBuilder> 
      where TBuilder : class, IContainer, IBuilder where TBuildingBlock : class, IBuildingBlock<TBuilder>
   {
      protected InteractionTasksForEnumerableBuildingBlockOfContainerBuilder(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask, IInteractionTasksForBuilder<TBuilder> builderTask) : base(interactionTaskContext, editTask, builderTask)
      {
      }

      protected InteractionTasksForEnumerableBuildingBlockOfContainerBuilder(IInteractionTaskContext interactionTaskContext, IEditTasksForBuildingBlock<TBuildingBlock> editTask) : base(interactionTaskContext, editTask)
      {
      }
   }
}