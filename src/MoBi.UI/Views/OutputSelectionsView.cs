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
      private IOutputSelectionsPresenter _presenter;

      public OutputSelectionsView(IMainView shell)
         : base(shell)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IOutputSelectionsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSettingsView(IView view)
      {
         panel.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.SimulationSettings;
         ApplicationIcon = ApplicationIcons.Simulation;

         ExtraCaption = AppConstants.Captions.MakeDefault;
         ExtraEnabled = true;
         ExtraVisible = true;

         btnLoadDefaults.Text = AppConstants.Captions.LoadFromDefaults;
         btnLoadDefaults.Click += (o, e) => OnEvent(loadDefaultsClicked);

         tablePanel.AdjustButton(btnLoadDefaults);
      }

      private void loadDefaultsClicked()
      {
         _presenter.LoadSelectionFromDefaults();
      }

      protected override void ExtraClicked()
      {
         _presenter.MakeCurrentSelectionDefault();
      }
   }
}