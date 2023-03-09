using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectObjectPathView : IModalView<ISelectObjectPathPresenter>
   {
      void AddSelectionView(ISelectEntityInTreeView view);
   }
}