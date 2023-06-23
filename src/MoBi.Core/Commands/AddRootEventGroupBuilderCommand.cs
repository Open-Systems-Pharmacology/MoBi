using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddRootEventGroupBuilderCommand : AddObjectBaseCommand<EventGroupBuilder, EventGroupBuildingBlock>
   {
      public AddRootEventGroupBuilderCommand(EventGroupBuildingBlock parent, EventGroupBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveRootEventGroupBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(EventGroupBuilder child, EventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveRootEventGroupBuilderCommand : RemoveObjectBaseCommand<EventGroupBuilder, EventGroupBuildingBlock>
   {
      public RemoveRootEventGroupBuilderCommand(EventGroupBuildingBlock parent, EventGroupBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddRootEventGroupBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(EventGroupBuilder childToRemove, EventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }

   public class AddRootApplicationBuilderCommand : AddObjectBaseCommand<ApplicationBuilder, EventGroupBuildingBlock>
   {
      public AddRootApplicationBuilderCommand(EventGroupBuildingBlock parent, ApplicationBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveRootApplicationBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }

      protected override void AddTo(ApplicationBuilder child, EventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveRootApplicationBuilderCommand : RemoveObjectBaseCommand<ApplicationBuilder, EventGroupBuildingBlock>
   {
      public RemoveRootApplicationBuilderCommand(EventGroupBuildingBlock parent, ApplicationBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddRootApplicationBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(ApplicationBuilder childToRemove, EventGroupBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }

   public class AddEventGroupBuilderCommand : AddObjectBaseCommand<EventGroupBuilder, EventGroupBuilder>
   {
      public AddEventGroupBuilderCommand(EventGroupBuilder parent, EventGroupBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveEventGroupBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(EventGroupBuilder child, EventGroupBuilder parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveEventGroupBuilderCommand : RemoveObjectBaseCommand<EventGroupBuilder, EventGroupBuilder>
   {
      public RemoveEventGroupBuilderCommand(EventGroupBuilder parent, EventGroupBuilder itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddEventGroupBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(EventGroupBuilder childToRemove, EventGroupBuilder parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }

   public class AddApplicationBuilderCommand : AddObjectBaseCommand<ApplicationBuilder, EventGroupBuilder>
   {
      public AddApplicationBuilderCommand(EventGroupBuilder parent, ApplicationBuilder itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveApplicationBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(ApplicationBuilder child, EventGroupBuilder parent, IMoBiContext context)
      {
         parent.Add(child);
      }
   }

   public class RemoveApplicationBuilderCommand : RemoveObjectBaseCommand<ApplicationBuilder, EventGroupBuilder>
   {
      public RemoveApplicationBuilderCommand(EventGroupBuilder parent, ApplicationBuilder itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddApplicationBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(ApplicationBuilder childToRemove, EventGroupBuilder parent, IMoBiContext context)
      {
         parent.RemoveChild(childToRemove);
      }
   }
}