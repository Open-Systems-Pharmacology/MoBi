using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
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
         ribbonControl.ShowPageHeadersMode = ShowPageHeadersMode.Hide;
         ribbonControl.ShowQatLocationSelector = false;
         ribbonControl.Minimized = true;

         btnRefresh.ImageOptions.SetImage(ApplicationIcons.Refresh);
         btnDelete.ImageOptions.SetImage(ApplicationIcons.Delete);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         // this.svgImageCollection.Add("actions_addcircled", "image://svgimages/icon builder/actions_addcircled.svg");
         // this.svgImageCollection.Add("expandcollapse", "image://svgimages/outlook inspired/expandcollapse.svg");
         // this.svgImageCollection.Add("actions_checkcircled", "image://svgimages/icon builder/actions_checkcircled.svg");
         // this.svgImageCollection.Add("actions_deletecircled", "image://svgimages/icon builder/actions_deletecircled.svg");

         btnAllowNegativeValues.Caption = AppConstants.Captions.Allowed;
         btnAllowNegativeValues.RibbonStyle = RibbonItemStyles.SmallWithText;
         btnAllowNegativeValues.ImageOptions.SvgImage = svgImageCollection["expandcollapse"];

         btnNotAllowNegativeValues.Caption = AppConstants.Captions.NotAllowed;
         btnNotAllowNegativeValues.RibbonStyle = RibbonItemStyles.SmallWithText;
         btnNotAllowNegativeValues.ImageOptions.SvgImage = svgImageCollection["actions_addcircled"];
         
         ribbonGroupNegativeValues.Text = AppConstants.Captions.NegativeValues;

         btnPresent.Caption = AppConstants.Captions.Present;
         btnPresent.RibbonStyle = RibbonItemStyles.SmallWithText;
         btnPresent.ImageOptions.SvgImage = svgImageCollection["actions_checkcircled"];

         btnNotPresent.Caption = AppConstants.Captions.NotPresent;
         btnNotPresent.RibbonStyle = RibbonItemStyles.SmallWithText;
         btnNotPresent.ImageOptions.SvgImage = svgImageCollection["actions_deletecircled"];

         ribbonGroupPresence.Text = AppConstants.Captions.Presence;

         btnRefresh.RibbonStyle = RibbonItemStyles.SmallWithText;
         btnRefresh.Caption = AppConstants.Captions.RefreshValues;

         btnDelete.RibbonStyle = RibbonItemStyles.SmallWithText;
         btnDelete.Caption = Captions.Delete;

         ribbonGroupEdit.Text = Captions.Edit;

         FixRibbonLayoutHeight();
      }

      protected void FixRibbonLayoutHeight()
      {
         layoutItemRibbon.SizeConstraintsType = SizeConstraintsType.Custom;
         layoutItemRibbon.MaxSize = new Size(0, ribbonControl.Size.Height);
         layoutItemRibbon.MinSize = new Size(0, ribbonControl.Size.Height);
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
      public void HideDeleteButton() => btnDelete.Visibility = BarItemVisibility.Never;
      public void HidePresenceRibbon() => ribbonGroupPresence.Visible = false;
      public void HideButtonRibbon()
      {
         layoutItemRibbon.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
      }

      public void HideNegativeValuesRibbon() => ribbonGroupNegativeValues.Visible = false;

      protected IReadOnlyList<TPathAndValueEntity> selectedStartValues
      {
         get { return gridView.GetSelectedRows().Select(rowHandle => _gridViewBinder.ElementAt(rowHandle)).ToList(); }
      }

      public IReadOnlyList<TPathAndValueEntity> VisibleStartValues => gridView.DataController.GetAllFilteredAndSortedRows().Cast<TPathAndValueEntity>().ToList();

      public void DisablePathColumns()
      {
         _pathElementsColumns.Each(x => x.AsReadOnly());
         _colName.AsReadOnly();
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

      private void btnDeleteClick(object sender, ItemClickEventArgs e)
      {
         _presenter.Delete(selectedStartValues);
      }
   }
}