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
   public partial class ParameterValuesView : BaseStartValuesView<ParameterValueDTO, ParameterValue>, IParameterValuesView
   {
      private readonly UxComboBoxUnit<ParameterValueDTO> _unitControl;
      private readonly IDimensionFactory _dimensionFactory;
      private readonly UxRepositoryItemComboBox _dimensionComboBoxRepository;

      public ParameterValuesView(IDimensionFactory dimensionFactory, ValueOriginBinder<ParameterValueDTO> valueOriginBinder):base(valueOriginBinder)
      {
         InitializeComponent();
         _unitControl = new UxComboBoxUnit<ParameterValueDTO>(gridControl);
         _dimensionFactory = dimensionFactory;
         _dimensionComboBoxRepository = new UxRepositoryItemComboBox(gridView);
      }

      public void AttachPresenter(IParameterValuesPresenter presenter)
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
            .WithFormat(dto => dto.ParameterValueFormatter())
            .WithEditorConfiguration(configureRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithOnValueUpdating(onParameterValueSet);

         InitializeValueOriginBinding();

         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => parameterValuesPresenter.SetFormula(o, e.NewValue.Formula));

         _gridViewBinder.Bind(x => x.Dimension).WithRepository(x => _dimensionComboBoxRepository)
            .WithOnValueUpdating((o,e) => OnEvent(() => onDimensionSet(o,e)));

         gridView.HiddenEditor += (o, e) => hideEditor();
      }

      private void onDimensionSet(ParameterValueDTO parameterValueDTO, PropertyValueSetEventArgs<IDimension> propertyValueSetEventArgs)
      {
         parameterValuesPresenter.UpdateDimension(parameterValueDTO, propertyValueSetEventArgs.NewValue);
      }

      private void onParameterValueSet(ParameterValueDTO psv, PropertyValueSetEventArgs<double?> e)
      {
         OnEvent(() => parameterValuesPresenter.SetValue(psv, e.NewValue));
      }

      private void setParameterUnit(ParameterValueDTO parameterValue, Unit unit)
      {
         this.DoWithinExceptionHandler(() =>
         {
            gridView.CloseEditor();
            parameterValuesPresenter.SetUnit(parameterValue, unit);
         });
      }

      private void hideEditor()
      {
         _unitControl.Hide();
      }

      private IParameterValuesPresenter parameterValuesPresenter => _presenter.DowncastTo<IParameterValuesPresenter>();

      private void configureRepository(BaseEdit activeEditor, ParameterValueDTO parameterValue)
      {
         _unitControl.UpdateUnitsFor(activeEditor, parameterValue);
      }
   }
}