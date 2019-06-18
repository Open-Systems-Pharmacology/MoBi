using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public abstract class AddMoleculeNameCommand : AddItemCommand<string, IMoleculeDependentBuilder, IBuildingBlock>
   {
      private readonly string _listType;

      protected AddMoleculeNameCommand(IMoleculeDependentBuilder moleculeDependentBuilder, string itemToAdd, IBuildingBlock buildingBlock, string listType)
         : base(moleculeDependentBuilder, itemToAdd, buildingBlock)
      {
         _listType = listType;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         AddMoleculeNames(_parent, _itemToAdd);
         var parentType = new ObjectTypeResolver().TypeFor(_parent);
         Description = AppConstants.Commands.AddMoleculeToListDescription(_itemToAdd, parentType, _parent.EntityPath(),_listType);

      }

      protected abstract void AddMoleculeNames(IMoleculeDependentBuilder moleculeDependentBuilder, string moleculeName);
   }

   public class AddMoleculeNameToIncludeCommand : AddMoleculeNameCommand
   {
      public AddMoleculeNameToIncludeCommand(IMoleculeDependentBuilder parent, string itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock,AppConstants.Captions.IncludeList)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMoleculeNameFromIncludeCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddMoleculeNames(IMoleculeDependentBuilder moleculeDependentBuilder, string moleculeName)
      {
         moleculeDependentBuilder.AddMoleculeName(moleculeName);
      }
   }

   public class AddMoleculeNameToExcludeCommand : AddMoleculeNameCommand
   {
      public AddMoleculeNameToExcludeCommand(IMoleculeDependentBuilder parent, string itemToAdd, IBuildingBlock buildingBlock)
         : base(parent, itemToAdd, buildingBlock,AppConstants.Captions.ExcludeList)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveMoleculeNameFromExcludeCommand(_parent, _itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddMoleculeNames(IMoleculeDependentBuilder moleculeDependentBuilder, string moleculeName)
      {
         moleculeDependentBuilder.AddMoleculeNameToExclude(moleculeName);
      }
   }

   public abstract class RemoveMoleculeNameCommand : RemoveItemCommand<string, IMoleculeDependentBuilder, IBuildingBlock>
   {
      private readonly string _listType;

      protected RemoveMoleculeNameCommand(IMoleculeDependentBuilder moleculeDependentBuilder, string itemToRemove, IBuildingBlock buildingBlock,string listType)
         : base(moleculeDependentBuilder, itemToRemove, buildingBlock)
      {
         _listType = listType;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         var parentType = new ObjectTypeResolver().TypeFor(_parent);
         Description = AppConstants.Commands.RemoveMoleculeFromListDescription(_itemToRemove, parentType, _parent.EntityPath(),_listType);
         RemoveMoleculeName(_parent, _itemToRemove);
      }

      protected abstract void RemoveMoleculeName(IMoleculeDependentBuilder observerBuilder, string moleculeName);
   }

   public class RemoveMoleculeNameFromIncludeCommand : RemoveMoleculeNameCommand
   {
      public RemoveMoleculeNameFromIncludeCommand(IMoleculeDependentBuilder parent, string itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock,AppConstants.Captions.IncludeList)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMoleculeNameToIncludeCommand(_parent,_itemToRemove, _buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveMoleculeName(IMoleculeDependentBuilder observerBuilder, string moleculeName)
      {
         observerBuilder.RemoveMoleculeName(moleculeName);
      }
   }

   public class RemoveMoleculeNameFromExcludeCommand : RemoveMoleculeNameCommand
   {
      public RemoveMoleculeNameFromExcludeCommand(IMoleculeDependentBuilder parent, string itemToRemove, IBuildingBlock buildingBlock)
         : base(parent, itemToRemove, buildingBlock, AppConstants.Captions.ExcludeList)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddMoleculeNameToExcludeCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveMoleculeName(IMoleculeDependentBuilder observerBuilder, string moleculeName)
      {
         observerBuilder.RemoveMoleculeNameToExclude(moleculeName);
      }
   }
}