using System;
using OSPSuite.Utility;
using MoBi.Presentation.DTO;

namespace MoBi.Presentation.Mappers
{
   public interface IItemToListItemMapper<T> : IMapper<T, ListItemDTO<T>>
   {
      void Initialize(Func<T, string> getName);
   }
   public class ItemToListItemMapper<T> : IItemToListItemMapper<T>
   {
      private Func<T, string> _getName;

      public void Initialize(Func<T, string> getName)
      {
         _getName = getName;
      }

      public ListItemDTO<T> MapFrom(T input)
      {
         return new ListItemDTO<T> {Item = input, DisplayName = _getName(input)};
      }
   }
}