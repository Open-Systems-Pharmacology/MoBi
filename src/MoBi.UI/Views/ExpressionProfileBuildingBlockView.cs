using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.Captions;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Extensions;
using DevExpress.XtraEditors;
using MoBi.Presentation.Formatters;
using DevExpress.XtraEditors.Controls;
using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
    public partial class ExpressionProfileBuildingBlockView : BaseUserControl, IExpressionProfileBuildingBlockView
   {
      private IExpressionProfileBuildingBlockPresenter _presenter;
      private readonly GridViewBinder<ExpressionParameterDTO> _gridViewBinder;
      private readonly ScreenBinder<ExpressionProfileBuildingBlockDTO> _screenBinder = new ScreenBinder<ExpressionProfileBuildingBlockDTO>();
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>();
      private readonly IDimensionFactory _dimensionFactory;
      private readonly UxRepositoryItemComboBox _dimensionComboBoxRepository;
      private readonly UxComboBoxUnit<ExpressionParameterDTO> _unitControl;

      public ExpressionProfileBuildingBlockView(IDimensionFactory dimensionFactory)
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<ExpressionParameterDTO>(gridView);
         _screenBinder.Bind(dto => dto.Species).To(tbSpecies);
         _screenBinder.Bind(dto => dto.MoleculeName).To(tbMoleculeName);
         _screenBinder.Bind(dto => dto.Category).To(tbCategory);
         _unitControl = new UxComboBoxUnit<ExpressionParameterDTO>(gridControl);
         _dimensionFactory = dimensionFactory;
         _dimensionComboBoxRepository = new UxRepositoryItemComboBox(gridView);
         _dimensionFactory = dimensionFactory;
         initializeBinders();
         gridView.ShowColumnChooser = true;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         tbSpecies.Enabled = false;
         tbMoleculeName.Enabled = false;
         tbCategory.Enabled = false;

         lblSpecies.Text = OSPSuite.Core.Assets.Captions.Species.FormatForLabel();
         lblCategory.Text = OSPSuite.Core.Assets.Captions.Category.FormatForLabel();
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      private void configureRepository(BaseEdit activeEditor, ExpressionParameterDTO expressionParameter)
      {
         _unitControl.UpdateUnitsFor(activeEditor, expressionParameter);
      }

      private void initializeBinders()
      {
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

         _gridViewBinder.AutoBind(dto => dto.Name)
            .WithCaption(ParameterName).AsReadOnly();

         _gridViewBinder.Bind(dto => dto.Value).WithCaption(Value)
            .WithOnValueUpdating(onExpressionParameterValueSet)
            .WithFormat(dto => dto.ExpressionParameterFormatter())
            .WithEditorConfiguration(configureRepository);

         _unitControl.ParameterUnitSet += setParameterUnit;

         _dimensionComboBoxRepository.FillComboBoxRepositoryWith(_dimensionFactory.DimensionsSortedByName);

         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => _presenter.SetFormula(o, e.NewValue.Formula));

         _gridViewBinder.Bind(x => x.Dimension).WithRepository(x => _dimensionComboBoxRepository).AsReadOnly();

         gridView.HiddenEditor += (o, e) => hideEditor();
      }

      protected RepositoryItem CreateFormulaRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.AllFormulas());

         repository.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
         repository.ButtonClick += OnFormulaButtonClick;

         return repository;
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
         OnEvent(() => _presenter.SetExpressionParameterValue(expressionParameterDTO, e.NewValue));
      }

      private void initializePathElementColumn(Expression<Func<ExpressionParameterDTO, string>> expression, string caption)
      {
         _pathElementsColumns.Add(_gridViewBinder.Bind(expression).WithCaption(caption).AsReadOnly());
      }

      private void initColumnVisibility()
      {
         _pathElementsColumns.Each(column => column.Visible = _presenter.HasAtLeastTwoDistinctValues(_pathElementsColumns.IndexOf(column)));
      }

      public void AttachPresenter(IExpressionProfileBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ExpressionProfileBuildingBlockDTO buildingBlockDTO)
      {
         _screenBinder.BindToSource(buildingBlockDTO);

         lblMoleculeName.Text = buildingBlockDTO.NameType.FormatForLabel();
         _gridViewBinder.BindToSource(buildingBlockDTO.ExpressionParameters);
         initColumnVisibility();
      }

      private void disposeBinders()
      {
         _screenBinder.Dispose();
         _gridViewBinder.Dispose();
      }
   }
}