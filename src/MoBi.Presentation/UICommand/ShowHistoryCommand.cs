using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter.Main;

namespace MoBi.Presentation.UICommand
{
   public class ShowHistoryCommand : IUICommand
   {
      private readonly IHistoryPresenter _historyPresenter;

      public ShowHistoryCommand(IHistoryPresenter historyPresenter)
      {
         _historyPresenter = historyPresenter;
      }

      public void Execute()
      {
         _historyPresenter.ToggleVisibility();
      }
   }
}