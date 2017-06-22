using MoBi.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using MoBi.Core;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class ValidationOptionsView : BaseUserControl, IValidationOptionsView
   {
      private ScreenBinder<ValidationSettings> _screenBinder;
      private IValidationOptionsPresenter _presenter;

      public ValidationOptionsView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder = new ScreenBinder<ValidationSettings>();
         _screenBinder.Bind(x => x.CheckDimensions).To(chkValidateDimensions).OnValueUpdating += dimensionValidationChanged;
         _screenBinder.Bind(x => x.ShowCannotCalcErrors).To(chkShowUnableToCalculateWarnings);
         _screenBinder.Bind(x => x.ShowPKSimDimensionProblemWarnings).To(chkShowPKSimWarnings);
         _screenBinder.Bind(x => x.ShowPKSimObserverMessages).To(chkValiadatePkSimStandardObserver);
         _screenBinder.Bind(x => x.CheckRules).To(chkValidateRules);
         RegisterValidationFor(_screenBinder);
      }

      private void dimensionValidationChanged(ValidationSettings arg1, PropertyValueSetEventArgs<bool> arg2)
      {
         _presenter.ValidateDimensionsChanged(arg2.NewValue);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkValiadatePkSimStandardObserver.Text = AppConstants.Captions.ValidatePKSimStandardObserver;
         chkValidateDimensions.Text = AppConstants.Captions.ValidateDimensions;
         chkShowPKSimWarnings.Text = AppConstants.Captions.ShowPKSimParameterWarnings;
         chkShowUnableToCalculateWarnings.Text = AppConstants.Captions.ShowUnableCalculateWarnings;
         chkValidateRules.Text = AppConstants.Captions.ValidateRules;
      }

      public void AttachPresenter(IValidationOptionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void EnableDisableValidationSubOptions(bool enabled)
      {
         chkShowPKSimWarnings.Enabled = enabled;
         chkShowUnableToCalculateWarnings.Enabled = enabled;
      }

      public void Show(ValidationSettings validationOptions)
      {
         _screenBinder.BindToSource(validationOptions);
      }
   }
}