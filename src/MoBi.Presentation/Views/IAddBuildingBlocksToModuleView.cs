using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IAddContentToModuleView<TPresenter> : IBaseModuleContentView<TPresenter> where TPresenter : IAddContentToModulePresenter
   {
      void ShowInitialConditionsName();
      void DisableDefaultMergeBehavior();
   }

   public interface IAddBuildingBlocksToModuleView : IAddContentToModuleView<IAddBuildingBlocksToModulePresenter>
   {
      void BindTo(AddBuildingBlocksToModuleDTO moduleContentDTO);
   }

   public interface IBaseModuleContentView<TPresenter> : IModalView<TPresenter> where TPresenter : IDisposablePresenter
   {
      void SetBehaviorDescription(string description);
   }
}