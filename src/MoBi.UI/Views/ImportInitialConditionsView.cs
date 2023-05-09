using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ImportInitialConditionsView : BaseModalView, IImportInitialConditionsView
   {
      public ImportInitialConditionsView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IImportInitialConditionsPresenter presenter)
      {
      }
   }
}
