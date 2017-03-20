using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditParameterStartValuesView :IEditBuildingBlockBaseView, IView<IEditParameterStartValuesPresenter>
   {
      void AddParameterView(IView view);
   }
}