using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RenameDataRepositoryUICommand : ObjectUICommand<DataRepository>
   {
      private readonly IObservedDataTask _observedDataTask;

      public RenameDataRepositoryUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.Rename(Subject);
      }
   }

   public class RenameSimulationResultsUICommand :  ObjectUICommand<DataRepository>
   {
      private readonly IEditTasksForSimulation _simulationTasks;

      public IMoBiSimulation Simulation { get;  set; }

      public RenameSimulationResultsUICommand(IEditTasksForSimulation simulationTasks)
      {
         _simulationTasks = simulationTasks;
      }

      protected override void PerformExecute()
      {
         _simulationTasks.RenameResults(Simulation, Subject);
      }
   }
}