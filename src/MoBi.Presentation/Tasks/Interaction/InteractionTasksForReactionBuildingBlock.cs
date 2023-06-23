using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForReactionBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<Module, MoBiReactionBuildingBlock, ReactionBuilder>
   {
      private readonly IReactionBuildingBlockFactory _reactionBuildingBlockFactory;

      public InteractionTasksForReactionBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<MoBiReactionBuildingBlock> editTask,
         IInteractionTasksForBuilder<ReactionBuilder> builderTask,
         IReactionBuildingBlockFactory reactionBuildingBlockFactory)
         : base(interactionTaskContext, editTask, builderTask)
      {
         _reactionBuildingBlockFactory = reactionBuildingBlockFactory;
      }

      public override MoBiReactionBuildingBlock CreateNewEntity(Module parent)
      {
         return _reactionBuildingBlockFactory.Create();
      }

      public override IMoBiCommand GetRemoveCommand(MoBiReactionBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<MoBiReactionBuildingBlock>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(MoBiReactionBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<MoBiReactionBuildingBlock>(itemToAdd, parent);
      }
   }
}