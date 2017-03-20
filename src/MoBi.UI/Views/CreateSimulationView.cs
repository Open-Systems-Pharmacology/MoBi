using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Assets;
using DevExpress.XtraTab;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class CreateSimulationView : WizardView, ICreateSimulationView
   {
      private readonly ScreenBinder<IObjectBaseDTO> _screenBinder;

      public CreateSimulationView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IObjectBaseDTO>();
         ClientSize = new System.Drawing.Size(UIConstants.UI.SIMULATION_VIEW_WITDH, UIConstants.UI.SIMULATION_VIEW_HEIGHT);

      }

      protected override int TopicId => HelpId.MoBi_SettingUpSimulation;

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

      public void AttachPresenter(ICreateSimulationPresenter presenter)
      {
         WizardPresenter = presenter;
      }

      public void BindTo(IObjectBaseDTO simulationDTO)
      {
         _screenBinder.BindToSource(simulationDTO);
         NotifyViewChanged();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         Icon = ApplicationIcons.Simulation.WithSize(IconSizes.Size16x16);
         Caption = AppConstants.Captions.SimulationCreationCaption;
         this.ReziseForCurrentScreen(fractionHeight: UIConstants.UI.SCREEN_RESIZE_FRACTION, fractionWidth: UIConstants.UI.SCREEN_RESIZE_FRACTION);

      }

      public override XtraTabControl TabControl
      {
         get { return tabWizard; }
      }

      public override bool HasError
      {
         get { return _screenBinder.HasError; }
      }
   }
}