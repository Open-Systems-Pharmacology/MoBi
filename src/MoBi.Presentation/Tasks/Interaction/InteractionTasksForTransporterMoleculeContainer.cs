using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForTransporterMoleculeContainer : IInteractionTasksForChildren<MoleculeBuilder, TransporterMoleculeContainer>
   {
   }

   public class InteractionTasksForTransporterMoleculeContainer : InteractionTasksForChildren<MoleculeBuilder, TransporterMoleculeContainer>, IInteractionTasksForTransporterMoleculeContainer
   {
      public InteractionTasksForTransporterMoleculeContainer(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TransporterMoleculeContainer> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(TransporterMoleculeContainer transportBuilderToRemove, MoleculeBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveActiveTransportBuilderContainerCommand((MoleculeBuilder) transportBuilderToRemove.ParentContainer, transportBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(TransporterMoleculeContainer transporterMoleculeContainer, MoleculeBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddActiveTransportBuilderContainerCommand(parent, transporterMoleculeContainer, buildingBlock);
      }
   }
}