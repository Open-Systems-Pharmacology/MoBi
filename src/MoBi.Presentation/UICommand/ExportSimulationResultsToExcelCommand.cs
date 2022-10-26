using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportSimulationResultsToExcelCommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _simulationTasks;
      public DataRepository DataRepository { get; set; }

      public ExportSimulationResultsToExcelCommand InitializeWith(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         Subject = simulation;
         DataRepository = dataRepository;
         return this;
      }

      public ExportSimulationResultsToExcelCommand(IEditTasksForSimulation simulationTasks)
      {
         _simulationTasks = simulationTasks;
      }

      protected override void PerformExecute()
      {
         _simulationTasks.ExportResultsToExcel(Subject, DataRepository ?? Subject.ResultsDataRepository);
      }
   }
}