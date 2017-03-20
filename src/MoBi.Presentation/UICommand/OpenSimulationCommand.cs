using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   internal class OpenSimulationCommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public OpenSimulationCommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.OpenSimulationAsProject();
      }
   }
}