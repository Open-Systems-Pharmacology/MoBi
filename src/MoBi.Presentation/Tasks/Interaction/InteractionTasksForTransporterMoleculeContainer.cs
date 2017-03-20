using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForTransporterMoleculeContainer : IInteractionTasksForChildren<IMoleculeBuilder, TransporterMoleculeContainer>
   {
   }

   public class InteractionTasksForTransporterMoleculeContainer : InteractionTasksForChildren<IMoleculeBuilder, TransporterMoleculeContainer>, IInteractionTasksForTransporterMoleculeContainer
   {
      public InteractionTasksForTransporterMoleculeContainer(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TransporterMoleculeContainer> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(TransporterMoleculeContainer transportBuilderToRemove, IMoleculeBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveActiveTransportBuilderContainerCommand((IMoleculeBuilder) transportBuilderToRemove.ParentContainer, transportBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(TransporterMoleculeContainer transporterMoleculeContainer, IMoleculeBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddActiveTransportBuilderContainerCommand(parent, transporterMoleculeContainer, buildingBlock);
      }
   }
}