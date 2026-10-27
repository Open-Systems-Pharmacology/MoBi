using System.Collections.Generic;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.UICommand;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;

namespace MoBi.Presentation
{
   public abstract class concern_for_RenameSummaryChartUICommand : ContextSpecification<RenameSummaryChartUICommand>
   {
      protected IOSPSuiteExecutionContext _executionContext;
      protected IEventPublisher _eventPublisher;
      protected IApplicationController _applicationController;
      protected IMoBiProjectRetriever _projectRetriever;
      protected IRenameObjectPresenter _renameObjectPresenter;
      protected MoBiProject _project;
      protected CurveChart _chart;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _applicationController = A.Fake<IApplicationController>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();
         _renameObjectPresenter = A.Fake<IRenameObjectPresenter>();

         _chart = new CurveChart {Name = "Comparison"};
         _project = new MoBiProject();
         _project.AddChart(_chart);
         _project.AddChart(new CurveChart {Name = "Other comparison"});

         A.CallTo(() => _projectRetriever.Current).Returns(_project);
         A.CallTo(() => _applicationController.Start<IRenameObjectPresenter>()).Returns(_renameObjectPresenter);

         sut = new RenameSummaryChartUICommand(_executionContext, _eventPublisher, _applicationController, _projectRetriever);
         sut.For(_chart);
      }

      protected override void Because()
      {
         sut.Execute();
      }
   }

   public class When_renaming_a_results_comparison_chart : concern_for_RenameSummaryChartUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_chart, A<IEnumerable<string>>._, A<string>._)).Returns("New name");
      }

      [Observation]
      public void the_names_of_the_charts_in_the_project_should_be_forbidden()
      {
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_chart, A<IEnumerable<string>>.That.IsSameSequenceAs(new[] {"Comparison", "Other comparison"}), A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_chart_should_have_the_new_name()
      {
         _chart.Name.ShouldBeEqualTo("New name");
      }

      [Observation]
      public void a_renamed_event_should_be_published_for_the_chart()
      {
         A.CallTo(() => _eventPublisher.PublishEvent(A<RenamedEvent>.That.Matches(x => x.RenamedObject == _chart))).MustHaveHappened();
      }

      [Observation]
      public void the_project_should_be_marked_as_changed()
      {
         A.CallTo(() => _executionContext.ProjectChanged()).MustHaveHappened();
      }
   }

   public class When_the_user_cancels_renaming_a_results_comparison_chart : concern_for_RenameSummaryChartUICommand
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _renameObjectPresenter.NewNameFrom(_chart, A<IEnumerable<string>>._, A<string>._)).Returns(string.Empty);
      }

      [Observation]
      public void the_chart_name_should_not_change()
      {
         _chart.Name.ShouldBeEqualTo("Comparison");
      }

      [Observation]
      public void the_project_should_not_be_marked_as_changed()
      {
         A.CallTo(() => _executionContext.ProjectChanged()).MustNotHaveHappened();
      }
   }
}
