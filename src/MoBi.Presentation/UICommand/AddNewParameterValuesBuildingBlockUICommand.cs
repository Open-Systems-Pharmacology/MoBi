using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   internal class AddNewParameterValuesBuildingBlockUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModule _interactionTasks;

      public AddNewParameterValuesBuildingBlockUICommand(IInteractionTasksForModule interactionTasks)
      {
         _interactionTasks = interactionTasks;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.AddNewParameterValuesBuildingBlock(Subject);
      }
   }
}