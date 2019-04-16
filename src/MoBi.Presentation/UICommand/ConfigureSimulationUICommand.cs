using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ConfigureSimulationUICommand : ActiveObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _simulationTask;

      public ConfigureSimulationUICommand(IEditTasksForSimulation simulationTask, IActiveSubjectRetriever activeSubjectRetriever):base(activeSubjectRetriever)
      {
         _simulationTask = simulationTask;
      }

      protected override void PerformExecute()
      {
         _simulationTask.Configure(Subject);
      }
   }
}