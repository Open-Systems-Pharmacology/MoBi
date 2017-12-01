using System;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class EditTableFormulaView : BaseUserControl, IEditTableFormulaView
   {
      private IEditTableFormulaPresenter _presenter;
      private readonly GridViewBinder<DTOValuePoint> _gridBinder;
      private bool _readOnly;
      private IGridViewColumn _colX;
      private IGridViewColumn _colY;
      private IGridViewColumn _colRestartSolver;
      private IGridViewColumn _colButtons;
      private readonly ScreenBinder<TableFormulaBuilderDTO> _screenBinder;
      private UxRepositoryItemCheckEdit _uxRepositoryItemCheckEdit;

      public EditTableFormulaView()
      {
         InitializeComponent();
         _gridBinder = new GridViewBinder<DTOValuePoint>(gridView);
         _screenBinder = new ScreenBinder<TableFormulaBuilderDTO>();
      }

      private void onPopUpMenuShowing(PopupMenuShowingEventArgs e)
      {
         if (e.MenuType != GridMenuType.Column) return;

         var menu = e.Menu as GridViewColumnMenu;
         if (menu == null) return;
         if (menu.Column == null) return;

         ValuePointColumn columnIndex;
         if (menu.Column == _colX.XtraColumn)
            columnIndex = ValuePointColumn.X;
         else if (menu.Column == _colY.XtraColumn)
            columnIndex = ValuePointColumn.Y;
         else
            return;

         menu.Items.Clear();

         foreach (var unit in _presenter.AvailableUnitsFor(columnIndex))
         {
            menu.Items.Add(createUnitMenuItem(columnIndex, unit));
         }
      }

      private DXMenuItem createUnitMenuItem(ValuePointColumn columnIndex, Unit unit)
      {
         var currentUnit = _presenter.UnitFor(columnIndex);
         var tag = new Tuple<ValuePointColumn, Unit>(columnIndex, unit);
         EventHandler handler = (o, e) => OnEvent(setUnit, o, e);

         if (Equals(currentUnit, unit))
            return new DXMenuCheckItem(unit.Name, check: true, image: null, checkedChanged: handler) { Tag = tag };

         return new DXMenuItem(unit.Name, handler) { Tag = tag };
      }

      private void setUnit(object sender, EventArgs e)
      {
         var item = sender as DXMenuItem;
         if (item == null) return;

         var tag = item.Tag as Tuple<ValuePointColumn, Unit>;
         if (tag == null) return;
         _presenter.SetUnit(tag.Item1, tag.Item2);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(dto => dto.UseDerivedValues)
            .To(chkUseDerivedValues)
            .OnValueUpdating += setUseDerivedValues;

         _colX = _gridBinder.AutoBind(dto => dto.XValue)
            .WithFormat(dto => dto.ValuePointXFormatter())
            .WithToolTip(ToolTips.Formula.X)
            .WithOnValueUpdating((o, e) => OnEvent(() => onXValueSet(o, e.NewValue)));

         _colY = _gridBinder.AutoBind(dto => dto.YValue)
            .WithFormat(dto => dto.ValuePointYFormatter())
            .WithToolTip(ToolTips.Formula.Y)
            .WithOnValueUpdating((o, e) => OnEvent(() => onYValueSet(o, e.NewValue)));

         _colRestartSolver = _gridBinder.Bind(dto => dto.RestartSolver)
            .WithRepository(dto => _uxRepositoryItemCheckEdit)
            .WithToolTip(ToolTips.Formula.RestartSolver)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH)
            .WithOnValueUpdating((o, e) => OnEvent(() => setRestartSolver(o, e.NewValue)));

         var buttonRepository = createButtonRepository();
         _colButtons = _gridBinder.AddUnboundColumn().WithRepository(dto => buttonRepository)
            .WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithFixedWidth(2 * OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         btnAddValuePoint.Click += (o, e) => OnEvent(_presenter.AddValuePoint);
         gridView.PopupMenuShowing += (o, e) => OnEvent(onPopUpMenuShowing, e);
         gridView.RowCountChanged += (o, e) => OnEvent(() => buttonRepository.Buttons[1].Enabled = _presenter.ShouldEnableDelete());
      }

      private void onYValueSet(DTOValuePoint valuePoint, double newYDisplayValue)
      {
         _presenter.SetYValue(valuePoint, newYDisplayValue);
      }

      private void onXValueSet(DTOValuePoint valuePoint, double newXDisplayValue)
      {
         _presenter.SetXValue(valuePoint, newXDisplayValue);
      }

      private void setUseDerivedValues(TableFormulaBuilderDTO dtoTableFormula, PropertyValueSetEventArgs<bool> e)
      {
         _presenter.SetUseDerivedValuesFor(dtoTableFormula, e.NewValue, e.OldValue);
      }

      private UxRepositoryItemButtonEdit createButtonRepository()
      {
         var buttonRepositoryItem = new UxRepositoryItemButtonEdit(ButtonPredefines.Plus);
         buttonRepositoryItem.Buttons[0].ToolTip = ToolTips.Formula.AddPoint;
         buttonRepositoryItem.AddButton(ButtonPredefines.Delete);
         buttonRepositoryItem.Buttons[1].ToolTip = ToolTips.Formula.DeletePoint;
         buttonRepositoryItem.ButtonClick += (o, e) => OnEvent(onButtonClick, e);
         return buttonRepositoryItem;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkUseDerivedValues.Text = AppConstants.Captions.UseDerivedValues;
         chkUseDerivedValues.ToolTip = ToolTips.Formula.UseDerivedValues;
         btnAddValuePoint.InitWithImage(ApplicationIcons.Add, text: AppConstants.Captions.AddValuePoint, toolTip: ToolTips.Formula.AddPoint);
         layoutItemAddValuePoint.AdjustButtonSize();
         _uxRepositoryItemCheckEdit = new UxRepositoryItemCheckEdit(gridView);
      }

      private void onButtonClick(ButtonPressedEventArgs e)
      {
         var button = e.Button;
         if (button.Kind.Equals(ButtonPredefines.Plus))
            _presenter.AddValuePoint();
         else
            _presenter.RemoveValuePoint(_gridBinder.FocusedElement);
      }

      private void setRestartSolver(DTOValuePoint valuePoint, bool newRestartSolverValue)
      {
         _presenter.SetRestartSolver(valuePoint, newRestartSolverValue);
      }

      public void AttachPresenter(IEditTableFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(TableFormulaBuilderDTO dtoTableFormulaBuilder)
      {
         _gridBinder.BindToSource(dtoTableFormulaBuilder.ValuePoints);
         _colX.Caption = dtoTableFormulaBuilder.XDisplayName;
         _colY.Caption = dtoTableFormulaBuilder.YDisplayName;
         _screenBinder.BindToSource(dtoTableFormulaBuilder);
      }

      public bool ReadOnly
      {
         get { return _readOnly; }
         set
         {
            _readOnly = value;
            setColumsReadOnly(_readOnly);
            btnAddValuePoint.Enabled = !_readOnly;
            chkUseDerivedValues.Enabled = !_readOnly;
         }
      }

      private void setColumsReadOnly(bool readOnly)
      {
         _colX.ReadOnly = readOnly;
         _colY.ReadOnly = readOnly;
         _colRestartSolver.ReadOnly = readOnly;
         _colButtons.Visible = !readOnly;
      }

      protected override int TopicId => HelpId.MoBi_ModelBuilding_ParametersTables;
   }
}