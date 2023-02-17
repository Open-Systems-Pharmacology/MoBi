using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseToObjectBaseDTOMapper : IMapper<IObjectBase, ObjectBaseDTO>
   {
   }

   public abstract class ObjectBaseToObjectBaseDTOMapperBase
   {
      protected T Map<T>(IObjectBase objectBase) where T : ObjectBaseDTO, new()
      {
         var dto = new T();
         MapProperties(objectBase, dto);
         return dto;
      }

      protected void MapProperties(IObjectBase objectBase, ObjectBaseDTO objectBaseDTO)
      {
         objectBaseDTO.Name = objectBase.Name;
         objectBaseDTO.Description = objectBase.Description;
         objectBaseDTO.Id = objectBase.Id;
         objectBaseDTO.Icon = objectBase.Icon;
         objectBase.PropertyChanged += objectBaseDTO.HandlePropertyChanged;
      }
   }

   public class ObjectBaseToObjectBaseDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IObjectBaseToObjectBaseDTOMapper
   {
      public ObjectBaseDTO MapFrom(IObjectBase objectBase)
      {
         return Map<ObjectBaseDTO>(objectBase);
      }
   }
}