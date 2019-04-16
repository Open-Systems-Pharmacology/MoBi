using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditFormulaPathListView : BaseUserControl, IEditFormulaPathListView, IViewWithPopup
   {
      private readonly GridViewBinder<FormulaUsablePathDTO> _gridViewBinder;
      private IGridViewColumn _colAlias;
      private IGridViewColumn _colDimension;
      private IGridViewColumn _colPath;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
      private readonly RepositoryItemButtonEdit _addButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Plus);
      private IGridViewColumn _colRemoveButton;
      private IGridViewColumn<FormulaUsablePathDTO> _colAddButton;
      private IEditFormulaPathListPresenter _presenter;
      private bool _readOnly;
      public BarManager PopupBarManager { get; }

      public EditFormulaPathListView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<FormulaUsablePathDTO>(gridView);
         gridView.AllowsFiltering = false;
         gridControl.AllowDrop = true;
         PopupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImages16x16};
      }

      public void AttachPresenter(IEditFormulaPathListPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _colAlias = _gridViewBinder.Bind(x => x.Alias)
            .WithOnValueUpdating((formulaUsablePathDTO, e) => OnEvent(() => _presenter.SetAlias(e.NewValue, e.OldValue, formulaUsablePathDTO.FormulaUsablePath)))
            .WithToolTip(ToolTips.Formula.ReferenceAlias);

         _colPath = _gridViewBinder.Bind(x => x.Path)
            .WithOnValueUpdating((dto, e) => OnEvent(() => _presenter.SetFormulaUsablePath(e.NewValue, dto)))
            .WithToolTip(ToolTips.Formula.ReferencePath);

         _colDimension = _gridViewBinder.Bind(x => x.Dimension)
            .WithRepository(dto => createDimensionRepository())
            .WithOnValueUpdating((formulaUsablePathDTO, e) => OnEvent(() => _presenter.SetFormulaPathDimension(e.NewValue, e.OldValue, formulaUsablePathDTO)))
            .WithToolTip(ToolTips.Formula.ReferenceDimension);

         _colRemoveButton = _gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _removeButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _colAddButton = _gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _addButtonRepository)
            .WithFixedWidth((OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH));

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(removeFormulaUsablePath, _gridViewBinder.FocusedElement);
         _addButtonRepository.ButtonClick += (o, e) => OnEvent(cloneFormulaUsablePath, _gridViewBinder.FocusedElement);

         gridView.MouseDown += (o, e) => OnEvent(() => onGridViewMouseDown(e));
         gridControl.DragDrop += (o, e) => OnEvent(onDragDrop, e);
         gridControl.DragOver += (o, e) => OnEvent(onDragOver, e);
         DragDrop += (o, e) => OnEvent(onDragDrop, e);
         DragOver += (o, e) => OnEvent(onDragOver, e);
      }

      private void cloneFormulaUsablePath(FormulaUsablePathDTO focusedElement)
      {
         _presenter.ClonePath(focusedElement);
      }

      private void removeFormulaUsablePath(FormulaUsablePathDTO focusedElement)
      {
         _presenter.RemovePath(focusedElement);
      }

      private void onGridViewMouseDown(MouseEventArgs mouseEventArgs)
      {
         if (mouseEventArgs.Button != MouseButtons.Right) return;

         var rowHandle = gridView.RowHandleAt(mouseEventArgs);
         _presenter.ShowContextMenu(_gridViewBinder.ElementAt(rowHandle), mouseEventArgs.Location);
      }

      private void onDragDrop(DragEventArgs e)
      {
         var referenceDTO = e.Data<ReferenceDTO>();
         if (!_presenter.DragDropAllowedFor(referenceDTO))
            return;

         _presenter.Drop(referenceDTO);
      }

      private void onDragOver(DragEventArgs e)
      {
         var referenceDTO = e.Data<ReferenceDTO>();
         e.Effect = _presenter.DragDropAllowedFor(referenceDTO) ? DragDropEffects.Copy : DragDropEffects.None;
      }

      private RepositoryItem createDimensionRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.GetDimensions());
         return repository;
      }

      public void SetParserError(string error)
      {
         //tODO
      }

      public bool ReadOnly
      {
         get => _readOnly;
         set
         {
            _readOnly = value;
            setColumnsReadonly(_readOnly);
         }
      }

      public override bool HasError => base.HasError && _gridViewBinder.HasError;

      public void BindTo(IReadOnlyList<FormulaUsablePathDTO> formulaUsablePathDTOs)
      {
         _gridViewBinder.BindToSource(formulaUsablePathDTOs);
      }

      private void setColumnsReadonly(bool readOnly)
      {
         _colAlias.ReadOnly = readOnly;
         _colPath.ReadOnly = readOnly;
         _colDimension.ReadOnly = readOnly;
         _colRemoveButton.Visible = !readOnly;
         _colAddButton.Visible = !readOnly;
      }
   }
}