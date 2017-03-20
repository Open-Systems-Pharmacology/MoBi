using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Mappers
{
   public interface IObjectBaseToObjectBaseDTOMapper : IMapper<IObjectBase, IObjectBaseDTO>
   {
   }

   public abstract class ObjectBaseToObjectBaseDTOMapperBase
   {
      protected T Map<T>(IObjectBase objectBase) where T : IObjectBaseDTO, new()
      {
         var dto = new T();
         MapProperties(objectBase, dto);
         return dto;
      }

      protected void MapProperties(IObjectBase objectBase, IObjectBaseDTO objectBaseDTO)
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
      public IObjectBaseDTO MapFrom(IObjectBase objectBase)
      {
         return Map<ObjectBaseDTO>(objectBase);
      }
   }


}