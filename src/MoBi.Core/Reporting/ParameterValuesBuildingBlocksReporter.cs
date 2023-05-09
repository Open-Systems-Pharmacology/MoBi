using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class ParameterValuesBuildingBlocksReporter : BuildingBlocksReporter<ParameterValuesBuildingBlock>
   {
      public ParameterValuesBuildingBlocksReporter(IDisplayUnitRetriever displayUnitRetriever)
         : base(new ParameterValuesBuildingBlockReporter(displayUnitRetriever), Constants.PARAMETER_VALUES_BUILDING_BLOCKS)
      {
      }
   }
}