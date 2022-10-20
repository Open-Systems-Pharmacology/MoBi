using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Helpers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditSimulationPresenter : ContextSpecification<IEditSimulationPresenter>
   {
      protected IEditSimulationView _view;
      protected ISimulationChartPresenter _chartPresenter;
      protected ISimulationPredictedVsObservedChartPresenter _simulationPredictedVsObservedChartPresenter;
      protected ISimulationResidualVsTimeChartPresenter _simulationResidualVsTimeChartPresenter;
      protected IHierarchicalSimulationPresenter _hierarchicalSimulationPresenter;
      protected ISimulationDiagramPresenter _diagramPresenter;
      protected IEditSolverSettingsPresenter _solverSettings;
      protected IEditOutputSchemaPresenter _outputSchemaPresenter;
      protected IEditInSimulationPresenterFactory _presenterFactory;
      protected IEditFavoritesInSimulationPresenter _editFavoritePresenter;
      protected IChartTasks _chartTasks;
      protected IUserDefinedParametersPresenter _userDefinedParametersPresenter;
      protected ISimulationOutputMappingPresenter _simulationOutputMappingPresenter;
      protected IMoBiContext _context;
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;

      protected override void Context()
      {
         _view = A.Fake<IEditSimulationView>();
         _chartPresenter = A.Fake<ISimulationChartPresenter>();
         _simulationPredictedVsObservedChartPresenter = A.Fake<ISimulationPredictedVsObservedChartPresenter>();
         _simulationResidualVsTimeChartPresenter = A.Fake<ISimulationResidualVsTimeChartPresenter>();
         _hierarchicalSimulationPresenter = A.Fake<IHierarchicalSimulationPresenter>();
         _diagramPresenter = A.Fake<ISimulationDiagramPresenter>();
         _solverSettings = A.Fake<IEditSolverSettingsPresenter>();
         _outputSchemaPresenter = A.Fake<IEditOutputSchemaPresenter>();
         _presenterFactory = A.Fake<IEditInSimulationPresenterFactory>();
         _editFavoritePresenter = A.Fake<IEditFavoritesInSimulationPresenter>();
         _chartTasks = A.Fake<IChartTasks>();
         _simulationOutputMappingPresenter = A.Fake<ISimulationOutputMappingPresenter>();
         _userDefinedParametersPresenter = A.Fake<IUserDefinedParametersPresenter>();
         _context = A.Fake<IMoBiContext>();
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();

         sut = new EditSimulationPresenter(_view, _chartPresenter, _hierarchicalSimulationPresenter, _diagramPresenter,
            _solverSettings, _outputSchemaPresenter, _presenterFactory, new HeavyWorkManagerForSpecs(),
            A.Fake<IChartFactory>(), _editFavoritePresenter, _chartTasks, _userDefinedParametersPresenter, _simulationOutputMappingPresenter,
            _simulationPredictedVsObservedChartPresenter, _simulationResidualVsTimeChartPresenter, _context, _entitiesInSimulationRetriever);
      }
   }

   public class When_the_simulation_simulation_presenter_is_handling_a_favorite_selected_event : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private IView _favoritesView;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
         _favoritesView = A.Fake<IEditParameterListView>();
         A.CallTo(() => _editFavoritePresenter.BaseView).Returns(_favoritesView);
      }

      protected override void Because()
      {
         sut.Handle(new FavoritesSelectedEvent(_simulation));
      }

      [Observation]
      public void should_show_favorites()
      {
         A.CallTo(() => _view.SetEditView(_favoritesView)).MustHaveHappened();
      }
   }

   public class When_the_simulation_simulation_presenter_is_handling_a_simulation_completed_event : concern_for_EditSimulationPresenter
   {
      private SimulationRunFinishedEvent _simulationRunFinishedEvent;
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _simulationRunFinishedEvent = new SimulationRunFinishedEvent(_simulation);
         sut.Edit(_simulation);
         A.CallTo(() => _view.ShowsResults).Returns(false);
      }

      protected override void Because()
      {
         sut.Handle(_simulationRunFinishedEvent);
      }

      [Observation]
      public void the_chart_origin_data_should_be_changed()
      {
         A.CallTo(() => _chartTasks.SetOriginText(_simulation.Name, _simulation.Chart)).MustHaveHappened();
      }

      [Observation]
      public void the_view_should_be_changed_to_the_result_view()
      {
         A.CallTo(() => _view.ShowResultsTab()).MustHaveHappened();
      }
   }

   public class When_the_simulation_simulation_presenter_is_notified_that_the_edit_simulation_was_reloaded : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationReloadEvent(_simulation));
      }

      [Observation]
      public void should_clear_presenter()
      {
         A.CallTo(() => _hierarchicalSimulationPresenter.Clear()).MustHaveHappened();
      }

      [Observation]
      public void should_initialise_hierarchical_presenter()
      {
         // Needs to be at least twice because first time it's called during set up.
         A.CallTo(() => _hierarchicalSimulationPresenter.Edit(_simulation)).MustHaveHappenedTwiceOrMore();
      }
   }

   public class
      When_the_simulation_simulation_presenter_is_notified_that_a_simulation_run_is_finished_for_the_edited_simulation :
         concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private CurveChart _chart;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
         _chart = A.Fake<CurveChart>();
         A.CallTo(() => _simulation.Chart).Returns(_chart);
         A.CallTo(() => _view.ShowsResults).Returns(true);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationRunFinishedEvent(_simulation));
      }

      [Observation]
      public void should_ask_view_if_resultData_is_shown()
      {
         A.CallTo(() => _view.ShowsResults).MustHaveHappened();
      }

      [Observation]
      public void should_show_simulation_chart()
      {
         A.CallTo(() => _chartPresenter.Show(_chart, A<IReadOnlyList<DataRepository>>._, A<CurveChartTemplate>._)).MustHaveHappened();
      }
   }

   public class When_the_simulation_simulation_presenter_is_loading_the_diagram : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
      }

      protected override void Because()
      {
         sut.LoadDiagram();
      }

      [Observation]
      public void should_initialise_Diagram_presenter()
      {
         A.CallTo(() => _diagramPresenter.Edit(_simulation)).MustHaveHappened();
      }
   }

   public class When_the_simulation_simulation_presenter_is_editing_a_simulation_with_data : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private DataRepository _observedDataRepository;
      private IEnumerable<DataRepository> _data;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation { ResultsDataRepository = new DataRepository() };

         var chart = new CurveChart();

         _observedDataRepository = new DataRepository();
         chart.AddCurve(createObservedCurve(_observedDataRepository));

         _simulation.Chart = chart;

         A.CallTo(() => _chartPresenter.Show(chart, A<IReadOnlyList<DataRepository>>._, null))
            .Invokes(x => _data = x.GetArgument<IEnumerable<DataRepository>>(1));
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void should_initialise_hirarchical_presenter()
      {
         A.CallTo(() => _hierarchicalSimulationPresenter.Edit(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_display_result_data()
      {
         A.CallTo(() => _chartPresenter.Show(A<CurveChart>._, A<IReadOnlyList<DataRepository>>._, A<CurveChartTemplate>._))
            .MustHaveHappened();
      }

      [Observation]
      public void should_Add_Observer_Data_repository()
      {
         _data.ShouldContain(_observedDataRepository);
      }

      [Observation]
      public void should_not_display_diagram()
      {
         A.CallTo(() => _diagramPresenter.Edit(_simulation)).MustNotHaveHappened();
      }

      private Curve createObservedCurve(DataRepository observedDataRepository)
      {
         var dimensionFactory = A.Fake<IDimensionFactory>();
         var curve = new Curve
         {
            Name = "OBS"
         };
         var baseGrid = new BaseGrid("baseGrid", DomainHelperForSpecs.TimeDimension)
         {
            DataInfo = new DataInfo(ColumnOrigins.BaseGrid),
            Repository = observedDataRepository
         };

         var ydata = new DataColumn("ydata", DomainHelperForSpecs.ConcentrationDimension, baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation),
            Repository = observedDataRepository
         };

         curve.SetxData(baseGrid, dimensionFactory);
         curve.SetyData(ydata, dimensionFactory);
         return curve;
      }
   }

   public class
      When_the_edit_simulation_presenter_is_notified_that_a_parameter_should_be_selected_for_the_edited_simulation :
         concern_for_EditSimulationPresenter
   {
      private IParameter _parameter;
      private IMoBiSimulation _simulation;
      private IContainer _rootContainer;
      private IContainer _parameterContainer;
      private IEditContainerInSimulationPresenter _;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _parameter = A.Fake<IParameter>();
         _rootContainer = A.Fake<IContainer>();
         //ensures that the parameter belongs to the simulation
         _simulation.Model.Root = _rootContainer;
         A.CallTo(() => _parameter.RootContainer).Returns(_rootContainer);
         sut.Edit(_simulation);
         _parameterContainer = A.Fake<IContainer>();
         _parameter.ParentContainer = _parameterContainer;
         _ = A.Fake<IEditContainerInSimulationPresenter>();
         A.CallTo(() => _presenterFactory.PresenterFor(_parameterContainer)).Returns(_);
      }

      protected override void Because()
      {
         sut.Handle(new EntitySelectedEvent(_parameter, new object()));
      }

      [Observation]
      public void should_retrieve_a_parameter_presenter_for_the_container_wheree_the_parameter_is_defined_and_edit_that_presenter()
      {
         A.CallTo(() => _.Edit(_parameterContainer)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_view_of_the_parameter_container__presenter_as_edited_view()
      {
         A.CallTo(() => _view.SetEditView(_.BaseView)).MustHaveHappened();
      }

      [Observation]
      public void should_set_the_edited_simulation_as_simulation_in_the_parameter_container_presenter()
      {
         _.Simulation.ShouldBeEqualTo(_simulation);
      }

      [Observation]
      public void should_select_the_parameter_in_the_view()
      {
         A.CallTo(() => _.SelectParameter(_parameter)).MustHaveHappened();
      }
   }
}