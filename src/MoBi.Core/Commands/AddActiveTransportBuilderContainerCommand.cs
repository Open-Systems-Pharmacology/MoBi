using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class RemoveActiveTransportBuilderContainerCommand : RemoveObjectBaseCommand<TransporterMoleculeContainer,MoleculeBuilder>
   {
      public RemoveActiveTransportBuilderContainerCommand(MoleculeBuilder parent, TransporterMoleculeContainer itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddActiveTransportBuilderContainerCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(TransporterMoleculeContainer childToRemove, MoleculeBuilder parent, IMoBiContext context)
      {
         parent.RemoveTransporterMoleculeContainer(childToRemove);
      }
   }

   public class AddActiveTransportBuilderContainerCommand : AddObjectBaseCommand<TransporterMoleculeContainer, MoleculeBuilder>
   {
      public AddActiveTransportBuilderContainerCommand(MoleculeBuilder parent, TransporterMoleculeContainer itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveActiveTransportBuilderContainerCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(TransporterMoleculeContainer child, MoleculeBuilder parent, IMoBiContext context)
      {
         parent.AddTransporterMoleculeContainer(child);
      }
   }
}