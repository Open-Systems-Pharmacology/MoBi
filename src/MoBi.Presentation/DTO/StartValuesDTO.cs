using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class StartValuesDTO : ObjectBaseDTO
   {

      public IMoleculeBuildingBlock Molecules { get; set; }
      public IMoBiSpatialStructure SpatialStructure { get; set; }
   }
}