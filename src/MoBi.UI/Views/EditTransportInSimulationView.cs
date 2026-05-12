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
   public partial class EditTransportInSimulationView : BaseUserControl, IEditTransportInSimulationView
   {
      private IEditTransportInSimulationPresenter _presenter;

      public EditTransportInSimulationView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditTransportInSimulationPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(TransportDTO dtoTransport)
      {
         txtName.Text = dtoTransport.Name;
         txtSource.Text = dtoTransport.Source;
         txtTarget.Text = dtoTransport.Target;
         txtMolecule.Text = dtoTransport.Molecule;
         txtDimension.Text = dtoTransport.Dimension?.DisplayName;
         htmlEditor.Text = dtoTransport.Description;
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
         layoutItemName.Text = AppConstants.Captions.Name.FormatForLabel();
         layoutItemSource.Text = AppConstants.Captions.Source.FormatForLabel();
         layoutItemTarget.Text = AppConstants.Captions.Target.FormatForLabel();
         layoutItemMolecule.Text = AppConstants.Captions.Molecule.FormatForLabel();
         layoutItemDimension.Text = AppConstants.Captions.Dimension.FormatForLabel();
         txtName.Properties.ReadOnly = true;
         txtSource.Properties.ReadOnly = true;
         txtTarget.Properties.ReadOnly = true;
         txtMolecule.Properties.ReadOnly = true;
         txtDimension.Properties.ReadOnly = true;
         htmlEditor.Properties.ReadOnly = true;
      }
   }
}
