using MoBi.Core.Repositories;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers

{
   public interface INeighborhoodToDTONeighborhoodMapper : IContainerToContainerDTOMapper
   {
   }

   class NeighborhoodToDTONeighborhoodMapper : ContainerToContainerDTOMapper, INeighborhoodToDTONeighborhoodMapper
   {
      private readonly ITransportToTransportDTOMapper _transportToTransportDTOMapper;

      public NeighborhoodToDTONeighborhoodMapper(ITransportToTransportDTOMapper transportToTransportDTOMapper, IIconRepository iconRepository)
         : base(iconRepository)
      {
         _transportToTransportDTOMapper = transportToTransportDTOMapper;
      }

      public override ContainerDTO MapFrom(IContainer input)
      {
         var dto = MapContainer(input, new NeighborhoodDTO());
         var neighborhood = input as INeighborhood;
         var transports = neighborhood.GetAllChildren<ITransport>();
         dto.Transports = transports.MapAllUsing(_transportToTransportDTOMapper);
         return dto;
      }
   }
}