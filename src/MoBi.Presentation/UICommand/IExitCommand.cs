using System.Windows.Forms;
using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core.Serialization.Xml;
using MoBi.Presentation.Tasks;

namespace MoBi.Presentation.UICommand
{
   public interface IExitCommand : IUICommand
   {
      bool Canceled { get; }
   }

   public class ExitCommand : IExitCommand
   {
      private readonly IProjectTask _projectTask;
      private readonly IUserSettingsPersistor _userSettingsPersistor;
      public bool Canceled { get; private set; }

      public ExitCommand(IProjectTask projectTask, IUserSettingsPersistor userSettingsPersistor)
      {
         _projectTask = projectTask;
         _userSettingsPersistor = userSettingsPersistor;
      }

      public void Execute()
      {
         Canceled = (_projectTask.CloseProject() == false);
         if (Canceled) return;

         _userSettingsPersistor.Save();

         Application.Exit();
      }
   }
}