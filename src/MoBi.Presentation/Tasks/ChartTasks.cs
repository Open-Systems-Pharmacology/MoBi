using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Tasks
{
   public interface IChartTasks
   {
      void ShowChart(CurveChart chart, IReadOnlyList<DataRepository> data);
      void ShowChart(CurveChart chart);
      void ShowData(IReadOnlyList<DataRepository> data);
      void ShowData(DataRepository dataRepository);
      void Remove(CurveChart initializer);
      void ExportToPDF(CurveChart chart);

      /// <summary>
      ///    Removes the <paramref name="charts" /> from the project
      /// </summary>
      void RemoveMultipleSummaryCharts(IReadOnlyList<CurveChart> charts);

      void SetOriginText(string simulationName, CurveChart chart);
   }

   public class ChartTasks : IChartTasks
   {
      private readonly IMoBiContext _context;
      private readonly IEventPublisher _eventPublisher;
      private readonly IMoBiApplicationController _applicationController;
      private readonly ExportChartToPDFCommand _exportChartToPDFCommand;
      private readonly IChartFactory _chartFactory;
      private readonly IDialogCreator _dialogCreator;
      private readonly IMoBiProjectRetriever _projectRetriever;

      public ChartTasks(IMoBiContext context, IEventPublisher eventPublisher, IMoBiApplicationController applicationController, ExportChartToPDFCommand exportChartToPDFCommand,
         IChartFactory chartFactory, IDialogCreator dialogCreator, IMoBiProjectRetriever projectRetriever)
      {
         _context = context;
         _eventPublisher = eventPublisher;
         _applicationController = applicationController;
         _exportChartToPDFCommand = exportChartToPDFCommand;
         _chartFactory = chartFactory;
         _dialogCreator = dialogCreator;
         _projectRetriever = projectRetriever;
      }

      public void ShowData(IReadOnlyList<DataRepository> data)
      {
         var chart = _chartFactory.Create<CurveChart>().WithAxes();
         chart.Id = ShortGuid.NewGuid();
         chart.Name = getChartName(data);
         addChartToProject(chart);
         ShowChart(chart, data);
      }

      private void addChartToProject(CurveChart chart)
      {
         _context.CurrentProject.AddChart(chart);
         _context.ProjectChanged();
         _eventPublisher.PublishEvent(new ChartAddedEvent(chart));
      }

      public void ShowData(DataRepository dataRepository)
      {
         dataRepository.SetPersistable(true);
         ShowData(new List<DataRepository> {dataRepository});
      }

      public void RemoveMultipleSummaryCharts(IReadOnlyList<CurveChart> charts)
      {
         if (_dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveSelectedResultsFromProject) != ViewResult.Yes)
            return;
         charts.Each(remove);
      }

      public void SetOriginText(string simulationName, CurveChart chart)
      {
         chart.SetOriginTextFor(_projectRetriever.CurrentProject.Name, simulationName);
      }

      public void Remove(CurveChart chart)
      {
         if (_dialogCreator.MessageBoxYesNo(AppConstants.Dialog.RemoveSelectedResultsFromProject) != ViewResult.Yes)
            return;
         remove(chart);
      }

      private void remove(CurveChart chart)
      {
         _applicationController.Close(chart);
         _context.CurrentProject.RemoveChart(chart);
         _eventPublisher.PublishEvent(new ChartDeletedEvent(chart));
      }

      public void ShowChart(CurveChart chart, IReadOnlyList<DataRepository> data)
      {
         var presenter = _applicationController.Open<IProjectChartPresenter, CurveChart>(chart, _context.HistoryManager);
         presenter.Show(chart, data);
      }

      public void ShowChart(CurveChart chart)
      {
         var data = new List<DataRepository>();
         foreach (var curve in chart.Curves)
         {
            data.AddUnique(curve.xData.Repository);
            data.AddUnique(curve.yData.Repository);
         }
         ShowChart(chart, data);
      }

      public void ExportToPDF(CurveChart chart)
      {
         _exportChartToPDFCommand.Subject = chart;
         _exportChartToPDFCommand.Execute();
      }

      private string getChartName(IEnumerable<DataRepository> data)
      {
         return data.First().Name;
      }
   }
}