using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectSpatialStructureAndMoleculesView : BaseModalView, ISelectSpatialStructureAndMoleculesView
   {
      private ISelectBuildingBlocksForExtendPresenter _presenter;
      private ScreenBinder<SelectSpatialStructureAndMoleculesDTO> _screenBinder;

      public SelectSpatialStructureAndMoleculesView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(ISelectBuildingBlocksForExtendPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AdjustForNoMoleculeRequired()
      {
         layoutControlItemMolecules.Visibility = LayoutVisibility.Never;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<SelectSpatialStructureAndMoleculesDTO>();
         _screenBinder.Bind(dto => dto.Molecules).To(cmbMolecules).WithValues(dto => _presenter.AllMolecules);
         _screenBinder.Bind(dto => dto.SpatialStructure).To(cmbSpatialStructure).WithValues(dto => _presenter.AllSpatialStructures);

         RegisterValidationFor(_screenBinder);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemMolecules.Text = ObjectTypes.MoleculeBuildingBlock.FormatForLabel();
         layoutControlItemSpatialStructure.Text = ObjectTypes.SpatialStructure.FormatForLabel();
      }

      public void Show(SelectSpatialStructureAndMoleculesDTO dto)
      {
         _screenBinder.BindToSource(dto);
      }

      public override bool HasError => _screenBinder.HasError;
   }
}