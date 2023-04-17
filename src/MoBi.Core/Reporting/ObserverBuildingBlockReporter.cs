using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class ObserverBuildingBlockReporter : BuildingBlockReporter<ObserverBuildingBlock, ObserverBuilder>
   {
      public ObserverBuildingBlockReporter() : base(Constants.OBSERVER_BUILDING_BLOCK, Constants.OBSERVER_BUILDING_BLOCKS)
      {
      }
   }
}