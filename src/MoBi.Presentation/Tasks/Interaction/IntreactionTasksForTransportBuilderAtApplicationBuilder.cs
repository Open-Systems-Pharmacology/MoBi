using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class IntreactionTasksForTransportBuilderAtApplicationBuilder : InteractionTasksForChildren<ApplicationBuilder, TransportBuilder>
   {
      public IntreactionTasksForTransportBuilderAtApplicationBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TransportBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(TransportBuilder objectToRemove, ApplicationBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveTransportBuilderFromApplicationBuilderCommand(parent, objectToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(TransportBuilder itemToAdd, ApplicationBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddTransportBuilderToApplicationBuilderCommand(parent, itemToAdd, buildingBlock);
      }
   }
}