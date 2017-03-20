using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditMoleculesBuildingBlockView : IView<IEditMoleculeBuildingBlockPresenter>, IEditBuildingBlockBaseView
   {
      void SetListView(IView view);
      void SetEditView(IView editEventView);
   }
}