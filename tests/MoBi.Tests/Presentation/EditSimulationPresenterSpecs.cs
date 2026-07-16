using FakeItEasy;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditSimulationPresenter : ContextSpecification<EditSimulationPresenter>
   {
      protected IEditSimulationView _view;
      protected IHierarchicalSimulationPresenter _hierarchicalSimulationPresenter;
      protected ISimulationDiagramPresenter _diagramPresenter;
      protected IEditSolverSettingsPresenter _solverSettings;
      protected IEditOutputSchemaPresenter _outputSchemaPresenter;
      protected IEditInSimulationPresenterFactory _presenterFactory;
      protected IEditFavoritesInSimulationPresenter _editFavoritePresenter;
      protected IUserDefinedParametersPresenter _userDefinedParametersPresenter;
      protected ISimulationOutputMappingPresenter _simulationOutputMappingPresenter;
      protected IMoBiContext _context;
      protected IOutputMappingMatchingTask _outputMappingMatchingTask;
      protected ISimulationEntitySourceReferenceFactory _entitySourceReferenceFactory;
      protected ISimulationRunner _simulationRunner;
      protected ISimulationAnalysisPresenterFactory _simulationAnalysisPresenterFactory;
      protected IEventPublisher _eventPublisher;
      private ISimulationChangesPresenter _simulationChangesPresenter;

      protected override void Context()
      {
         _view = A.Fake<IEditSimulationView>();
         _hierarchicalSimulationPresenter = A.Fake<IHierarchicalSimulationPresenter>();
         _diagramPresenter = A.Fake<ISimulationDiagramPresenter>();
         _solverSettings = A.Fake<IEditSolverSettingsPresenter>();
         _outputSchemaPresenter = A.Fake<IEditOutputSchemaPresenter>();
         _presenterFactory = A.Fake<IEditInSimulationPresenterFactory>();
         _editFavoritePresenter = A.Fake<IEditFavoritesInSimulationPresenter>();
         _simulationOutputMappingPresenter = A.Fake<ISimulationOutputMappingPresenter>();
         _userDefinedParametersPresenter = A.Fake<IUserDefinedParametersPresenter>();
         _context = A.Fake<IMoBiContext>();
         _simulationChangesPresenter = A.Fake<ISimulationChangesPresenter>();
         _outputMappingMatchingTask = A.Fake<IOutputMappingMatchingTask>();
         _entitySourceReferenceFactory = A.Fake<ISimulationEntitySourceReferenceFactory>();
         _simulationRunner = A.Fake<ISimulationRunner>();
         _simulationAnalysisPresenterFactory = A.Fake<ISimulationAnalysisPresenterFactory>();
         _eventPublisher = A.Fake<IEventPublisher>();

         sut = new EditSimulationPresenter(_view, _hierarchicalSimulationPresenter, _diagramPresenter,
            _solverSettings, _outputSchemaPresenter, _presenterFactory, new HeavyWorkManagerForSpecs(),
            _editFavoritePresenter, _userDefinedParametersPresenter, _simulationOutputMappingPresenter,
            _context, _outputMappingMatchingTask, _simulationChangesPresenter, _entitySourceReferenceFactory,
            _simulationRunner, _simulationAnalysisPresenterFactory, _eventPublisher);
      }
   }

   public class When_editing_a_simulation_in_the_simulation_presenter : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation();
         _simulation.Configuration = new SimulationConfiguration { SimulationSettings = new SimulationSettings() };
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void the_favorites_presenter_has_tracking_enabled()
      {
         _editFavoritePresenter.TrackableSimulation.Simulation.ShouldBeEqualTo(_simulation);
      }
   }

   public class When_editing_a_simulation_with_existing_analyses : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private MoBiSimulationTimeProfileChart _timeProfileChart;
      private SimulationPredictedVsObservedChart _predictedVsObservedChart;
      private ISimulationAnalysisPresenter _timeProfilePresenter;
      private ISimulationAnalysisPresenter _predictedVsObservedPresenter;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _timeProfileChart = new MoBiSimulationTimeProfileChart();
         _predictedVsObservedChart = new SimulationPredictedVsObservedChart();
         A.CallTo(() => _simulation.Analyses).Returns(new ISimulationAnalysis[] { _timeProfileChart, _predictedVsObservedChart });

         _timeProfilePresenter = A.Fake<ISimulationAnalysisPresenter>();
         _predictedVsObservedPresenter = A.Fake<ISimulationAnalysisPresenter>();
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_timeProfileChart)).Returns(_timeProfilePresenter);
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_predictedVsObservedChart)).Returns(_predictedVsObservedPresenter);
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void should_create_a_presenter_for_each_analysis()
      {
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_timeProfileChart)).MustHaveHappened();
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_predictedVsObservedChart)).MustHaveHappened();
      }

      [Observation]
      public void should_initialize_each_analysis_presenter()
      {
         A.CallTo(() => _timeProfilePresenter.InitializeAnalysis(_timeProfileChart, _simulation)).MustHaveHappened();
         A.CallTo(() => _predictedVsObservedPresenter.InitializeAnalysis(_predictedVsObservedChart, _simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_add_each_analysis_to_the_view()
      {
         A.CallTo(() => _view.AddAnalysis(_timeProfilePresenter)).MustHaveHappened();
         A.CallTo(() => _view.AddAnalysis(_predictedVsObservedPresenter)).MustHaveHappened();
      }
   }

   public class When_editing_a_simulation_with_no_analyses : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.Analyses).Returns(new ISimulationAnalysis[] { });
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void should_not_add_any_analysis_to_the_view()
      {
         A.CallTo(() => _view.AddAnalysis(A<ISimulationAnalysisPresenter>._)).MustNotHaveHappened();
      }
   }

   public class When_handling_a_simulation_analysis_created_event_for_the_edited_simulation : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private MoBiSimulationTimeProfileChart _chart;
      private ISimulationAnalysisPresenter _analysisPresenter;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.Analyses).Returns(new ISimulationAnalysis[] { });
         sut.Edit(_simulation);

         _chart = new MoBiSimulationTimeProfileChart();
         _analysisPresenter = A.Fake<ISimulationAnalysisPresenter>();
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_chart)).Returns(_analysisPresenter);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationAnalysisCreatedEvent(_simulation, _chart));
      }

      [Observation]
      public void should_create_a_presenter_for_the_analysis()
      {
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_chart)).MustHaveHappened();
      }

      [Observation]
      public void should_initialize_the_presenter()
      {
         A.CallTo(() => _analysisPresenter.InitializeAnalysis(_chart, _simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_add_the_analysis_to_the_view()
      {
         A.CallTo(() => _view.AddAnalysis(_analysisPresenter)).MustHaveHappened();
      }
   }

   public class When_handling_a_simulation_analysis_created_event_for_a_different_simulation : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private IMoBiSimulation _otherSimulation;
      private MoBiSimulationTimeProfileChart _chart;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _otherSimulation = A.Fake<IMoBiSimulation>();
         A.CallTo(() => _simulation.Analyses).Returns(new ISimulationAnalysis[] { });
         sut.Edit(_simulation);
         _chart = new MoBiSimulationTimeProfileChart();
      }

      protected override void Because()
      {
         sut.Handle(new SimulationAnalysisCreatedEvent(_otherSimulation, _chart));
      }

      [Observation]
      public void should_not_add_any_analysis_to_the_view()
      {
         A.CallTo(() => _view.AddAnalysis(A<ISimulationAnalysisPresenter>._)).MustNotHaveHappened();
      }
   }

   public class When_removing_an_analysis : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private MoBiSimulationTimeProfileChart _chart;
      private ISimulationAnalysisPresenter _analysisPresenter;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _chart = new MoBiSimulationTimeProfileChart();
         A.CallTo(() => _simulation.Analyses).Returns(new ISimulationAnalysis[] { _chart });

         _analysisPresenter = A.Fake<ISimulationAnalysisPresenter>();
         A.CallTo(() => _analysisPresenter.Analysis).Returns(_chart);
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_chart)).Returns(_analysisPresenter);

         sut.Edit(_simulation);
      }

      protected override void Because()
      {
         sut.RemoveAnalysis(_analysisPresenter);
      }

      [Observation]
      public void should_remove_the_analysis_from_the_simulation()
      {
         A.CallTo(() => _simulation.RemoveAnalysis(_chart)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_analysis_from_the_view()
      {
         A.CallTo(() => _view.RemoveAnalysis(_analysisPresenter)).MustHaveHappened();
      }

      [Observation]
      public void should_clear_the_presenter()
      {
         A.CallTo(() => _analysisPresenter.Clear()).MustHaveHappened();
      }

      [Observation]
      public void should_release_the_presenter_from_events()
      {
         A.CallTo(() => _analysisPresenter.ReleaseFrom(_eventPublisher)).MustHaveHappened();
      }
   }

   public class When_the_simulation_presenter_is_handling_a_favorite_selected_event : concern_for_EditSimulationPresenter
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

   public class When_the_simulation_presenter_is_handling_a_simulation_completed_event : concern_for_EditSimulationPresenter
   {
      private SimulationRunFinishedEvent _simulationRunFinishedEvent;
      private IMoBiSimulation _simulation;
      private ISimulationAnalysisPresenter _analysisPresenter;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         var chart = new MoBiSimulationTimeProfileChart();
         A.CallTo(() => _simulation.Analyses).Returns(new ISimulationAnalysis[] { chart });
         _analysisPresenter = A.Fake<ISimulationAnalysisPresenter>();
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(chart)).Returns(_analysisPresenter);

         sut.Edit(_simulation);
         _simulationRunFinishedEvent = new SimulationRunFinishedEvent(_simulation, true);
         A.CallTo(() => _view.ShowsResults).Returns(false);
      }

      protected override void Because()
      {
         sut.Handle(_simulationRunFinishedEvent);
      }

      [Observation]
      public void should_update_all_analysis_presenters()
      {
         A.CallTo(() => _analysisPresenter.UpdateAnalysisBasedOn(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void the_view_should_be_changed_to_the_result_view()
      {
         A.CallTo(() => _view.ShowResultsTab()).MustHaveHappened();
      }
   }

   public class When_the_simulation_presenter_is_notified_that_the_edit_simulation_was_reloaded : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private ISimulationAnalysisPresenter _analysisPresenter;
      private MoBiSimulationTimeProfileChart _chart;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         _chart = new MoBiSimulationTimeProfileChart();
         A.CallTo(() => _simulation.Analyses).Returns(new ISimulationAnalysis[] { _chart });
         _analysisPresenter = A.Fake<ISimulationAnalysisPresenter>();
         A.CallTo(() => _simulationAnalysisPresenterFactory.PresenterFor(_chart)).Returns(_analysisPresenter);
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

      [Observation]
      public void should_release_existing_analysis_presenters_before_reloading()
      {
         A.CallTo(() => _analysisPresenter.Clear()).MustHaveHappened();
         A.CallTo(() => _analysisPresenter.ReleaseFrom(_eventPublisher)).MustHaveHappened();
      }
   }

   public class
      When_the_simulation_presenter_is_notified_that_a_simulation_run_is_finished_for_the_edited_simulation_but_failed :
      concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
         A.CallTo(() => _view.ShowsResults).Returns(true);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationRunFinishedEvent(_simulation, false));
      }

      [Observation]
      public void should_not_show_results_tab()
      {
         A.CallTo(() => _view.ShowResultsTab()).MustNotHaveHappened();
      }
   }

   public class
      When_the_simulation_presenter_is_notified_that_a_simulation_run_is_finished_for_the_edited_simulation :
      concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
         A.CallTo(() => _view.ShowsResults).Returns(true);
      }

      protected override void Because()
      {
         sut.Handle(new SimulationRunFinishedEvent(_simulation, true));
      }

      [Observation]
      public void should_ask_view_if_results_are_shown()
      {
         A.CallTo(() => _view.ShowsResults).MustHaveHappened();
      }

      [Observation]
      public void should_not_show_results_tab_when_already_showing()
      {
         A.CallTo(() => _view.ShowResultsTab()).MustNotHaveHappened();
      }
   }

   public class When_the_simulation_presenter_is_loading_the_diagram : concern_for_EditSimulationPresenter
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
         _.TrackableSimulation.Simulation.ShouldBeEqualTo(_simulation);
      }

      [Observation]
      public void should_select_the_parameter_in_the_view()
      {
         A.CallTo(() => _.SelectParameter(_parameter)).MustHaveHappened();
      }
   }

   public class When_handling_simulation_run_started_event : concern_for_EditSimulationPresenter
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
         sut.Handle(new SimulationRunStartedEvent(_simulation));
      }

      [Observation]
      public void should_disable_parameters_tab()
      {
         A.CallTo(() => _view.SetParametersTabEnabled(false)).MustHaveHappened();
      }
   }

   public class When_handling_show_simulation_changes_event : concern_for_EditSimulationPresenter
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
         sut.Handle(new ShowSimulationChangesEvent(_simulation));
      }

      [Observation]
      public void should_show_changes_tab()
      {
         A.CallTo(() => _view.ShowChangesTab()).MustHaveHappened();
      }
   }
}