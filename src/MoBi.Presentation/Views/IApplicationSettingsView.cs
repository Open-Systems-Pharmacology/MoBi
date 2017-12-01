using MoBi.Presentation.DTO;
using MoBi.Presentation.UICommand;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IApplicationSettingsView : IView<IApplicationSettingsPresenter>
   {
      void BindTo(ApplicationSettingsDTO applicationSettingsDTO);
   }
}