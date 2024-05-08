using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectManyPresenter<T> : IInitializablePresenter<IEnumerable<T>>, IPresenter<ISelectManyView<T>>
   {
      IEnumerable<T> Selections { get; }
      string GetName(T item);
   }

   public abstract class SelectManyPresenter<T> : SelectionPresenter<ISelectManyView<T>, ISelectManyPresenter<T>, T>, ISelectManyPresenter<T>
   {
      protected SelectManyPresenter(ISelectManyView<T> view, IItemToListItemMapper<T> itemToListItemMapper)
         : base(view, itemToListItemMapper)
      {

      }

      protected override IEnumerable<ListItemDTO<T>> MapAllItems()
      {
         return base.MapAllItems().OrderBy(x => x.DisplayName);
      }

      public IEnumerable<T> Selections
      {
         get { return _view.Selections.Select(x => x.Item); }
      }
   }
}