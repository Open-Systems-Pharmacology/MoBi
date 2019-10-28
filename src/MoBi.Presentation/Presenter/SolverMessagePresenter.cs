using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.SimModel;

namespace MoBi.Presentation.Presenter
{
   public interface ISolverMessagePresenter : IDisposablePresenter
   {
      void Show(IEnumerable<SolverWarning> warnings);
   }

   internal class SolverMessagePresenter : AbstractDisposablePresenter<ISolverMessageView, ISolverMessagePresenter>, ISolverMessagePresenter
   {
      public SolverMessagePresenter(ISolverMessageView view) : base(view)
      {
         _view.CancelVisible = false;
         _view.Caption = AppConstants.Captions.WarningsCaption;
      }

      public void Show(IEnumerable<SolverWarning> warnings)
      {
         _view.BindTo(warnings);
         _view.Display();
      }
   }
}