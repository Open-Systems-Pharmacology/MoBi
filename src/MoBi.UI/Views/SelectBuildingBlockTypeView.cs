using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public partial class SelectBuildingBlockTypeView : BaseModalView, ISelectBuildingBlockTypeView
   {
      private readonly ScreenBinder<SelectBuildingBlockTypeDTO> _screenBinder = new ScreenBinder<SelectBuildingBlockTypeDTO>();
      private readonly IFormatter<BuildingBlockType> _formatter = new BuildingBlockTypeFormatter();

      public SelectBuildingBlockTypeView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISelectBuildingBlockTypePresenter presenter)
      {
      }

      public void BindTo(SelectBuildingBlockTypeDTO selectBuildingBlockTypeDTO)
      {
         _screenBinder.BindToSource(selectBuildingBlockTypeDTO);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.Module;
         descriptionLabelControl.Text = AppConstants.Captions.SelectBuildingBlockType.FormatForLabel();
         buildingBlockSelectionlayoutControlItem.Text = AppConstants.Captions.BuildingBlockType.FormatForLabel();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.SelectedBuildingBlockType)
            .To(buildingBlockSelectionComboBoxEdit)
            .WithValues(x => x.AllowedBuildingBlockTypes)
            .WithFormat(_formatter);
      }
   }
}