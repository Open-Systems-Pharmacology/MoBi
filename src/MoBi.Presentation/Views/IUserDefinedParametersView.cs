using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IUserDefinedParametersView : IView<IUserDefinedParametersPresenter>
   {
      void AddParametersView(IView view);
   }
}