using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Extensions;
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
using Unit = OSPSuite.Core.Domain.UnitSystem.Unit;

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

      public override void InitializeResources()
      {
         // Make changes that affect the height before calling the base method
         // where the layout item height will be fixed
         ribbonControl.DrawGroupCaptions = DefaultBoolean.False;
         base.InitializeResources();
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

         gridView.MouseDown += (o, e) => OnEvent(onGridViewMouseDown, e);
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
         if (e.Button != MouseButtons.Right) 
            return;

         if (gridView.CalcHitInfo(e.Location).HitTest != GridHitTest.EmptyRow)
            return;

         ((ParameterValuesPresenter)_presenter).ShowContextMenu(null, this.CalculateRelativeOffset(e.Location, gridControl));
      }

      private IParameterValuesPresenter parameterValuesPresenter => _presenter.DowncastTo<IParameterValuesPresenter>();

      public BarManager PopupBarManager { get; }
   }
}