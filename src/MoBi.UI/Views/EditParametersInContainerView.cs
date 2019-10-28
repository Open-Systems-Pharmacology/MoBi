using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
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
      private IGridViewColumn _colDescription;
      private readonly UxComboBoxUnit<ParameterDTO> _unitControl;
      private readonly RepositoryItemTextEdit _stantdardParameterEditRepository = new RepositoryItemTextEdit();
      private RepositoryItemButtonEdit _isFixedParameterEditRepository;
      private readonly RepositoryItemButtonEdit _addRemoveButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
      private RepositoryItemButtonEdit _nameButtonRepository;
      private readonly UxRepositoryItemCheckEdit _checkBoxRepository;

      public EditParametersInContainerView(IToolTipCreator toolTipCreator, ValueOriginBinder<ParameterDTO> valueOriginBinder)
      {
         _toolTipCreator = toolTipCreator;
         _valueOriginBinder = valueOriginBinder;
         InitializeComponent();
         var toolTipController = new ToolTipController {AllowHtmlText = true};
         _unitControl = new UxComboBoxUnit<ParameterDTO>(gridControl);
         _gridView.HiddenEditor += (o, e) => hideEditor();
         gridControl.KeyDown += gridViewKeyDown;
         gridControl.ToolTipController = toolTipController;
         //specific grid settings for parameter
         _gridView.ShowRowIndicator = true;
         _gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         _gridView.OptionsView.ShowGroupPanel = false;
         splitContainerControl.CollapsePanel = SplitCollapsePanel.Panel2;
         splitContainerControl.PanelVisibility = SplitPanelVisibility.Panel1;
         _gridView.GroupFormat = "[#image]{1}";
         _gridView.EndGrouping += (o, e) => _gridView.ExpandAllGroups();

         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;

         _checkBoxRepository = new UxRepositoryItemCheckEdit(_gridView);
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btAddParameter.InitWithImage(ApplicationIcons.Add, AppConstants.Captions.AddParameter);
         btLoadParameter.InitWithImage(ApplicationIcons.PKMLLoad, AppConstants.Captions.LoadParameter);
         layoutControlItemAddParameter.AdjustLargeButtonSize();
         layoutControlItemLoadParameter.AdjustLargeButtonSize();
         chkShowAdvancedParameter.Text = AppConstants.Captions.ShowAdvancedParameters;
         chkGroupBy.Text = AppConstants.Captions.GroupParameters;
         _addRemoveButtonRepository.Buttons[0].ToolTip = ToolTips.ParameterList.DeleteParameter;
         _gridView.MultiSelect = true;
      }

      private void createResetButtonItem()
      {
         _isFixedParameterEditRepository = new UxRepositoryItemButtonImage(ApplicationIcons.Reset, ToolTips.ResetParameterToolTip) {TextEditStyle = TextEditStyles.Standard};
         _isFixedParameterEditRepository.Buttons[0].IsLeft = true;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         createResetButtonItem();

         _gridViewBinder = new GridViewBinder<ParameterDTO>(_gridView) {ValidationMode = ValidationMode.LeavingCell};

         _nameButtonRepository = createNameEdit();
         _nameButtonRepository.ButtonClick += (o, e) => onRenameClick(e, _gridViewBinder.FocusedElement);
         _gridViewBinder.Bind(dto => dto.Name)
            .WithRepository(dto => _nameButtonRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _colValue = _gridViewBinder.Bind(dto => dto.Value)
            .WithFormat(dto => dto.ParameterFormatter())
            .WithRepository(repositoryForValue)
            .WithEditorConfiguration(configureRepository)
            .WithToolTip(ToolTips.ParameterList.SetParameterValue)
            .WithOnValueUpdating(onParameterValueSet)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

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

         _colDescription = _gridViewBinder.Bind(dto => dto.Description)
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
            .WithRepository(dto => _addRemoveButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _addRemoveButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveParameter(_gridViewBinder.FocusedElement));


         _gridView.FocusedRowChanged += (o, e) => OnEvent(gridViewRowChanged, e);

         chkShowAdvancedParameter.CheckedChanged += (o, e) => OnEvent(showAdvancedParameterChanged);
         chkGroupBy.CheckStateChanged += (o, e) => OnEvent(groupByChanged);
         btAddParameter.Click += (o, e) => OnEvent(_presenter.AddParameter);
         btLoadParameter.Click += (o, e) => OnEvent(_presenter.LoadParameter);
      }

      private void onIsFavoriteSet(ParameterDTO parameterDTO, bool newValue)
      {
         _presenter.SetIsFavorite(parameterDTO, newValue);
      }

      private void onIsPersistableSet(ParameterDTO parameterDTO, bool newValue)
      {
         _presenter.SetIsPersistable(parameterDTO, newValue);
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != _gridView.GridControl) return;


         var parameterDTO = _gridViewBinder.ElementAt(e);
         if (parameterDTO == null) return;

         //check if subclass want to display a tool tip as well
         var hi = _gridView.CalcHitInfo(e.ControlMousePosition);
         var superToolTip = GetToolTipFor(parameterDTO, hi);

         //An object that uniquely identifies a row cell
         e.Info = new ToolTipControlInfo(parameterDTO, string.Empty) {SuperTip = superToolTip, ToolTipType = ToolTipType.SuperTip};
      }

      protected virtual SuperToolTip GetToolTipFor(ParameterDTO parameterDTO, GridHitInfo hi)
      {
         return _toolTipCreator.ToolTipFor(parameterDTO);
      }

      private void setParameterUnit(ParameterDTO parameter, Unit unit)
      {
         OnEvent(() =>
         {
            _gridView.CloseEditor();
            _presenter.SetParamterUnit(parameter, unit);
         });
      }

      private void onSetDimension(ParameterDTO parameter, IDimension newValue)
      {
         _presenter.SetDimensionFor(parameter, newValue);
      }

      private void onParameterValueSet(ParameterDTO parameter, PropertyValueSetEventArgs<double> e)
      {
         OnEvent(() => _presenter.OnParameterValueSet(parameter, e.NewValue));
      }

      private void onParameterValueOriginSet(ParameterDTO parameter, ValueOrigin valueOrigin)
      {
         OnEvent(() => _presenter.OnParameterValueOriginSet(parameter, valueOrigin));
      }

      private RepositoryItem repositoryForValue(ParameterDTO parameter)
      {
         if (_presenter.IsFixedValue(parameter))
            return _isFixedParameterEditRepository;

         return _stantdardParameterEditRepository;
      }

      private void configureRepository(BaseEdit activeEditor, ParameterDTO parameter)
      {
         _unitControl.UpdateUnitsFor(activeEditor, parameter);
      }

      private RepositoryItem createBuildModeRepository()
      {
         var repository = new UxRepositoryItemComboBox(_gridView);
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
         var buttonRepository = new RepositoryItemButtonEdit {TextEditStyle = TextEditStyles.DisableTextEditor};
         buttonRepository.Buttons[0].Kind = ButtonPredefines.Ellipsis;
         buttonRepository.Buttons[0].ToolTip = ToolTips.ParameterList.NewParameterName;
         return buttonRepository;
      }

      private void onResetValue(ParameterDTO parameter)
      {
         OnEvent(() =>
         {
            _presenter.ResetValueFor(parameter);
            _gridView.CloseEditor();
         });
      }

      private RepositoryItem createDimensionRepository()
      {
         var repository = new UxRepositoryItemComboBox(_gridView);
         repository.FillComboBoxRepositoryWith(_presenter.GetDimensions());
         return repository;
      }

      public void BindTo(IEnumerable<ParameterDTO> parameters)
      {
         chkShowAdvancedParameter.Checked = _presenter.ShowAdvancedParameters;
         updateGroupVisibility();

         _gridViewBinder.BindToSource(parameters.ToBindingList());

         //do not select all row if only one parameter is available in list
         _gridView.OptionsSelection.EnableAppearanceFocusedRow = (parameters.Count() > 1);
      }

      private void updateGroupVisibility()
      {
         chkGroupBy.Checked = _presenter.GroupParameters;
         _colGroup.XtraColumn.GroupIndex = _presenter.GroupParameters ? 0 : -1;
         _colGroup.Visible = _presenter.GroupParameters;
      }

      public void AttachPresenter(IEditParametersInContainerPresenter presenter)
      {
         _presenter = presenter;
      }

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
                  btAddParameter.Visible = true;
                  btAddParameter.Enabled = true;
                  btLoadParameter.Visible = true;
                  btLoadParameter.Enabled = true;
                  _nameButtonRepository.Buttons[0].Visible = true;
                  break;
               case EditParameterMode.ValuesOnly:
                  _colValue.ReadOnly = false;
                  _colPersistable.Visible = true;
                  _colFormula.Visible = true;
                  _colRHSFormula.Visible = false;
                  _colButtons.Visible = false;
                  _colDimension.ReadOnly = true;
                  btAddParameter.Enabled = false;
                  btLoadParameter.Enabled = false;
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
                  btAddParameter.Enabled = false;
                  btLoadParameter.Enabled = false;
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
         get { return _colBuildMode.Visible; }
      }

      public string ParentName
      {
         set => lblParentName.Text = value.FormatForLabel(checkCase: false);
      }

      public void SetEditParameterView(IView subView)
      {
         splitContainerControl.PanelVisibility = subView != null ? SplitPanelVisibility.Both : SplitPanelVisibility.Panel1;
         splitContainerControl.Panel2.FillWith(subView);
      }

      public void RefreshList()
      {
         _gridViewBinder.Rebind();
      }

      public void Select(ParameterDTO parameterToSelect)
      {
         var rowHandle = _gridViewBinder.RowHandleFor(parameterToSelect);
         _gridView.FocusedRowHandle = rowHandle;
      }

      private void gridViewRowChanged(FocusedRowChangedEventArgs e)
      {
         var selectedItem = _gridViewBinder.ElementAt(e.FocusedRowHandle);
         if (selectedItem == null) return;
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

      private void showAdvancedParameterChanged()
      {
         _presenter.ShowAdvancedParameters = chkShowAdvancedParameter.Checked;
      }

      private void groupByChanged()
      {
         _presenter.GroupParameters = chkGroupBy.Checked;
      }
   }
}