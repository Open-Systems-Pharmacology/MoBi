using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IModuleSelectionView : IView<IModuleSelectionPresenter>, IResizableView
   {
      void BindTo(ModuleSelectionDTO moduleSelectionDTO);
   }
   public interface IBuildingBlockSelectionView : IView<IBuildingBlockSelectionPresenter>, IResizableView
   {
      bool NewVisible { set; }
      void RefreshElementList();
      void BindTo(BuildingBlockSelectionDTO buildingBlockSelectionDTO);
   }
}