using System.Collections.Generic;
using System.Drawing;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;

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

      public override void BindTo(AddBuildingBlocksToModuleDTO moduleContentDTO)
      {
         base.BindTo(moduleContentDTO);
         if (moduleContentDTO.AllowOnlyInitialConditions)
            adjustViewForOnlyInitialConditions();
         if (moduleContentDTO.AllowOnlyParameterValues)
            adjustViewForOnlyParameterValues();
      }

      protected override void StartValueCheckChanged(bool enabled, LayoutControlItem namingLayoutControlItem)
      {
         namingLayoutControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(enabled);
      }

      public void AttachPresenter(IAddBuildingBlocksToModulePresenter presenter)
      {
      }

      public void adjustViewForOnlyParameterValues()
      {
         ShowStartValueNameControls();
         cbParameterValues.Checked = true;
         cbParameterValues.Enabled = false;

         hideAllItemsExcept(new List<BaseLayoutItem> { parameterValuesNameItem, parameterValuesItem });
      }

      public void adjustViewForOnlyInitialConditions()
      {
         ShowStartValueNameControls();
         cbInitialConditions.Checked = true;
         cbInitialConditions.Enabled = false;

         hideAllItemsExcept(new List<BaseLayoutItem> { initialConditionsNameItem, initialConditionsItem });
      }

      private void adjustHeight(int heightAdjustment)
      {
         Size = new Size(Width, Height + heightAdjustment);
      }

      private void hideAllItemsExcept(List<BaseLayoutItem> itemsToShow)
      {
         var initialGroupHeight = createBuildingBlocksGroup.Size.Height;
         createBuildingBlocksGroup.Items.Each(item =>
         {
            if (!itemsToShow.Contains(item))
               item.Visibility = LayoutVisibility.Never;
         });
         adjustHeight(createBuildingBlocksGroup.Size.Height - initialGroupHeight);
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