using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Presenter.BasePresenter
{
   public abstract class MoBiDisposablePresenter<TView, TPresenter> : AbstractDisposablePresenter<TView, TPresenter> where TPresenter : IDisposablePresenter where TView : IView<TPresenter>, IModalView
   {
      protected MoBiDisposablePresenter(TView view) : base(view)
      {
      }

      public override void ViewChanged()
      {
         _view.OkEnabled = CanClose;
      }
   }
}