using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.UICommand
{
   public interface IApplicationSettingsPresenter : IPresenter<IApplicationSettingsView>
   {
      void SelectPKSimPath();
   }

   public class ApplicationSettingsPresenter : AbstractPresenter<IApplicationSettingsView, IApplicationSettingsPresenter>, IApplicationSettingsPresenter
   {
      private readonly IApplicationSettings _applicationSettings;
      private readonly IDialogCreator _dialogCreator;
      private readonly ApplicationSettingsDTO _applicationSettingsDTO;

      public ApplicationSettingsPresenter(IApplicationSettingsView view, IApplicationSettings applicationSettings, IDialogCreator dialogCreator) : base(view)
      {
         _applicationSettings = applicationSettings;
         _dialogCreator = dialogCreator;
         _applicationSettingsDTO = new ApplicationSettingsDTO(_applicationSettings);
         _view.BindTo(_applicationSettingsDTO);
      }

      public void SelectPKSimPath()
      {
         var pkSimPath = _dialogCreator.AskForFileToOpen(AppConstants.Captions.SelectPKSimExecutablePath, AppConstants.Filter.PKSIM_FILE_FILTER, Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(pkSimPath)) return;
         _applicationSettingsDTO.PKSimPath = pkSimPath;
      }

   }
}