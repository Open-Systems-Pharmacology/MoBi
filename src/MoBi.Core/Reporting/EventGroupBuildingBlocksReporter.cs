using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class EventGroupBuildingBlocksReporter : BuildingBlocksReporter<EventGroupBuildingBlock>
   {
      public EventGroupBuildingBlocksReporter() : base(new EventGroupBuildingBlockReporter(), Constants.EVENT_BUILDING_BLOCKS)
      {
      }
   }
}