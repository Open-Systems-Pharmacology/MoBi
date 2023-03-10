using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface ISpatialStructureToSpatialStructureDTOMapper : IMapper<ISpatialStructure, SpatialStructureDTO>
   {
   }

   internal class SpatialStructureToSpatialStructureDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, ISpatialStructureToSpatialStructureDTOMapper
   {
      private readonly IContainerToContainerDTOMapper _containerToDTOContainerMapper;

      public SpatialStructureToSpatialStructureDTOMapper(IContainerToContainerDTOMapper containerToDTOContainerMapper)
      {
         _containerToDTOContainerMapper = containerToDTOContainerMapper;
      }

      public SpatialStructureDTO MapFrom(ISpatialStructure spatialStructure)
      {
         var dto = Map<SpatialStructureDTO>(spatialStructure);
         dto.TopContainer = spatialStructure.TopContainers.MapAllUsing(_containerToDTOContainerMapper);
         dto.Neighborhoods = _containerToDTOContainerMapper.MapFrom(spatialStructure.NeighborhoodsContainer);
         dto.MoleculeProperties = _containerToDTOContainerMapper.MapFrom(spatialStructure.GlobalMoleculeDependentProperties);
         return dto;
      }
   }
}