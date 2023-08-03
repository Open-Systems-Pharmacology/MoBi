using System;
using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
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

namespace MoBi.Presentation
{
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
         _dragEvent = A.Fake<IDragEvent>();

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

         _chartDisplayPresenter = new ChartDisplayPresenter(_chartDisplayView, _curveBinderFactory, _contextMenuFactoryForProjectItem, _axisBinderFactory, _dataModeMapper, _chartExportTask, _applicationSettings, _applicationController);


         A.CallTo(() => _chartPresenterContext.DisplayPresenter).Returns(_chartDisplayPresenter);

         sut = new SimulationChartPresenter(_chartView, _context, _userSettings, _chartTemplatingTask, _curveNamer, _chartUpdater, _chartPresenterContext, _outputMappingMatchingTask);
         _chartDisplayPresenter.Edit(new CurveChart());

         A.CallTo(() => _dragEvent.Data<IEnumerable<ITreeNode>>()).Returns(_treeNodes);
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

      [Observation]
      public void columns_showing_should_be_default()
      {
         A.CallTo(()=> _chartPresenterContext.EditorPresenter.SetShowDataColumnInDataBrowserDefinition(A<Func<DataColumn, bool>>.Ignored)).MustNotHaveHappened();
      }
   }
}