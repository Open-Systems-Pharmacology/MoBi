using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   internal class OpenProjectCommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public OpenProjectCommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.Open();
      }
   }
}