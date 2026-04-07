using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Chart;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Events;

namespace MoBi.Core.Service
{
   public abstract class concern_for_MoBiSimulationAnalysisCreator : ContextSpecification<MoBiSimulationAnalysisCreator>
   {
      protected IContainerTask _containerTask;
      protected IOSPSuiteExecutionContext _executionContext;
      protected IChartFactory _chartFactory;
      protected ICloneManager _cloneManager;
      protected IMoBiSimulation _simulation;

      protected override void Context()
      {
         _containerTask = A.Fake<IContainerTask>();
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _chartFactory = A.Fake<IChartFactory>();
         _cloneManager = A.Fake<ICloneManager>();
         _simulation = new MoBiSimulation();

         A.CallTo(() => _containerTask.CreateUniqueName(A<IEnumerable<IWithName>>._, A<string>._, A<bool>._))
            .ReturnsLazily((IEnumerable<IWithName> _, string name, bool __) => name);

         sut = new MoBiSimulationAnalysisCreator(_containerTask, _executionContext, _chartFactory, _cloneManager);
      }
   }

   public class When_creating_a_time_profile_analysis : concern_for_MoBiSimulationAnalysisCreator
   {
      private ISimulationAnalysis _result;
      private MoBiSimulationTimeProfileChart _chart;

      protected override void Context()
      {
         base.Context();
         _chart = new MoBiSimulationTimeProfileChart();
         A.CallTo(() => _chartFactory.Create<MoBiSimulationTimeProfileChart>()).Returns(_chart);
      }

      protected override void Because()
      {
         _result = sut.CreateTimeProfileAnalysisFor(_simulation);
      }

      [Observation]
      public void should_return_a_time_profile_chart()
      {
         _result.ShouldBeEqualTo(_chart);
      }

      [Observation]
      public void should_add_the_chart_to_the_simulation()
      {
         _simulation.Analyses.ShouldContain(_chart);
      }

      [Observation]
      public void should_publish_a_simulation_analysis_created_event()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<SimulationAnalysisCreatedEvent>.That.Matches(
            e => e.Analysable == _simulation && e.SimulationAnalysis == _chart))).MustHaveHappened();
      }
   }

   public class When_creating_a_predicted_vs_observed_analysis : concern_for_MoBiSimulationAnalysisCreator
   {
      private ISimulationAnalysis _result;
      private SimulationPredictedVsObservedChart _chart;

      protected override void Context()
      {
         base.Context();
         _chart = new SimulationPredictedVsObservedChart();
         A.CallTo(() => _chartFactory.Create<SimulationPredictedVsObservedChart>()).Returns(_chart);
      }

      protected override void Because()
      {
         _result = sut.CreatePredictedVsObservedAnalysisFor(_simulation);
      }

      [Observation]
      public void should_return_a_predicted_vs_observed_chart()
      {
         _result.ShouldBeEqualTo(_chart);
      }

      [Observation]
      public void should_add_the_chart_to_the_simulation()
      {
         _simulation.Analyses.ShouldContain(_chart);
      }

      [Observation]
      public void should_publish_a_simulation_analysis_created_event()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<SimulationAnalysisCreatedEvent>.That.Matches(
            e => e.Analysable == _simulation && e.SimulationAnalysis == _chart))).MustHaveHappened();
      }
   }

   public class When_creating_a_residuals_vs_time_analysis : concern_for_MoBiSimulationAnalysisCreator
   {
      private ISimulationAnalysis _result;
      private SimulationResidualVsTimeChart _chart;

      protected override void Context()
      {
         base.Context();
         _chart = new SimulationResidualVsTimeChart();
         A.CallTo(() => _chartFactory.Create<SimulationResidualVsTimeChart>()).Returns(_chart);
      }

      protected override void Because()
      {
         _result = sut.CreateResidualsVsTimeAnalysisFor(_simulation);
      }

      [Observation]
      public void should_return_a_residuals_vs_time_chart()
      {
         _result.ShouldBeEqualTo(_chart);
      }

      [Observation]
      public void should_add_the_chart_to_the_simulation()
      {
         _simulation.Analyses.ShouldContain(_chart);
      }

      [Observation]
      public void should_publish_a_simulation_analysis_created_event()
      {
         A.CallTo(() => _executionContext.PublishEvent(A<SimulationAnalysisCreatedEvent>.That.Matches(
            e => e.Analysable == _simulation && e.SimulationAnalysis == _chart))).MustHaveHappened();
      }
   }

   public class When_creating_an_analysis_based_on_a_curve_chart : concern_for_MoBiSimulationAnalysisCreator
   {
      private ISimulationAnalysis _result;
      private MoBiSimulationTimeProfileChart _sourceChart;
      private MoBiSimulationTimeProfileChart _clonedChart;

      protected override void Context()
      {
         base.Context();
         _sourceChart = new MoBiSimulationTimeProfileChart();
         _clonedChart = new MoBiSimulationTimeProfileChart();
         A.CallTo(() => _cloneManager.Clone(A<IUpdatable>.That.IsEqualTo(_sourceChart))).Returns(_clonedChart);
      }

      protected override void Because()
      {
         _result = sut.CreateAnalysisBasedOn(_sourceChart);
      }

      [Observation]
      public void should_return_a_clone_of_the_source_chart()
      {
         _result.ShouldBeEqualTo(_clonedChart);
      }
   }

   public class When_creating_an_analysis_based_on_a_non_chart_analysis : concern_for_MoBiSimulationAnalysisCreator
   {
      private ISimulationAnalysis _result;
      private ISimulationAnalysis _sourceAnalysis;

      protected override void Context()
      {
         base.Context();
         _sourceAnalysis = A.Fake<ISimulationAnalysis>();
      }

      protected override void Because()
      {
         _result = sut.CreateAnalysisBasedOn(_sourceAnalysis);
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }
}
