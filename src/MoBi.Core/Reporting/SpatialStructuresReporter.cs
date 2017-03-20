using MoBi.Core.Domain.Model;

namespace MoBi.Core.Reporting
{
   internal class SpatialStructuresReporter : BuildingBlocksReporter<IMoBiSpatialStructure>
   {
      public SpatialStructuresReporter(SpatialStructureReporter spatialStructureReporter)
         : base(spatialStructureReporter, Constants.SPATIAL_STRUCTURES)
      {
      }
   }
}