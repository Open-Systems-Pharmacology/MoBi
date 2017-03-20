using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IOutputSelectionsView : IModalView<IOutputSelectionsPresenter>
   {
      void AddSettingsView(IView view);
   }
}