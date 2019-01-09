using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.Utility.Extensions;

using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.UI.Helper;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditConstantFormulaView : BaseUserControl, IEditConstantFormulaView
   {
      private IEditConstantFormulaPresenter _presenter;
      private ScreenBinder<ConstantFormulaBuilderDTO> _screenBinder;

      public EditConstantFormulaView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemValue.Text = AppConstants.Captions.Value.FormatForLabel();
         valueEdit.ToolTip = ToolTips.Formula.ConstantValue;
      }

      public void AttachPresenter(IEditConstantFormulaPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ConstantFormulaBuilderDTO constantFormulaBuilderDTO)
      {
         _screenBinder.BindToSource(constantFormulaBuilderDTO);
      }

      public bool ReadOnly
      {
         get { return !valueEdit.Enabled; }
         set { valueEdit.Enabled = !value; }
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ConstantFormulaBuilderDTO>();
         _screenBinder.Bind(x => x.Value).To(valueEdit);
         valueEdit.ValueChanged += onValueUpdating;
         valueEdit.UnitChanged += onUnitChange;


         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      private void onUnitChange(ValueEditDTO valueEditDTO, Unit unit)
      {
         this.DoWithinExceptionHandler(() => _presenter.SetDisplayUnit(valueEditDTO, unit));
      }

      private void onValueUpdating(ValueEditDTO valueEditDTO, double value)
      {
         this.DoWithinExceptionHandler(() => _presenter.SetDisplayValue(valueEditDTO, value));
      }
   }
}