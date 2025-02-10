using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddTransportBuilderToApplicationBuilderCommand : AddObjectBaseCommand<TransportBuilder, ApplicationBuilder>
   {
      public AddTransportBuilderToApplicationBuilderCommand(ApplicationBuilder parent, TransportBuilder itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveTransportBuilderFromApplicationBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(TransportBuilder child, ApplicationBuilder parent, IMoBiContext context)
      {
         parent.AddTransport(child);
      }
   }

   public class RemoveTransportBuilderFromApplicationBuilderCommand : RemoveObjectBaseCommand<TransportBuilder, ApplicationBuilder>
   {
      public RemoveTransportBuilderFromApplicationBuilderCommand(ApplicationBuilder parent, TransportBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddTransportBuilderToApplicationBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(TransportBuilder childToRemove, ApplicationBuilder parent, IMoBiContext context)
      {
         _parent.RemoveTransport(_itemToRemove);
      }
   }
}