using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class CreateSimulationReportUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _editTasksesForSimulation;

      public CreateSimulationReportUICommand(IEditTasksForSimulation editTasksForSimulation)
      {
         _editTasksesForSimulation = editTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _editTasksesForSimulation.CreateReport(Subject);
      }
   }
}