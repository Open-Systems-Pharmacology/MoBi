using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IAddContentToModuleView<TPresenter> : IModalView<TPresenter> where TPresenter : IAddContentToModulePresenter
   {
      void ShowInitialConditionsName();
      void DisableDefaultMergeBehavior();
   }

   public interface IAddBuildingBlocksToModuleView : IAddContentToModuleView<IAddBuildingBlocksToModulePresenter>
   {
      void BindTo(AddBuildingBlocksToModuleDTO moduleContentDTO);
   }
}