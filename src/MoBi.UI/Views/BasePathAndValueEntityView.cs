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
   public abstract partial class BasePathAndValueEntityView<TPathAndValueEntity, T> : BaseUserControl, IPathAndValueEntitiesView<TPathAndValueEntity> where TPathAndValueEntity : BreadCrumbsDTO<T>, IPathAndValueEntityDTO where T : IValidatable, INotifier
   {
      private readonly ValueOriginBinder<TPathAndValueEntity> _valueOriginBinder;
      protected readonly GridViewBinder<TPathAndValueEntity> _gridViewBinder;
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>();
      protected IStartValuesPresenter<TPathAndValueEntity> _presenter;

      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
      private readonly IList<string> _pathValues;
      private readonly RepositoryItemComboBox _pathRepositoryItemComboBox;

      private readonly ScreenBinder<IStartValuesPresenter<TPathAndValueEntity>> _screenBinder;
      private IGridViewColumn<TPathAndValueEntity> _deleteColumn;
      public bool CanCreateNewFormula { get; set; }

      protected BasePathAndValueEntityView(ValueOriginBinder<TPathAndValueEntity> valueOriginBinder)
      {
         _valueOriginBinder = valueOriginBinder;
         InitializeComponent();
         configureGridView();
         _gridViewBinder = new GridViewBinder<TPathAndValueEntity>(gridView);
         _pathValues = new List<string>();

         _pathRepositoryItemComboBox = new RepositoryItemComboBox {TextEditStyle = TextEditStyles.Standard};
         _pathRepositoryItemComboBox.SelectedValueChanged += (o, e) => gridView.PostEditor();

         checkFilterModified.Text = AppConstants.Captions.ShowOnlyChangedValues;
         checkFilterModified.CheckStateChanged += (o, e) => OnEvent(filterChanged);

         checkFilterNew.Text = AppConstants.Captions.ShowOnlyNewValues;
         checkFilterNew.CheckStateChanged += (o, e) => OnEvent(filterChanged);

         gridView.CustomRowFilter += gridViewOnCustomRowFilter;

         _screenBinder = new ScreenBinder<IStartValuesPresenter<TPathAndValueEntity>>();
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
         var pathAndValueEntity = _gridViewBinder.SourceElementAt(e.ListSourceRow);
         if (pathAndValueEntity == null)
            return;

         e.Visible = _presenter.ShouldShow(pathAndValueEntity);

         if (!e.Visible)
            e.Handled = true;
      }

      private void filterChanged()
      {
         gridView.RefreshData();
      }

      public IReadOnlyList<TPathAndValueEntity> SelectedStartValues
      {
         get { return gridView.GetSelectedRows().Select(rowHandle => _gridViewBinder.ElementAt(rowHandle)).ToList(); }
      }

      public IReadOnlyList<TPathAndValueEntity> VisibleStartValues => gridView.DataController.GetAllFilteredAndSortedRows().Cast<TPathAndValueEntity>().ToList();

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

      public TPathAndValueEntity FocusedStartValue
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

      private void removeStartValue(TPathAndValueEntity elementToRemove)
      {
         _presenter.RemoveStartValue(elementToRemove);
      }

      public GridControl GridControl => gridControl;

      public GridView GridView => gridView;

      public GridViewBinder<TPathAndValueEntity> Binder => _gridViewBinder;

      public void OnPathElementSet(TPathAndValueEntity pathAndValueEntity, PropertyValueSetEventArgs<string> eventArgs, int index)
      {
         if (index == AppConstants.NotFoundIndex)
            return;
         _presenter.UpdateStartValueContainerPath(pathAndValueEntity, index, eventArgs.NewValue);
      }

      private void initPathElementColumn(Expression<Func<TPathAndValueEntity, string>> expression, string caption)
      {
         var index = _pathElementsColumns.Count;

         _pathElementsColumns.Add(
            _gridViewBinder.Bind(expression)
               .WithCaption(caption)
               .WithRepository(x => _pathRepositoryItemComboBox)
               .WithOnValueUpdating((o, e) => OnEvent(() => OnPathElementSet(o, e, index))));
      }

      public void BindTo(IEnumerable<TPathAndValueEntity> startValues)
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

      protected void OnNameSet(TPathAndValueEntity startValueDTO, PropertyValueSetEventArgs<string> eventArgs)
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
         var pathAndValueEntity = _gridViewBinder.ElementAt(e.RowHandle);
         if (pathAndValueEntity == null) return;
         var color = _presenter.BackgroundColorFor(pathAndValueEntity);
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