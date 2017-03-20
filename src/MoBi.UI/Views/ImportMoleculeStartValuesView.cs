using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class ImportMoleculeStartValuesView : BaseModalView, IImportMoleculeStartValuesView
   {
      public ImportMoleculeStartValuesView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IImportMoleculeStartValuePresenter presenter)
      {
      }
   }
}
