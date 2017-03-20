using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IAboutView : IModalView<IAboutPresenter>
   {
      string Product { set; }
      string LicenseAgreementFilePath { set; }
      string VersionInformation { set; }
   }
}