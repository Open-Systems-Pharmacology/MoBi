using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddObserverBuilderCommand : AddObjectBaseCommand<IObserverBuilder, IObserverBuildingBlock>
   {
      public AddObserverBuilderCommand(IObserverBuildingBlock observerBuildingBlock, IObserverBuilder itemToAdd) : base(observerBuildingBlock, itemToAdd, observerBuildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveObserverBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(IObserverBuilder child, IObserverBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveObserverBuilderCommand : RemoveObjectBaseCommand<IObserverBuilder, IObserverBuildingBlock>
   {
      public RemoveObserverBuilderCommand(IObserverBuildingBlock parent, IObserverBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddObserverBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(IObserverBuilder childToRemove, IObserverBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }
}