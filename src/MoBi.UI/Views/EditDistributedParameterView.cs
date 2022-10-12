using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Forms;
using DevExpress.Utils.Layout;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Helper;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

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
            .OnValueUpdating += onValueUpdating;

         _screenBinder.Bind(x => x.FormulaType)
            .To(cbFormulaType)
            .WithValues(dto => _presenter.AllFormulaTypes())
            .AndDisplays(type => _presenter.DisplayFormulaTypeFor(type))
            .Changed += () => OnEvent(_presenter.UpdateDistributionFormula);

         _screenBinder.Bind(dto => dto.Dimension)
            .To(cbDimension)
            .WithValues(getDimensions)
            .Changed += () => OnEvent(_presenter.DimensionChanged);

         bindDistributionParameter(dto => dto.Value, veValue);

         _screenBinder.Bind(dto => dto.Percentile)
            .To(tbPercentile);
         tbPercentile.Enabled = false;

         bindDistributionParameter(dto => dto.Minimum, veMinimum);
         bindDistributionParameter(dto => dto.Maximum, veMaximum);
         bindDistributionParameter(dto => dto.Mean, veMean);
         bindDistributionParameter(dto => dto.Deviation, veDeviation);
         bindDistributionParameter(dto => dto.GeometricDeviation, veGeoStd);

         _screenBinder.Bind(dto => dto.Description)
            .To(htmlEditor).OnValueUpdating += onValueUpdating;

         RegisterValidationFor(_screenBinder, NotifyViewChanged);

         btName.ButtonClick += (o, e) => OnEvent(_presenter.RenameSubject);
      }

      private void bindDistributionParameter(Expression<Func<DistributedParameterDTO, DistributionParameterDTO>> expression, ValueEdit valueEdit)
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

      private void onValueUpdating<T>(DistributedParameterDTO distributedParameter, PropertyValueSetEventArgs<T> e)
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

         tablePanel.RowFor(labelMinimum).Visible = formulaType == DistributionFormulaType.UniformDistribution;
         tablePanel.RowFor(labelMaximum).Visible = tablePanel.RowFor(labelMinimum).Visible;

         var needsMeanParameter = formulaType == DistributionFormulaType.NormalDistribution ||
                                  formulaType == DistributionFormulaType.LogNormalDistribution ||
                                  formulaType == DistributionFormulaType.DiscreteDistribution;

         tablePanel.RowFor(labelMean).Visible = needsMeanParameter;
         tablePanel.RowFor(labelDeviation).Visible = formulaType == DistributionFormulaType.NormalDistribution;
         tablePanel.RowFor(labelGeoStd).Visible = formulaType == DistributionFormulaType.LogNormalDistribution;
         tableProperties.RowFor(labelPercentile).Visible = formulaType != DistributionFormulaType.DiscreteDistribution;

         layoutControl.ResumeLayout();
      }

      public override bool HasError => _screenBinder.HasError;

      public override void InitializeResources()
      {
         base.InitializeResources();
         var height = cbFormulaType.Height;
         adjustControlSize(tablePanel, veDeviation, height: height);
         adjustControlSize(tablePanel, veGeoStd, height: height);
         adjustControlSize(tablePanel, veMean, height: height);
         adjustControlSize(tablePanel, veMinimum, height: height);
         adjustControlSize(tablePanel, veMaximum, height: height);
         adjustControlSize(tableProperties, veValue, height: height);

         labelMean.Text = AppConstants.Captions.Mean.FormatForLabel();
         labelDeviation.Text = AppConstants.Captions.StandardDeviation.FormatForLabel();
         labelGeoStd.Text = AppConstants.Captions.GeometricDeviation.FormatForLabel();
         labelMinimum.Text = AppConstants.Captions.Minimum.FormatForLabel();
         labelMaximum.Text = AppConstants.Captions.Maximum.FormatForLabel();
         labelName.Text = AppConstants.Captions.Name.FormatForLabel();
         labelDimension.Text = AppConstants.Captions.Dimension.FormatForLabel();
         labelValue.Text = AppConstants.Captions.Value.FormatForLabel();
         labelPercentile.Text = AppConstants.Captions.Percentile.FormatForLabel();
         labellDistribution.Text = AppConstants.Captions.Distribution.FormatForLabel();
      }

      public static void adjustControlSize(TablePanel tablePanel, Control control, int? width = null, int? height = null)
      {
         var row = RowFor(tablePanel, control);
         var col = ColumnFor(tablePanel, control);
         if (width.HasValue)
         {
            col.Style = TablePanelEntityStyle.AutoSize;
            // col.Width = width.Value + control.Margin.Horizontal;
            // control.MaximumSize = new Size(width.Value, control.Height);
            control.Width = width.Value;
         }

         if (height.HasValue)
         {
            row.Style = TablePanelEntityStyle.AutoSize;

            // row.Height = height.Value + control.Margin.Vertical;
            // control.MaximumSize = new Size(control.Width, height.Value);
            control.Height = height.Value;
         }
      }

      /// <summary>
      ///    Returns the <see cref="TablePanelRow" /> where the <paramref name="control" /> is located
      /// </summary>
      public static TablePanelRow RowFor(TablePanel tablePanel, Control control)
      {
         return tablePanel.Rows[tablePanel.GetRow(control)];
      }

      /// <summary>
      ///    Returns the <see cref="TablePanelColumn" /> where the <paramref name="control" /> is located
      /// </summary>
      public static TablePanelColumn ColumnFor(TablePanel tablePanel, Control control)
      {
         return tablePanel.Columns[tablePanel.GetColumn(control)];
      }
   }
}