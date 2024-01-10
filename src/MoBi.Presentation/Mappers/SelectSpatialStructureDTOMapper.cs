using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface ISelectSpatialStructureDTOMapper : IMapper<SpatialStructure, SelectSpatialStructureDTO>
   {
   }

   public class SelectSpatialStructureDTOMapper : ISelectSpatialStructureDTOMapper
   {
      public SelectSpatialStructureDTO MapFrom(SpatialStructure spatialStructure)
      {
         return new SelectSpatialStructureDTO
         {
            SpatialStructure = spatialStructure,
         };
      }
   }
}