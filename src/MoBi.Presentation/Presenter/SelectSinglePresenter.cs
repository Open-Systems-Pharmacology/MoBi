using System.Drawing;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectSinglePresenter<T> : ISelectionPresenter<T>, IPresenter<ISelectSingleView<T>>
   {
      T Selection { get; }
      Size? ModalSize { get; }
      void SetDescription(string description);
   }

   public abstract class SelectSinglePresenter<T> : SelectionPresenter<ISelectSingleView<T>, ISelectSinglePresenter<T>, T>, ISelectSinglePresenter<T>
   {
      protected SelectSinglePresenter(ISelectSingleView<T> view, IItemToListItemMapper<T> itemToListItemMapper) : base(view, itemToListItemMapper)
      {
      }

      public T Selection => _view.Selection.Item;
      public Size? ModalSize => _view.ModalSize;

      public void SetDescription(string description) => _view.SetDescription(description);
   }
}