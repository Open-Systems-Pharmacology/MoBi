using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddApplicationMoleculBuilderToApplicationBuilderCommand : AddObjectBaseCommand<ApplicationMoleculeBuilder, ApplicationBuilder>
   {
      public AddApplicationMoleculBuilderToApplicationBuilderCommand(ApplicationBuilder parent, ApplicationMoleculeBuilder itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(ApplicationMoleculeBuilder child, ApplicationBuilder parent, IMoBiContext context)
      {
         parent.AddMolecule(child);
      }
   }

   public class RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand : RemoveObjectBaseCommand<ApplicationMoleculeBuilder, ApplicationBuilder>
   {
      public RemoveApplicationMoleculeBuilderFromApplicationBuilderCommand(ApplicationBuilder parent, ApplicationMoleculeBuilder itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddApplicationMoleculBuilderToApplicationBuilderCommand(_parent, _itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(ApplicationMoleculeBuilder childToRemove, ApplicationBuilder parent, IMoBiContext context)
      {
         _parent.RemoveMolecule(_itemToRemove);
      }
   }
}