using FakeItEasy;
using MoBi.Core.Chart;
using MoBi.Presentation.Presenter;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Presentation.Presenters;
using IContainer = OSPSuite.Utility.Container.IContainer;
using SimulationAnalysisPresenterFactory = MoBi.Presentation.Presenter.SimulationAnalysisPresenterFactory;

namespace MoBi.Presentation;

public abstract class concern_for_SimulationAnalysisPresenterFactory : ContextSpecification<SimulationAnalysisPresenterFactory>
{
   protected IContainer _container;
   protected ISimulationAnalysisPresenter _result;

   protected override void Context()
   {
      _container = A.Fake<IContainer>();
      sut = new SimulationAnalysisPresenterFactory(_container);
   }
}

public class When_creating_a_presenter_for_a_time_profile_chart : concern_for_SimulationAnalysisPresenterFactory
{
   private ISimulationChartPresenter _timeProfilePresenter;

   protected override void Context()
   {
      base.Context();
      _timeProfilePresenter = A.Fake<ISimulationChartPresenter>();
      A.CallTo(() => _container.Resolve<ISimulationChartPresenter>()).Returns(_timeProfilePresenter);
   }

   protected override void Because()
   {
      _result = sut.PresenterFor(new MoBiSimulationTimeProfileChart());
   }

   [Observation]
   public void should_return_a_simulation_chart_presenter()
   {
      _result.ShouldBeEqualTo(_timeProfilePresenter);
   }
}

public class When_creating_a_presenter_for_a_predicted_vs_observed_chart : concern_for_SimulationAnalysisPresenterFactory
{
   private ISimulationPredictedVsObservedChartPresenter _predictedVsObservedPresenter;

   protected override void Context()
   {
      base.Context();
      _predictedVsObservedPresenter = A.Fake<ISimulationPredictedVsObservedChartPresenter>();
      A.CallTo(() => _container.Resolve<ISimulationPredictedVsObservedChartPresenter>()).Returns(_predictedVsObservedPresenter);
   }

   protected override void Because()
   {
      _result = sut.PresenterFor(new SimulationPredictedVsObservedChart());
   }

   [Observation]
   public void should_return_a_predicted_vs_observed_chart_presenter()
   {
      _result.ShouldBeEqualTo(_predictedVsObservedPresenter);
   }
}

public class When_creating_a_presenter_for_a_residuals_vs_time_chart : concern_for_SimulationAnalysisPresenterFactory
{
   private ISimulationResidualVsTimeChartPresenter _residualsVsTimePresenter;

   protected override void Context()
   {
      base.Context();
      _residualsVsTimePresenter = A.Fake<ISimulationResidualVsTimeChartPresenter>();
      A.CallTo(() => _container.Resolve<ISimulationResidualVsTimeChartPresenter>()).Returns(_residualsVsTimePresenter);
   }

   protected override void Because()
   {
      _result = sut.PresenterFor(new SimulationResidualVsTimeChart());
   }

   [Observation]
   public void should_return_a_residuals_vs_time_chart_presenter()
   {
      _result.ShouldBeEqualTo(_residualsVsTimePresenter);
   }
}
