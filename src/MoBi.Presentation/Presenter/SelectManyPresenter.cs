using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectManyPresenter<T> : ISelectionPresenter<T>, IPresenter<ISelectManyView<T>>
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

      public IEnumerable<T> Selections => _view.Selections.Select(x => x.Item);
   }
}