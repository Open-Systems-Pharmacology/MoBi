using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadDataRepositoryUICommand : IUICommand
   {
      private readonly IObservedDataTask _observedDataTask;

      public LoadDataRepositoryUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      public void Execute()
      {
         _observedDataTask.LoadObservedDataIntoProject();
      }
   }

   public class ImportDataRepositoryUICommand : IUICommand
   {
      private readonly IObservedDataTask _observedDataTask;

      public ImportDataRepositoryUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      public void Execute()
      {
         _observedDataTask.AddObservedDataToProject();
      }
   }

   internal class RemoveSimulationResultCommand : ObjectUICommand<DataRepository>
   {
      private readonly IObservedDataTask _dataTask;
      public IMoBiSimulation Simulation { get; set; }

      public RemoveSimulationResultCommand(IObservedDataTask dataTask)
      {
         _dataTask = dataTask;
      }

      protected override void PerformExecute()
      {
         _dataTask.DeleteResultsFromSimulationCommand(Simulation, Subject);
      }
   }

   internal class RemoveDataRepositoryUICommand : ObjectUICommand<DataRepository>
   {
      private readonly IObservedDataTask _dataTask;

      public RemoveDataRepositoryUICommand(IObservedDataTask dataTask)
      {
         _dataTask = dataTask;
      }

      protected override void PerformExecute()
      {
         _dataTask.Delete(Subject);
      }
   }

   internal class ShowDataRepositoryUICommand : ObjectUICommand<DataRepository>
   {
      private readonly IChartTasks _chartTasks;

      public ShowDataRepositoryUICommand(IChartTasks chartTasks)
      {
         _chartTasks = chartTasks;
      }

      protected override void PerformExecute()
      {
         _chartTasks.ShowData(Subject);
      }
   }

   internal class EditSummaryChartUICommand : ObjectUICommand<CurveChart>
   {
      private readonly IChartTasks _chartTasks;

      public EditSummaryChartUICommand(IChartTasks chartTasks)
      {
         _chartTasks = chartTasks;
      }

      protected override void PerformExecute()
      {
         _chartTasks.ShowChart(Subject);
      }
   }
}