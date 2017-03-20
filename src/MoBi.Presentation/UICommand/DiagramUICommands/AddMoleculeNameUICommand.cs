using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Main;

namespace MoBi.Presentation.UICommand.DiagramUICommands
{
   public class AddMoleculeNameUICommand : IUICommand
   {
      private readonly IMainViewPresenter _mainViewPresenter;

      public AddMoleculeNameUICommand(IMainViewPresenter mainViewPresenter)
      {
         _mainViewPresenter = mainViewPresenter;
      }

      public void Execute()
      {
         var presenter = (IEditReactionBuildingBlockPresenter) _mainViewPresenter.ActivePresenter;
         presenter.AddReactionMoleculeNode();
      }
   }
}