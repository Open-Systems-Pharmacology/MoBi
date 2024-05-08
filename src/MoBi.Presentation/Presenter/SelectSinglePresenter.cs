using System.Collections.Generic;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectSinglePresenter<T> : IInitializablePresenter<IEnumerable<T>>, IPresenter<ISelectSingleView<T>>
   {
      T Selection { get; }
      void SetDescription(string description);
   }

   public abstract class SelectSinglePresenter<T> : SelectionPresenter<ISelectSingleView<T>, ISelectSinglePresenter<T>, T>, ISelectSinglePresenter<T>
   {
      protected SelectSinglePresenter(ISelectSingleView<T> view, IItemToListItemMapper<T> itemToListItemMapper) : base(view, itemToListItemMapper)
      {
      }

      public T Selection => _view.Selection.Item;

      public void SetDescription(string description) => _view.SetDescription(description);
   }
}