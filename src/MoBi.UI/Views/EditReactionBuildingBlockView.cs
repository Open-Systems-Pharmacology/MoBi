using OSPSuite.Assets;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditReactionBuildingBlockView : EditBuildingBlockBaseView, IEditReactionBuildingBlockView
   {
      public EditReactionBuildingBlockView(IMainView mainView)
         : base(mainView)
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         tabFavorites.InitWith(Captions.Favorites, ApplicationIcons.Favorites);
         tabUserDefined.InitWith(AppConstants.Captions.UserDefined, ApplicationIcons.UserDefinedVariability);
         tabFlowChart.InitWith(AppConstants.Captions.Chart, ApplicationIcons.Diagram);
         tabList.InitWith(AppConstants.Captions.List, ApplicationIcons.Parameter);
         splitContainerControl1.CollapsePanel = SplitCollapsePanel.Panel1;
         EditCaption = AppConstants.Captions.Reactions;
         tabOverviewControl.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanged, e);

      }

      private void tabSelectionChanged(TabPageChangingEventArgs e)
      {
         if (e.Page.Equals(tabUserDefined))
            reactionBuildingBlockPresenter.UpdateUserDefinedParameters();
      }

      private IEditReactionBuildingBlockPresenter reactionBuildingBlockPresenter => _presenter.DowncastTo<IEditReactionBuildingBlockPresenter>();

      public void AttachPresenter(IEditReactionBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void SetEditReactionView(IView view)
      {
         splitContainerControl1.Panel2.FillWith(view);
      }

      public void SetReactionListView(IView view)
      {
         tabList.FillWith(view);
      }

      public void SetReactionDiagram(IView view)
      {
         tabFlowChart.FillWith(view);
      }

      public void SetFavoritesReactionView(IView view)
      {
         tabFavorites.FillWith(view);
      }

      public void SetUserDefinedParametersView(IView view)
      {
         tabUserDefined.FillWith(view);
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Reaction;
   }
}