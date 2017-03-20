using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   public class SaveProjectAsCommand : IUICommand
   {
      private readonly IProjectTask _projectTask;

      public SaveProjectAsCommand(IProjectTask projectTask)
      {
         _projectTask = projectTask;
      }

      public void Execute()
      {
         _projectTask.SaveAs();
      }
   }
}