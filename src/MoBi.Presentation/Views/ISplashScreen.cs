using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface ISplashScreen : IView<ISplashScreenPresenter>
   {
      void StopProgress();
      void StartProgress(int startingProgress, string caption);
      void ShowProgress(int startingProgress, string caption);
   }
}