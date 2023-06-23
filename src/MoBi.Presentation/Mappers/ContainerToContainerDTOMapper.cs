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
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IIconRepository _iconRepository;

      public ContainerToContainerDTOMapper(IObjectPathFactory objectPathFactory, IIconRepository iconRepository)
      {
         _objectPathFactory = objectPathFactory;
         _iconRepository = iconRepository;
      }

      public virtual ContainerDTO MapFrom(IContainer container)
      {
         var dto = MapContainer(container, new ContainerDTO(container));
         UpdateParentPath(container, dto);
         return dto;
      }

      protected void UpdateParentPath(IContainer container, ContainerDTO containerDTO)
      {
         var parentContainer = container.ParentContainer;
         if (parentContainer != null)
         {
            containerDTO.ParentPath = _objectPathFactory.CreateAbsoluteObjectPath(parentContainer);
            containerDTO.ParentPathEditable = false;
         }
         else
         {
            containerDTO.ParentPath = container.ParentPath ?? string.Empty;
            containerDTO.ParentPathEditable = true;
         }
      }

      protected T MapContainer<T>(IContainer container, T dto) where T : ContainerDTO
      {
         MapProperties(container, dto);
         dto.ContainerType = container.ContainerType;
         dto.Mode = container.Mode;
         if (dto.Icon == null)
            dto.Icon = _iconRepository.IconFor(container);

         return dto;
      }
   }
}