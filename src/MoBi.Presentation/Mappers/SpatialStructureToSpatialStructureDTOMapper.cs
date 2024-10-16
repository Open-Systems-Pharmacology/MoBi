﻿using MoBi.Presentation.DTO;
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
         dto.Neighborhoods = _containerToDTOContainerMapper.MapFrom(spatialStructure.NeighborhoodsContainer);
         dto.MoleculeProperties = _containerToDTOContainerMapper.MapFrom(spatialStructure.GlobalMoleculeDependentProperties);
         dto.Name = spatialStructure.DisplayName;

         return dto;
      }
   }
}