using MoBi.Core.Domain.Model;

namespace MoBi.Core.Reporting
{
   internal class ReactionBuildingBlocksReporter : BuildingBlocksReporter<IMoBiReactionBuildingBlock>
   {
      public ReactionBuildingBlocksReporter(ReactionBuildingBlockReporter reactionBuildingBlockReporter)
         : base(reactionBuildingBlockReporter, Constants.REACTION_BUILDING_BLOCKS)
      {
      }
   }
}