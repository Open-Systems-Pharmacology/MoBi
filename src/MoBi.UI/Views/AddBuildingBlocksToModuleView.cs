using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;

namespace MoBi.UI.Views
{
   public class AddBuildingBlocksToModuleView : BaseModuleContentView<AddBuildingBlocksToModuleDTO>,
      IAddBuildingBlocksToModuleView
   {
      public AddBuildingBlocksToModuleView()
      {
         
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         DisableRename();
         ShowStartValueNameControls();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.ParameterValuesName).To(tbParameterValuesName);
         _screenBinder.Bind(x => x.InitialConditionsName).To(tbInitialConditionsName);
      }

      public override void BindTo(AddBuildingBlocksToModuleDTO addBuildingBlocksToModuleDTO)
      {
         base.BindTo(addBuildingBlocksToModuleDTO);
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