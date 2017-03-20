using System.Collections.Generic;
using MoBi.Assets;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using SimModelNET;

namespace MoBi.Presentation.Presenter
{
   public interface ISolverMessagePresenter : IDisposablePresenter
   {
      void Show(IEnumerable<ISolverWarning> warnings);
   }

   internal class SolverMessagePresenter : AbstractDisposablePresenter<ISolverMessageView, ISolverMessagePresenter>, ISolverMessagePresenter
   {
      public SolverMessagePresenter(ISolverMessageView view) : base(view)
      {
         _view.CancelVisible = false;
         _view.Caption = AppConstants.Captions.WarningsCaption;
      }

      public void Show(IEnumerable<ISolverWarning> warnings)
      {
         _view.BindTo(warnings);
         _view.Display();
      }
   }
}