using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditMoleculeStartValuesView :IEditBuildingBlockBaseView, IView<IEditMoleculeStartValuesPresenter>
   {
      void AddMoleculeStartValuesView(IView view);
   }
}