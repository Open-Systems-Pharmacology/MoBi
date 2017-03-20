using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.Views
{
   public interface IBuildingBlockSelectionView : OSPSuite.Presentation.Views.IView<IBuildingBlockSelectionPresenter>, OSPSuite.Presentation.Views.IResizableView
   {
      bool NewVisible { set; }
      void BindTo(BuildingBlockSelectionDTO buildingBlockSelectionDTO);
      void RefreshElementList();
   }
}