using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   public class SaveProjectCommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public SaveProjectCommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.Save();
      }
   }
}