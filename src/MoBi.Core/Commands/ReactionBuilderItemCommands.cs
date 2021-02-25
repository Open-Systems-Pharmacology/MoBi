using System;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Assets;

namespace MoBi.Core.Commands
{
   public class RemovedReactionModifierEvent
   {
      public IReactionBuilder Reaction { get; set; }
      public string ModifierName { get; set; }

      public RemovedReactionModifierEvent(IReactionBuilder reaction, string modifierName)
      {
         Reaction = reaction;
         ModifierName = modifierName;
      }
   }

   public abstract class RemoveItemFromReactionCommand<T> : RemoveItemCommand<T, IReactionBuilder, IMoBiReactionBuildingBlock>
   {
      protected IReactionBuilder _reaction;
      private readonly Func<IReactionBuilder, Action<T>> _removeMethod;
      private readonly string _moleculeName;

      protected RemoveItemFromReactionCommand(IReactionBuilder reactionBuilder, T itemToRemove, IMoBiReactionBuildingBlock reactionBuildingBlock, Func<IReactionBuilder, Action<T>> removeMethod) : base(reactionBuilder, itemToRemove, reactionBuildingBlock)
      {
         _removeMethod = removeMethod;
         _reaction = reactionBuilder;
         _moleculeName = MoleculeNameFrom(itemToRemove);
      }

      protected abstract string MoleculeNameFrom(T itemToRemove);

      protected override void ClearReferences()
      {
         base.ClearReferences();
         _reaction = null;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _removeMethod(_reaction).Invoke(_itemToRemove);
         Description = AppConstants.Commands.RemoveFromDescription(ObjectType, _moleculeName, _reaction.Name);
    
         var diagramManager = _buildingBlock.DiagramManager.DowncastTo<IMoBiReactionDiagramManager>();
         diagramManager.RemoveMolecule(_reaction, _moleculeName);

         removeMoleculeFromFormula(context);
      }

      private void removeMoleculeFromFormula(IMoBiContext context)
      {
         var pathToRemove = getPathToRemoveInReactionFormula(_moleculeName, context);
         if (pathToRemove == null) return;

         _reaction.Formula.RemoveObjectPath(pathToRemove);
         context.PublishEvent(new RemovedFormulaUsablePathEvent(_reaction.Formula, pathToRemove));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _reaction = context.Get<IReactionBuilder>(_parentId);
      }

      private IFormulaUsablePath getPathToRemoveInReactionFormula(string moleculeName, IMoBiContext context)
      {
         var pathToLookFor = context.ObjectPathFactory.CreateFormulaUsablePathFrom(ObjectPath.PARENT_CONTAINER, moleculeName);
         return _reaction.Formula.ObjectPaths.FirstOrDefault(path => path.PathAsString.Equals(pathToLookFor.PathAsString));
      }
   }

   public abstract class RemovePartnerFromReactionCommand : RemoveItemFromReactionCommand<IReactionPartnerBuilder>
   {
      protected RemovePartnerFromReactionCommand(IReactionBuilder reactionBuilder, IReactionPartnerBuilder itemToRemove, IMoBiReactionBuildingBlock reactionBuildingBlock, Func<IReactionBuilder, Action<IReactionPartnerBuilder>> removeMethod) : base(reactionBuilder, itemToRemove, reactionBuildingBlock, removeMethod)
      {
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new RemovedReactionPartnerEvent(_itemToRemove, _reaction));
      }

      protected override string MoleculeNameFrom(IReactionPartnerBuilder reactionPartnerBuilder)
      {
         return reactionPartnerBuilder.MoleculeName;
      }
   }

   public class RemoveReactionPartnerFromEductCollection : RemovePartnerFromReactionCommand
   {
      public RemoveReactionPartnerFromEductCollection(IReactionBuilder reactionBuilder, IReactionPartnerBuilder reactionPartnerBuilder, IMoBiReactionBuildingBlock reactionBuildingBlock)
         : base(reactionBuilder, reactionPartnerBuilder, reactionBuildingBlock, x => x.RemoveEduct)
      {
         ObjectType =ObjectTypes.Educt;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddReactionPartnerToEductCollection(_buildingBlock, _itemToRemove, _reaction).AsInverseFor(this);
      }
   }

   public class RemoveReactionPartnerFromProductCollection : RemovePartnerFromReactionCommand
   {
      public RemoveReactionPartnerFromProductCollection(IReactionBuilder reactionBuilder, IReactionPartnerBuilder reactionPartnerBuilder, IMoBiReactionBuildingBlock reactionBuildingBlock)
         : base(reactionBuilder, reactionPartnerBuilder, reactionBuildingBlock, x => x.RemoveProduct)
      {
         ObjectType =ObjectTypes.Product;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddReactionPartnerToProductCollection(_buildingBlock, _itemToRemove, _reaction).AsInverseFor(this);
      }
   }

   public class RemoveItemFromModifierCollectionCommand : RemoveItemFromReactionCommand<string>
   {
      public RemoveItemFromModifierCollectionCommand(IReactionBuilder reactionBuilder, string modififer, IMoBiReactionBuildingBlock reactionBuildingBlock)
         : base(reactionBuilder, modififer, reactionBuildingBlock, x => x.RemoveModifier)
      {
         ObjectType =ObjectTypes.Modifier;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddItemToModifierCollectionCommand(_buildingBlock, _itemToRemove, _reaction).AsInverseFor(this);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new RemovedReactionModifierEvent(_reaction, _itemToRemove));
      }

      protected override string MoleculeNameFrom(string modifierName)
      {
         return modifierName;
      }
   }
}