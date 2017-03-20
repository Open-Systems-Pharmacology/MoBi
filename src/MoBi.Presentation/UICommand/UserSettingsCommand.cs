using OSPSuite.Presentation.MenuAndBars;
using MoBi.Core;
using MoBi.Presentation.Settings;

namespace MoBi.Presentation.UICommand
{
   internal class UserSettingsCommand : IUICommand
   {
      private readonly IUserSettings _userSettings;

      private readonly IMoBiApplicationController _applicationController;

      public UserSettingsCommand(IUserSettings userSettings, IMoBiApplicationController applicationController)
      {
         _userSettings = userSettings;
         _applicationController = applicationController;
      }

      public void Execute()
      {
         using (var presenter = _applicationController.Start<IUserSettingsPresenter>())
         {
            presenter.Edit(_userSettings);
         }
      }
   }
}