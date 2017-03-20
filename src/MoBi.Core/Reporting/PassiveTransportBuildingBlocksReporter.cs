﻿using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Reporting
{
   internal class PassiveTransportBuildingBlocksReporter : BuildingBlocksReporter<IPassiveTransportBuildingBlock>
   {
      public PassiveTransportBuildingBlocksReporter() : base(new PassiveTransportBuildingBlockReporter(), Constants.PASSIVE_TRANSPORT_BUILDING_BLOCKS)
      {
      }
   }
}