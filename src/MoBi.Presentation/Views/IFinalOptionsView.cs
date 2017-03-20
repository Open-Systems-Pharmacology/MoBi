using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IFinalOptionsView : IView<IFinalOptionsPresenter>
   {
      void SetValidationOptionsView(IView view);
   }
}