using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;

namespace MoBi.Presentation.Views
{
   public interface ICloneBuildingBlocksToModuleView : IBaseModuleContentView<ICloneBuildingBlocksToModulePresenter>
   {
      void BindTo(CloneBuildingBlocksToModuleDTO cloneBuildingBlocksToModuleDTO);
   }
}