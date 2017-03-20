using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace MoBi.Core.Services
{
   public class CreateCommitChangesToSpatialStructureCommandTask : CreateCommitChangesToBuildingBlockCommandTask<ISpatialStructure>
   {
      public CreateCommitChangesToSpatialStructureCommandTask(ICloneManagerForBuildingBlock cloneManager) : base(cloneManager, x => x.SpatialStructure)
      {
      }
   }
}