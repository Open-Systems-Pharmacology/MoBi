using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class StartValuesDTO : ObjectBaseDTO
   {
      public MoleculeBuildingBlock Molecules { get; set; }
      public IMoBiSpatialStructure SpatialStructrue { get; set; }
   }
}