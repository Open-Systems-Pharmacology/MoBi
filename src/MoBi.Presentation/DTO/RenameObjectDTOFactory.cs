using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;

namespace MoBi.Presentation.DTO
{
   public class RenameObjectDTOFactory : IRenameObjectDTOFactory
   {
      private readonly IObjectTypeResolver _objectTypeResolver;

      public RenameObjectDTOFactory(IObjectTypeResolver objectTypeResolver)
      {
         _objectTypeResolver = objectTypeResolver;
      }

      public RenameObjectDTO CreateFor(IWithName objectBase)
      {
         var dto = new RenameObjectDTO(objectBase.Name);

         var entity = objectBase as IEntity;

         if (entity == null || entity.ParentContainer == null)
            return dto;

         dto.ContainerType = _objectTypeResolver.TypeFor(entity.ParentContainer);
         dto.AddUsedNames(entity.ParentContainer.AllChildrenNames());

         return dto;
      }
   }
}