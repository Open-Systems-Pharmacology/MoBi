using System.Drawing;
using DevExpress.XtraTab;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class CreateSimulationConfigurationView : WizardView, ICreateSimulationConfigurationView
   {
      private readonly ScreenBinder<ObjectBaseDTO> _screenBinder;

      public CreateSimulationConfigurationView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ObjectBaseDTO>();
         ClientSize = new Size(UIConstants.UI.SIMULATION_VIEW_WIDTH, UIConstants.UI.SIMULATION_VIEW_HEIGHT);
      }

      protected override void SetActiveControl()
      {
         ActiveControl = tbName;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.Name).To(tbName);
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public void AttachPresenter(ICreateSimulationConfigurationPresenter presenter)
      {
         WizardPresenter = presenter;
      }

      public void BindTo(ObjectBaseDTO simulationDTO)
      {
         _screenBinder.BindToSource(simulationDTO);
         NotifyViewChanged();
      }

      public void DisableNaming()
      {
         tbName.Enabled = false;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         ApplicationIcon = ApplicationIcons.Simulation;
         Caption = AppConstants.Captions.SimulationConfigurationWizard;
         this.ResizeForCurrentScreen(fractionHeight: UIConstants.UI.SCREEN_RESIZE_FRACTION, fractionWidth: UIConstants.UI.SCREEN_RESIZE_FRACTION);
      }

      public override XtraTabControl TabControl => tabWizard;

      public override bool HasError => _screenBinder.HasError;
   }
}