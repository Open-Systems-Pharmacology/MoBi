using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddTransportBuilderToApplicationBuilderCommand : AddObjectBaseCommand<ITransportBuilder, IApplicationBuilder>
   {
      public AddTransportBuilderToApplicationBuilderCommand(IApplicationBuilder parent, ITransportBuilder itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveTransportBuilderFromApplicationBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(ITransportBuilder child, IApplicationBuilder parent, IMoBiContext context)
      {
         parent.AddTransport(child);
      }
   }

   public class RemoveTransportBuilderFromApplicationBuilderCommand : RemoveObjectBaseCommand<ITransportBuilder, IApplicationBuilder>
   {
      public RemoveTransportBuilderFromApplicationBuilderCommand(IApplicationBuilder parent, ITransportBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddTransportBuilderToApplicationBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(ITransportBuilder childToRemove, IApplicationBuilder parent, IMoBiContext context)
      {
         _parent.RemoveTransport(_itemToRemove);
      }
   }
}