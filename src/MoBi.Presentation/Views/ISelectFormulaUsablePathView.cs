using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISelectFormulaUsablePathView : IModalView<ISelectFormulaUsablePathPresenter>
   {
      void AddSelectionView(IView view);
      string Text { set; get; }
   }
}