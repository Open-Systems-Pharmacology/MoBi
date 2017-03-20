using System.Drawing;
using DevExpress.Utils;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditReactionInSimulationView : BaseUserControl, IEditReactionInSimulationView
   {
      private IEditReactionInSimulationPresenter _presenter;

      public EditReactionInSimulationView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditReactionInSimulationPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ReactionDTO dtoReaction)
      {
         lblStoichiometric.Text = dtoReaction.Stoichiometric;
         txtName.Text = dtoReaction.Name;
         htmlEditor.Text = dtoReaction.Description;
      }

      public void SetParameterView(IView view)
      {
         tabParameters.FillWith(view);
      }

      public void SetFormulaView(IView view)
      {
         pnlKinetic.FillWith(view);
      }

      public void ShowParameters()
      {
         tabParameters.Show();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutControlDescription.Text = AppConstants.Captions.Description.FormatForLabel();
         layoutControlKinetic.Text = AppConstants.Captions.Kinetic.FormatForLabel();
         layoutControlKinetic.TextLocation = Locations.Top;
         layoutControlKinetic.MinSize = new Size(3 * OSPSuite.UI.UIConstants.Size.BUTTON_HEIGHT, 2 * OSPSuite.UI.UIConstants.Size.BUTTON_WIDTH);
         layoutControlStoichiometrie.Text = AppConstants.Captions.Stoichiometry.FormatForLabel();
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         txtName.Enabled = false;
         htmlEditor.Properties.ReadOnly = true;
      }
   }
}