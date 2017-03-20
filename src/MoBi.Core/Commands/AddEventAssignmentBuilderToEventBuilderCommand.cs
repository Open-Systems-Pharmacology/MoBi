using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddEventAssignmentBuilderToEventBuilderCommand:AddObjectBaseCommand<IEventAssignmentBuilder,IEventBuilder>
   {
      public AddEventAssignmentBuilderToEventBuilderCommand(IEventBuilder parent, IEventAssignmentBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveEventAssigmentBuilderFromEventBuilderCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(IEventAssignmentBuilder child, IEventBuilder parent, IMoBiContext context)
      {
         parent.AddAssignment(child);
      }
   }

   public class RemoveEventAssigmentBuilderFromEventBuilderCommand : RemoveObjectBaseCommand<IEventAssignmentBuilder,IEventBuilder>
   {
      public RemoveEventAssigmentBuilderFromEventBuilderCommand(IEventBuilder parent, IEventAssignmentBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddEventAssignmentBuilderToEventBuilderCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }
     
      protected override void RemoveFrom(IEventAssignmentBuilder childToRemove, IEventBuilder parent, IMoBiContext context)
      {
         parent.RemoveAssignment(childToRemove);
      }
   }
}