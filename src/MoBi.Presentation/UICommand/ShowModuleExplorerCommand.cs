using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter.Main;

namespace MoBi.Presentation.UICommand
{
    internal class ShowModuleExplorerCommand : IUICommand
   {
      private readonly IModuleExplorerPresenter _moduleExplorerPresenter;

      public ShowModuleExplorerCommand(IModuleExplorerPresenter moduleExplorerPresenter)
      {
         _moduleExplorerPresenter = moduleExplorerPresenter;
      }

      public void Execute()
      {
         _moduleExplorerPresenter.ToggleVisibility();
      }
   }
}