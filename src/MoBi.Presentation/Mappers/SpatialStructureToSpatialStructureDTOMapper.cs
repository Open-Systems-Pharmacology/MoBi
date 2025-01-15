using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface ISpatialStructureToSpatialStructureDTOMapper : IMapper<SpatialStructure, SpatialStructureDTO>
   {
   }

   internal class SpatialStructureToSpatialStructureDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ISpatialStructureToSpatialStructureDTOMapper
   {
      private readonly IContainerToContainerDTOMapper _containerToDTOContainerMapper;

      public SpatialStructureToSpatialStructureDTOMapper(IContainerToContainerDTOMapper containerToDTOContainerMapper)
      {
         _containerToDTOContainerMapper = containerToDTOContainerMapper;
      }

      public SpatialStructureDTO MapFrom(SpatialStructure spatialStructure)
      {
         var dto = Map(new SpatialStructureDTO(spatialStructure));
         dto.TopContainers = spatialStructure.TopContainers.MapAllUsing(_containerToDTOContainerMapper);
         
         if(spatialStructure.NeighborhoodsContainer != null && spatialStructure.NeighborhoodsContainer.Any())
            dto.Neighborhoods = _containerToDTOContainerMapper.MapFrom(spatialStructure.NeighborhoodsContainer);

         if(spatialStructure.GlobalMoleculeDependentProperties != null && spatialStructure.GlobalMoleculeDependentProperties.Any())
            dto.MoleculeProperties = _containerToDTOContainerMapper.MapFrom(spatialStructure.GlobalMoleculeDependentProperties);
         
         dto.Name = spatialStructure.DisplayName;

         return dto;
      }
   }
}