using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   internal class CloseProjectCommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public CloseProjectCommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.CloseProject();
      }
   }
}