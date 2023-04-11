using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class RemoveModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModuleBuildingBlocks _interactionTasks;

      public RemoveModuleUICommand(IInteractionTasksForModuleBuildingBlocks interactionTasksForModule)
      {
         _interactionTasks = interactionTasksForModule;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.RemoveModule(Subject);
      }
   }
}