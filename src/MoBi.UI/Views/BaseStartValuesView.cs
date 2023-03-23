using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace MoBi.UI.Views
{
   public abstract partial class BaseStartValuesView<TStartValue, T> : BaseUserControl, IStartValuesView<TStartValue> where TStartValue : BreadCrumbsDTO<T>, IStartValueDTO where T : IValidatable, INotifier
   {
      private readonly ValueOriginBinder<TStartValue> _valueOriginBinder;
      protected readonly GridViewBinder<TStartValue> _gridViewBinder;
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>();
      protected IStartValuesPresenter<TStartValue> _presenter;

      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
      private readonly IList<string> _pathValues;
      private readonly RepositoryItemComboBox _pathRepositoryItemComboBox;

      private readonly ScreenBinder<IStartValuesPresenter<TStartValue>> _screenBinder;
      private IGridViewColumn<TStartValue> _deleteColumn;
      public bool CanCreateNewFormula { get; set; }

      protected BaseStartValuesView(ValueOriginBinder<TStartValue> valueOriginBinder)
      {
         _valueOriginBinder = valueOriginBinder;
         InitializeComponent();
         configureGridView();
         _gridViewBinder = new GridViewBinder<TStartValue>(gridView);
         _pathValues = new List<string>();

         _pathRepositoryItemComboBox = new RepositoryItemComboBox {TextEditStyle = TextEditStyles.Standard};
         _pathRepositoryItemComboBox.SelectedValueChanged += (o, e) => gridView.PostEditor();

         checkFilterModified.Text = AppConstants.Captions.ShowOnlyChangedStartValues;
         checkFilterModified.CheckStateChanged += (o, e) => OnEvent(filterChanged);

         checkFilterNew.Text = AppConstants.Captions.ShowOnlyNewStartValues;
         checkFilterNew.CheckStateChanged += (o, e) => OnEvent(filterChanged);

         gridView.CustomRowFilter += gridViewOnCustomRowFilter;

         _screenBinder = new ScreenBinder<IStartValuesPresenter<TStartValue>>();
      }

      public void HideIsPresentView()
      {
         layoutItemIsPresent.Visibility = LayoutVisibility.Never;
      }

      public void HideNegativeValuesAllowedView()
      {
         layoutItemNegativeValuesAllowed.Visibility = LayoutVisibility.Never;
      }

      private void gridViewOnCustomRowFilter(object sender, RowFilterEventArgs e)
      {
         var startValue = _gridViewBinder.SourceElementAt(e.ListSourceRow);
         if (startValue == null)
            return;

         e.Visible = _presenter.ShouldShow(startValue);

         if (!e.Visible)
            e.Handled = true;
      }

      private void filterChanged()
      {
         gridView.RefreshData();
      }

      public IReadOnlyList<TStartValue> SelectedStartValues
      {
         get { return gridView.GetSelectedRows().Select(rowHandle => _gridViewBinder.ElementAt(rowHandle)).ToList(); }
      }

      public IReadOnlyList<TStartValue> VisibleStartValues => gridView.DataController.GetAllFilteredAndSortedRows().Cast<TStartValue>().ToList();

      public void AddDeleteStartValuesView(IView view)
      {
         panelDeleteStartValues.FillWith(view);
      }

      public void HideRefreshStartValuesView()
      {
         layoutItemRefreshStartValues.Visibility = LayoutVisibility.Never;
      }

      public void HideDeleteView()
      {
         layoutItemDelete.Visibility = LayoutVisibility.Never;
      }

      public void HideLegend()
      {
         layoutItemLegend.Visibility = LayoutVisibility.Never;
      }

      public void HideDeleteColumn()
      {
         _deleteColumn.AsHidden();
      }

      public void AddLegendView(IView view)
      {
         legendPanel.FillWith(view);
      }

      public void HideSubPresenterGrouping()
      {
         emptySpaceItem.Visibility = LayoutVisibility.Never;
         layoutGroupPanel.GroupBordersVisible = false;
      }

      public void RefreshData()
      {
         gridView.RefreshData();
      }

      public TStartValue FocusedStartValue
      {
         get => _gridViewBinder.FocusedElement;
         set
         {
            if (value == null)
               return;

            gridView.FocusedRowHandle = _gridViewBinder.RowHandleFor(value);
         }
      }

      protected virtual bool IsEditable(GridColumn column)
      {
         return true;
      }

      protected abstract void DoInitializeBinding();

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         initPathElementColumn(dto => dto.PathElement0, Captions.PathElement(0));
         initPathElementColumn(dto => dto.PathElement1, Captions.PathElement(1));
         initPathElementColumn(dto => dto.PathElement2, Captions.PathElement(2));
         initPathElementColumn(dto => dto.PathElement3, Captions.PathElement(3));
         initPathElementColumn(dto => dto.PathElement4, Captions.PathElement(4));
         initPathElementColumn(dto => dto.PathElement5, Captions.PathElement(5));
         initPathElementColumn(dto => dto.PathElement6, Captions.PathElement(6));
         initPathElementColumn(dto => dto.PathElement7, Captions.PathElement(7));
         initPathElementColumn(dto => dto.PathElement8, Captions.PathElement(8));
         initPathElementColumn(dto => dto.PathElement9, Captions.PathElement(9));

         DoInitializeBinding();

         _deleteColumn = _gridViewBinder.AddUnboundColumn();

         _deleteColumn.WithCaption(OSPSuite.UI.UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(dto => _removeButtonRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => removeStartValue(_gridViewBinder.FocusedElement));


         _screenBinder.Bind(x => x.IsModifiedFilterOn).To(checkFilterModified);
         _screenBinder.Bind(x => x.IsNewFilterOn).To(checkFilterNew);
         _screenBinder.BindToSource(_presenter);
      }

      protected void InitializeValueOriginBinding()
      {
         _valueOriginBinder.InitializeBinding(_gridViewBinder, (o, e) => OnEvent(() => _presenter.SetValueOrigin(o, e)));
      }

      private void removeStartValue(TStartValue elementToRemove)
      {
         _presenter.RemoveStartValue(elementToRemove);
      }

      public GridControl GridControl => gridControl;

      public GridView GridView => gridView;

      public GridViewBinder<TStartValue> Binder => _gridViewBinder;

      public void OnPathElementSet(TStartValue startValue, PropertyValueSetEventArgs<string> eventArgs, int index)
      {
         if (index == AppConstants.NotFoundIndex)
            return;
         _presenter.UpdateStartValueContainerPath(startValue, index, eventArgs.NewValue);
      }

      private void initPathElementColumn(Expression<Func<TStartValue, string>> expression, string caption)
      {
         var index = _pathElementsColumns.Count;

         _pathElementsColumns.Add(
            _gridViewBinder.Bind(expression)
               .WithCaption(caption)
               .WithRepository(x => _pathRepositoryItemComboBox)
               .WithOnValueUpdating((o, e) => OnEvent(() => OnPathElementSet(o, e, index))));
      }

      public void BindTo(IEnumerable<TStartValue> startValues)
      {
         _gridViewBinder.BindToSource(startValues);
      }

      public void InitializePathColumns()
      {
         _pathRepositoryItemComboBox.FillComboBoxRepositoryWith(_pathValues);
         initColumnVisibilty();
      }

      public void AddPathItems(IEnumerable<string> pathValues)
      {
         pathValues.Each(x =>
         {
            if (!_pathValues.Contains(x))
               _pathValues.Add(x);
         });
      }

      public void ClearPathItems()
      {
         _pathValues.Clear();
      }

      private void initColumnVisibilty()
      {
         for (var i = 0; i < _pathElementsColumns.Count; i++)
         {
            _pathElementsColumns[i].Visible = _presenter.HasAtLeastTwoDistinctValues(i);
         }
      }

      protected void OnNameSet(TStartValue startValueDTO, PropertyValueSetEventArgs<string> eventArgs)
      {
         _presenter.UpdateStartValueName(startValueDTO, eventArgs.NewValue);
      }

      private void configureGridView()
      {
         gridView.RowStyle += (o, e) => OnEvent(updateRowStyle, o, e);
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsSelection.MultiSelect = true;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
      }

      private void updateRowStyle(object sender, RowStyleEventArgs e)
      {
         var startValue = _gridViewBinder.ElementAt(e.RowHandle);
         if (startValue == null) return;
         var color = _presenter.BackgroundColorFor(startValue);
         if (!_presenter.IsColorDefault(color))
            gridView.AdjustAppearance(e, color);
      }

      protected void OnFormulaButtonClick(object sender, ButtonPressedEventArgs e)
      {
         if (!e.Button.Kind.Equals(ButtonPredefines.Plus)) return;

         var startValueDTO = _gridViewBinder.ElementAt(gridView.FocusedRowHandle);
         _presenter.AddNewFormula(startValueDTO);
         var comboBox = sender as ComboBoxEdit;
         if (comboBox != null)
            comboBox.FillComboBoxEditorWith(_presenter.AllFormulas());
      }

      protected RepositoryItem CreateFormulaRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.AllFormulas());
         if (CanCreateNewFormula)
         {
            repository.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
            repository.ButtonClick += OnFormulaButtonClick;
         }

         return repository;
      }

      public virtual void AddRefreshStartValuesView(IView view)
      {
         panelRefreshStartValues.FillWith(view);
      }
   }
}