﻿using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddMoleculeBuilderCommand : AddObjectBaseCommand<IMoleculeBuilder, MoleculeBuildingBlock>
   {
      public AddMoleculeBuilderCommand(MoleculeBuildingBlock parent, IMoleculeBuilder itemToAdd) : base(parent, itemToAdd, parent)
      {
      }

      protected override void AddTo(IMoleculeBuilder child, MoleculeBuildingBlock parent, IMoBiContext context)
      {
         parent.Add(child);
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMoleculeBuilderCommand(_parent, _itemToAdd).AsInverseFor(this);
      }
   }

   public class RemoveMoleculeBuilderCommand : RemoveObjectBaseCommand<IMoleculeBuilder, MoleculeBuildingBlock>
   {
      public RemoveMoleculeBuilderCommand(MoleculeBuildingBlock parent, IMoleculeBuilder itemToRemove) : base(parent, itemToRemove, parent)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMoleculeBuilderCommand(_parent, _itemToRemove).AsInverseFor(this);
      }

      protected override void RemoveFrom(IMoleculeBuilder childToRemove, MoleculeBuildingBlock parent, IMoBiContext context)
      {
         parent.Remove(childToRemove);
      }
   }
}