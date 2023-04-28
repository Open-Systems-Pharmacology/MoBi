﻿using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForReactionBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<MoBiReactionBuildingBlock, ReactionBuilder>
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
      
      public override MoBiReactionBuildingBlock CreateNewEntity(MoBiProject parent)
      {
         return _reactionBuildingBlockFactory.Create();
      }
   }
}