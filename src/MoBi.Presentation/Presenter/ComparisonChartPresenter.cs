using MoBi.Core.Domain.Model;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
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

      public ComparisonChartPresenter(IChartView chartView, IMoBiContext context, IUserSettings userSettings, IChartTemplatingTask chartTemplatingTask, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper, IChartUpdater chartUpdater, ChartPresenterContext chartPresenterContext, IOutputMappingMatchingService outputMappingMatchingService) :
         base(chartView, chartPresenterContext, context, userSettings, chartTemplatingTask, chartUpdater, outputMappingMatchingService)
      {
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
      }

      protected override string CurveNameDefinition(DataColumn column)
      {
         var simulationForDataColumn = _dataRepositoryCache[column.Repository];
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