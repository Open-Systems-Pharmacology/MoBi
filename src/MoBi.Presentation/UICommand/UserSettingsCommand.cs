using MoBi.Presentation.Settings;
using OSPSuite.Presentation.MenuAndBars;

namespace MoBi.Presentation.UICommand
{
   internal class UserSettingsCommand : IUICommand
   {
      private readonly ICloneableUserSettings _userSettings;

      private readonly IMoBiApplicationController _applicationController;

      public UserSettingsCommand(ICloneableUserSettings userSettings, IMoBiApplicationController applicationController)
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