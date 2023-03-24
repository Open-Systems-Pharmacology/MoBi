using MoBi.Core.Repositories;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface INeighborhoodBuilderToNeighborhoodBuilderDTOMapper : IMapper<NeighborhoodBuilder, NeighborhoodBuilderDTO>
   {
   }

   public class NeighborhoodBuilderToNeighborhoodBuilderDTOMapper : ContainerToContainerDTOMapper, INeighborhoodBuilderToNeighborhoodBuilderDTOMapper
   {
      public NeighborhoodBuilderToNeighborhoodBuilderDTOMapper(IObjectPathFactory objectPathFactory, IIconRepository iconRepository) : base(objectPathFactory, iconRepository)
      {
      }

      public NeighborhoodBuilderDTO MapFrom(NeighborhoodBuilder neighborhoodBuilder)
      {
         var dto = MapContainer(neighborhoodBuilder, new NeighborhoodBuilderDTO(neighborhoodBuilder));
         dto.FirstNeighborPath = neighborhoodBuilder.FirstNeighborPath?.ToPathString();
         dto.SecondNeighborPath = neighborhoodBuilder.SecondNeighborPath?.ToPathString();
         return dto;
      }
   }
}