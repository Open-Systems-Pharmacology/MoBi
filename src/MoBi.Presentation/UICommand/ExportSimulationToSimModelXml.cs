using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportSimulationToSimModelXml : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _editTasksesForSimulation;

      public ExportSimulationToSimModelXml(IEditTasksForSimulation editTasksForSimulation)
      {
         _editTasksesForSimulation = editTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _editTasksesForSimulation.ExportSimModelXml(Subject);
      }

   }
}