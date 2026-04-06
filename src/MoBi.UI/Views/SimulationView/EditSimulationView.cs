using System;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
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
         tabs.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
         tabs.CloseButtonClick += (o, e) => OnEvent(closeButtonClick, e as ClosePageButtonEventArgs);
      }

      public void AttachPresenter(IEditSimulationPresenter presenter)
      {
         _presenter = presenter;
      }

      private IEditSimulationPresenter simulationPresenter => _presenter.DowncastTo<IEditSimulationPresenter>();

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.Simulation;
         tabDiagram.InitWith(AppConstants.Captions.ModelDiagram, ApplicationIcons.Diagram);
         tabTree.InitWith(AppConstants.Captions.Tree, ApplicationIcons.Tree);
         tabSimulation.InitWith(AppConstants.Captions.SimulationParameters, ApplicationIcons.Parameter);
         tabData.InitWith(AppConstants.Captions.SimulationObservedData, ApplicationIcons.ObservedData);
         tabChanges.InitWith(AppConstants.Captions.Changes, ApplicationIcons.Comparison);

         tabsNavigation.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanged, e);
         tabs.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanged, e);

         spliterDiagram.Horizontal = true;
         spliterDiagram.SplitterPosition = Convert.ToInt32(Height * AppConstants.Diagram.SplitterDiagramRatio);
      }

      private void tabSelectionChanged(TabPageChangingEventArgs e)
      {
         if (e.Page.Equals(tabDiagram))
            simulationPresenter.LoadDiagram();
         else if (e.Page.Equals(tabChanges))
            simulationPresenter.LoadChanges();
      }

      private void closeButtonClick(ClosePageButtonEventArgs e)
      {
         var closingTab = e.Page as XtraTabPage;
         if (closingTab?.Tag is not ISimulationAnalysisPresenter analysisPresenter)
            return;

         simulationPresenter.RemoveAnalysis(analysisPresenter);
      }

      public void SetEditView(IView view)
      {
         splitSimulationParameters.Panel2.FillWith(view);
      }

      public void SetTreeView(IView view)
      {
         tabTree.FillWith(view);
      }

      public void SetDataView(ISimulationOutputMappingView view)
      {
         tabData.FillWith(view);
      }

      public void SetModelDiagram(ISimulationDiagramView subView)
      {
         spliterDiagram.Panel2.FillWith(subView);
      }

      public bool ShowsResults => tabs.SelectedTabPage?.Tag is ISimulationAnalysisPresenter;

      public void ShowResultsTab()
      {
         var firstAnalysisTab = tabs.TabPages.FirstOrDefault(x => x.Tag is ISimulationAnalysisPresenter);
         if (firstAnalysisTab != null)
            tabs.SelectedTabPage = firstAnalysisTab;
      }

      public void AddAnalysis(ISimulationAnalysisPresenter analysisPresenter)

      {
         var page = new XtraTabPage();
         page.Tag = analysisPresenter;
         page.ShowCloseButton = DefaultBoolean.True;
         page.InitializeFrom(analysisPresenter.BaseView);

         var changesIndex = tabs.TabPages.IndexOf(tabChanges);
         tabs.TabPages.Insert(changesIndex, page);
         tabs.SelectedTabPage = page;
      }

      public void RemoveAnalysis(ISimulationAnalysisPresenter analysisPresenter)
      {
         var tab = tabs.TabPages.FirstOrDefault(x => Equals(x.Tag, analysisPresenter));
         if (tab == null)
            return;

         tabs.TabPages.Remove(tab);
      }

      public void ShowChangesTab()
      {
         tabs.SelectedTabPage = tabChanges;
      }

      public void SetChangesView(ISimulationChangesView view)
      {
         tabChanges.FillWith(view);
      }

      public void SetParametersTabEnabled(bool enabled)
      {
         splitSimulationParameters.Enabled = enabled;
      }
   }
}