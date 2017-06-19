using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
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

      public SimulationChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTasks chartTasks, IChartTemplatingTask chartTemplatingTask, ICurveNamer curveNamer, IChartUpdater chartUpdater, ChartPresenterContext chartPresenterContext)
         : base(chartView, context, userSettings, chartTasks, chartTemplatingTask, chartUpdater, chartPresenterContext)
      {
         _curveNamer = curveNamer;
      }

      protected override bool CanDropSimulation => false;

      protected override void MarkChartOwnerAsChanged()
      {
         _simulations.Each(simulation => simulation.HasChanged = true);
      }

      protected override string CurveNameDefinition(DataColumn column)
      {
         return _curveNamer.CurveNameForColumn(_simulations[column.Repository], column, addSimulationName: false);
      }
   }
}