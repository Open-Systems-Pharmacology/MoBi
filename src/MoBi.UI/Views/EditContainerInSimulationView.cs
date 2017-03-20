using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditContainerInSimulationView : BaseUserControl, IEditContainerInSimulationView
   {
      public EditContainerInSimulationView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditContainerInSimulationPresenter presenter)
      {
         /*nothing to do*/
      }

      public void SetContainerView(IView view)
      {
         this.FillWith(view);
      }
   }
}
