using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.ModelDiagram;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Views;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation
{
   public abstract class concern_for_EditSimulationPresenter : ContextSpecification<IEditSimulationPresenter>
   {
      protected IEditSimulationView _view;
      protected ISimulationChartPresenter _chartPresenter;
      protected IHierarchicalSimulationPresenter _hirarchicPresenter;
      protected ISimulationDiagramPresenter _diagramPresenter;
      protected IEditSolverSettingsPresenter _solverSettings;
      protected IEditOutputSchemaPresenter _outputSchemaPresenter;
      protected IEditInSimulationPresenterFactory _presenterFactory;
      protected IEditFavoritesInSimulationPresenter _editFavoritePresenter;
      protected IChartTasks _chartTasks;

      protected override void Context()
      {
         _view = A.Fake<IEditSimulationView>();
         _chartPresenter = A.Fake<ISimulationChartPresenter>();
         _hirarchicPresenter = A.Fake<IHierarchicalSimulationPresenter>();
         _diagramPresenter = A.Fake<ISimulationDiagramPresenter>();
         _solverSettings = A.Fake<IEditSolverSettingsPresenter>();
         _outputSchemaPresenter = A.Fake<IEditOutputSchemaPresenter>();
         _presenterFactory = A.Fake<IEditInSimulationPresenterFactory>();
         _editFavoritePresenter = A.Fake<IEditFavoritesInSimulationPresenter>();
         _chartTasks = A.Fake<IChartTasks>();
         sut = new EditSimulationPresenter(_view, _chartPresenter, _hirarchicPresenter, _diagramPresenter,
            _solverSettings, _outputSchemaPresenter, _presenterFactory, new HeavyWorkManagerForSpecs(),
            A.Fake<IChartFactory>(), _editFavoritePresenter, _chartTasks);
      }
   }

   class When_handling_a_favorite_selected_event : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private IView _favoritesView;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
         _favoritesView = A.Fake<IEditFavoritesView>();
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

   public class When_handling_a_simulation_completed_event : concern_for_EditSimulationPresenter
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

   internal class handling_a_reload_edit_simulation_event : concern_for_EditSimulationPresenter
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
         A.CallTo(() => _hirarchicPresenter.Clear()).MustHaveHappened();
      }

      [Observation]
      public void should_initialise_hirarchical_presenter()
      {
         // Needs to be at least twice because first time it's calles during set up.
         A.CallTo(() => _hirarchicPresenter.Edit(_simulation)).MustHaveHappened(Repeated.AtLeast.Twice);
      }
   }

   internal class edit_simulation_presenter_handels_a_edit_simulation_run_finished_command :
      concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private SimulationRunFinishedEvent _event;
      private CurveChart _chart;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
         sut.Edit(_simulation);
         _chart = A.Fake<CurveChart>();
         A.CallTo(() => _simulation.Chart).Returns(_chart);
         A.CallTo(() => _view.ShowsResults).Returns(true);
         _event = new SimulationRunFinishedEvent(_simulation);
      }

      protected override void Because()
      {
         sut.Handle(_event);
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

   internal class edit_simulation_is_told_to_show_diagram : concern_for_EditSimulationPresenter
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

   internal class told_to_show_a_edit_simulation : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<IMoBiSimulation>();
      }

      protected override void Because()
      {
         sut.Edit(_simulation);
      }

      [Observation]
      public void should_initialise_hirarchical_presenter()
      {
         A.CallTo(() => _hirarchicPresenter.Edit(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_not_display_result_data()
      {
         A.CallTo(() => _chartPresenter.Show(A<CurveChart>._, A<IReadOnlyList<DataRepository>>._, null))
            .MustNotHaveHappened();
      }

      [Observation]
      public void should_not_display_diagram()
      {
         A.CallTo(() => _diagramPresenter.Edit(_simulation)).MustNotHaveHappened();
      }
   }

   internal class told_edit_simulation_to_show_data : concern_for_EditSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private DataRepository _observedDataRepository;
      private IEnumerable<DataRepository> _data;

      private Curve createObservedCurve(DataRepository observedDataRepository)
      {
         var dimensionFactory = A.Fake<IDimensionFactory>();
         var curve = new Curve
         {
            Name = "OBS"
         };
         var baseGrid = new BaseGrid("baseGrid", HelperForSpecs.TimeDimension)
         {
            DataInfo = new DataInfo(ColumnOrigins.BaseGrid),
            Repository = observedDataRepository
         };

         var ydata = new DataColumn("ydata", HelperForSpecs.ConcentrationDimension, baseGrid)
         {
            DataInfo = new DataInfo(ColumnOrigins.Observation),
            Repository = observedDataRepository
         };

         curve.SetxData(baseGrid, dimensionFactory);
         curve.SetyData(ydata, dimensionFactory);
         return curve;
      }

      protected override void Context()
      {
         base.Context();
         _simulation = new MoBiSimulation {Results = new DataRepository()};

         var chart = new CurveChart();

         _observedDataRepository = new DataRepository();
         chart.AddCurve(createObservedCurve(_observedDataRepository));

         _simulation.Chart = chart;

         A.CallTo(() => _chartPresenter.Show(chart, A<IReadOnlyList<DataRepository>>._, null))
            .Invokes(x => _data = x.GetArgument<IEnumerable<DataRepository>>(1));

         sut.Edit(_simulation);
      }

      protected override void Because()
      {
         sut.ShowData();
      }

      [Observation]
      public void should_Add_Observer_Data_repository()
      {
         _data.ShouldContain(_observedDataRepository);
      }
   }
}