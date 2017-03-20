using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToEventGoupBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<IEventGroupBuildingBlock>
   {
      public CreateCommitChangesToEventGoupBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.EventGroups)
      {
      }
   }
}