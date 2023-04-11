using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadBuildingBlocksFromTemplateToModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModuleBuildingBlocks _interactionTasks;

      public LoadBuildingBlocksFromTemplateToModuleUICommand(IInteractionTasksForModuleBuildingBlocks interactionTasksForModuleModule, IMoBiContext context)
      {
         _interactionTasks = interactionTasksForModuleModule;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.LoadBuildingBlocksFromTemplateToModule(Subject);
      }
   }
}