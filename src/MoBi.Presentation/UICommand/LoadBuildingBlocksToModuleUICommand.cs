using MoBi.Core.Domain.Model;
using MoBi.Presentation.Tasks.Interaction;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class LoadBuildingBlocksToModuleUICommand : ObjectUICommand<Module>
   {
      private readonly IInteractionTasksForModuleBuildingBlocks _interactionTasks;

      public LoadBuildingBlocksToModuleUICommand(IInteractionTasksForModuleBuildingBlocks interactionTasksForModuleModule, IMoBiContext context)
      {
         _interactionTasks = interactionTasksForModuleModule;
      }

      protected override void PerformExecute()
      {
         _interactionTasks.LoadBuildingBlocksToModule(Subject);
      }
   }
}