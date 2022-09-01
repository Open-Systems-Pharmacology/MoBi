﻿using System;
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
         ApplicationIcon = ApplicationIcons.Simulation;
         tabDiagram.InitWith(AppConstants.Captions.ModelDiagram, ApplicationIcons.Diagram);
         tabTree.InitWith(AppConstants.Captions.Tree, ApplicationIcons.Tree);
         tabSimulation.InitWith(AppConstants.Captions.SimulationParameters, ApplicationIcons.Parameter);
         tabResults.InitWith(AppConstants.Captions.Results, ApplicationIcons.TimeProfileAnalysis);
         tabData.InitWith(AppConstants.Captions.SimulationData, ApplicationIcons.PKSim);
         tabTimeProfile.InitWith(AppConstants.Captions.TimeProfile, ApplicationIcons.TimeProfileAnalysis);
         tabPredVsObs.InitWith(AppConstants.Captions.PredictedVsObserved, ApplicationIcons.PredictedVsObservedAnalysis);
         tabResidVsTime.InitWith(AppConstants.Captions.ResidualsVsTime, ApplicationIcons.ResidualVsTimeAnalysis);

         tabsNavigation.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanged, e);
         tabs.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanged, e);

         spliterDiagram.Horizontal = true;
         spliterDiagram.SplitterPosition = Convert.ToInt32(Height * AppConstants.Diagram.SplitterDiagramRatio);
      }

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

      public void SetChartView(IChartView chartView)
      {
         chartView.CaptionChanged += (o, e) => OnEvent(() => tabResults.Text = simulationPresenter.CreateResultTabCaption(chartView.Caption));
         tabTimeProfile.FillWith(chartView);
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

      public void SetDataView(ISimulationOutputMappingView view)
      {
         tabData.FillWith(view);
      }

      public void SetPredictedVsObservedView(ISimulationRunAnalysisView view)
      {
         tabPredVsObs.FillWith(view);
      }

      public void SetResidualsVsTimeView(ISimulationRunAnalysisView view)
      {
         tabResidVsTime.FillWith(view);
      }
   }
}