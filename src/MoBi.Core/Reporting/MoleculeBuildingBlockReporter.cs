using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class MoleculeBuildingBlockReporter : BuildingBlockReporter<IMoleculeBuildingBlock, IMoleculeBuilder>
   {
      public MoleculeBuildingBlockReporter() : base(Constants.MOLECULE_BUILDING_BLOCK, Constants.MOLECULE_BUILDING_BLOCKS)
      {
      }
   }
}