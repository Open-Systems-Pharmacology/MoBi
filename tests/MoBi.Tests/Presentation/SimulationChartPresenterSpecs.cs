using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Views.Charts;
using IApplicationSettings = OSPSuite.Core.IApplicationSettings;
using IChartTemplatingTask = MoBi.Presentation.Tasks.IChartTemplatingTask;

namespace MoBi.Presentation;

public class concern_for_SimulationChartPresenter : ContextSpecification<SimulationChartPresenter>
{
   protected IChartView _chartView;
   protected IMoBiContext _context;
   protected IUserSettings _userSettings;
   protected IChartTemplatingTask _chartTemplatingTask;
   protected ICurveNamer _curveNamer;
   protected IChartUpdater _chartUpdater;
   protected ChartPresenterContext _chartPresenterContext;
   protected IOutputMappingMatchingTask _outputMappingMatchingTask;
   protected IChartTasks _chartTasks;
   protected IChartDisplayPresenter _chartDisplayPresenter;
   private IChartDisplayView _chartDisplayView;
   private ICurveBinderFactory _curveBinderFactory;
   private IViewItemContextMenuFactory _contextMenuFactoryForProjectItem;
   private IAxisBinderFactory _axisBinderFactory;
   private ICurveToDataModeMapper _dataModeMapper;
   private ICurveChartExportTask _chartExportTask;
   private IApplicationSettings _applicationSettings;
   private IApplicationController _applicationController;
   protected IDragEvent _dragEvent;
   private IEnumerable<ITreeNode> _treeNodes;
   private IDialogCreator _dialogCreator;

   protected override void Context()
   {
      _chartView = A.Fake<IChartView>();
      _context = A.Fake<IMoBiContext>();
      _userSettings = A.Fake<IUserSettings>();
      _chartTemplatingTask = A.Fake<IChartTemplatingTask>();
      _curveNamer = A.Fake<ICurveNamer>();
      _chartUpdater = A.Fake<IChartUpdater>();
      _chartPresenterContext = A.Fake<ChartPresenterContext>();
      _outputMappingMatchingTask = A.Fake<IOutputMappingMatchingTask>();
      _chartTasks = A.Fake<IChartTasks>();
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

      var classifiableObservedData = new ClassifiableObservedData
      {
         Subject = new DataRepository("dataRepository")
      };

      var observedDataNode = new ObservedDataNode(classifiableObservedData);
      _treeNodes = new List<ITreeNode>
      {
         observedDataNode
      };

      _chartDisplayPresenter = CreateDisplayPresenter();

      A.CallTo(() => _chartPresenterContext.DisplayPresenter).Returns(_chartDisplayPresenter);

      sut = new SimulationChartPresenter(_chartView, _context, _userSettings, _chartTemplatingTask, _curveNamer, _chartUpdater, _chartPresenterContext, _outputMappingMatchingTask, _chartTasks);
      _chartDisplayPresenter.Edit(new CurveChart());

      A.CallTo(() => _dragEvent.Data<IEnumerable<ITreeNode>>()).Returns(_treeNodes);
   }

   protected virtual IChartDisplayPresenter CreateDisplayPresenter()
   {
      return new ChartDisplayPresenter(_chartDisplayView, _curveBinderFactory, _contextMenuFactoryForProjectItem, _axisBinderFactory, _dataModeMapper, _chartExportTask, _applicationSettings, _applicationController, _dialogCreator);
   }
}

public class When_adding_observed_data_to_the_chart_color_grouping : concern_for_SimulationChartPresenter
{
   private int _observedDataAddedEventCounter;

   protected override void Context()
   {
      base.Context();
      _userSettings.ColorGroupObservedDataFromSameFolder = true;
      sut.OnObservedDataAddedToChart += (sender, args) => { _observedDataAddedEventCounter++; };
   }

   protected override void Because()
   {
      _chartDisplayPresenter.OnDragDrop(_dragEvent);
   }

   [Observation]
   public void the_event_must_have_been_called_once()
   {
      _observedDataAddedEventCounter.ShouldBeEqualTo(1);
   }
}

public class When_handling_observed_data_removed_event_and_the_data_is_in_the_chart : concern_for_SimulationChartPresenter
{
   private DataRepository _dataRepository;
   private IProject _project;

   protected override void Context()
   {
      base.Context();
      _dataRepository = new DataRepository("id")
      {
         new DataColumn(),
         new DataColumn()
      };
      _project = new MoBiProject();
      A.CallTo(() => _chartPresenterContext.EditorPresenter.AllDataColumns).Returns(_dataRepository.Columns.ToList());
   }

   protected override IChartDisplayPresenter CreateDisplayPresenter()
   {
      return A.Fake<IChartDisplayPresenter>();
   }

   protected override void Because()
   {
      sut.Handle(new ObservedDataRemovedEvent(new[] { _dataRepository }, _project));
   }

   [Observation]
   public void the_display_is_not_refreshed()
   {
      A.CallTo(() => _chartDisplayPresenter.Refresh()).MustHaveHappened();
   }
}

