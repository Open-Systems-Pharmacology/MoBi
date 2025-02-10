using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public partial class ExpressionProfileBuildingBlockView : BaseUserControl, IExpressionProfileBuildingBlockView
   {
      private IExpressionProfileBuildingBlockPresenter _presenter;
      private readonly GridViewBinder<ExpressionParameterDTO> _gridViewBinder;
      private readonly ScreenBinder<ExpressionProfileBuildingBlockDTO> _screenBinder = new ScreenBinder<ExpressionProfileBuildingBlockDTO>();
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>();
      private readonly UxComboBoxUnit<ExpressionParameterDTO> _unitControl;
      private readonly ValueOriginBinder<ExpressionParameterDTO> _valueOriginBinder;

      private readonly PopupContainerControl _popupControl = new PopupContainerControl();
      private readonly RepositoryItemPopupContainerEdit _repositoryItemPopupContainerEdit = new RepositoryItemPopupContainerEdit();
      private IGridViewBoundColumn<ExpressionParameterDTO, double?> _valueColumn;

      public ExpressionProfileBuildingBlockView(ValueOriginBinder<ExpressionParameterDTO> valueOriginBinder)
      {
         InitializeComponent();
         _valueOriginBinder = valueOriginBinder;
         _gridViewBinder = new GridViewBinder<ExpressionParameterDTO>(gridView);
         _unitControl = new UxComboBoxUnit<ExpressionParameterDTO>(gridControl);
         _repositoryItemPopupContainerEdit.PopupControl = _popupControl;
         _repositoryItemPopupContainerEdit.QueryDisplayText += (o, e) => OnEvent(queryText, e);
      }

      private void queryText(QueryDisplayTextEventArgs e)
      {
         var distributedParameter = _gridViewBinder.FocusedElement;
         if (distributedParameter == null)
            return;
         e.DisplayText = distributedParameter.ExpressionParameterFormatter().Format(distributedParameter.Value);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         initializeBinders();
         initializeValueOriginBinding();
         gridView.ShowColumnChooser = true;
      }

      private void initializeValueOriginBinding()
      {
         _valueOriginBinder.InitializeBinding(_gridViewBinder, (o, e) => OnEvent(() => _presenter.SetValueOrigin(o, e)));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         tbSpecies.Enabled = false;
         tbMoleculeName.Enabled = false;
         tbCategory.Enabled = false;
         tbPKSimVersion.Enabled = false;

         speciesControlItem.Text = Captions.Species.FormatForLabel();
         categoryControlItem.Text = Captions.Phenotype.FormatForLabel();
         pkSimVersionControlItem.Text = PKSimVersion.FormatForLabel(checkCase: false);

         btnLoadFromDatabase.InitWithImage(ApplicationIcons.ExpressionProfile, DatabaseQuery);
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      private void configureRepository(BaseEdit activeEditor, ExpressionParameterDTO expressionParameterDTO)
      {
         if (expressionParameterDTO.IsDistributed)
         {
            _presenter.EditDistributedParameter(expressionParameterDTO);
            return;
         }

         _unitControl.UpdateUnitsFor(activeEditor, expressionParameterDTO);
      }

      private void initializeBinders()
      {
         _screenBinder.Bind(dto => dto.Species).To(tbSpecies);
         _screenBinder.Bind(dto => dto.MoleculeName).To(tbMoleculeName);
         _screenBinder.Bind(dto => dto.Category).To(tbCategory);
         _screenBinder.Bind(dto => dto.PKSimVersion).To(tbPKSimVersion);
         
         _gridViewBinder.AutoBind(dto => dto.Name)
            .WithCaption(ParameterName).AsReadOnly();

         initializePathElementColumn(dto => dto.PathElement0, Captions.PathElement(0));
         initializePathElementColumn(dto => dto.PathElement1, Captions.PathElement(1));
         initializePathElementColumn(dto => dto.PathElement2, Captions.PathElement(2));
         initializePathElementColumn(dto => dto.PathElement3, Captions.PathElement(3));
         initializePathElementColumn(dto => dto.PathElement4, Captions.PathElement(4));
         initializePathElementColumn(dto => dto.PathElement5, Captions.PathElement(5));
         initializePathElementColumn(dto => dto.PathElement6, Captions.PathElement(6));
         initializePathElementColumn(dto => dto.PathElement7, Captions.PathElement(7));
         initializePathElementColumn(dto => dto.PathElement8, Captions.PathElement(8));
         initializePathElementColumn(dto => dto.PathElement9, Captions.PathElement(9));

         _valueColumn = _gridViewBinder.Bind(dto => dto.Value).WithCaption(Value)
            .WithOnValueUpdating(onExpressionParameterValueSet)
            .WithFormat(dto => dto.ExpressionParameterFormatter())
            .WithRepository(valueRepositoryFor)
            .WithEditorConfiguration(configureRepository);

         _unitControl.ParameterUnitSet += setParameterUnit;

         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => _presenter.SetFormula(o, e.NewValue.Formula));

         _gridViewBinder.Bind(x => x.Dimension).WithRepository(createDimensionRepository).AsReadOnly();

         gridView.HiddenEditor += (o, e) => hideEditor();
         btnLoadFromDatabase.Click += (ot, e) => OnEvent(_presenter.LoadExpressionFromPKSimDatabaseQuery);
      }

      private RepositoryItem createDimensionRepository(ExpressionParameterDTO arg)
      {
         var dimensionComboBoxRepository = new UxRepositoryItemComboBox(gridView);
         dimensionComboBoxRepository.FillComboBoxRepositoryWith(_presenter.DimensionsSortedByName());
         return dimensionComboBoxRepository;
      }

      protected RepositoryItem CreateFormulaRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.AllFormulas());

         repository.Buttons.Add(new EditorButton(ButtonPredefines.Ellipsis));
         repository.ButtonClick += OnFormulaButtonClick;

         return repository;
      }

      protected void OnFormulaButtonClick(object sender, ButtonPressedEventArgs e)
      {
         if (!e.Button.Kind.Equals(ButtonPredefines.Ellipsis)) return;

         var expressionParameterDTO = _gridViewBinder.ElementAt(gridView.FocusedRowHandle);
         _presenter.EditFormula(expressionParameterDTO);

         if (sender is ComboBoxEdit comboBox)
            comboBox.FillComboBoxEditorWith(_presenter.AllFormulas());
      }

      private void setParameterUnit(ExpressionParameterDTO expressionParameter, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            _presenter.SetUnit(expressionParameter, unit);
         });
      }

      private void onExpressionParameterValueSet(ExpressionParameterDTO expressionParameterDTO, PropertyValueSetEventArgs<double?> e)
      {
         OnEvent(() => _presenter.SetValue(expressionParameterDTO, e.NewValue));
      }

      private void initializePathElementColumn(Expression<Func<ExpressionParameterDTO, string>> expression, string caption)
      {
         _pathElementsColumns.Add(_gridViewBinder.Bind(expression).WithCaption(caption).AsReadOnly());
      }

      private void initColumnVisibility()
      {
         _pathElementsColumns.Each(column => column.Visible = _presenter.HasAtLeastOneValue(_pathElementsColumns.IndexOf(column)));
      }

      public void AttachPresenter(IExpressionProfileBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ExpressionProfileBuildingBlockDTO buildingBlockDTO)
      {
         _screenBinder.BindToSource(buildingBlockDTO);

         moleculeControlItem.Text = buildingBlockDTO.NameType.FormatForLabel();
         _gridViewBinder.BindToSource(buildingBlockDTO.ParameterDTOs);
         initColumnVisibility();
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
         _gridViewBinder.Dispose();
      }

      private RepositoryItem valueRepositoryFor(ExpressionParameterDTO parameterDTO)
      {
         if (parameterDTO.IsDistributed)
            return _repositoryItemPopupContainerEdit;

         return _valueColumn.DefaultRepository();
      }

      public void AddDistributedParameterView(IView view)
      {
         _popupControl.FillWith(view);
      }

      public void RefreshForUpdatedEntity()
      {
         gridView.CloseEditor();
         gridView.ShowEditor();
      }
   }
}