using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForActiveTransportBuilder : IInteractionTasksForChildren<TransporterMoleculeContainer, ITransportBuilder>
   {
   }

   public class InteractionTasksForActiveTransportBuilder : InteractionTasksForChildren<TransporterMoleculeContainer, ITransportBuilder>, IInteractionTasksForActiveTransportBuilder
   {
      public InteractionTasksForActiveTransportBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ITransportBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override ITransportBuilder CreateNewEntity(TransporterMoleculeContainer transporterMoleculeContainer)
      {
         return base.CreateNewEntity(transporterMoleculeContainer)
            .WithDimension(_interactionTaskContext.DimensionByName(Constants.Dimension.AMOUNT_PER_TIME));
      }

      public override IMoBiCommand GetRemoveCommand(ITransportBuilder transportBuilderToRemove, TransporterMoleculeContainer transporterMoleculeContainer, IBuildingBlock buildingBlock)
      {
         return new RemoveActiveTransportBuilderCommand(transporterMoleculeContainer, transportBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(ITransportBuilder transportBuilder, TransporterMoleculeContainer transporterMoleculeContainer, IBuildingBlock buildingBlock)
      {
         return new AddActiveTransportBuilderCommand(transporterMoleculeContainer, transportBuilder, buildingBlock);
      }
   }
}