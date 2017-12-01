using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditExplicitFormulaView : BaseUserControl, IEditExplicitFormulaView, IViewWithPopup
   {
      private readonly GridViewBinder<FormulaUsablePathDTO> _gridBinder;
      private IEditExplicitFormulaPresenter _presenter;
      private readonly ScreenBinder<ExplicitFormulaBuilderDTO> _screenBinder;
      private readonly DXErrorProvider _warningProvider;
      private bool _readOnly;
      private IGridViewColumn _colAlias;
      private IGridViewColumn _colDimension;
      private IGridViewColumn _colPath;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
      private readonly RepositoryItemButtonEdit _addButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Plus);
      private IGridViewColumn _colRemoveButton;
      private IGridViewColumn<FormulaUsablePathDTO> _colAddButton;
      public BarManager PopupBarManager { get; private set; }

      public EditExplicitFormulaView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _warningProvider = new DXErrorProvider(this);
         grdObjectPaths.AllowDrop = true;
         PopupBarManager = new BarManager { Form = this, Images = imageListRetriever.AllImages16x16 };
         _screenBinder = new ScreenBinder<ExplicitFormulaBuilderDTO>();
         _gridBinder = new GridViewBinder<FormulaUsablePathDTO>(gridViewReferencePaths);
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

      public void AttachPresenter(IEditExplicitFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(ExplicitFormulaBuilderDTO dto)
      {
         _screenBinder.BindToSource(dto);
         _gridBinder.BindToSource(dto.ObjectPaths);
         txtFormulaString.TextChanged += (o, e) => OnEvent(formulaStringChanged);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(item => item.FormulaString)
            .To(txtFormulaString)
            .OnValueUpdating += (o, e) => OnEvent(() => _presenter.SetFormulaString(e.NewValue, e.OldValue));

         _colAlias = _gridBinder.Bind(x => x.Alias)
            .WithOnValueUpdating((formulaUsablePathDTO, e) => OnEvent(() => _presenter.SetAlias(e.NewValue, e.OldValue, formulaUsablePathDTO.FormulaUsablePath)))
            .WithToolTip(ToolTips.Formula.ReferenceAlias);

         _colPath = _gridBinder.Bind(x => x.Path)
            .WithOnValueUpdating((dto, e) => OnEvent(() => _presenter.SetFormulaUsablePath(e.NewValue, dto)))
            .WithToolTip(ToolTips.Formula.ReferencePath);

         _colDimension = _gridBinder.Bind(x => x.Dimension)
            .WithRepository(dto => createDimensionRepository())
            .WithOnValueUpdating((formulaUsablePathDTO, e) => OnEvent(() => _presenter.SetFormulaPathDimension(e.NewValue, e.OldValue, formulaUsablePathDTO)))
            .WithToolTip(ToolTips.Formula.ReferenceDimension);

         _colRemoveButton = _gridBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _removeButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _colAddButton = _gridBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _addButtonRepository)
            .WithFixedWidth((OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH));

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(removeFormulaUsablePath, _gridBinder.FocusedElement);
         _addButtonRepository.ButtonClick += (o, e) => OnEvent(cloneFormulaUsablePath, _gridBinder.FocusedElement);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         ReadOnly = false;
         gridViewReferencePaths.MouseDown += (o, e) => OnEvent(() => onGridViewMouseDown(e));
         grdObjectPaths.DragDrop += (o, e) => OnEvent(onDragDrop, e);
         grdObjectPaths.DragOver += (o, e) => OnEvent(onDragOver, e);
         DragDrop += (o, e) => OnEvent(onDragDrop, e);
         DragOver += (o, e) => OnEvent(onDragOver, e);
      }

      private void onGridViewMouseDown(MouseEventArgs mouseEventArgs)
      {
         if (mouseEventArgs.Button != MouseButtons.Right) return;

         var rowHandle = gridViewReferencePaths.RowHandleAt(mouseEventArgs);
         _presenter.ShowContextMenu(_gridBinder.ElementAt(rowHandle), mouseEventArgs.Location);
      }

      private void cloneFormulaUsablePath(FormulaUsablePathDTO focusedElement)
      {
         _presenter.ClonePath(focusedElement);
      }

      private void removeFormulaUsablePath(FormulaUsablePathDTO focusedElement)
      {
         _presenter.RemovePath(focusedElement);
      }

      public override bool HasError
      {
         get { return base.HasError || _screenBinder.HasError || _gridBinder.HasError; }
      }

      private RepositoryItem createDimensionRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridViewReferencePaths);
         repository.FillComboBoxRepositoryWith(_presenter.GetDimensions());
         return repository;
      }

      private void formulaStringChanged()
      {
         _presenter.Validate(txtFormulaString.Text);
      }

      public void SetParserError(string parserError)
      {
         if (string.IsNullOrEmpty(parserError))
            _warningProvider.SetError(txtFormulaString, null);
         else
            _warningProvider.SetError(txtFormulaString, parserError, ErrorType.Critical);

         _presenter.ViewChanged();
      }

      public void SetFormulaCaption(string caption)
      {
         if (string.IsNullOrEmpty(caption))
            layoutItemFormulaString.TextVisible = false;
         else
         {
            layoutItemFormulaString.TextVisible = true;
            layoutItemFormulaString.Text = caption;
         }
      }

      public void HideFormulaCaption()
      {
         SetFormulaCaption(string.Empty);
      }

      public bool ReadOnly
      {
         get { return _readOnly; }
         set
         {
            _readOnly = value;
            setColumsReadonly(_readOnly);
         }
      }

      private void setColumsReadonly(bool readOnly)
      {
         _colAlias.ReadOnly = readOnly;
         _colPath.ReadOnly = readOnly;
         _colDimension.ReadOnly = readOnly;
         txtFormulaString.Enabled = !readOnly;
         _colRemoveButton.Visible = !readOnly;
         _colAddButton.Visible = !readOnly;
      }
   }
}