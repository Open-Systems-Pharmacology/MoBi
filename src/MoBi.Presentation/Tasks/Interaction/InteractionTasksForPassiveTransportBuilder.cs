using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForPassiveTransportBuilder : IInteractionTasksForChildren<PassiveTransportBuildingBlock, TransportBuilder>
   {
   }

   public class InteractionTasksForPassiveTransportBuilder : InteractionTasksForBuilder<TransportBuilder, PassiveTransportBuildingBlock>, IInteractionTasksForPassiveTransportBuilder
   {
      public InteractionTasksForPassiveTransportBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TransportBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(TransportBuilder transportBuilderToRemove, PassiveTransportBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemovePassiveTransportBuilderCommand(parent, transportBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(TransportBuilder builder, PassiveTransportBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(TransportBuilder transportBuilder, PassiveTransportBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(transportBuilder, parent);
      }

      public override TransportBuilder CreateNewEntity(PassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         return base.CreateNewEntity(passiveTransportBuildingBlock)
            .WithDimension(_interactionTaskContext.DimensionByName(Constants.Dimension.AMOUNT_PER_TIME));
      }

      public override IMoBiCommand GetAddCommand(TransportBuilder transportBuilder, PassiveTransportBuildingBlock buildingBlock)
      {
         return new AddPassiveTransportBuilderCommand(buildingBlock, transportBuilder);
      }
   }
}