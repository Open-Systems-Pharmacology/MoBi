using System;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views.SimulationView
{
   public partial class EditSimulationView : BaseMdiChildView, IEditSimulationView
   {
      public EditSimulationView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
         spliterDiagram.CollapsePanel = SplitCollapsePanel.Panel1;
         splitSimulationParameters.CollapsePanel = SplitCollapsePanel.Panel1;
      }

      public void AttachPresenter(IEditSimulationPresenter presenter)
      {
         _presenter = presenter;
      }

      private IEditSimulationPresenter simulationPresenter => _presenter.DowncastTo<IEditSimulationPresenter>();

      public override void InitializeResources()
      {
         base.InitializeResources();
         tabDiagram.InitWith(AppConstants.Captions.ModelDiagram, ApplicationIcons.Diagram);
         tabTree.InitWith(AppConstants.Captions.Tree, ApplicationIcons.Tree);
         tabSimulation.InitWith(AppConstants.Captions.SimulationParameters, ApplicationIcons.Parameter);
         tabResults.InitWith(AppConstants.Captions.Results, ApplicationIcons.TimeProfileAnalysis);

         tabsNavigation.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanged, e);
         tabs.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanged, e);

         spliterDiagram.Horizontal = true;
         spliterDiagram.SplitterPosition = Convert.ToInt32(Height * AppConstants.Diagram.SplitterDiagramRatio);
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Simulation;

      private void tabSelectionChanged(TabPageChangingEventArgs e)
      {
         if (e.Page.Equals(tabDiagram))
            simulationPresenter.LoadDiagram();
      }

      public void SetEditView(IView view)
      {
         splitSimulationParameters.Panel2.FillWith(view);
      }

      public void SetTreeView(IView view)
      {
         tabTree.FillWith(view);
      }

      public void SetChartView(IView view)
      {
         view.CaptionChanged += (o, e) => OnEvent(() => tabResults.Text = captionNameFrom(view));
         tabResults.FillWith(view);
      }

      private string captionNameFrom(IView view)
      {
         return string.IsNullOrWhiteSpace(view.Caption) ? AppConstants.Captions.Results : view.Caption;
      }

      public void SetModelDiagram(ISimulationDiagramView subView)
      {
         spliterDiagram.Panel2.FillWith(subView);
         subView.Overview = modelOverview;
      }

      public bool ShowsResults => tabs.SelectedTabPage.Equals(tabResults);

      public void ShowResultsTab()
      {
         tabs.SelectedTabPage = tabResults;
      }
   }
}