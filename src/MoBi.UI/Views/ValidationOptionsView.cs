using DevExpress.XtraLayout.Utils;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI.Controls;
using ToolTips = MoBi.Assets.ToolTips;

namespace MoBi.UI.Views
{
   public partial class ValidationOptionsView : BaseUserControl, IValidationOptionsView
   {
      private readonly ScreenBinder<ValidationSettings> _screenBinder;
      private readonly ScreenBinder<SimulationSolverOptionsDTO> _solverOptionsBinder;
      private IValidationOptionsPresenter _presenter;

      public ValidationOptionsView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ValidationSettings>();
         _solverOptionsBinder = new ScreenBinder<SimulationSolverOptionsDTO>();
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

         _screenBinder.Bind(x => x.WarnForNonFiniteQuantities)
            .To(chkShowNonFiniteQuantityWarnings);

         _screenBinder.Bind(x => x.CheckRules)
            .To(chkValidateRules);

         _screenBinder.Bind(x => x.CheckCircularReference)
            .To(chkPerformCircularReferenceCheck);

         _solverOptionsBinder.Bind(x => x.CheckForNegativeValues)
            .To(chkCheckForNegativeValues);

         RegisterValidationFor(_screenBinder, NotifyViewChanged);
         RegisterValidationFor(_solverOptionsBinder, NotifyViewChanged);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkValiadatePkSimStandardObserver.Text = AppConstants.Captions.ValidatePKSimStandardObserver;
         chkValidateDimensions.Text = AppConstants.Captions.ValidateDimensions;
         chkShowPKSimWarnings.Text = AppConstants.Captions.ShowPKSimParameterWarnings;
         chkShowUnableToCalculateWarnings.Text = AppConstants.Captions.ShowUnableCalculateWarnings;
         chkShowUnresolvedInitialConditions.Text = AppConstants.Captions.ShowUnresolvedEndosomeWarningsForInitialConditions;
         chkShowNonFiniteQuantityWarnings.Text = AppConstants.Captions.ShowWarningsForNonFiniteQuantities;
         chkShowNonFiniteQuantityWarnings.ToolTip = ToolTips.ShowWarningsForNonFiniteQuantities;
         chkValidateRules.Text = AppConstants.Captions.ValidateRules;
         chkCheckForNegativeValues.Text = AppConstants.Captions.CheckForNegativeValues;
         chkCheckForNegativeValues.ToolTip = ToolTips.CheckForNegativeValues;
         chkPerformCircularReferenceCheck.Text = AppConstants.Captions.PerformCircularReferenceCheck;

         //The solver options are only shown when editing the options of a simulation
         layoutItemCheckForNegativeValues.Visibility = LayoutVisibility.Never;
      }

      public void AttachPresenter(IValidationOptionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ValidationSettings validationOptions)
      {
         _screenBinder.BindToSource(validationOptions);
      }

      public void BindTo(SimulationSolverOptionsDTO simulationSolverOptionsDTO)
      {
         layoutItemCheckForNegativeValues.Visibility = LayoutVisibility.Always;
         _solverOptionsBinder.BindToSource(simulationSolverOptionsDTO);
      }
   }
}