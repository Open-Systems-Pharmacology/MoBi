using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddPassiveTransportBuilderCommand:AddObjectBaseCommand<TransportBuilder,PassiveTransportBuildingBlock>
   {
      public AddPassiveTransportBuilderCommand(PassiveTransportBuildingBlock parent, TransportBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemovePassiveTransportBuilderCommand(_parent,_itemToAdd).AsInverseFor(this);
      }
      
      protected override void AddTo(TransportBuilder child, PassiveTransportBuildingBlock parent, IMoBiContext context)
      {
         _parent.Add(_itemToAdd);
      }
   }

   public class RemovePassiveTransportBuilderCommand : RemoveObjectBaseCommand<TransportBuilder,PassiveTransportBuildingBlock>
   {
      public RemovePassiveTransportBuilderCommand(PassiveTransportBuildingBlock parent, TransportBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddPassiveTransportBuilderCommand(_parent,_itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(TransportBuilder childToRemove, PassiveTransportBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }

   public class AddActiveTransportBuilderCommand : AddObjectBaseCommand<TransportBuilder, TransporterMoleculeContainer>
   {
      public AddActiveTransportBuilderCommand(TransporterMoleculeContainer parent, TransportBuilder itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveActiveTransportBuilderCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(TransportBuilder child, TransporterMoleculeContainer parent, IMoBiContext context)
      {
         parent.AddActiveTransportRealization(child);
      }
   }

   public class RemoveActiveTransportBuilderCommand : RemoveObjectBaseCommand<TransportBuilder, TransporterMoleculeContainer>
   {
      public RemoveActiveTransportBuilderCommand(TransporterMoleculeContainer parent, TransportBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddActiveTransportBuilderCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(TransportBuilder childToRemove, TransporterMoleculeContainer parent, IMoBiContext context)
      {
         parent.RemoveActiveTransportRealization(childToRemove);
      }
   }
   
}