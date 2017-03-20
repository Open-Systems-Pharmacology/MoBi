using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddMatchAllConditionToObserverBuilderCommand : AddMatchAllConditionCommandBase<IObserverBuilder>
   {
      public AddMatchAllConditionToObserverBuilderCommand(IObserverBuilder observerBuilder, IBuildingBlock buildingBlock) :
         base(observerBuilder, buildingBlock, x => x.ContainerCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMatchAllConditionFromObserverBuilderCommand(_taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveMatchAllConditionFromObserverBuilderCommand : RemoveMatchAllConditionCommandBase<IObserverBuilder>
   {
      public RemoveMatchAllConditionFromObserverBuilderCommand(IObserverBuilder observerBuilder, IBuildingBlock buildingBlock) :
         base(observerBuilder, buildingBlock, x => x.ContainerCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMatchAllConditionToObserverBuilderCommand(_taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class AddContainerNotMatchTagConditionToObserverBuilderCommand : AddNotMatchTagConditionCommandBase<IObserverBuilder>
   {
      public AddContainerNotMatchTagConditionToObserverBuilderCommand(string tag, IObserverBuilder observerBuilder, IBuildingBlock buildingBlock)
         : base(tag, observerBuilder, buildingBlock, x => x.ContainerCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveContainerNotMatchTagConditionFromObserverBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveContainerNotMatchTagConditionFromObserverBuilderCommand : RemoveNotMatchTagConditionCommandBase<IObserverBuilder>
   {
      public RemoveContainerNotMatchTagConditionFromObserverBuilderCommand(string tag, IObserverBuilder observerBuilder, IBuildingBlock buildingBlock)
         : base(tag, observerBuilder, buildingBlock, x => x.ContainerCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddContainerNotMatchTagConditionToObserverBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class AddContainerMatchTagConditionToObserverBuilderCommand : AddMatchTagConditionCommandBase<IObserverBuilder>
   {
      public AddContainerMatchTagConditionToObserverBuilderCommand(string tag, IObserverBuilder observerBuilder, IBuildingBlock buildingBlock)
         : base(tag, observerBuilder, buildingBlock, x => x.ContainerCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveContainerMatchTagConditionFromObserverBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }

   public class RemoveContainerMatchTagConditionFromObserverBuilderCommand : RemoveMatchTagConditionCommandBase<IObserverBuilder>
   {
      public RemoveContainerMatchTagConditionFromObserverBuilderCommand(string tag, IObserverBuilder observerBuilder, IBuildingBlock buildingBlock)
         : base(tag, observerBuilder, buildingBlock, x => x.ContainerCriteria)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddContainerMatchTagConditionToObserverBuilderCommand(_tag, _taggedObject, _buildingBlock).AsInverseFor(this);
      }
   }
}