public class When_handling_observed_data_removed_event_and_the_data_is_not_in_the_chart : concern_for_SimulationChartPresenter
{
   private DataRepository _dataRepository;
   private IProject _project;

   protected override void Context()
   {
      base.Context();
      _dataRepository = new DataRepository("id")
      {
         new DataColumn(),
         new DataColumn()
      };
      _project = new MoBiProject();
   }

   protected override IChartDisplayPresenter CreateDisplayPresenter()
   {
      return A.Fake<IChartDisplayPresenter>();
   }

   protected override void Because()
   {
      sut.Handle(new ObservedDataRemovedEvent(new[] { _dataRepository }, _project));
   }

   [Observation]
   public void the_display_is_not_refreshed()
   {
      A.CallTo(() => _chartDisplayPresenter.Refresh()).MustNotHaveHappened();
   }
}

public class When_adding_observed_data_to_the_chart_not_color_grouping : concern_for_SimulationChartPresenter
{
   private int _observedDataAddedEventCounter;

   protected override void Context()
   {
      base.Context();
      _userSettings.ColorGroupObservedDataFromSameFolder = false;
      sut.OnObservedDataAddedToChart += (sender, args) => { _observedDataAddedEventCounter++; };
   }

   protected override void Because()
   {
      _chartDisplayPresenter.OnDragDrop(_dragEvent);
   }

   [Observation]
   public void the_event_must_have_been_called_once()
   {
      _observedDataAddedEventCounter.ShouldBeEqualTo(1);
   }
}

public class When_adding_observed_data_to_the_chart : concern_for_SimulationChartPresenter
{
   protected override void Context()
   {
      base.Context();
      _userSettings.ColorGroupObservedDataFromSameFolder = false;
   }

   protected override void Because()
   {
      _chartDisplayPresenter.OnDragDrop(_dragEvent);
   }

   [Observation]
   public void columns_showing_should_be_default()
   {
      A.CallTo(() => _chartPresenterContext.EditorPresenter.SetShowDataColumnInDataBrowserDefinition(A<Func<DataColumn, bool>>.Ignored)).MustNotHaveHappened();
   }
}

public class When_initializing_analysis_with_a_simulation_and_chart : concern_for_SimulationChartPresenter
{
   private IMoBiSimulation _simulation;
   private MoBiSimulationTimeProfileChart _analysisChart;
   private DataRepository _resultsDataRepository;

   protected override void Context()
   {
      base.Context();
      _simulation = A.Fake<IMoBiSimulation>();
      _analysisChart = new MoBiSimulationTimeProfileChart();
      _resultsDataRepository = new DataRepository("results");
      A.CallTo(() => _simulation.ResultsDataRepository).Returns(_resultsDataRepository);
      A.CallTo(() => _simulation.OutputMappings).Returns(new OutputMappings());
      A.CallTo(() => _chartTemplatingTask.TemplateFrom(A<CurveChart>._, A<bool>._)).Returns(new CurveChartTemplate());
   }

   protected override IChartDisplayPresenter CreateDisplayPresenter()
   {
      return A.Fake<IChartDisplayPresenter>();
   }

   protected override void Because()
   {
      sut.InitializeAnalysis(_analysisChart, _simulation);
   }

   [Observation]
   public void should_set_the_origin_text_on_the_chart()
   {
      A.CallTo(() => _chartTasks.SetOriginText(_simulation.Name, _analysisChart)).MustHaveHappened();
   }

   [Observation]
   public void should_expose_the_analysis_chart()
   {
      sut.Analysis.ShouldBeEqualTo(_analysisChart);
   }
}

public class When_updating_analysis_based_on_a_simulation : concern_for_SimulationChartPresenter
{
   private IMoBiSimulation _simulation;
   private MoBiSimulationTimeProfileChart _analysisChart;
   private DataRepository _resultsDataRepository;

   protected override void Context()
   {
      base.Context();
      _simulation = A.Fake<IMoBiSimulation>();
      _analysisChart = new MoBiSimulationTimeProfileChart();
      _resultsDataRepository = new DataRepository("results");
      A.CallTo(() => _simulation.ResultsDataRepository).Returns(_resultsDataRepository);
      A.CallTo(() => _simulation.OutputMappings).Returns(new OutputMappings());
      A.CallTo(() => _chartTemplatingTask.TemplateFrom(A<CurveChart>._, A<bool>._)).Returns(new CurveChartTemplate());
      sut.InitializeAnalysis(_analysisChart, _simulation);
   }

   protected override IChartDisplayPresenter CreateDisplayPresenter()
   {
      return A.Fake<IChartDisplayPresenter>();
   }

   protected override void Because()
   {
      sut.UpdateAnalysisBasedOn(_simulation);
   }

   [Observation]
   public void should_update_the_origin_text_on_the_chart()
   {
      // Called once during InitializeAnalysis and once during UpdateAnalysisBasedOn
      A.CallTo(() => _chartTasks.SetOriginText(_simulation.Name, _analysisChart)).MustHaveHappened(2, Times.Exactly);
   }
}