using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
using MoBi.UI.Services;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditParametersInContainerView : BaseUserControl, IEditParametersInContainerView
   {
      private readonly IToolTipCreator _toolTipCreator;
      private readonly ValueOriginBinder<ParameterDTO> _valueOriginBinder;
      private IEditParametersInContainerPresenter _presenter;
      private GridViewBinder<ParameterDTO> _gridViewBinder;
      private IGridViewColumn _colValue;
      private IGridViewColumn _colPersistable;
      private IGridViewColumn _colFormula;
      private IGridViewColumn _colRHSFormula;
      private IGridViewColumn _colButtons;
      private IGridViewColumn _colGroup;
      private EditParameterMode _editMode;
      private IGridViewColumn _colDimension;
      private IGridViewColumn _colBuildMode;
      private readonly UxComboBoxUnit<ParameterDTO> _unitControl;
      private readonly RepositoryItemTextEdit _standardParameterEditRepository = new RepositoryItemTextEdit();
      private RepositoryItemButtonEdit _isFixedParameterEditRepository;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
      private readonly RepositoryItemButtonEdit _navigateButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Search);
      private RepositoryItemButtonEdit _nameButtonRepository;
      private readonly UxRepositoryItemCheckEdit _checkBoxRepository;
      private readonly ScreenBinder<IEditParametersInContainerPresenter> _screenBinder;
      private IGridViewBoundColumn<ParameterDTO, string> _colModule;
      private IGridViewBoundColumn<ParameterDTO, string> _colBuildingBlock;
      private IGridViewColumn<ParameterDTO> _colNavigate;

      public EditParametersInContainerView(IToolTipCreator toolTipCreator, ValueOriginBinder<ParameterDTO> valueOriginBinder)
      {
         _toolTipCreator = toolTipCreator;
         _valueOriginBinder = valueOriginBinder;
         InitializeComponent();
         var toolTipController = new ToolTipController { AllowHtmlText = true };
         toolTipController.Initialize();
         _unitControl = new UxComboBoxUnit<ParameterDTO>(gridControl);
         gridView.HiddenEditor += (o, e) => hideEditor();
         gridControl.KeyDown += gridViewKeyDown;
         gridControl.ToolTipController = toolTipController;
         //specific grid settings for parameter
         gridView.ShowRowIndicator = true;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.OptionsView.ShowGroupPanel = false;
         splitContainerControl.CollapsePanel = SplitCollapsePanel.Panel2;
         splitContainerControl.PanelVisibility = SplitPanelVisibility.Panel1;
         gridView.GroupFormat = "[#image]{1}";
         gridView.EndGrouping += (o, e) => gridView.ExpandAllGroups();
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         gridView.PopupMenuShowing += OnPopupMenuShowing;
         _checkBoxRepository = new UxRepositoryItemCheckEdit(gridView);
         _screenBinder = new ScreenBinder<IEditParametersInContainerPresenter>();
      }

      private void OnPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         if (e.HitInfo.HitTest != GridHitTest.RowIndicator || !e.Menu.Items.Any() || gridView.GetSelectedRows().Length != 1)
            return;

         e.Menu.Items.Add(copyPathMenuItem());
      }

      private DXMenuItem copyPathMenuItem()
      {
         return new DXMenuItem(
            AppConstants.Captions.CopyPath,
            (s, args) => copyPath(),
            ApplicationIcons.Copy
         );
      }

      private void copyPath() => _presenter.CopyPathForParameter(_gridViewBinder.FocusedElement);

      private void hideEditor() => _unitControl.Hide();

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnAddParameter.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.AddParameter);
         btnLoadParameter.InitWithImage(ApplicationIcons.PKMLLoad, AppConstants.Captions.LoadParameter);
         layoutControlItemAddParameter.AdjustButtonSize();
         layoutControlItemLoadParameter.AdjustButtonSize();
         chkShowAdvancedParameter.Text = AppConstants.Captions.ShowAdvancedParameters;
         chkGroupBy.Text = AppConstants.Captions.GroupParameters;
         _removeButtonRepository.Buttons[0].ToolTip = ToolTips.ParameterList.DeleteParameter;
         _navigateButtonRepository.Buttons[0].ToolTip = ToolTips.ParameterList.GoToSource;
         gridView.MultiSelect = true;

         // This aligns the text in controls with different heights
         // Label height is less than check edit so the text becomes mis-aligned.
         layoutControlItemShowAdvanceParameters.ContentVertAlignment = VertAlignment.Top;
         layoutControlItemGroupBy.ContentVertAlignment = VertAlignment.Top;

         layoutControlItemSelectIndividual.Text = AppConstants.Captions.ShowParametersFromIndividual.FormatForLabel();
         chkGroupBy.MaximumSize = new Size(150, 0);
         chkShowAdvancedParameter.MaximumSize = new Size(200, 0);
         cbSelectIndividual.ToolTip = ToolTips.BuildingBlockSpatialStructure.PreviewSpatialStructureWithIndividualSelection;

         tbContainerPath.ReadOnly = true;
         layoutControlItemContainerPath.Text = AppConstants.Captions.ContainerPath.FormatForLabel();
      }

      private void createResetButtonItem()
      {
         _isFixedParameterEditRepository = new UxRepositoryItemButtonImage(ApplicationIcons.Reset, ToolTips.ResetParameterToolTip) { TextEditStyle = TextEditStyles.Standard };
         _isFixedParameterEditRepository.Buttons[0].IsLeft = true;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         createResetButtonItem();

         _gridViewBinder = new GridViewBinder<ParameterDTO>(gridView) { ValidationMode = ValidationMode.LeavingCell };

         _nameButtonRepository = createNameEdit();
         _nameButtonRepository.ButtonClick += (o, e) => onRenameClick(e, _gridViewBinder.FocusedElement);
         _gridViewBinder.Bind(dto => dto.Name)
            .WithRepository(dto => _nameButtonRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _colValue = _gridViewBinder.Bind(dto => dto.Value)
            .WithFormat(dto => dto.ParameterFormatter(checkForEditable: false))
            .WithRepository(repositoryForValue)
            .WithEditorConfiguration(configureRepository)
            .WithToolTip(ToolTips.ParameterList.SetParameterValue)
            .WithOnValueUpdating(onParameterValueSet)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _colModule = _gridViewBinder.Bind(dto => dto.ModuleName)
            .WithCaption(AppConstants.Captions.Module)
            .AsReadOnly();

         _colBuildingBlock = _gridViewBinder.Bind(dto => dto.BuildingBlockName)
            .WithCaption(AppConstants.Captions.BuildingBlock)
            .AsReadOnly();

         _valueOriginBinder.InitializeBinding(_gridViewBinder, onParameterValueOriginSet);

         _unitControl.ParameterUnitSet += setParameterUnit;

         _isFixedParameterEditRepository.ButtonClick += (o, e) => this.DoWithinExceptionHandler(() => onResetValue(_gridViewBinder.FocusedElement));

         _colFormula = _gridViewBinder.Bind(dto => dto.Formula)
            .AsReadOnly();

         _colRHSFormula = _gridViewBinder.Bind(dto => dto.RHSFormula)
            .AsReadOnly();

         _colDimension = _gridViewBinder.Bind(dto => dto.Dimension)
            .WithRepository(dto => createDimensionRepository())
            .WithOnValueUpdating((o, e) => OnEvent(() => onSetDimension(o, e.NewValue)));

         _colGroup = _gridViewBinder.Bind(dto => dto.GroupName)
            .AsReadOnly();

         _colBuildMode = _gridViewBinder.Bind(dto => dto.BuildMode)
            .WithRepository(dto => createBuildModeRepository())
            .WithCaption(AppConstants.ParameterType)
            .WithOnValueUpdating((o, e) => OnEvent(() => _presenter.SetBuildModeFor(o, e.NewValue)))
            .WithShowInColumnChooser(true);

         _gridViewBinder.Bind(dto => dto.Description)
            .AsHidden()
            .AsReadOnly()
            .WithShowInColumnChooser(true);

         _gridViewBinder.Bind(dto => dto.IsFavorite)
            .WithCaption(Captions.Favorites)
            .WithWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithRepository(x => _checkBoxRepository)
            .WithToolTip(OSPSuite.Assets.ToolTips.FavoritesToolTip)
            .WithOnValueUpdating((o, e) => OnEvent(() => onIsFavoriteSet(o, e.NewValue)));

         _colPersistable = _gridViewBinder.Bind(dto => dto.Persistable)
            .WithCaption(AppConstants.Captions.Persistable)
            .WithWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithRepository(x => _checkBoxRepository)
            .WithOnValueUpdating((o, e) => OnEvent(() => onIsPersistableSet(o, e.NewValue)));

         _colButtons = _gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _removeButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _colNavigate = _gridViewBinder.AddUnboundColumn()
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _navigateButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH)
            .AsHidden();

         gridView.ShowingEditor += (o, e) => OnEvent(onShowingEditor, e);
         gridView.RowStyle += (o, e) => OnEvent(onRowStyle, e);

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(_presenter.RemoveParameter, _gridViewBinder.FocusedElement);
         _navigateButtonRepository.ButtonClick += (o, e) => OnEvent(_presenter.NavigateToParameter, _gridViewBinder.FocusedElement);

         gridView.FocusedRowChanged += (o, e) => OnEvent(gridViewRowChanged, e);

         chkShowAdvancedParameter.CheckedChanged += (o, e) => OnEvent(showAdvancedParameterChanged);
         chkGroupBy.CheckStateChanged += (o, e) => OnEvent(groupByChanged);
         btnAddParameter.Click += (o, e) => OnEvent(_presenter.AddParameter);
         btnLoadParameter.Click += (o, e) => OnEvent(_presenter.LoadParameter);

         _screenBinder.Bind(x => x.SelectedIndividual)
            .To(cbSelectIndividual)
            .WithValues(x => x.AllIndividuals)
            .OnValueUpdated += (o, e) => OnEvent(_presenter.UpdatePreview);
      }

      private void onRowStyle(RowStyleEventArgs e)
      {
         var dto = _gridViewBinder.ElementAt(e.RowHandle);

         if (dto != null && dto.IsIndividualPreview)
            gridView.AdjustAppearance(e, isEnabled: false);
      }

      private void onShowingEditor(CancelEventArgs e)
      {
         var dto = _gridViewBinder.FocusedElement;
         var column = gridView.FocusedColumn;
         if (dto == null || column == null)
            return;

         e.Cancel = dto.IsIndividualPreview;
      }

      private void onIsFavoriteSet(ParameterDTO parameterDTO, bool newValue) => _presenter.SetIsFavorite(parameterDTO, newValue);

      private void onIsPersistableSet(ParameterDTO parameterDTO, bool newValue) => _presenter.SetIsPersistable(parameterDTO, newValue);

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != gridView.GridControl) return;


         var parameterDTO = _gridViewBinder.ElementAt(e);
         if (parameterDTO == null)
            return;

         e.Info = gridView.CreateToolTipControlInfoFor(parameterDTO, e.ControlMousePosition, _toolTipCreator.ToolTipFor);
      }

      private void setParameterUnit(ParameterDTO parameter, Unit unit)
      {
         OnEvent(() =>
         {
            gridView.CloseEditor();
            _presenter.SetParameterUnit(parameter, unit);
         });
      }

      private void onSetDimension(ParameterDTO parameter, IDimension newValue) => _presenter.SetDimensionFor(parameter, newValue);

      private void onParameterValueSet(ParameterDTO parameter, PropertyValueSetEventArgs<double> e) => OnEvent(() => _presenter.OnParameterValueSet(parameter, e.NewValue));

      private void onParameterValueOriginSet(ParameterDTO parameter, ValueOrigin valueOrigin) => OnEvent(() => _presenter.OnParameterValueOriginSet(parameter, valueOrigin));

      private RepositoryItem repositoryForValue(ParameterDTO parameter)
      {
         if (_presenter.IsFixedValue(parameter))
            return _isFixedParameterEditRepository;

         return _standardParameterEditRepository;
      }

      private void configureRepository(BaseEdit activeEditor, ParameterDTO parameter) => _unitControl.UpdateUnitsFor(activeEditor, parameter);

      private RepositoryItem createBuildModeRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.ParameterBuildModes);
         return repository;
      }

      private void onRenameClick(ButtonPressedEventArgs buttonPressedEventArgs, ParameterDTO parameterDTOBuilderExplicit)
      {
         OnEvent(() =>
         {
            var pressedButton = buttonPressedEventArgs.Button;
            if (pressedButton.Kind.Equals(ButtonPredefines.Ellipsis))
            {
               _presenter.Rename(parameterDTOBuilderExplicit);
            }
         });
      }

      private RepositoryItemButtonEdit createNameEdit()
      {
         var buttonRepository = new RepositoryItemButtonEdit { TextEditStyle = TextEditStyles.DisableTextEditor };
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Ellipsis;
         buttonRepository.Buttons[0].ToolTip = ToolTips.ParameterList.NewParameterName;
         return buttonRepository;
      }

      private void onResetValue(ParameterDTO parameter)
      {
         OnEvent(() =>
         {
            _presenter.ResetValueFor(parameter);
            gridView.CloseEditor();
         });
      }

      private RepositoryItem createDimensionRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.GetDimensions());
         return repository;
      }

      public void BindTo(IReadOnlyList<ParameterDTO> parameterDTOs)
      {
         chkShowAdvancedParameter.Checked = _presenter.ShowAdvancedParameters;
         updateGroupVisibility();

         _gridViewBinder.BindToSource(parameterDTOs.ToBindingList());

         //do not select all row if only one parameter is available in list
         gridView.OptionsSelection.EnableAppearanceFocusedRow = parameterDTOs.Count > 1;

         _colModule.Visible = _presenter.HasModules();
         _colBuildingBlock.Visible = _presenter.HasBuildingBlocks();
         _colNavigate.Visible = _colBuildingBlock.Visible;
      }

      private void updateGroupVisibility()
      {
         chkGroupBy.Checked = _presenter.GroupParameters;
         _colGroup.XtraColumn.GroupIndex = _presenter.GroupParameters ? 0 : -1;
         _colGroup.Visible = _presenter.GroupParameters;
      }

      public void AttachPresenter(IEditParametersInContainerPresenter presenter) => _presenter = presenter;

      public EditParameterMode EditMode
      {
         get => _editMode;
         set
         {
            _editMode = value;
            switch (value)
            {
               case EditParameterMode.All:
                  _colValue.ReadOnly = false;
                  _colFormula.Visible = false;
                  _colRHSFormula.Visible = false;
                  _colButtons.Visible = true;
                  _colDimension.ReadOnly = false;
                  _colPersistable.Visible = false;
                  layoutControlItemAddParameter.Visibility = LayoutVisibility.Always;
                  layoutControlItemLoadParameter.Visibility = LayoutVisibility.Always;
                  btnAddParameter.Enabled = true;
                  btnLoadParameter.Enabled = true;
                  _nameButtonRepository.Buttons[0].Visible = true;
                  break;
               case EditParameterMode.ValuesOnly:
                  _colValue.ReadOnly = false;
                  _colPersistable.Visible = true;
                  _colFormula.Visible = true;
                  _colRHSFormula.Visible = false;
                  _colButtons.Visible = false;
                  _colDimension.ReadOnly = true;
                  btnAddParameter.Enabled = false;
                  btnLoadParameter.Enabled = false;
                  layoutControlItemAddParameter.Visibility = LayoutVisibility.Never;
                  layoutControlItemLoadParameter.Visibility = LayoutVisibility.Never;
                  splitContainerControl.PanelVisibility = SplitPanelVisibility.Panel1;
                  _nameButtonRepository.Buttons[0].Visible = false;
                  break;
               case EditParameterMode.ReadOnly:
                  _colValue.ReadOnly = true;
                  _colButtons.Visible = false;
                  _colPersistable.Visible = false;
                  _colFormula.Visible = true;
                  _colRHSFormula.Visible = true;
                  _colDimension.ReadOnly = true;
                  btnAddParameter.Enabled = false;
                  btnLoadParameter.Enabled = false;
                  layoutControlItemAddParameter.Visibility = LayoutVisibility.Never;
                  layoutControlItemLoadParameter.Visibility = LayoutVisibility.Never;
                  _nameButtonRepository.Buttons[0].Visible = false;
                  break;
               default:
                  throw new ArgumentOutOfRangeException("EditMode");
            }
         }
      }

      public bool ShowBuildMode
      {
         set => _colBuildMode.Visible = value;
         get => _colBuildMode.Visible;
      }

      public string ContainerPath
      {
         set => tbContainerPath.Text = value;
      }

      public void SetEditParameterView(IView subView)
      {
         splitContainerControl.PanelVisibility = subView != null ? SplitPanelVisibility.Both : SplitPanelVisibility.Panel1;
         splitContainerControl.Panel2.FillWith(subView);
      }

      public void RefreshList() => _gridViewBinder.Rebind();

      public void Select(ParameterDTO parameterToSelect)
      {
         var rowHandle = _gridViewBinder.RowHandleFor(parameterToSelect);
         gridView.FocusedRowHandle = rowHandle;
         gridView.GetSelectedRows().Each(gridView.UnselectRow);
         gridView.SelectRow(rowHandle);
      }

      public void CopyToClipBoard(string text) => Clipboard.SetText(text);

      public void ShowIndividualSelection(bool show)
      {
         layoutControlItemSelectIndividual.Visibility = LayoutVisibilityConvertor.FromBoolean(show);

         if (show)
            _screenBinder.BindToSource(_presenter);
      }

      private void gridViewRowChanged(FocusedRowChangedEventArgs e)
      {
         var selectedItem = _gridViewBinder.ElementAt(e.FocusedRowHandle);
         if (selectedItem == null)
            return;
         _presenter.Select(selectedItem);
      }

      //only to copy content to clipboard
      private void gridViewKeyDown(object sender, KeyEventArgs e)
      {
         if (!e.Control)
         {
            e.Handled = false;
            return;
         }

         switch (e.KeyCode)
         {
            case Keys.C:
               _presenter.CopyToClipBoard(_gridViewBinder.FocusedElement);
               break;
            case Keys.V:
               _presenter.PasteFromClipBoard();
               e.Handled = true;
               break;
            case Keys.X:
               _presenter.CutToClipBoard(_gridViewBinder.FocusedElement);
               e.Handled = true;
               break;
            default:
               //no treatment for this key
               e.Handled = false;
               break;
         }
      }

      private void showAdvancedParameterChanged() => _presenter.ShowAdvancedParameters = chkShowAdvancedParameter.Checked;

      private void groupByChanged() => _presenter.GroupParameters = chkGroupBy.Checked;

      private void disposeBinders()
      {
         _screenBinder.Dispose();
         _valueOriginBinder.Dispose();
         _gridViewBinder.Dispose();
      }
   }
}