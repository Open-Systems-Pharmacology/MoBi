using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   internal class LoadProjectUICommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public LoadProjectUICommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.LoadSimulationIntoProject();
      }
   }
}