using OSPSuite.Assets;
using DevExpress.XtraEditors;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditEventGroupBuildingBlockView : EditBuildingBlockBaseView, IEditEventGroupBuildingBlockView
   {
      public EditEventGroupBuildingBlockView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
         splitContainerControl.CollapsePanel = SplitCollapsePanel.Panel1;
      }

      public void AttachPresenter(IEditEventGroupBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetListView(IView view)
      {
         splitContainerControl.Panel1.FillWith(view);
      }

      public void SetEditView(IView editEventView)
      {
         splitContainerControl.Panel2.FillWith(editEventView);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         EditCaption = AppConstants.Captions.Events;
         ApplicationIcon = ApplicationIcons.PassiveTransport;
      }
   }
}