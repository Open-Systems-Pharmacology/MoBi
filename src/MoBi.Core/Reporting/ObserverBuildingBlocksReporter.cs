using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class ObserverBuildingBlocksReporter : BuildingBlocksReporter<ObserverBuildingBlock>
   {
      public ObserverBuildingBlocksReporter() : base(new ObserverBuildingBlockReporter(), Constants.OBSERVER_BUILDING_BLOCKS)
      {
      }
   }
}