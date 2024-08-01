using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class ParameterValuesView : BasePathAndValueEntityView<ParameterValueDTO, ParameterValue>, IParameterValuesView, IViewWithPopup
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly UxRepositoryItemComboBox _dimensionComboBoxRepository;

      public ParameterValuesView(IDimensionFactory dimensionFactory, ValueOriginBinder<ParameterValueDTO> valueOriginBinder, IImageListRetriever imageListRetriever) : base(valueOriginBinder)
      {
         InitializeComponent();

         _dimensionFactory = dimensionFactory;
         _dimensionComboBoxRepository = new UxRepositoryItemComboBox(gridView);
         PopupBarManager = new BarManager { Form = this, Images = imageListRetriever.AllImages16x16 };
      }

      public void AttachPresenter(IParameterValuesPresenter presenter)
      {
         _presenter = presenter;
      }

      protected override void DoInitializeBinding()
      {
         base.DoInitializeBinding();

         _unitControl.ParameterUnitSet += setParameterUnit;

         _dimensionComboBoxRepository.FillComboBoxRepositoryWith(_dimensionFactory.DimensionsSortedByName);

         BindValueColumn(dto => dto.Value)
            .WithCaption(AppConstants.Captions.ParameterValue)
            .WithFormat(dto => dto.ParameterValueFormatter())
            .WithOnValueUpdating(onParameterValueSet);

         InitializeValueOriginBinding();

         _gridViewBinder.Bind(x => x.Formula)
            .WithEditRepository(dto => CreateFormulaRepository())
            .WithOnValueUpdating((o, e) => parameterValuesPresenter.SetFormula(o, e.NewValue.Formula));

         _gridViewBinder.Bind(x => x.Dimension).WithRepository(x => _dimensionComboBoxRepository)
            .WithOnValueUpdating((o, e) => OnEvent(() => onDimensionSet(o, e)));

         _gridViewBinder.GridView.MouseDown += (o, e) => OnEvent(onGridViewMouseDown, e);
      }

      public override string NameColumnCaption => AppConstants.Captions.ParameterName;

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

      private void onGridViewMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;
         var location = new Point(e.X, e.Y);
         ((ParameterValuesPresenter)_presenter).ShowContextMenu(null, location);
      }

      private IParameterValuesPresenter parameterValuesPresenter => _presenter.DowncastTo<IParameterValuesPresenter>();

      public BarManager PopupBarManager { get; }
   }
}