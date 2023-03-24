using MoBi.Core.Repositories;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers

{
   public interface INeighborhoodToDTONeighborhoodMapper : IMapper<Neighborhood, NeighborhoodDTO>
   {
   }

   internal class NeighborhoodToDTONeighborhoodMapper : ContainerToContainerDTOMapper, INeighborhoodToDTONeighborhoodMapper
   {
      private readonly ITransportToTransportDTOMapper _transportToTransportDTOMapper;

      public NeighborhoodToDTONeighborhoodMapper(
         ITransportToTransportDTOMapper transportToTransportDTOMapper,
         IObjectPathFactory objectPathFactory,
         IIconRepository iconRepository) : base(objectPathFactory, iconRepository)
      {
         _transportToTransportDTOMapper = transportToTransportDTOMapper;
      }

      public NeighborhoodDTO MapFrom(Neighborhood neighborhood)
      {
         var dto = MapContainer(neighborhood, new NeighborhoodDTO(neighborhood));
         var transports = neighborhood.GetAllChildren<ITransport>();
         dto.Transports = transports.MapAllUsing(_transportToTransportDTOMapper);
         return dto;
      }
   }
}