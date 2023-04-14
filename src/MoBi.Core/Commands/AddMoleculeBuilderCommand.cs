using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddMoleculeBuilderCommand : AddObjectBaseCommand<MoleculeBuilder, MoleculeBuildingBlock>
   {
      public AddMoleculeBuilderCommand(MoleculeBuildingBlock parent, MoleculeBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override void AddTo(MoleculeBuilder child, MoleculeBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMoleculeBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }
   }

   public class RemoveMoleculeBuilderCommand : RemoveObjectBaseCommand<MoleculeBuilder, MoleculeBuildingBlock>
   {
      public RemoveMoleculeBuilderCommand(MoleculeBuildingBlock parent, MoleculeBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMoleculeBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(MoleculeBuilder childToRemove, MoleculeBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }
}