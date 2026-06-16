using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.UICommand
{
   public interface IApplicationSettingsPresenter : IPresenter<IApplicationSettingsView>
   {
      void Edit(ApplicationSettingsDTO applicationSettingsDTO);
      void SelectPKSimPath();
   }

   public class ApplicationSettingsPresenter : AbstractPresenter<IApplicationSettingsView, IApplicationSettingsPresenter>, IApplicationSettingsPresenter
   {
      private readonly IDialogCreator _dialogCreator;
      private ApplicationSettingsDTO _applicationSettingsDTO;

      public ApplicationSettingsPresenter(IApplicationSettingsView view, IDialogCreator dialogCreator) : base(view)
      {
         _dialogCreator = dialogCreator;
      }

      public void Edit(ApplicationSettingsDTO applicationSettingsDTO)
      {
         _applicationSettingsDTO = applicationSettingsDTO;
         _view.BindTo(_applicationSettingsDTO);
      }

      public void SelectPKSimPath()
      {
         var pkSimPath = _dialogCreator.AskForFileToOpen(AppConstants.Captions.SelectPKSimExecutablePath, AppConstants.Filter.PKSIM_FILE_FILTER, Constants.DirectoryKey.PROJECT);
         if (string.IsNullOrEmpty(pkSimPath))
            return;

         _applicationSettingsDTO.PKSimPath = pkSimPath;
      }
   }
}
