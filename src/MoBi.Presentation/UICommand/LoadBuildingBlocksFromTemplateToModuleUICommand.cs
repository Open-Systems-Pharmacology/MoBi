using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadBuildingBlocksFromTemplateToModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModule _interactionTasks;

      public LoadBuildingBlocksFromTemplateToModuleUICommand(IInteractionTasksForModule interactionTasksForModule, IMoBiContext context)
      {
         _interactionTasks = interactionTasksForModule;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.LoadBuildingBlocksFromTemplateToModule(Subject);
      }
   }
}