using System.Linq;
using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.RepositoryItems;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectMoleculesForBuildingBlockView : BaseModalView, ISelectMoleculesForBuildingBlockView
   {
      private ScreenBinder<SelectMoleculesDTO> _screenBinder;
      private GridViewBinder<DTOMoleculeSelection> _gridBinder;
      private ISelectMoleculesForBuildingBlockPresenter _presenter;
      private IGridViewBoundColumn<DTOMoleculeSelection, bool> _colSelected;

      public SelectMoleculesForBuildingBlockView()
      {
         InitializeComponent();
      }
      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<SelectMoleculesDTO>();
         _screenBinder.Bind(dto => dto.Name).To(txtName);
         _screenBinder.OnValidated += OnClearError;
         _screenBinder.OnValidationError += OnValidationError;

         gridMolecules.EndGrouping += (o, e) => gridMolecules.ExpandAllGroups();
         gridMolecules.GroupFormat = "[#image]{1}";

         _gridBinder = new GridViewBinder<DTOMoleculeSelection>(gridMolecules);
         var colBuildingBlock = _gridBinder.Bind(dto => dto.BuildingBlock).AsReadOnly();
         colBuildingBlock.XtraColumn.GroupIndex = 0;
         _gridBinder.Bind(dto => dto.Molecule).AsReadOnly();
         _colSelected = _gridBinder.Bind(dto => dto.Selected).WithRepository(dto=>new UxRepositoryItemCheckEdit(gridMolecules));
         _colSelected.OnChanged += checkSelection;
      }

      private void checkSelection(DTOMoleculeSelection arg1)
      {
         var message = _presenter.CheckSelection();
         gridMolecules.SetColumnError(_colSelected.XtraColumn,message);
         base.SetOkButtonEnable();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutControlItemMolecules.Text = AppConstants.Captions.Molecules.FormatForLabel();
         Text = AppConstants.Captions.NewWindow(ObjectTypes.MoleculeBuildingBlock);
      }

      public override bool HasError
      {
         get
         {
            return base.HasError || _gridBinder.HasError || gridMolecules.HasColumnErrors;
         }
      }

      public void AttachPresenter(ISelectMoleculesForBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(SelectMoleculesDTO moleculesDTOSelectMolecules)
      {
         _screenBinder.BindToSource(moleculesDTOSelectMolecules);
         if(moleculesDTOSelectMolecules.Molecules.Count()>0)
         {
            _gridBinder.BindToSource(moleculesDTOSelectMolecules.Molecules);
         }
         else
         {
            layoutControl1.HideItem(layoutControlItemMolecules);
         }
      }
   }
}
