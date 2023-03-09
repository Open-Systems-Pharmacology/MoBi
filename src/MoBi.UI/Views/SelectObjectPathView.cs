using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectObjectPathView : BaseModalView, ISelectObjectPathView
   {
      private ISelectObjectPathPresenter _presenter;

      public SelectObjectPathView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.SelectChangedEntity;
      }

      public void AttachPresenter(ISelectObjectPathPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSelectionView(ISelectEntityInTreeView view)
      {
         panelControl.FillWith(view);
      }
   }
}