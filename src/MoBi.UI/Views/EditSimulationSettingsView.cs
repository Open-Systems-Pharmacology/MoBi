using DevExpress.XtraTab;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EditSimulationSettingsView : BaseMdiChildTabbedView, IEditSimulationSettingsView
   {
      public EditSimulationSettingsView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditSimulationSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public override XtraTabControl TabControl => tabControl;

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.SimulationSettings;
      }
   }
}