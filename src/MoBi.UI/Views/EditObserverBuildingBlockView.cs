using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditObserverBuildingBlockView : EditBuildingBlockBaseView, IEditObserverBuildingBlockView
   {
      public EditObserverBuildingBlockView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
         tabAmountObserverList.Tag = ObserverType.AmountObserver;
         tabContainerObserverList.Tag = ObserverType.ContainerObserver;
         splitContainerControl.CollapsePanel = SplitCollapsePanel.Panel1;
      }

      public void AttachPresenter(IEditObserverBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetAmountObserverView(IView view)
      {
         tabAmountObserverList.FillWith(view);
      }

      public void SetContainerObserverView(IView view)
      {
         tabContainerObserverList.FillWith(view);
      }

      public void SetEditObserverBuilderView(IView view)
      {
         splitContainerControl.Panel2.FillWith(view);
      }

      public ObserverType ObserverType
      {
         get { return ((ObserverType) tabControl.SelectedTabPage.Tag); }
      }

      private void selectedPageChanged(object sender, TabPageChangedEventArgs e)
      {
         OnEvent(() => editObserverBuildingBlockPresenter.Select((ObserverType) e.Page.Tag));
      }

      private IEditObserverBuildingBlockPresenter editObserverBuildingBlockPresenter
      {
         get { return Presenter.DowncastTo<IEditObserverBuildingBlockPresenter>(); }
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Observer;

      public override void InitializeResources()
      {
         base.InitializeResources();
         tabAmountObserverList.InitWith(AppConstants.Captions.MoleculeObserver, ApplicationIcons.MoleculeObserver);
         tabContainerObserverList.InitWith(AppConstants.Captions.ContainerObserver, ApplicationIcons.ContainerObserver);
         EditCaption = AppConstants.Captions.Observers;
      }
   }
}