using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IAddBuildingBlocksToModuleView : IModalView<IAddBuildingBlocksToModulePresenter>
   {
      void BindTo(AddBuildingBlocksToModuleDTO moduleContentDTO);
   }

   public interface ICloneBuildingBlocksToModuleView : IModalView<ICloneBuildingBlocksToModulePresenter>
   {
      void BindTo(CloneBuildingBlocksToModuleDTO cloneBuildingBlocksToModuleDTO);
   }
}