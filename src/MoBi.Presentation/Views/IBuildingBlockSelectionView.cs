using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IBuildingBlockSelectionView : IView<IBuildingBlockSelectionPresenter>, IResizableView
   {
      bool NewVisible { set; }
      void RefreshElementList();
      void BindTo(BuildingBlockSelectionDTO buildingBlockSelectionDTO);
   }
}