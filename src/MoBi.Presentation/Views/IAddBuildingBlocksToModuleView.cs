using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IAddContentToModuleView<TPresenter> : IModalView<TPresenter> where TPresenter : IAddContentToModulePresenter
   {
      void ShowInitialConditionsName();
   }

   public interface IAddBuildingBlocksToModuleView : IAddContentToModuleView<IAddBuildingBlocksToModulePresenter>
   {
      void BindTo(AddBuildingBlocksToModuleDTO moduleContentDTO);
   }
}