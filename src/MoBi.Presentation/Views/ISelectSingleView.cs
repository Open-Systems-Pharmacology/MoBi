using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectSingleView<T> : IView<ISelectSinglePresenter<T>>, ISelectionView<T>
   {
      ListItemDTO<T> Selection { get; }
      void SetDescription(string description);
   }
}