using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForPassiveTransportBuildingBlock : InteractionTasksForEnumerableBuildingBlockOfContainerBuilder<Module, PassiveTransportBuildingBlock, TransportBuilder>
   {
      public InteractionTasksForPassiveTransportBuildingBlock(
         IInteractionTaskContext interactionTaskContext,
         IEditTasksForBuildingBlock<PassiveTransportBuildingBlock> editTask,
         IInteractionTasksForBuilder<TransportBuilder> builderTask)
         : base(interactionTaskContext, editTask, builderTask)
      {
      }
   }
}