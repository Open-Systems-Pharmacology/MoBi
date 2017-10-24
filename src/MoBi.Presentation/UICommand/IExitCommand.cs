using System.Windows.Forms;
using MoBi.Core;
using MoBi.Presentation.Serialization.Xml;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Tasks;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   public interface IExitCommand : IUICommand
   {
      bool Canceled { get; }
   }

   public class ExitCommand : IExitCommand
   {
      private readonly IProjectTask _projectTask;
      private readonly ISettingsPersistor<IUserSettings> _userSettingsPersistor;
      private readonly ISettingsPersistor<IApplicationSettings> _applicationSettingsPersistor;
      public bool Canceled { get; private set; }

      public ExitCommand(IProjectTask projectTask, ISettingsPersistor<IUserSettings> userSettingsPersistor, ISettingsPersistor<IApplicationSettings> applicationSettingsPersistor)
      {
         _projectTask = projectTask;
         _userSettingsPersistor = userSettingsPersistor;
         _applicationSettingsPersistor = applicationSettingsPersistor;
      }

      public void Execute()
      {
         Canceled = (_projectTask.CloseProject() == false);
         if (Canceled) return;

         _userSettingsPersistor.Save();
         _applicationSettingsPersistor.Save();

         Application.Exit();
      }
   }
}