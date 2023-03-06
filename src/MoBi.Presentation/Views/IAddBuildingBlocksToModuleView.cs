using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IAddBuildingBlocksToModuleView : IModalView<IAddBuildingBlocksToModulePresenter>   
   {
      void BindTo(AddBuildingBlocksToModuleDTO createModuleDTO);
      void DisableExistingBuildingBlocks(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO);
   }
}
