using DevExpress.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectSpatialStructureAndMoleculesView : BaseModalView, ISelectSpatialStructureAndMoleculesView
   {
      private ISelectSpatialStructureAndMoleculesPresenter _presenter;
      private ScreenBinder<SelectSpatialStructureDTO> _screenBinder;

      public SelectSpatialStructureAndMoleculesView()
      {
         InitializeComponent();

         descriptionLabel.AsDescription();
         descriptionLabel.Text = AppConstants.Captions.ExtendDescription;
      }

      public void AttachPresenter(ISelectSpatialStructureAndMoleculesPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder = new ScreenBinder<SelectSpatialStructureDTO>();
         _screenBinder.Bind(dto => dto.SpatialStructure).To(cmbSpatialStructure).WithValues(dto => _presenter.AllSpatialStructures);

         RegisterValidationFor(_screenBinder);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemMolecules.Text = AppConstants.Captions.Molecules.FormatForLabel();
         layoutControlItemMolecules.TextLocation = Locations.Top;
         layoutControlItemMolecules.TextVisible = true;
         layoutControlItemSpatialStructure.Text = ObjectTypes.SpatialStructure.FormatForLabel();

         Text = AppConstants.Captions.NewWindow(ObjectTypes.MoleculeBuildingBlock);
      }

      public void Show(SelectSpatialStructureDTO dto)
      {
         _screenBinder.BindToSource(dto);
      }

      public void AddMoleculeSelectionView(IView view)
      {
         moleculeSelectionPanel.FillWith(view);
      }

      public void MoleculeSelectionChanged()
      {
         SetOkButtonEnable();
      }

      public override bool HasError => !_presenter.CanClose || base.HasError || _screenBinder.HasError;

      private void disposeBinders()
      {
         _screenBinder.Dispose();
      }
   }
}