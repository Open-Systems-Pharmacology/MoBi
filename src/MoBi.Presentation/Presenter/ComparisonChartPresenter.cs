using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation.Presenter
{
   public interface IComparisonChartPresenter : IChartPresenter
   {
   }

   public class ComparisonChartPresenter : ChartPresenter, IComparisonChartPresenter
   {
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;

      public ComparisonChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTasks chartTasks, IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter, 
         IChartTemplatingTask chartTemplatingTask, IDataColumnToPathElementsMapper dataColumnToPathElementsMapper, IChartEditorLayoutTask chartEditorLayoutTask, 
         IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper, IChartUpdater chartUpdater) :
            base(chartView, context, userSettings, chartTasks, chartEditorAndDisplayPresenter, chartTemplatingTask, dataColumnToPathElementsMapper, chartEditorLayoutTask, chartUpdater)
      {
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
      }

      protected override string CurveNameDefinition(DataColumn column)
      {
         var simulationForDataColumn = _simulations[column.Repository];
         //Always use repository name for curve name when comparing results
         return _quantityDisplayPathMapper.DisplayPathAsStringFor(simulationForDataColumn, column, column.Repository.Name);
      }

      protected override bool CanDropSimulation => true;

      protected override void MarkChartOwnerAsChanged()
      {
         _context.ProjectChanged();
      }
   }
}