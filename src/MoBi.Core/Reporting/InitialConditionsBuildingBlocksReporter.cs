using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class InitialConditionsBuildingBlocksReporter : BuildingBlocksReporter<InitialConditionsBuildingBlock>
   {
      public InitialConditionsBuildingBlocksReporter(IDisplayUnitRetriever displayUnitRetriever)
         : base(new InitialConditionsBuildingBlockReporter(displayUnitRetriever), Constants.INITIAL_CONDITIONS_BUILDING_BLOCKS)
      {
      }

   }
}