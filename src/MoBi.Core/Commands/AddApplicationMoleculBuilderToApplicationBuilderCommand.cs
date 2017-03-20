using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddApplicationMoleculBuilderToApplicationBuilderCommand : AddObjectBaseCommand<IApplicationMoleculeBuilder, IApplicationBuilder>
   {
      public AddApplicationMoleculBuilderToApplicationBuilderCommand(IApplicationBuilder parent, IApplicationMoleculeBuilder itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(IApplicationMoleculeBuilder child, IApplicationBuilder parent, IMoBiContext context)
      {
         parent.AddMolecule(child);
      }
   }

   public class RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand : RemoveObjectBaseCommand<IApplicationMoleculeBuilder, IApplicationBuilder>
   {
      public RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand(IApplicationBuilder parent, IApplicationMoleculeBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddApplicationMoleculBuilderToApplicationBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(IApplicationMoleculeBuilder childToRemove, IApplicationBuilder parent, IMoBiContext context)
      {
         _parent.RemoveMolecule(_itemToRemove);
      }
   }
}