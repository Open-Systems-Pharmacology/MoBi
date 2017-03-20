using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class AddChildToEventGroupCommand<TChild> : AddObjectBaseCommand<TChild, IEventGroupBuilder> where TChild: class, IEntity
   {
      protected AddChildToEventGroupCommand(IEventGroupBuilder parent, TChild itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override void AddTo(TChild child, IEventGroupBuilder parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public abstract class RemoveChildFromEventGroupCommand<TChild> : RemoveObjectBaseCommand<TChild, IEventGroupBuilder> where TChild : IEntity
   {
      protected RemoveChildFromEventGroupCommand(IEventGroupBuilder parent, TChild itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override void RemoveFrom(TChild childToRemove, IEventGroupBuilder parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }

   public class RemoveContainerFromEventGroupCommand : RemoveChildFromEventGroupCommand<IContainer>
   {
      public RemoveContainerFromEventGroupCommand(IEventGroupBuilder parent, IContainer itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddContainerToEventGroupCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }
   }

   public class AddContainerToEventGroupCommand:AddChildToEventGroupCommand<IContainer>
   {
      public AddContainerToEventGroupCommand(IEventGroupBuilder parent, IContainer itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveContainerFromEventGroupCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveEventFromEventGroupCommand : RemoveChildFromEventGroupCommand<IEventBuilder>
   {
      public RemoveEventFromEventGroupCommand(IEventGroupBuilder parent, IEventBuilder itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddEventToEventGroupCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }
   }

   public class AddEventToEventGroupCommand : AddChildToEventGroupCommand<IEventBuilder>
   {
      public AddEventToEventGroupCommand(IEventGroupBuilder parent, IEventBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveEventFromEventGroupCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }
   }
}