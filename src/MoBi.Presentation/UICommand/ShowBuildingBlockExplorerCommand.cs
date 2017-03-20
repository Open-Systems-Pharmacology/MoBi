using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter.Main;

namespace MoBi.Presentation.UICommand
{
    internal class ShowBuildingBlockExplorerCommand : IUICommand
   {
      private readonly IBuildingBlockExplorerPresenter _buildingBlockExplorerPresenter;

      public ShowBuildingBlockExplorerCommand(IBuildingBlockExplorerPresenter buildingBlockExplorerPresenter)
      {
         _buildingBlockExplorerPresenter = buildingBlockExplorerPresenter;
      }

      public void Execute()
      {
         _buildingBlockExplorerPresenter.ToggleVisibility();
      }
   }
}