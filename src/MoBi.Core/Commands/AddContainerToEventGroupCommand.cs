using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class AddChildToEventGroupCommand<TChild> : AddObjectBaseCommand<TChild, EventGroupBuilder> where TChild: class, IEntity
   {
      protected AddChildToEventGroupCommand(EventGroupBuilder parent, TChild itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override void AddTo(TChild child, EventGroupBuilder parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public abstract class RemoveChildFromEventGroupCommand<TChild> : RemoveObjectBaseCommand<TChild, EventGroupBuilder> where TChild : IEntity
   {
      protected RemoveChildFromEventGroupCommand(EventGroupBuilder parent, TChild itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override void RemoveFrom(TChild childToRemove, EventGroupBuilder parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }

   public class RemoveContainerFromEventGroupCommand : RemoveChildFromEventGroupCommand<IContainer>
   {
      public RemoveContainerFromEventGroupCommand(EventGroupBuilder parent, IContainer itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddContainerToEventGroupCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }
   }

   public class AddContainerToEventGroupCommand:AddChildToEventGroupCommand<IContainer>
   {
      public AddContainerToEventGroupCommand(EventGroupBuilder parent, IContainer itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveContainerFromEventGroupCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveEventFromEventGroupCommand : RemoveChildFromEventGroupCommand<EventBuilder>
   {
      public RemoveEventFromEventGroupCommand(EventGroupBuilder parent, EventBuilder itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddEventToEventGroupCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }
   }

   public class AddEventToEventGroupCommand : AddChildToEventGroupCommand<EventBuilder>
   {
      public AddEventToEventGroupCommand(EventGroupBuilder parent, EventBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveEventFromEventGroupCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }
   }
}