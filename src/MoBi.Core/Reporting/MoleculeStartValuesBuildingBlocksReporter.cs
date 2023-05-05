using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;

namespace MoBi.Core.Reporting
{
   internal class MoleculeStartValuesBuildingBlocksReporter : BuildingBlocksReporter<InitialConditionsBuildingBlock>
   {
      public MoleculeStartValuesBuildingBlocksReporter(IDisplayUnitRetriever displayUnitRetriever)
         : base(new MoleculeStartValuesBuildingBlockReporter(displayUnitRetriever), Constants.MOLECULE_START_VALUES_BUILDING_BLOCKS)
      {
      }

   }
}