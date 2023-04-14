using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddObserverBuilderCommand : AddObjectBaseCommand<ObserverBuilder, ObserverBuildingBlock>
   {
      public AddObserverBuilderCommand(ObserverBuildingBlock observerBuildingBlock, ObserverBuilder itemToAdd) : base(observerBuildingBlock, itemToAdd, observerBuildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveObserverBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(ObserverBuilder child, ObserverBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveObserverBuilderCommand : RemoveObjectBaseCommand<ObserverBuilder, ObserverBuildingBlock>
   {
      public RemoveObserverBuilderCommand(ObserverBuildingBlock parent, ObserverBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddObserverBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(ObserverBuilder childToRemove, ObserverBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }
}