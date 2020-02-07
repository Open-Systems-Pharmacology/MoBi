using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
  internal class ExportODEForRUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _editTasksForSimulation;

      public ExportODEForRUICommand(IEditTasksForSimulation editTasksForSimulation)
      {
         _editTasksForSimulation = editTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _editTasksForSimulation.ExportODEForR(Subject);
      }
   }
}