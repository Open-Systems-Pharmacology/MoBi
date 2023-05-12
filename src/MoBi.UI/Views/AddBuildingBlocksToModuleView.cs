using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
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
      public override void InitializeResources()
      {
         base.InitializeResources();
         DisableRename();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.ParameterValuesName).To(tbParameterValuesName);
         _screenBinder.Bind(x => x.InitialConditionsName).To(tbInitialConditionsName);
      }

      protected override void StartValueCheckChanged(bool enabled, LayoutControlItem namingLayoutControlItem)
      {
         namingLayoutControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(enabled);
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