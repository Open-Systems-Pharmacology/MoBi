using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Main;

namespace MoBi.Presentation.UICommand.DiagramUICommands
{
   public class LayoutByForcesCommand : IUICommand
   {
      private readonly IMainViewPresenter _mainViewPresenter;

      public LayoutByForcesCommand(IMainViewPresenter mainViewPresenter)
      {
         _mainViewPresenter = mainViewPresenter;
      }

      public void Execute()
      {
         var diagramPresenter = _mainViewPresenter.ActivePresenter as IDiagramBuildingBlockPresenter;
         if (diagramPresenter != null) diagramPresenter.LayoutByForces();
      }
   }
}