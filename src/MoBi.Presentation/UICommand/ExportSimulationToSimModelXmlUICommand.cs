using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportSimulationToSimModelXmlUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IEditTasksForSimulation _editTasksForSimulation;

      public ExportSimulationToSimModelXmlUICommand(IEditTasksForSimulation editTasksForSimulation)
      {
         _editTasksForSimulation = editTasksForSimulation;
      }

      protected override void PerformExecute()
      {
         _editTasksForSimulation.ExportSimModelXml(Subject);
      }
   }
}