using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class EventGroupBuildingBlockReporter : BuildingBlockReporter<IEventGroupBuildingBlock, IEventGroupBuilder>
   {
      public EventGroupBuildingBlockReporter() : base(Constants.EVENT_BUILDING_BLOCK, Constants.EVENT_BUILDING_BLOCKS)
      {
      }
   }
}