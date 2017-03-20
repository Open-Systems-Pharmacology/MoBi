using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class ParameterStartValuesBuildingBlocksReporter : BuildingBlocksReporter<IParameterStartValuesBuildingBlock>
   {
      public ParameterStartValuesBuildingBlocksReporter(IDisplayUnitRetriever displayUnitRetriever)
         : base(new ParameterStartValuesBuildingBlockReporter(displayUnitRetriever), Constants.PARAMETER_START_VALUES_BUILDING_BLOCKS)
      {
      }

   }
}