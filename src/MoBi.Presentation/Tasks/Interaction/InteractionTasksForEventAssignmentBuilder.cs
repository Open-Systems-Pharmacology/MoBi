using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public class InteractionTasksForEventAssignmentBuilder : InteractionTasksForChildren<IEventBuilder, IEventAssignmentBuilder>
   {
      public InteractionTasksForEventAssignmentBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IEventAssignmentBuilder> editTasks)
         : base(interactionTaskContext, editTasks)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IEventAssignmentBuilder eventAssignmentBuildertoRemove, IEventBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventAssigmentBuilderFromEventBuilderCommand(parent, eventAssignmentBuildertoRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IEventAssignmentBuilder eventAssignmentBuilder, IEventBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventAssignmentBuilderToEventBuilderCommand(parent, eventAssignmentBuilder, buildingBlock);
      }
   }
}