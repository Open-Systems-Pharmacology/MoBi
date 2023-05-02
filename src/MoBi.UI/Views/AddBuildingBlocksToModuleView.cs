using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   public class AddBuildingBlocksToModuleView : BaseModuleContentView<AddBuildingBlocksToModuleDTO>,
      IAddBuildingBlocksToModuleView
   {
      public AddBuildingBlocksToModuleView()
      {
         DisableRename();
      }

      public void AttachPresenter(IAddBuildingBlocksToModulePresenter presenter)
      {
      }
   }

   public class CreateModuleView : BaseModuleContentView<ModuleContentDTO>, ICreateModuleView
   {
      public void AttachPresenter(ICreateModulePresenter presenter)
      {

      }
   }

   public class CloneBuildingBlocksToModuleView : BaseModuleContentView<CloneBuildingBlocksToModuleDTO>,
      ICloneBuildingBlocksToModuleView
   {
      public void AttachPresenter(ICloneBuildingBlocksToModulePresenter presenter)
      {
         
      }
   }
}