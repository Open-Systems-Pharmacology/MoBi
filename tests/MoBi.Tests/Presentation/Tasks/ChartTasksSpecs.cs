using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using FakeItEasy;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;

namespace MoBi.Presentation.Tasks
{
   public abstract class concern_for_ChartTasks : ContextSpecification<ChartTasks>
   {
      protected IMoBiContext _moBiContext;
      protected IEventPublisher _eventPublisher;
      protected IMoBiApplicationController _moBiApplicationController;
      protected ExportChartToPDFCommand _exportChartToPDFCommand;
      protected IChartFactory _chartFactory;
      protected IDialogCreator _dialogCreator;
      protected IMoBiProject _currentProject;
      private IMoBiProjectRetriever _projectRetriever;

      protected override void Context()
      {
         _moBiContext = A.Fake<IMoBiContext>();
         _eventPublisher = A.Fake<IEventPublisher>();
         _moBiApplicationController = A.Fake<IMoBiApplicationController>();
         _exportChartToPDFCommand = A.Fake<ExportChartToPDFCommand>();
         _chartFactory = A.Fake<IChartFactory>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _currentProject = A.Fake<IMoBiProject>();
         _projectRetriever = A.Fake<IMoBiProjectRetriever>();

         sut = new ChartTasks(_moBiContext, _eventPublisher, _moBiApplicationController, _exportChartToPDFCommand,
            _chartFactory, _dialogCreator, _projectRetriever);

         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_currentProject);
         A.CallTo(() => _moBiContext.CurrentProject).Returns(_currentProject);
      }
   }

   public class When_setting_origin_text_on_a_chart : concern_for_ChartTasks
   {
      private ICurveChart _chart;

      protected override void Context()
      {
         base.Context();
         _chart = new CurveChart();
         _currentProject.Name = "projectName";
      }

      protected override void Because()
      {
         sut.SetOriginText("simulationName", _chart);
      }

      [Observation]
      public void chart_origin_text_must_include_the_project_name()
      {
         _chart.OriginText.Contains(_currentProject.Name).ShouldBeTrue();
      }

      [Observation]
      public void chart_origin_text_must_include_the_simulation_name()
      {
         _chart.OriginText.Contains("simulationName").ShouldBeTrue();
      }
   }

   public class When_deleting_charts_and_the_user_decides_to_go_ahead_with_delete : concern_for_ChartTasks
   {
      private IReadOnlyList<ICurveChart> _charts;

      protected override void Context()
      {
         base.Context();
         _charts = new List<ICurveChart> { new CurveChart() };
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
      }

      protected override void Because()
      {
         sut.RemoveMultipleSummaryCharts(_charts);
      }

      [Observation]
      public void should_remove_the_charts()
      {
         A.CallTo(() => _currentProject.RemoveChart(A<ICurveChart>.Ignored)).MustHaveHappened();
      }
   }

   public class When_deleting_multiple_charts_and_the_user_decides_not_to_go_ahead_with_delete : concern_for_ChartTasks
   {
      private IReadOnlyList<ICurveChart> _charts;

      protected override void Context()
      {
         base.Context();
         _charts = new List<ICurveChart>{new CurveChart()};
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
      }

      protected override void Because()
      {
         sut.RemoveMultipleSummaryCharts(_charts);
      }

      [Observation]
      public void should_not_remove_the_charts()
      {
         A.CallTo(() => _currentProject.RemoveChart(A<ICurveChart>.Ignored)).MustNotHaveHappened();
      }
   }

   public class When_showing_historical_results_as_chart : concern_for_ChartTasks
   {
      private DataRepository _historialResult;

      protected override void Context()
      {
         base.Context();
         _historialResult = new DataRepository();
      }

      protected override void Because()
      {
         sut.ShowData(_historialResult);
      }

      [Observation]
      public void should_automatically_set_the_persistable_flag_to_true()
      {
         _historialResult.IsPersistable().ShouldBeTrue();
      }
   }

}
