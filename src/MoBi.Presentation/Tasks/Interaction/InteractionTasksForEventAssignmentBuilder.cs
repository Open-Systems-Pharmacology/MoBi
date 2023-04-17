using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForEventAssignmentBuilder : InteractionTasksForChildren<EventBuilder, EventAssignmentBuilder>
   {
      public InteractionTasksForEventAssignmentBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<EventAssignmentBuilder> editTasks)
         : base(interactionTaskContext, editTasks)
      {
      }

      public override IMoBiCommand GetRemoveCommand(EventAssignmentBuilder eventAssignmentBuildertoRemove, EventBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventAssignmentBuilderFromEventBuilderCommand(parent, eventAssignmentBuildertoRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(EventAssignmentBuilder eventAssignmentBuilder, EventBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventAssignmentBuilderToEventBuilderCommand(parent, eventAssignmentBuilder, buildingBlock);
      }
   }
}