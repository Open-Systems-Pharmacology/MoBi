using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter.Main;

namespace MoBi.Presentation.UICommand
{
   public class ShowNotificationCommand : IUICommand
   {
      private readonly INotificationPresenter _presenter;

      public ShowNotificationCommand(INotificationPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Execute()
      {
         _presenter.ToggleVisibility();
      }
   }
}