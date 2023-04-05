using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using static MoBi.Assets.AppConstants.Captions;

namespace MoBi.UI.Views
{
   public partial class IndividualBuildingBlockView : BaseUserControl, IIndividualBuildingBlockView
   {
      private IIndividualBuildingBlockPresenter _presenter;
      private readonly GridViewBinder<IndividualParameterDTO> _gridViewBinder;
      private readonly IList<IGridViewColumn> _pathElementsColumns = new List<IGridViewColumn>();
      private readonly UxComboBoxUnit<IndividualParameterDTO> _unitControl;
      private readonly List<TextEdit> _textBoxes = new List<TextEdit>();

      public IndividualBuildingBlockView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<IndividualParameterDTO>(gridView);
         _unitControl = new UxComboBoxUnit<IndividualParameterDTO>(gridControl);
         initializeGridViewBinders();
         gridGroup.Text = Parameters;
      }

      private void initializePathElementColumn(Expression<Func<IndividualParameterDTO, string>> expression, string caption)
      {
         _pathElementsColumns.Add(_gridViewBinder.Bind(expression).WithCaption(caption).AsReadOnly());
      }

      private void onExpressionParameterValueSet(IndividualParameterDTO expressionParameterDTO, PropertyValueSetEventArgs<double?> e)
      {
         OnEvent(() => _presenter.SetParameterValue(expressionParameterDTO, e.NewValue));
      }

      private void initializeGridViewBinders()
      {
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

         _gridViewBinder.Bind(dto => dto.Value).WithCaption(Value)
            .WithOnValueUpdating(onExpressionParameterValueSet)
            .WithFormat(dto => dto.IndividualParameterFormatter())
            .WithEditorConfiguration(configureRepository);

         _unitControl.ParameterUnitSet += setParameterUnit;

         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => _presenter.SetFormula(o, e.NewValue.Formula));

         _gridViewBinder.Bind(x => x.Dimension).WithRepository(createDimensionRepository).AsReadOnly();

         gridView.HiddenEditor += (o, e) => hideEditor();
      }

      private void configureRepository(BaseEdit activeEditor, IndividualParameterDTO expressionParameter)
      {
         _unitControl.UpdateUnitsFor(activeEditor, expressionParameter);
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

      private void setParameterUnit(IndividualParameterDTO individualParameter, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            _presenter.SetUnit(individualParameter, unit);
         });
      }

      private RepositoryItem createDimensionRepository(IndividualParameterDTO arg)
      {
         var dimensionComboBoxRepository = new UxRepositoryItemComboBox(gridView);
         dimensionComboBoxRepository.FillComboBoxRepositoryWith(_presenter.DimensionsSortedByName());
         return dimensionComboBoxRepository;
      }

      protected RepositoryItem CreateFormulaRepository()
      {
         var repository = new UxRepositoryItemComboBox(gridView);
         repository.FillComboBoxRepositoryWith(_presenter.AllFormulas());

         repository.Buttons.Add(new EditorButton(ButtonPredefines.Plus));
         repository.ButtonClick += OnFormulaButtonClick;

         return repository;
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      public void AttachPresenter(IIndividualBuildingBlockPresenter presenter)
      {
         _presenter = presenter;
      }

      private void initColumnVisibility()
      {
         _pathElementsColumns.Each(column => column.Visible = _presenter.HasAtLeastTwoDistinctValues(_pathElementsColumns.IndexOf(column)));
      }

      public void BindTo(IndividualBuildingBlockDTO buildingBlockDTO)
      {
         createNameValuePairs(buildingBlockDTO);
         _gridViewBinder.BindToSource(buildingBlockDTO.Parameters);
         initColumnVisibility();
      }

      private void createNameValuePairs(IndividualBuildingBlockDTO buildingBlock)
      {
         var flowGroup = uxLayoutControl.AddGroup();
         flowGroup.Text = OriginData;
         flowGroup.LayoutMode = LayoutMode.Flow;
         flowGroup.Move(gridGroup, InsertType.Top);
         var extendedProperties = buildingBlock.OriginData;

         extendedProperties.Each(x => addOriginDataToView(x, flowGroup));
         addPKSimVersionToView(buildingBlock.PKSimVersion, flowGroup);
         resizeTextBoxesToBestFit();
         uxLayoutControl.BestFit();
      }

      private void addPKSimVersionToView(string pkSimVersion, LayoutControlGroup layoutControlGroup)
      {
         addControlToFlowLayout(PKSimVersion, createTextBox(pkSimVersion), layoutControlGroup);
      }


      private void resizeTextBoxesToBestFit()
      {
         var maxTextWidth = _textBoxes.Max(x => x.CalcBestSize().Width);

         // Adding some extra width to reduce crowding
         var maxTextBoxWidth = (int)(maxTextWidth * 1.1);

         _textBoxes.Each(x => x.MaximumSize = new Size(maxTextBoxWidth, x.MaximumSize.Height));
      }

      private void addControlToFlowLayout(string name, Control control, LayoutControlGroup layoutControlGroup)
      {
         var layoutControlItem = uxLayoutControl.AddItem();
         layoutControlItem.Text = name.FormatForLabel(checkCase: false);

         layoutControlItem.Control = control;
         layoutControlGroup.AddItem(layoutControlItem);
      }

      private void addOriginDataToView(IExtendedProperty originDataItem, LayoutControlGroup layoutControlGroup)
      {
         addControlToFlowLayout(originDataItem.Name, createTextBox(originDataItem.ValueAsObject.ToString()), layoutControlGroup);
      }

      private Control createTextBox(string textValue)
      {
         var tb = new TextEdit();
         tb.Properties.AllowFocused = false;
         tb.Properties.ReadOnly = true;
         tb.Text = textValue;
         _textBoxes.Add(tb);
         return tb;
      }

      private void disposeBinders()
      {
         _gridViewBinder.Dispose();
      }
   }
}