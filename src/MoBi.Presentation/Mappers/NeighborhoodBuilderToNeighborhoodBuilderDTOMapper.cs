using System.Collections.Generic;
using MoBi.Core.Repositories;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface INeighborhoodBuilderToNeighborhoodBuilderDTOMapper
   {
      NeighborhoodBuilderDTO MapFrom(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<NeighborhoodBuilder> existingNeighborhoods);
   }

   public class NeighborhoodBuilderToNeighborhoodBuilderDTOMapper : ContainerToContainerDTOMapper, INeighborhoodBuilderToNeighborhoodBuilderDTOMapper
   {
      public NeighborhoodBuilderToNeighborhoodBuilderDTOMapper(IObjectPathFactory objectPathFactory, IIconRepository iconRepository) : base(objectPathFactory, iconRepository)
      {
      }

      public NeighborhoodBuilderDTO MapFrom(NeighborhoodBuilder neighborhoodBuilder, IReadOnlyList<NeighborhoodBuilder> existingNeighborhoods)
      {
         return MapContainer(neighborhoodBuilder, new NeighborhoodBuilderDTO(neighborhoodBuilder, existingNeighborhoods));
      }
   }
}