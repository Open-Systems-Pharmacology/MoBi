using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToPassiveTransportsBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<IPassiveTransportBuildingBlock>
   {
      public CreateCommitChangesToPassiveTransportsBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.PassiveTransports)
      {
      }
   }
}