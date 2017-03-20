using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class IntreactionTasksForTransportBuilderAtApplicationBuilder : InteractionTasksForChildren<IApplicationBuilder, ITransportBuilder>
   {
      public IntreactionTasksForTransportBuilderAtApplicationBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ITransportBuilder> editTask) : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(ITransportBuilder objectToRemove, IApplicationBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveTransportBuilderFromApplicationBuilderCommand(parent, objectToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(ITransportBuilder itemToAdd, IApplicationBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddTransportBuilderToApplicationBuilderCommand(parent, itemToAdd, buildingBlock);
      }
   }
}