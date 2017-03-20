using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditSolverSettingsView : IView<IEditSolverSettingsPresenter>
   {
      void Show(SolverSettingsDTO dto);
      bool ShowGroupCaption { set; }
   }
}