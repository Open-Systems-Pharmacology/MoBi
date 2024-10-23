using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForPassiveTransportBuildingBlock : IInteractionTasksForBuildingBlock<Module, PassiveTransportBuildingBlock>
   {
      IMoBiCommand GetRemoveCommand(PassiveTransportBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock);
   }

   public class InteractionTasksForPassiveTransportBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<Module, PassiveTransportBuildingBlock, TransportBuilder>, IInteractionTasksForPassiveTransportBuildingBlock
   {
      public InteractionTasksForPassiveTransportBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<PassiveTransportBuildingBlock> editTask,
         IInteractionTasksForBuilder<TransportBuilder> builderTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(PassiveTransportBuildingBlock objectToRemove, Module parent, IBuildingBlock buildingBlock)
      {
         return new RemoveBuildingBlockFromModuleCommand<PassiveTransportBuildingBlock>(objectToRemove, parent);
      }

      public override IMoBiCommand GetAddCommand(PassiveTransportBuildingBlock itemToAdd, Module parent, IBuildingBlock buildingBlock)
      {
         return new AddBuildingBlockToModuleCommand<PassiveTransportBuildingBlock>(itemToAdd, parent);
      }
   }
}