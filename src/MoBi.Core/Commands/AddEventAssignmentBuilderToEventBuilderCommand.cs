using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddEventAssignmentBuilderToEventBuilderCommand:AddObjectBaseCommand<EventAssignmentBuilder,EventBuilder>
   {
      public AddEventAssignmentBuilderToEventBuilderCommand(EventBuilder parent, EventAssignmentBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveEventAssignmentBuilderFromEventBuilderCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(EventAssignmentBuilder child, EventBuilder parent, IMoBiContext context)
      {
         parent.AddAssignment(child);
      }
   }

   public class RemoveEventAssignmentBuilderFromEventBuilderCommand : RemoveObjectBaseCommand<EventAssignmentBuilder,EventBuilder>
   {
      public RemoveEventAssignmentBuilderFromEventBuilderCommand(EventBuilder parent, EventAssignmentBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddEventAssignmentBuilderToEventBuilderCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }
     
      protected override void RemoveFrom(EventAssignmentBuilder childToRemove, EventBuilder parent, IMoBiContext context)
      {
         parent.RemoveAssignment(childToRemove);
      }
   }
}