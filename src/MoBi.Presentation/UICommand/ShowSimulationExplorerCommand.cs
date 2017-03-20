using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter.Main;

namespace MoBi.Presentation.UICommand
{
   internal class ShowSimulationExplorerCommand : IUICommand
   {
      private readonly ISimulationExplorerPresenter _simulationExplorerPresenter;

      public ShowSimulationExplorerCommand(ISimulationExplorerPresenter simulationExplorerPresenter)
      {
         _simulationExplorerPresenter = simulationExplorerPresenter;
      }

      public void Execute()
      {
         _simulationExplorerPresenter.ToggleVisibility();
      }
   }
}