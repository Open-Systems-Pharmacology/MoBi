using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportSimulationToCppUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _editTasksForSimulation;

      public ExportSimulationToCppUICommand(IEditTasksForSimulation editTasksForSimulation)
      {
         _editTasksForSimulation = editTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _editTasksForSimulation.ExportSimulationForCpp(Subject);
      }
   }
}