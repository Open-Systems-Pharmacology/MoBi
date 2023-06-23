using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class PassiveTransportBuildingBlockReporter : BuildingBlockReporter<PassiveTransportBuildingBlock, TransportBuilder>
   {
      public PassiveTransportBuildingBlockReporter() : base(Constants.PASSIVE_TRANSPORT_BUILDING_BLOCK, Constants.PASSIVE_TRANSPORT_BUILDING_BLOCKS)
      {
      }
   }
}