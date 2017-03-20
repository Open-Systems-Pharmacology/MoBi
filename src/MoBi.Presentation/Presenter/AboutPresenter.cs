using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IAboutPresenter : IDisposablePresenter
   {
   }

   public class AboutPresenter : AbstractDisposablePresenter<IAboutView, IAboutPresenter>, IAboutPresenter
   {
      private readonly IMoBiConfiguration _configuration;

      public AboutPresenter(IAboutView view, IMoBiConfiguration configuration) : base(view)
      {
         _configuration = configuration;
         view.LicenseAgreementFilePath = _configuration.LicenseAgreementFilePath;
      }

      public override void Initialize()
      {
         _view.Product = AppConstants.ProductName;
         _view.VersionInformation = $"Version {_configuration.FullVersion}";
         _view.Display();
      }
   }
}