using System.Collections.Generic;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveMultipleModulesUICommand : ObjectUICommand<IReadOnlyList<Module>>
   {
      private readonly IInteractionTasksForModule _interactionTasks;

      public RemoveMultipleModulesUICommand(IInteractionTasksForModule interactionTasks)
      {
         _interactionTasks = interactionTasks;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.RemoveMultipleModules(Subject);
      }
   }
}