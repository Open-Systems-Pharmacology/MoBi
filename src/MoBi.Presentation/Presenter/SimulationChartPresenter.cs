using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Utility.Extensions;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation.Presenter
{
   public interface ISimulationChartPresenter : IChartPresenter, ISimulationAnalysisPresenter
   {
      void RefreshSimulationChart();
   }

   public class SimulationChartPresenter : ChartPresenter, ISimulationChartPresenter
   {
      private readonly ICurveNamer _curveNamer;
      private readonly IChartTasks _chartTasks;
      private IMoBiSimulation _simulation;
      private AnalysisChart _analysisChart;

      public SimulationChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTemplatingTask chartTemplatingTask, ICurveNamer curveNamer, IChartUpdater chartUpdater, ChartPresenterContext chartPresenterContext, IOutputMappingMatchingTask outputMappingMatchingTask, IChartTasks chartTasks)
         : base(chartView, chartPresenterContext, context, userSettings, chartTemplatingTask, chartUpdater, outputMappingMatchingTask)
      {
         _curveNamer = curveNamer;
         _chartTasks = chartTasks;
         ChartEditorPresenter.SetLinkSimDataMenuItemVisibility(true);
      }

      protected override bool CanDropSimulation => false;

      protected override void MarkChartOwnerAsChanged()
      {
         _dataRepositoryCache.Each(simulation => simulation.HasChanged = true);
      }

      protected override string CurveNameDefinition(DataColumn column)
      {
         return _curveNamer.CurveNameForColumn(_dataRepositoryCache[column.Repository], column, addSimulationName: false);
      }

      public void RefreshSimulationChart()
      {
         Refresh();
      }

      public ISimulationAnalysis Analysis => _analysisChart;

      public void InitializeAnalysis(ISimulationAnalysis simulationAnalysis, IAnalysable analysable)
      {
         _analysisChart = simulationAnalysis as AnalysisChart;
         _simulation = analysable as IMoBiSimulation;
         UpdateTemplatesFor(_simulation);
         _chartTasks.SetOriginText(_simulation.Name, _analysisChart);
         refreshChartData();
      }

      public void UpdateAnalysisBasedOn(IAnalysable analysable)
      {
         _simulation = analysable as IMoBiSimulation;
         _chartTasks.SetOriginText(_simulation.Name, _analysisChart);
         refreshChartData();
      }

      private void refreshChartData()
      {
         CurveChartTemplate defaultTemplate = null;
         var plottedData = new List<DataRepository>();

         if (_simulation.ResultsDataRepository != null)
            plottedData.Add(_simulation.ResultsDataRepository);

         if (_analysisChart.Curves.Count == 0)
            defaultTemplate = _simulation.DefaultChartTemplate;

         addObservedDataRepositories(plottedData, _analysisChart.Curves);
         var notPlottedData = mappedObservedDataExcept(plottedData);

         Show(_analysisChart, plottedData, notPlottedData, defaultTemplate);
      }

      private void addObservedDataRepositories(IList<DataRepository> data, IEnumerable<Curve> curves)
      {
         foreach (var curve in curves.Where(c => c.IsObserved()))
         {
            data.AddUnique(curve.xData.Repository);
            data.AddUnique(curve.yData.Repository);
         }
      }

      private IReadOnlyList<DataRepository> mappedObservedDataExcept(IReadOnlyList<DataRepository> plottedData)
      {
         return _simulation.OutputMappings.AllDataRepositoryMappedFor(_simulation).Except(plottedData).ToList();
      }
   }
}