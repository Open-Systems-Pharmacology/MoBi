using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class EditSimulationUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _simulationTasks;

      public EditSimulationUICommand(IEditTasksForSimulation simulationTasks)
      {
         _simulationTasks = simulationTasks;
      }

      protected override void PerformExecute()
      {
         _simulationTasks.Edit(Subject);
      }

   }
}

