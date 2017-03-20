using MoBi.Core.Commands;
using MoBi.Presentation.Tasks.Edit;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Tasks.Interaction
{
   public abstract class InteractionTasksForEventGroupChildren<TChild> : InteractionTasksForChildren<IEventGroupBuilder, TChild> where TChild : class, IEntity
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

      public override IMoBiCommand GetRemoveCommand(IContainer containerToRemove, IEventGroupBuilder eventGroupBuilder, IBuildingBlock buildingBlock)
      {
         return new RemoveContainerFromEventGroupCommand(eventGroupBuilder, containerToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IContainer containerToAdd, IEventGroupBuilder eventGroupBuilder, IBuildingBlock buildingBlock)
      {
         return new AddContainerToEventGroupCommand(eventGroupBuilder, containerToAdd, buildingBlock);
      }

      public override IContainer CreateNewEntity(IEventGroupBuilder eventGroupBuilder)
      {
         var newEntity = base.CreateNewEntity(eventGroupBuilder);
         newEntity.ContainerType = ContainerType.Other;
         return newEntity;
      }
   }

   public class InteractionTasksForEventGroupBuilderAsEventGroupChild : InteractionTasksForEventGroupChildren<IEventGroupBuilder>
   {
      public InteractionTasksForEventGroupBuilderAsEventGroupChild(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IEventGroupBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IEventGroupBuilder eventGroupBuilderToRemove, IEventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventGroupBuilderCommand(parent, eventGroupBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IEventGroupBuilder eventGroupBuilderToAdd, IEventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventGroupBuilderCommand(parent, eventGroupBuilderToAdd, buildingBlock);
      }
   }

   public class InteractionTasksForApplicationBuilderAsEventGroupChild : InteractionTasksForEventGroupChildren<IApplicationBuilder>
   {
      public InteractionTasksForApplicationBuilderAsEventGroupChild(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IApplicationBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IApplicationBuilder applicationBuilderToRemove, IEventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventGroupBuilderCommand(parent, applicationBuilderToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IApplicationBuilder applicationBuilderToAdd, IEventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventGroupBuilderCommand(parent, applicationBuilderToAdd, buildingBlock);
      }
   }

  

   public class InteractionTasksForEventBuilder : InteractionTasksForEventGroupChildren<IEventBuilder>
   {
      public InteractionTasksForEventBuilder(IInteractionTaskContext interactionTaskContext, IEditTaskFor<IEventBuilder> editTask)
         : base(interactionTaskContext, editTask)
      {
      }

      public override IMoBiCommand GetRemoveCommand(IEventBuilder eventToRemove, IEventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new RemoveEventFromEventGroupCommand(parent, eventToRemove, buildingBlock);
      }

      public override IMoBiCommand GetAddCommand(IEventBuilder eventToAdd, IEventGroupBuilder parent, IBuildingBlock buildingBlock)
      {
         return new AddEventToEventGroupCommand(parent, eventToAdd, buildingBlock);
      }
   }
}