using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter.Main;

namespace MoBi.Presentation.UICommand
{
    internal class ShowModuleExplorerCommand : IUICommand
   {
      private readonly IBuildingBlockExplorerPresenter _buildingBlockExplorerPresenter;
      private readonly IModuleExplorerPresenter _moduleExplorerPresenter;

      public ShowModuleExplorerCommand(IBuildingBlockExplorerPresenter buildingBlockExplorerPresenter, IModuleExplorerPresenter moduleExplorerPresenter)
      {
         _buildingBlockExplorerPresenter = buildingBlockExplorerPresenter;
         _moduleExplorerPresenter = moduleExplorerPresenter;
      }

      public void Execute()
      {
         _moduleExplorerPresenter.ToggleVisibility();
         _buildingBlockExplorerPresenter.ToggleVisibility();
      }
   }
}