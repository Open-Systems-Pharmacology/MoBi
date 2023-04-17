using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForInteractionContainer : IInteractionTasksForChildren<MoleculeBuilder, InteractionContainer>
   {
   }

   public class InteractionTasksForInteractionContainer :InteractionTasksForChildren<MoleculeBuilder,InteractionContainer>, IInteractionTasksForInteractionContainer
   {
      public InteractionTasksForInteractionContainer(IInteractionTaskContext interactionTaskContext, IEditTaskFor<InteractionContainer> editTask) : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(InteractionContainer itemToRemove, MoleculeBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveInteracionContainerCommand(parent,itemToRemove,buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(InteractionContainer itemToAdd, MoleculeBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddInteracionContainerCommand(parent,itemToAdd,buildingBlock);
      }
   }
}