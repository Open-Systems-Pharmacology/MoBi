using System.Windows.Forms;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class OutputSelectionsView : BaseModalView, IOutputSelectionsView
   {
      public OutputSelectionsView(IMainView shell)
         : base(shell)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IOutputSelectionsPresenter presenter)
      {
      }

      public void AddSettingsView(IView view)
      {
         panel.FillWith(view);
      }

      public void AddDefaultButtonsView(IView view)
      {
         ReplaceExtraButtonWith(view as Control);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.SimulationSettings;
         ApplicationIcon = ApplicationIcons.Simulation;

         ExtraEnabled = true;
         ExtraVisible = true;
      }
   }
}