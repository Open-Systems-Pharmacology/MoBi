using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditContainerInSimulationView : IView<IEditContainerInSimulationPresenter>
   {
      void SetContainerView(IView view);
   }
}