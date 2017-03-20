using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToSimulationSettingsBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<ISimulationSettings>
   {
      public CreateCommitChangesToSimulationSettingsBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.SimulationSettings)
      {
      }
   }
}