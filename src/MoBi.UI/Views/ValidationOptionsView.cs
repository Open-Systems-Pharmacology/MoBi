using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   public partial class ValidationOptionsView : BaseUserControl, IValidationOptionsView
   {
      private readonly ScreenBinder<ValidationSettings> _screenBinder;
      private IValidationOptionsPresenter _presenter;

      public ValidationOptionsView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ValidationSettings>();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.CheckDimensions)
            .To(chkValidateDimensions);

         _screenBinder.Bind(x => x.CheckDimensions)
            .ToEnableOf(chkShowPKSimWarnings)
            .EnabledWhen(x => x);

         _screenBinder.Bind(x => x.CheckDimensions)
            .ToEnableOf(chkShowUnableToCalculateWarnings)
            .EnabledWhen(x => x);

         _screenBinder.Bind(x => x.ShowCannotCalcErrors)
            .To(chkShowUnableToCalculateWarnings);

         _screenBinder.Bind(x => x.ShowPKSimDimensionProblemWarnings)
            .To(chkShowPKSimWarnings);

         _screenBinder.Bind(x => x.ShowPKSimObserverMessages)
            .To(chkValiadatePkSimStandardObserver);

         _screenBinder.Bind(x => x.ShowUnresolvedEndosomesWarningsForInitialConditions)
            .To(chkShowUnresolvedInitialConditions);

         _screenBinder.Bind(x => x.CheckRules)
            .To(chkValidateRules);

         _screenBinder.Bind(x => x.CheckCircularReference)
            .To(chkPerformCircularReferenceCheck);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkValiadatePkSimStandardObserver.Text = AppConstants.Captions.ValidatePKSimStandardObserver;
         chkValidateDimensions.Text = AppConstants.Captions.ValidateDimensions;
         chkShowPKSimWarnings.Text = AppConstants.Captions.ShowPKSimParameterWarnings;
         chkShowUnableToCalculateWarnings.Text = AppConstants.Captions.ShowUnableCalculateWarnings;
         chkShowUnresolvedInitialConditions.Text = AppConstants.Captions.ShowUnresolvedEndosomeWarningsForInitialConditions;
         chkValidateRules.Text = AppConstants.Captions.ValidateRules;
         chkPerformCircularReferenceCheck.Text = AppConstants.Captions.PerformCircularReferenceCheck;
      }

      public void AttachPresenter(IValidationOptionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ValidationSettings validationOptions)
      {
         _screenBinder.BindToSource(validationOptions);
      }
   }
}