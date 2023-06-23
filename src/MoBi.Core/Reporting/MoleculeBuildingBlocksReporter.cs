using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class MoleculeBuildingBlocksReporter : BuildingBlocksReporter<MoleculeBuildingBlock>
   {
      public MoleculeBuildingBlocksReporter(): base(new MoleculeBuildingBlockReporter(), Constants.MOLECULE_BUILDING_BLOCKS)
      {
      }
   }
}