using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditParameterValuesView :IEditBuildingBlockBaseView, IView<IEditParameterValuesPresenter>
   {
      void AddParameterView(IView view);
   }
}