using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class ExportMatlabODEUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _editTasksesForSimulation;

      public ExportMatlabODEUICommand(IEditTasksForSimulation editTasksForSimulation)
      {
         _editTasksesForSimulation = editTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _editTasksesForSimulation.ExportMatlabDifferentialSystem(Subject);
      }
   }
}