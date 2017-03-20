using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToMoleculesBuildingBlockCommandTask : CreateCommitChangesToBuildingBlockCommandTask<IMoleculeBuildingBlock>
   {
      public CreateCommitChangesToMoleculesBuildingBlockCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.Molecules)
      {
      }
   }
}