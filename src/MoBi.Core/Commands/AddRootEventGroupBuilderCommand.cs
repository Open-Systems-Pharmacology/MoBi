using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddRootEventGroupBuilderCommand : AddObjectBaseCommand<IEventGroupBuilder, IEventGroupBuildingBlock>
   {
      public AddRootEventGroupBuilderCommand(IEventGroupBuildingBlock parent, IEventGroupBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveRootEventGroupBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(IEventGroupBuilder child, IEventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveRootEventGroupBuilderCommand : RemoveObjectBaseCommand<IEventGroupBuilder, IEventGroupBuildingBlock>
   {
      public RemoveRootEventGroupBuilderCommand(IEventGroupBuildingBlock parent, IEventGroupBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddRootEventGroupBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(IEventGroupBuilder childToRemove, IEventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }

   public class AddRootApplicationBuilderCommand : AddObjectBaseCommand<IApplicationBuilder, IEventGroupBuildingBlock>
   {
      public AddRootApplicationBuilderCommand(IEventGroupBuildingBlock parent, IApplicationBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveRootApplicationBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(IApplicationBuilder child, IEventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveRootApplicationBuilderCommand : RemoveObjectBaseCommand<IApplicationBuilder, IEventGroupBuildingBlock>
   {
      public RemoveRootApplicationBuilderCommand(IEventGroupBuildingBlock parent, IApplicationBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddRootApplicationBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(IApplicationBuilder childToRemove, IEventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }

   public class AddEventGroupBuilderCommand : AddObjectBaseCommand<IEventGroupBuilder, IEventGroupBuilder>
   {
      public AddEventGroupBuilderCommand(IEventGroupBuilder parent, IEventGroupBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveEventGroupBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(IEventGroupBuilder child, IEventGroupBuilder parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveEventGroupBuilderCommand : RemoveObjectBaseCommand<IEventGroupBuilder, IEventGroupBuilder>
   {
      public RemoveEventGroupBuilderCommand(IEventGroupBuilder parent, IEventGroupBuilder itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddEventGroupBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(IEventGroupBuilder childToRemove, IEventGroupBuilder parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }

   public class AddApplicationBuilderCommand : AddObjectBaseCommand<IApplicationBuilder, IEventGroupBuilder>
   {
      public AddApplicationBuilderCommand(IEventGroupBuilder parent, IApplicationBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveApplicationBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(IApplicationBuilder child, IEventGroupBuilder parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveApplicationBuilderCommand : RemoveObjectBaseCommand<IApplicationBuilder, IEventGroupBuilder>
   {
      public RemoveApplicationBuilderCommand(IEventGroupBuilder parent, IApplicationBuilder itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddApplicationBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(IApplicationBuilder childToRemove, IEventGroupBuilder parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }
}