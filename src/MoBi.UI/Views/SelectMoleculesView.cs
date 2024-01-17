using System.Collections.Generic;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Grid;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Controls;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public partial class SelectMoleculesView : BaseUserControl, ISelectMoleculesView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private GridViewBinder<MoleculeSelectionDTO> _gridViewBinder;
      private ISelectMoleculesPresenter _presenter;

      public SelectMoleculesView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<MoleculeSelectionDTO>(gridView);

         gridView.RowStyle += (o, e) => OnEvent(() => gridViewRowStyle(e));
         gridView.SelectionChanged += (o, e) => OnEvent(gridViewSelectionChanged);

         // Disable "selected" appearance for rows. UxRepositoryItemImageComboBox do not have a "selected" appearance.
         Load += (o, e) => OnEvent(formLoad);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         gridView.PopupMenuShowing += (o, e) => OnEvent(() => gridViewPopupMenuShowing(o, e));

         gridView.ConfigureGridForCheckBoxSelect(nameof(MoleculeSelectionDTO.Selected));
         gridView.DisableGrouping();
         configureGridGrouping();
      }

      private void configureGridGrouping()
      {
         // When grouping, the default is to show the column name and the value. So we would see
         // "BuildingBlock: Molecules" instead of just "Molecules". We just want to see the value in this case
         gridView.GroupFormat = "[#image]{1}";
         gridView.EndGrouping += (o, e) => gridView.ExpandAllGroups();
      }

      public void AttachPresenter(ISelectMoleculesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IReadOnlyList<MoleculeSelectionDTO> dtoMolecules)
      {
         _gridViewBinder.BindToSource(dtoMolecules);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _gridViewBinder = new GridViewBinder<MoleculeSelectionDTO>(gridView);
         var colBuildingBlock = _gridViewBinder.Bind(dto => dto.BuildingBlockDisplayName).AsReadOnly();
         colBuildingBlock.XtraColumn.GroupIndex = 0;

         _gridViewBinder.AutoBind(dto => dto.MoleculeName).WithRepository(configureMoleculeRepository).WithCaption(Molecule).AsReadOnly();
      }

      private RepositoryItem configureMoleculeRepository(MoleculeSelectionDTO molecule)
      {
         var repository = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
         return repository.AddItem(molecule.MoleculeName, molecule.Icon);
      }

      private void gridViewSelectionChanged()
      {
         // The select/unselect of a molecule will affect the validation of another molecule
         // with the same name. If both were selected, and one is unselected, then the other becomes
         // valid.
         gridView.RefreshData();
         _presenter.SelectionChanged();
      }

      private void gridViewRowStyle(RowStyleEventArgs e)
      {
         if (e.Appearance == gridView.PaintAppearance.SelectedRow)
            e.Appearance.Assign(gridView.PaintAppearance.Row);
         e.HighPriority = true;
      }

      private void formLoad()
      {
         gridControl.ForceInitialize();
         gridView.Appearance.SelectedRow.Assign(gridView.PaintAppearance.Row);
      }

      private void gridViewPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         e.Menu.Hide(GridStringId.MenuColumnColumnCustomization);
         e.Menu.Hide(GridStringId.MenuColumnGroupBox);
         e.Menu.Hide(GridStringId.MenuColumnGroup);
         e.Menu.Hide(GridStringId.MenuColumnRemoveColumn);
      }

      private void disposeBinders()
      {
         _gridViewBinder.Dispose();
      }
   }
}