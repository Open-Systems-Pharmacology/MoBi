using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForEventGroupChildren<TChild> : InteractionTasksForChildren<EventGroupBuilder, TChild> where TChild : class, IEntity
   {
      protected InteractionTasksForEventGroupChildren(IInteractionTaskContext interactionTaskContext, IEditTaskFor<TChild> editTask)
         : base(interactionTaskContext, editTask)
      {
      }
   }

   public class InteractionTasksForContainerAtEventGroup : InteractionTasksForEventGroupChildren<IContainer>
   {
      public InteractionTasksForContainerAtEventGroup(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IContainer> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IContainer containerToRemove, EventGroupBuilder eventGroupBuilder, IBuildingBlock buildingBlock)
      {
         return new RemoveContainerFromEventGroupCommand(eventGroupBuilder, containerToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IContainer containerToAdd, EventGroupBuilder eventGroupBuilder, IBuildingBlock buildingBlock)
      {
         return new AddContainerToEventGroupCommand(eventGroupBuilder, containerToAdd, buildingBlock);
      }

      public override IContainer CreateNewEntity(EventGroupBuilder eventGroupBuilder)
      {
         var newEntity = base.CreateNewEntity(eventGroupBuilder);
         newEntity.ContainerType = ContainerType.Other;
         return newEntity;
      }
   }

   public class InteractionTasksForEventGroupBuilderAsEventGroupChild : InteractionTasksForEventGroupChildren<EventGroupBuilder>
   {
      public InteractionTasksForEventGroupBuilderAsEventGroupChild(IInteractionTaskContext interactionTaskContext, IEditTaskFor<EventGroupBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(EventGroupBuilder eventGroupBuilderToRemove, EventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventGroupBuilderCommand(parent, eventGroupBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(EventGroupBuilder eventGroupBuilderToAdd, EventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventGroupBuilderCommand(parent, eventGroupBuilderToAdd, buildingBlock);
      }
   }

   public class InteractionTasksForApplicationBuilderAsEventGroupChild : InteractionTasksForEventGroupChildren<ApplicationBuilder>
   {
      public InteractionTasksForApplicationBuilderAsEventGroupChild(IInteractionTaskContext interactionTaskContext, IEditTaskFor<ApplicationBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(ApplicationBuilder applicationBuilderToRemove, EventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventGroupBuilderCommand(parent, applicationBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(ApplicationBuilder applicationBuilderToAdd, EventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventGroupBuilderCommand(parent, applicationBuilderToAdd, buildingBlock);
      }
   }

  

   public class InteractionTasksForEventBuilder : InteractionTasksForEventGroupChildren<EventBuilder>
   {
      public InteractionTasksForEventBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<EventBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(EventBuilder eventToRemove, EventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventFromEventGroupCommand(parent, eventToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(EventBuilder eventToAdd, EventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventToEventGroupCommand(parent, eventToAdd, buildingBlock);
      }
   }
}