using OSPSuite.Assets;
using DevExpress.XtraTab;
using MoBi.Presentation.Presenter;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ConfigureSimulationView : WizardView, IConfigureSimulationView
   {
      public ConfigureSimulationView()
      {
         InitializeComponent();
         ClientSize = new System.Drawing.Size(UIConstants.UI.SIMULATION_VIEW_WITDH, UIConstants.UI.SIMULATION_VIEW_HEIGHT);
      }

      public void AttachPresenter(IConfigureSimulationPresenter presenter)
      {
         WizardPresenter = presenter;
      }

      public override XtraTabControl TabControl
      {
         get { return tabWizard; }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Icon = ApplicationIcons.Simulation.WithSize(IconSizes.Size16x16);
         this.ReziseForCurrentScreen(fractionHeight: UIConstants.UI.SCREEN_RESIZE_FRACTION, fractionWidth: UIConstants.UI.SCREEN_RESIZE_FRACTION);
      }
   }
}