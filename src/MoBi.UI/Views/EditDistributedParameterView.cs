using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Helper;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class EditDistributedParameterView : BaseUserControl, IEditDistributedParameterView
   {
      private IEditDistributedParameterPresenter _presenter;
      private ScreenBinder<DistributedParameterDTO> _screenBinder;

      public EditDistributedParameterView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditDistributedParameterPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(DistributedParameterDTO dtoDistributedParameter)
      {
         _screenBinder.BindToSource(dtoDistributedParameter);
         setControlVisibility(dtoDistributedParameter);
         initNameControl(dtoDistributedParameter);
      }

      public override void InitializeBinding()
      {
         _screenBinder = new ScreenBinder<DistributedParameterDTO>();
         _screenBinder.Bind(dto => dto.Name).To(btName)
            .OnValueUpdating += OnValueUpdating;

         _screenBinder.Bind(x => x.FormulaType)
            .To(cbFormulaType)
            .WithValues(dto => _presenter.AllFormulaTypes())
            .AndDisplays(type => _presenter.DisplayFormulaTypeFor(type))
            .Changed += () => OnEvent(_presenter.UpdateDistributionFormula);

         _screenBinder.Bind(dto => dto.Dimension)
            .To(cbDimension)
            .WithValues(getDimensions)
            .Changed += () => OnEvent(_presenter.DimensionChanged);

         bindDisitributionParameter(dto => dto.Value, veValue);

         _screenBinder.Bind(dto => dto.Percentile)
            .To(tbPercentile);
         tbPercentile.Enabled = false;

         bindDisitributionParameter(dto => dto.Minimum, veMinimum);
         bindDisitributionParameter(dto => dto.Maximum, veMaximum);
         bindDisitributionParameter(dto => dto.Mean, veMean);
         bindDisitributionParameter(dto => dto.Deviation, veDeviation);
         bindDisitributionParameter(dto => dto.GeometricDeviation, veGeoStd);

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor).OnValueUpdating += OnValueUpdating;

         RegisterValidationFor(_screenBinder,NotifyViewChanged);

         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
      }

      private void bindDisitributionParameter(Expression<Func<DistributedParameterDTO, DistributionParameterDTO>> expression, ValueEdit valueEdit)
      {
         _screenBinder.Bind(expression).To(valueEdit);
         valueEdit.ValueChanged += parameterValueSet;
         valueEdit.UnitChanged += parameterUnitSet;
      }

      protected override void OnValidationError(Control control, string errorMessage)
      {
         if (control.IsAnImplementationOf<ValueEdit>())
         {
            var valueEdit = control.DowncastTo<ValueEdit>();
            valueEdit.SetError(errorMessage);
         }
         else
         {
            base.OnValidationError(control, errorMessage);
         }
      }

      protected override void OnClearError(Control control)
      {
         if (control.IsAnImplementationOf<ValueEdit>())
         {
            var valueEdit = control.DowncastTo<ValueEdit>();
            valueEdit.SetError(String.Empty);
         }
         else
         {
            base.OnClearError(control);
         }
      }

      private IReadOnlyList<IDimension> getDimensions(DistributedParameterDTO dto)
      {
         return _presenter.GetDimensions();
      }

      private void parameterValueSet(ValueEditDTO valueEditDTO, double valueInGuiUnit)
      {
         OnEvent(() => _presenter.SetParameterValue(valueEditDTO.DowncastTo<DistributionParameterDTO>(), valueInGuiUnit));
      }

      private void parameterUnitSet(ValueEditDTO valueEditDTO, Unit unit)
      {
         OnEvent(() => _presenter.SetParameterUnit(valueEditDTO.DowncastTo<DistributionParameterDTO>(), unit));
      }

      private void OnValueUpdating<T>(DistributedParameterDTO distributedParameter, PropertyValueSetEventArgs<T> e)
      {
         OnEvent(() => _presenter.SetPropertyValueFromView(e.PropertyName, e.NewValue, e.OldValue));
      }

      private void initNameControl(DistributedParameterDTO dto)
      {
         var isInit = dto.Name.IsNullOrEmpty();
         btName.Properties.Buttons[0].Enabled = !isInit;
         btName.Properties.Buttons[0].Visible = !isInit;
         btName.Properties.ReadOnly = !isInit;
      }

      private void setControlVisibility(DistributedParameterDTO dtoDistributedParameter)
      {
         setControlVisibility(dtoDistributedParameter.FormulaType);
      }

      private void setControlVisibility(DistributionFormulaType formulaType)
      {
         layoutControl.SuspendLayout();
         layoutItemMinimum.Visibility = LayoutVisibilityConvertor.FromBoolean(formulaType == DistributionFormulaType.UniformDistribution);
         layoutItemMaximum.Visibility = layoutItemMinimum.Visibility;

         bool needsMeanParameter = formulaType == DistributionFormulaType.NormalDistribution ||
                                   formulaType == DistributionFormulaType.LogNormalDistribution ||
                                   formulaType == DistributionFormulaType.DiscreteDistribution;

         layoutItemMean.Visibility = LayoutVisibilityConvertor.FromBoolean(needsMeanParameter);
         layoutItemDeviation.Visibility = LayoutVisibilityConvertor.FromBoolean(formulaType == DistributionFormulaType.NormalDistribution);
         layoutItemGeoDeviation.Visibility = LayoutVisibilityConvertor.FromBoolean(formulaType == DistributionFormulaType.LogNormalDistribution);
         layoutItemPercentile.Visibility = LayoutVisibilityConvertor.FromBoolean(formulaType != DistributionFormulaType.DiscreteDistribution);

         layoutControl.ResumeLayout();
      }

      public override bool HasError
      {
         get { return _screenBinder.HasError; }
      }
   }
}