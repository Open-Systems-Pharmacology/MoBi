using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToReactionBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<IReactionBuildingBlock>
   {
      public CreateCommitChangesToReactionBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.Reactions)
      {
      }
   }
}