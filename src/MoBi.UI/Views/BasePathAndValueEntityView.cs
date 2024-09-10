using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Properties;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
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
   public abstract partial class BasePathAndValueEntityView<TPathAndValueEntity, T> : BaseUserControl, IPathAndValueEntitiesView<TPathAndValueEntity> where TPathAndValueEntity : PathAndValueEntityDTO<T>, IPathAndValueEntityDTO where T : PathAndValueEntity, IValidatable, INotifier
   {
      private readonly ValueOriginBinder<TPathAndValueEntity> _valueOriginBinder;
      protected readonly GridViewBinder<TPathAndValueEntity> _gridViewBinder;
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>();
      protected IExtendablePathAndValueBuildingBlockPresenter<TPathAndValueEntity> _presenter;
      protected IGridViewAutoBindColumn<TPathAndValueEntity, string> _colName;
      protected readonly UxComboBoxUnit<TPathAndValueEntity> _unitControl;

      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
      private readonly IList<string> _pathValues;
      private readonly RepositoryItemComboBox _pathRepositoryItemComboBox;

      private IGridViewColumn<TPathAndValueEntity> _deleteColumn;
      public bool CanCreateNewFormula { get; set; }
      public event Action IsPresentAction;
      public event Action IsNotPresentAction;
      public event Action NegativeValuesAllowedAction;
      public event Action NegativeValuesNotAllowedAction;
      public event Action RefreshAction;
      public event Action DeleteAction;
      private readonly PopupContainerControl _popupControl = new PopupContainerControl();
      private readonly RepositoryItemPopupContainerEdit _repositoryItemPopupContainerEdit = new RepositoryItemPopupContainerEdit();
      private IGridViewAutoBindColumn<TPathAndValueEntity, double?> _valueColumn;

      protected BasePathAndValueEntityView(ValueOriginBinder<TPathAndValueEntity> valueOriginBinder)
      {
         _valueOriginBinder = valueOriginBinder;
         InitializeComponent();
         configureGridView();
         _gridViewBinder = new GridViewBinder<TPathAndValueEntity>(gridView);
         _pathValues = new List<string>();
         _unitControl = new UxComboBoxUnit<TPathAndValueEntity>(gridControl);
         _pathRepositoryItemComboBox = new RepositoryItemComboBox { TextEditStyle = TextEditStyles.Standard };
         _pathRepositoryItemComboBox.SelectedValueChanged += (o, e) => gridView.PostEditor();
         _repositoryItemPopupContainerEdit.PopupControl = _popupControl;
         _repositoryItemPopupContainerEdit.QueryDisplayText += (o, e) => OnEvent(queryText, e);
         gridView.HiddenEditor += (o, e) => hideEditor();

         btnNotAllowNegativeValues.ImageOptions.SetImage(ApplicationIcons.Search);
         btnRefresh.ImageOptions.SetImage(ApplicationIcons.Search);
         btnDelete.ImageOptions.SetImage(ApplicationIcons.Search);
          

      }

      private void hideEditor() => _unitControl.Hide();

      private void queryText(QueryDisplayTextEventArgs e)
      {
         var distributedParameter = _gridViewBinder.FocusedElement;
         if (distributedParameter == null)
            return;
         e.DisplayText = distributedParameter.PathAndValueEntityFormatter().Format(distributedParameter.Value);
      }

      private RepositoryItem valueRepositoryFor(TPathAndValueEntity parameterDTO)
      {
         if (parameterDTO.IsDistributed)
            return _repositoryItemPopupContainerEdit;

         return _valueColumn.DefaultRepository();
      }

      protected IGridViewAutoBindColumn<TPathAndValueEntity, double?> BindValueColumn(Expression<Func<TPathAndValueEntity, double?>> propertyToBindTo)
      {
         _valueColumn = _gridViewBinder.AutoBind(propertyToBindTo).WithRepository(valueRepositoryFor).WithEditorConfiguration(ConfigureValueRepository);
         return _valueColumn;
      }

      protected virtual void ConfigureValueRepository(BaseEdit activeEditor, TPathAndValueEntity individualParameter)
      {
         if (individualParameter.IsDistributed)
         {
            _presenter.EditDistributedParameter(individualParameter);
            return;
         }

         _unitControl.UpdateUnitsFor(activeEditor, individualParameter);
      }

      protected virtual void DoInitializeBinding()
      {
         _colName = _gridViewBinder.AutoBind(dto => dto.Name)
            .WithCaption(NameColumnCaption)
            .WithOnValueUpdating((o, e) => OnEvent(() => OnNameSet(o, e)));

         //to put the name in the first column
         _colName.XtraColumn.VisibleIndex = 0;
      }

      public abstract string NameColumnCaption { get; }

      public void HideRefreshButton() => btnRefresh.Visibility = BarItemVisibility.Never;
      public void HideIsPresentButton() => btnPresent.Visibility = BarItemVisibility.Never;
      public void HideNegativeValuesAllowedButton() => btnAllowNegativeValues.Visibility = BarItemVisibility.Never;
      public void HideNegativeValuesNotAllowedButton() => btnNotAllowNegativeValues.Visibility = BarItemVisibility.Never;
      public void HideIsNotPresentButton() => btnNotPresent.Visibility = BarItemVisibility.Never;
      public void HideDeleteButton() => btnDelete.Visibility = BarItemVisibility.Never;

      public IReadOnlyList<TPathAndValueEntity> SelectedStartValues
      {
         get { return gridView.GetSelectedRows().Select(rowHandle => _gridViewBinder.ElementAt(rowHandle)).ToList(); }
      }

      public IReadOnlyList<TPathAndValueEntity> VisibleStartValues => gridView.DataController.GetAllFilteredAndSortedRows().Cast<TPathAndValueEntity>().ToList();

      public void DisablePathColumns()
      {
         _pathElementsColumns.Each(x => x.AsReadOnly());
         _colName.AsReadOnly();
      }

      public void AddDeleteValuesPresenter(IApplyToSelectionPresenter presenter)
      {
         btnDelete.ItemClick += (s, e) => { presenter.PerformSelectionHandler(); };
      }

      public void HideDeleteColumn() => _deleteColumn.AsHidden();

      public void RefreshData() => gridView.RefreshData();

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

      protected virtual bool IsEditable(GridColumn column) => true;

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
      }

      public void AddDistributedParameterView(IView view) => _popupControl.FillWith(view);

      public void HideValueOriginColumn() => _valueOriginBinder.ValueOriginColumn.AsHidden().WithShowInColumnChooser(true);

      public void RefreshForUpdatedEntity()
      {
         gridView.CloseEditor();
         gridView.ShowEditor();
      }

      protected void InitializeValueOriginBinding() => _valueOriginBinder.InitializeBinding(_gridViewBinder, (o, e) => OnEvent(() => _presenter.SetValueOrigin(o, e)));

      private void removeStartValue(TPathAndValueEntity elementToRemove) => _presenter.RemovePathAndValueEntity(elementToRemove);

      public GridControl GridControl => gridControl;

      public GridView GridView => gridView;

      public GridViewBinder<TPathAndValueEntity> Binder => _gridViewBinder;

      public void OnPathElementSet(TPathAndValueEntity pathAndValueEntity, PropertyValueSetEventArgs<string> eventArgs, int index)
      {
         if (index == AppConstants.NotFoundIndex)
            return;
         _presenter.UpdatePathAndValueEntityContainerPath(pathAndValueEntity, index, eventArgs.NewValue);
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

      public void BindTo(IEnumerable<TPathAndValueEntity> startValues) => _gridViewBinder.BindToSource(startValues);

      public void InitializePathColumns()
      {
         _pathRepositoryItemComboBox.FillComboBoxRepositoryWith(_pathValues);
         initColumnVisibility();
      }

      public void AddPathItems(IEnumerable<string> pathValues)
      {
         pathValues.Each(x =>
         {
            if (!_pathValues.Contains(x))
               _pathValues.Add(x);
         });
      }

      public void ClearPathItems() => _pathValues.Clear();

      private void initColumnVisibility()
      {
         for (var i = 0; i < _pathElementsColumns.Count; i++)
         {
            var shouldShow = _presenter.HasAtLeastOneValue(i);
            var appearing = !_pathElementsColumns[i].Visible && shouldShow;
            _pathElementsColumns[i].Visible = shouldShow;

            if (appearing)
               initializePathColumnVisibleIndex(i);
         }
      }

      // When hiding and showing columns automatically based on presence of path elements
      // we have to override how the visibility index is set. That's because visibility index
      // of a column is updated as other columns are hidden. So, if you iterate in order and hide all columns
      // then their internal hidden visibility index will be 0. If you iterate in order and show them,
      // they appear in the reverse order
      private void initializePathColumnVisibleIndex(int i)
      {
         var previousColumn = i > 0 ? _pathElementsColumns[i - 1] : _colName;

         // make this appearing column display to the right the previous column
         _pathElementsColumns[i].XtraColumn.VisibleIndex = previousColumn.XtraColumn.VisibleIndex + 1;
      }

      protected void OnNameSet(TPathAndValueEntity startValueDTO, PropertyValueSetEventArgs<string> eventArgs)
         => _presenter.UpdatePathAndValueEntityName(startValueDTO, eventArgs.NewValue);

      private void configureGridView()
      {
         gridView.OptionsSelection.MultiSelect = true;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
      }

      protected void OnFormulaButtonClick(object sender, ButtonPressedEventArgs e)
      {
         if (!e.Button.Kind.Equals(ButtonPredefines.Plus)) return;

         var startValueDTO = _gridViewBinder.ElementAt(gridView.FocusedRowHandle);
         _presenter.AddNewFormula(startValueDTO);

         if (sender is ComboBoxEdit comboBox)
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

      private void btnPresent_ItemClick(object sender, ItemClickEventArgs e) => IsPresentAction?.Invoke();

      private void btnDelete_ItemClick(object sender, ItemClickEventArgs e) => DeleteAction?.Invoke();

      private void btnNotPresent_ItemClick(object sender, ItemClickEventArgs e) => IsNotPresentAction?.Invoke();

      private void btnAllowNegativeValues_ItemClick(object sender, ItemClickEventArgs e) => NegativeValuesAllowedAction?.Invoke();

      private void btnNotAllowNegativeValues_ItemClick(object sender, ItemClickEventArgs e) => NegativeValuesNotAllowedAction?.Invoke();

      private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e) => RefreshAction?.Invoke();
   }
}