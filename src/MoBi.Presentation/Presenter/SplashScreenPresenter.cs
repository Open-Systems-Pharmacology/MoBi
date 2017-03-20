using OSPSuite.Utility.Events;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISplashScreenPresenter : IPresenter<ISplashScreen>,
                                             IListener<ProgressingEvent>,
                                             IListener<ProgressDoneEvent>,
                                             IListener<ProgressInitEvent>
   {
   }

   public class SplashScreenPresenter : AbstractPresenter<ISplashScreen, ISplashScreenPresenter>, ISplashScreenPresenter
   {
      public SplashScreenPresenter(ISplashScreen view) : base(view)
      {
      }

      public void Handle(ProgressingEvent eventToHandle)
      {
         _view.StartProgress(eventToHandle.ProgressPercent, eventToHandle.Message);
      }

      public void Handle(ProgressDoneEvent eventToHandle)
      {
         _view.StopProgress();
      }

      public void Handle(ProgressInitEvent eventToHandle)
      {
         _view.StartProgress(0, eventToHandle.Message);
      }
   }
}