using OSPSuite.Presentation.MenuAndBars;
using MoBi.Presentation.Tasks.Interaction;

namespace MoBi.Presentation.UICommand
{
   internal class AddNewMoleculeBuildingBlockFromSelectionUiCommand : IUICommand
   {
      private readonly IInteractionTasksForMoleculeBuildingBlock _tasks;

      public AddNewMoleculeBuildingBlockFromSelectionUiCommand(IInteractionTasksForMoleculeBuildingBlock tasks)
      {
         _tasks = tasks;
      }

      public void Execute()
      {
         _tasks.CreateNewFromSelection();
      }
   }
}