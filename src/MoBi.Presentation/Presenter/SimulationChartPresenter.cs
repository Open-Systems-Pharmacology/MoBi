using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Utility.Extensions;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation.Presenter
{
   public interface ISimulationChartPresenter : IChartPresenter
   {
   }

   public class SimulationChartPresenter : ChartPresenter, ISimulationChartPresenter
   {
      private readonly ICurveNamer _curveNamer;

      public SimulationChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTemplatingTask chartTemplatingTask, ICurveNamer curveNamer, IChartUpdater chartUpdater, ChartPresenterContext chartPresenterContext, IOutputMappingMatchingService outputMappingMatchingService)
         : base(chartView, chartPresenterContext, context, userSettings,  chartTemplatingTask, chartUpdater, outputMappingMatchingService)
      {
         _curveNamer = curveNamer;
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
   }
}