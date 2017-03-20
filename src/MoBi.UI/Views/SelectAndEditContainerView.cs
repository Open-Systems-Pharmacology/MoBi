using OSPSuite.Assets;
using DevExpress.XtraEditors;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class SelectAndEditContainerView : BaseUserControl, ISelectAndEditContainerView
   {
      private ISelectAndEditPresenter _presenter;

      public SelectAndEditContainerView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISelectAndEditPresenter presenter)
      {
         _presenter = presenter;
      }

      public string Description
      {
         set { lblEditName.Text = value; }
      }

      public void AddEditView(IView view)
      {
         panel.FillWith(view);
      }

      public void AddLegendView(IView view)
      {
         legendPanel.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnAddBuildingBlockToProject.Text = AppConstants.Captions.SaveChangesAsNewBuildingBlock;
         btnAddBuildingBlockToProject.ImageLocation = ImageLocation.MiddleLeft;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnAddBuildingBlockToProject.Click += (o, e) => OnEvent(_presenter.AddToProject);
      }

      public override ApplicationIcon ApplicationIcon
      {
         get { return _presenter.Icon; }
      }
   }
}