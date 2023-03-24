using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToSimulationSettingsBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<SimulationSettings>
   {
      public CreateCommitChangesToSimulationSettingsBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.SimulationSettings)
      {
      }
   }
}