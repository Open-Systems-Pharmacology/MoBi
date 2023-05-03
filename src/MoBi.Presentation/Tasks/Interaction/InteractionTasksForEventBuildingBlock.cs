using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForEventBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<Module, EventGroupBuildingBlock, EventGroupBuilder>
   {
      public InteractionTasksForEventBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<EventGroupBuildingBlock> editTask,
         IInteractionTasksForBuilder<EventGroupBuilder> builderTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(EventGroupBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<EventGroupBuildingBlock>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(EventGroupBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<EventGroupBuildingBlock>(itemToAdd, parent);
      }
   }
}