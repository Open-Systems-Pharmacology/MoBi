using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToObserverBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<IObserverBuildingBlock>
   {
      public CreateCommitChangesToObserverBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.Observers)
      {
      }
   }
}