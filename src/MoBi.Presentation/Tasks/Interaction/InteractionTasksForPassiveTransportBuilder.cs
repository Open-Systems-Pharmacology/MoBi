using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public interface IInteractionTasksForPassiveTransportBuilder : IInteractionTasksForChildren<IPassiveTransportBuildingBlock, ITransportBuilder>
   {
   }

   public class InteractionTasksForPassiveTransportBuilder : InteractionTasksForBuilder<ITransportBuilder, IPassiveTransportBuildingBlock>, IInteractionTasksForPassiveTransportBuilder
   {
      public InteractionTasksForPassiveTransportBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ITransportBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(ITransportBuilder transportBuilderToRemove, IPassiveTransportBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return new RemovePassiveTransportBuilderCommand(parent, transportBuilderToRemove);
      }

      public override IMoBiCommand GetRemoveCommand(ITransportBuilder builder, IPassiveTransportBuildingBlock buildingBlock)
      {
         return GetRemoveCommand(builder, buildingBlock, null);
      }

      public override IMoBiCommand GetAddCommand(ITransportBuilder transportBuilder, IPassiveTransportBuildingBlock parent, IBuildingBlock buildingBlock)
      {
         return GetAddCommand(transportBuilder, parent);
      }

      public override ITransportBuilder CreateNewEntity(IPassiveTransportBuildingBlock passiveTransportBuildingBlock)
      {
         return base.CreateNewEntity(passiveTransportBuildingBlock)
            .WithDimension(_interactionTaskContext.DimensionByName(Constants.Dimension.AMOUNT_PER_TIME));
      }

      public override IMoBiCommand GetAddCommand(ITransportBuilder transportBuilder, IPassiveTransportBuildingBlock buildingBlock)
      {
         return new AddPassiveTransportBuilderCommand(buildingBlock, transportBuilder);
      }
   }
}