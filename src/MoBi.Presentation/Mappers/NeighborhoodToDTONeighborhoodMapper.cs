using OSPSuite.Utility.Extensions;
using MoBi.Core.Repositories;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers

{
   public interface INeighborhoodToDTONeighborhoodMapper : IContainerToContainerDTOMapper
   {
   }
   class NeighborhoodToDTONeighborhoodMapper :ContainerToContainerDTOMapper, INeighborhoodToDTONeighborhoodMapper
   {
      private readonly ITransportToTransportDTOMapper _transportToTransportDTOMapper;

      public NeighborhoodToDTONeighborhoodMapper(ITagToTagDTOMapper tagDTOMapper, ITransportToTransportDTOMapper transportToTransportDTOMapper, IIconRepository iconRepository) 
         : base(tagDTOMapper,iconRepository)
      {
         _transportToTransportDTOMapper = transportToTransportDTOMapper;
      }

      public override ContainerDTO MapFrom(IContainer input)
      {
         var dto = MapContainer(input, new NeighborhoodDTO());
         var neighborhood = input as INeighborhood;
         var tranports = neighborhood.GetAllChildren<ITransport>();
         dto.Transports = tranports.MapAllUsing(_transportToTransportDTOMapper);
         return dto;
      }
   }



   

   
}