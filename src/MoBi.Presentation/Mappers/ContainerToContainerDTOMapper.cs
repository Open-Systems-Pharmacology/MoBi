using MoBi.Core.Repositories;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IContainerToContainerDTOMapper : IMapper<IContainer, ContainerDTO>
   {
   }

   public class ContainerToContainerDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IContainerToContainerDTOMapper
   {
      private readonly IIconRepository _iconRepository;

      public ContainerToContainerDTOMapper(IIconRepository iconRepository)
      {
         _iconRepository = iconRepository;
      }

      public virtual ContainerDTO MapFrom(IContainer container)
      {
         return MapContainer(container, new ContainerDTO());
      }

      protected T MapContainer<T>(IContainer container, T dto) where T : ContainerDTO, new()
      {
         MapProperties(container, dto);
         dto.ContainerType = container.ContainerType;
         dto.Mode = container.Mode;
         if (string.IsNullOrEmpty(dto.Icon))
            dto.Icon = _iconRepository.IconFor(container);

         return dto;
      }
   }
}