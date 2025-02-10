using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class EventGroupBuildingBlockReporter : BuildingBlockReporter<EventGroupBuildingBlock, EventGroupBuilder>
   {
      public EventGroupBuildingBlockReporter() : base(Constants.EVENT_BUILDING_BLOCK, Constants.EVENT_BUILDING_BLOCKS)
      {
      }
   }
}