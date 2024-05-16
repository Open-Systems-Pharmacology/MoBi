using System.Collections.Generic;
using MoBi.Presentation.DTO;

namespace MoBi.Presentation.Views
{
   public interface ISelectionView<T>
   {
      void InitializeWith(IEnumerable<ListItemDTO<T>> allItems);
   }
}