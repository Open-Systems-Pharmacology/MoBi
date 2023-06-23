using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditInitialConditionsView :IEditBuildingBlockBaseView, IView<IEditInitialConditionsPresenter>
   {
      void AddInitialConditionsView(IView view);
   }
}