using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddPassiveTransportBuilderCommand:AddObjectBaseCommand<ITransportBuilder,IPassiveTransportBuildingBlock>
   {
      public AddPassiveTransportBuilderCommand(IPassiveTransportBuildingBlock parent, ITransportBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemovePassiveTransportBuilderCommand(_parent,_itemToAdd).AsInverseFor(this);
      }
      
      protected override void AddTo(ITransportBuilder child, IPassiveTransportBuildingBlock parent, IMoBiContext context)
      {
         _parent.Add(_itemToAdd);
      }
   }

   public class RemovePassiveTransportBuilderCommand : RemoveObjectBaseCommand<ITransportBuilder,IPassiveTransportBuildingBlock>
   {
      public RemovePassiveTransportBuilderCommand(IPassiveTransportBuildingBlock parent, ITransportBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddPassiveTransportBuilderCommand(_parent,_itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(ITransportBuilder childToRemove, IPassiveTransportBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }

   public class AddActiveTransportBuilderCommand : AddObjectBaseCommand<ITransportBuilder, TransporterMoleculeContainer>
   {
      public AddActiveTransportBuilderCommand(TransporterMoleculeContainer parent, ITransportBuilder itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveActiveTransportBuilderCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(ITransportBuilder child, TransporterMoleculeContainer parent, IMoBiContext context)
      {
         parent.AddActiveTransportRealization(child);
      }
   }

   public class RemoveActiveTransportBuilderCommand : RemoveObjectBaseCommand<ITransportBuilder, TransporterMoleculeContainer>
   {
      public RemoveActiveTransportBuilderCommand(TransporterMoleculeContainer parent, ITransportBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddActiveTransportBuilderCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(ITransportBuilder childToRemove, TransporterMoleculeContainer parent, IMoBiContext context)
      {
         parent.RemoveActiveTransportRealization(childToRemove);
      }
   }
   
}