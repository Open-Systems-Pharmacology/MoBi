using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.HelpersForTests;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;
using IApplicationSettings = OSPSuite.Core.IApplicationSettings;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation;

public class concern_for_ComparisonChartPresenter : ContextSpecification<ComparisonChartPresenter>
{
   protected IChartView _chartView;
   protected IMoBiContext _context;
   protected IUserSettings _userSettings;
   protected IChartTemplatingTask _chartTemplatingTask;
   protected IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;
   protected IChartUpdater _chartUpdater;
   protected ChartPresenterContext _chartPresenterContext;
   protected IOutputMappingMatchingTask _outputMappingMatchingTask;
   protected IChartDisplayPresenter _chartDisplayPresenter;
   protected IDragEvent _dragEvent;
   private IChartDisplayView _chartDisplayView;
   private ICurveBinderFactory _curveBinderFactory;
   private IViewItemContextMenuFactory _contextMenuFactoryForProjectItem;
   private IAxisBinderFactory _axisBinderFactory;
   private ICurveToDataModeMapper _dataModeMapper;
   private ICurveChartExportTask _chartExportTask;
   private IApplicationSettings _applicationSettings;
   private IApplicationController _applicationController;
   private IDialogCreator _dialogCreator;

   protected override void Context()
   {
      _chartView = A.Fake<IChartView>();
      _context = A.Fake<IMoBiContext>();
      _userSettings = A.Fake<IUserSettings>();
      _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
      _quantityDisplayPathMapper = A.Fake<IQuantityPathToQuantityDisplayPathMapper>();
      _chartUpdater = A.Fake<IChartUpdater>();
      _chartPresenterContext = A.Fake<ChartPresenterContext>();
      _outputMappingMatchingTask = A.Fake<IOutputMappingMatchingTask>();
      _dragEvent = A.Fake<IDragEvent>();
      _dialogCreator = A.Fake<IDialogCreator>();

      _chartDisplayView = A.Fake<IChartDisplayView>();
      _curveBinderFactory = A.Fake<ICurveBinderFactory>();
      _contextMenuFactoryForProjectItem = A.Fake<IViewItemContextMenuFactory>();
      _axisBinderFactory = A.Fake<IAxisBinderFactory>();
      _dataModeMapper = A.Fake<ICurveToDataModeMapper>();
      _chartExportTask = A.Fake<ICurveChartExportTask>();
      _applicationSettings = A.Fake<IApplicationSettings>();
      _applicationController = A.Fake<IApplicationController>();

      A.CallTo(() => _context.CurrentProject).Returns(new MoBiProject());

      _chartDisplayPresenter = CreateDisplayPresenter();
      A.CallTo(() => _chartPresenterContext.DisplayPresenter).Returns(_chartDisplayPresenter);

      sut = new ComparisonChartPresenter(_chartView, _context, _userSettings, _chartTemplatingTask,
         _quantityDisplayPathMapper, _chartUpdater, _chartPresenterContext, _outputMappingMatchingTask);
      _chartDisplayPresenter.Edit(new CurveChart());
   }

   protected virtual IChartDisplayPresenter CreateDisplayPresenter()
   {
      return new ChartDisplayPresenter(_chartDisplayView, _curveBinderFactory, _contextMenuFactoryForProjectItem,
         _axisBinderFactory, _dataModeMapper, _chartExportTask, _applicationSettings, _applicationController, _dialogCreator);
   }
}

public class When_dropping_historical_results_on_the_comparison_chart : concern_for_ComparisonChartPresenter
{
   private DataRepository _historicalResults;
   private DataColumn _outputColumn;

   protected override void Context()
   {
      base.Context();
      _historicalResults = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Sim2");
      _outputColumn = _historicalResults.AllButBaseGrid().Single();
      var nodes = new List<ITreeNode> { new HistoricalResultsNode(_historicalResults) };
      A.CallTo(() => _dragEvent.Data<IList<ITreeNode>>()).Returns(nodes);
   }

   protected override void Because()
   {
      _chartDisplayPresenter.OnDragDrop(_dragEvent);
   }

   [Observation]
   public void a_curve_is_added_for_each_output_column_of_the_dropped_historical_results()
   {
      A.CallTo(() => _chartPresenterContext.EditorPresenter.AddCurveForColumn(_outputColumn, A<CurveOptions>._, A<bool>._)).MustHaveHappened();
   }

   [Observation]
   public void the_dropped_repository_is_added_to_the_data_browser()
   {
      A.CallTo(() => _chartPresenterContext.EditorPresenter.AddDataRepositories(A<IReadOnlyList<DataRepository>>.That.Contains(_historicalResults))).MustHaveHappened();
   }

   [Observation]
   public void the_dropped_repository_is_marked_as_persistable()
   {
      _historicalResults.IsPersistable().ShouldBeTrue();
   }

   [Observation]
   public void the_chart_is_refreshed_so_the_newly_added_curves_are_painted()
   {
      // ChartPresenterContext.Refresh() is non-virtual so we can't intercept it directly;
      // assert on its underlying EditorPresenter.Refresh() delegate call instead.
      A.CallTo(() => _chartPresenterContext.EditorPresenter.Refresh()).MustHaveHappened();
   }
}
