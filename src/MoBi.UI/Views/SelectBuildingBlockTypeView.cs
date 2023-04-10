using DevExpress.XtraEditors;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Format;

namespace MoBi.UI.Views
{
   public class BuildingBlockTypeFormatter : IFormatter<BuildingBlockType>
   {
      public string Format(BuildingBlockType valueToFormat)
      {
         return valueToFormat.ToString();
      }
   }


   public partial class SelectBuildingBlockTypeView : BaseModalView, ISelectBuildingBlockTypeView
   {
      private ISelectBuildingBlockTypePresenter _presenter;
      private readonly ScreenBinder<SelectBuildingBlockTypeDTO> _screenBinder = new ScreenBinder<SelectBuildingBlockTypeDTO>();
      private readonly IFormatter<BuildingBlockType> _formatter = new BuildingBlockTypeFormatter();

      public SelectBuildingBlockTypeView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISelectBuildingBlockTypePresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(SelectBuildingBlockTypeDTO selectBuildingBlockTypeDTO)
      {
         _screenBinder.BindToSource(selectBuildingBlockTypeDTO);
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