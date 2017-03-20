using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectFormulaUsablePathView : BaseModalView, ISelectFormulaUsablePathView
   {
      public SelectFormulaUsablePathView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISelectFormulaUsablePathPresenter presenter)
      {
      }

      public void AddSelectionView(IView view)
      {
         panelView.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemPanelView.TextVisible = false;
      }
   }
}