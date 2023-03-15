using MoBi.Assets;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseToObjectBaseDTOMapper : IMapper<IObjectBase, ObjectBaseDTO>
   {
   }

   public abstract class ObjectBaseToObjectBaseDTOMapperBase
   {
      protected T Map<T>(T dto) where T : ObjectBaseDTO
      {
         MapProperties(dto.ObjectBase, dto);
         return dto;
      }

      protected void MapProperties(IObjectBase objectBase, ObjectBaseDTO objectBaseDTO)
      {
         objectBaseDTO.Name = objectBase.Name;
         objectBaseDTO.Description = descriptionFor(objectBase);
         objectBaseDTO.Icon = objectBase.Icon;
      }

      private static string descriptionFor(IObjectBase objectBase)
      {
         //special case for neighborhood where we can create a dynamic description if not there
         if (!string.IsNullOrEmpty(objectBase.Description))
            return objectBase.Description;


         if (!(objectBase is NeighborhoodBuilder neighborhood))
            return string.Empty;

         return ToolTips.Neighborhood.Between(neighborhood);
      }
   }

   public class ObjectBaseToObjectBaseDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IObjectBaseToObjectBaseDTOMapper
   {
      public ObjectBaseDTO MapFrom(IObjectBase objectBase)
      {
         return Map(new ObjectBaseDTO(objectBase));
      }
   }
}