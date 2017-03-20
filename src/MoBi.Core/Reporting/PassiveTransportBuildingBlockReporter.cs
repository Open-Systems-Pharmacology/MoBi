using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class PassiveTransportBuildingBlockReporter : BuildingBlockReporter<IPassiveTransportBuildingBlock, ITransportBuilder>
   {
      public PassiveTransportBuildingBlockReporter() : base(Constants.PASSIVE_TRANSPORT_BUILDING_BLOCK, Constants.PASSIVE_TRANSPORT_BUILDING_BLOCKS)
      {
      }
   }
}