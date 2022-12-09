using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class ParameterStartValuesView : BaseStartValuesView<ParameterStartValueDTO, ParameterStartValue>, IParameterStartValuesView
   {
      private readonly UxComboBoxUnit<ParameterStartValueDTO> _unitControl;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly UxRepositoryItemComboBox _dimensionComboBoxRepository;

      public ParameterStartValuesView(IDimensionFactory dimensionFactory, ValueOriginBinder<ParameterStartValueDTO> valueOriginBinder):base(valueOriginBinder)
      {
         InitializeComponent();
         _unitControl = new UxComboBoxUnit<ParameterStartValueDTO>(gridControl);
         _dimensionFactory = dimensionFactory;
         _dimensionComboBoxRepository = new UxRepositoryItemComboBox(gridView);
      }

      public void AttachPresenter(IParameterStartValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      protected override void DoInitializeBinding()
      {
         _unitControl.ParameterUnitSet += setParameterUnit;

         _dimensionComboBoxRepository.FillComboBoxRepositoryWith(_dimensionFactory.DimensionsSortedByName);

         var colName = _gridViewBinder.AutoBind(dto => dto.Name)
            .WithCaption(AppConstants.Captions.ParameterName).WithOnValueUpdating((o,e) => OnEvent(() => OnNameSet(o,e)));

         //to put the name in the first column
         colName.XtraColumn.VisibleIndex = 0;

         _gridViewBinder.AutoBind(dto => dto.StartValue)
            .WithCaption(AppConstants.Captions.StartValue)
            .WithFormat(dto => dto.ParameterStartValueFormatter())
            .WithEditorConfiguration(configureRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithOnValueUpdating(onParameterStartValueSet);

         InitializeValueOriginBinding();

         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => parameterStartValuesPresenter.SetFormula(o, e.NewValue.Formula));

         _gridViewBinder.Bind(x => x.Dimension).WithRepository(x => _dimensionComboBoxRepository)
            .WithOnValueUpdating((o,e) => OnEvent(() => onDimensionSet(o,e)));

         gridView.HiddenEditor += (o, e) => hideEditor();
      }

      private void onDimensionSet(ParameterStartValueDTO parameterStartValueDTO, PropertyValueSetEventArgs<IDimension> propertyValueSetEventArgs)
      {
         parameterStartValuesPresenter.UpdateDimension(parameterStartValueDTO, propertyValueSetEventArgs.NewValue);
      }

      private void onParameterStartValueSet(ParameterStartValueDTO psv, PropertyValueSetEventArgs<double?> e)
      {
         OnEvent(() => parameterStartValuesPresenter.SetValue(psv, e.NewValue));
      }

      private void setParameterUnit(ParameterStartValueDTO parameterStartValue, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            parameterStartValuesPresenter.SetUnit(parameterStartValue, unit);
         });
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      private IParameterStartValuesPresenter parameterStartValuesPresenter => _presenter.DowncastTo<IParameterStartValuesPresenter>();

      private void configureRepository(BaseEdit activeEditor, ParameterStartValueDTO parameterStartValue)
      {
         _unitControl.UpdateUnitsFor(activeEditor, parameterStartValue);
      }
   }
}