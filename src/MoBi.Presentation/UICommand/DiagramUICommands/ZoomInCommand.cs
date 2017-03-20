using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Main;

namespace MoBi.Presentation.UICommand.DiagramUICommands
{
   public class ZoomInCommand : IUICommand
   {
      private readonly IMainViewPresenter _mainViewPresenter;

      public ZoomInCommand(IMainViewPresenter mainViewPresenter)
      {
         _mainViewPresenter = mainViewPresenter;
      }

      public void Execute()
      {
         var diagramPresenter = _mainViewPresenter.ActivePresenter as IDiagramBuildingBlockPresenter;
         if (diagramPresenter != null) diagramPresenter.ZoomIn();
      }
   }
}