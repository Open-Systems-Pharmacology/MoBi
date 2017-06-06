using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
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

      public SimulationChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTasks chartTasks,
         IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, IChartTemplatingTask chartTemplatingTask, IDataColumnToPathElementsMapper dataColumnToPathElementsMapper,
         IChartEditorLayoutTask chartEditorLayoutTask, ICurveNamer curveNamer)
         : base(chartView, context, userSettings, chartTasks, chartEditorAndDisplayPresenter, chartTemplatingTask, dataColumnToPathElementsMapper, chartEditorLayoutTask)
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