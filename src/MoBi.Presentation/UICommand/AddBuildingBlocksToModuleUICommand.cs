using System.Runtime.InteropServices;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class AddBuildingBlocksToModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModule _interactionTasks;

      public AddBuildingBlocksToModuleUICommand(IInteractionTasksForModule interactionTasksForModule, IMoBiContext context)
      {
         _interactionTasks = interactionTasksForModule;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.AddBuildingBlocksToModule(Subject);
      }
   }
}