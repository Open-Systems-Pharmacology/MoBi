using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Events;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Commands
{
   public class AddedReactionModifierEvent
   {
      public ReactionBuilder Reaction { get; set; }
      public string ModifierName { get; set; }

      public AddedReactionModifierEvent(ReactionBuilder reaction, string modifierName)
      {
         Reaction = reaction;
         ModifierName = modifierName;
      }
   }

   public abstract class AddItemToReactionCommand<T> : AddItemCommand<T, ReactionBuilder, MoBiReactionBuildingBlock>
   {
      private readonly Func<ReactionBuilder, Action<T>> _addMethod;
      protected string _moleculeName;

      protected AddItemToReactionCommand(MoBiReactionBuildingBlock reactionBuildingBlock, T itemToAdd, ReactionBuilder reactionBuilder, Func<ReactionBuilder, Action<T>> addMethod)
         : base(reactionBuilder, itemToAdd, reactionBuildingBlock)
      {
         _addMethod = addMethod;
         _moleculeName = MoleculeNameFrom(_itemToAdd);
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         _addMethod(_parent).Invoke(_itemToAdd);
         addItemAsFormulaUsableInReactionFormula(context);
         var diagramManager = _buildingBlock.DiagramManager.DowncastTo<IMoBiReactionDiagramManager>();
         diagramManager.AddMolecule(_parent, _moleculeName);
         Description = AppConstants.Commands.AddToDescription(ObjectType, _moleculeName, _parent.Name);
      }

      protected abstract string MoleculeNameFrom(T itemToAdd);

      private void addItemAsFormulaUsableInReactionFormula(IMoBiContext context)
      {
         var formula = _parent.Formula;
         var formulaUsablePath = createPath(_moleculeName, formula.ObjectPaths.Select(x => x.Alias), context);

         //One element was already defined with the same path. Nothing to do 
         if (formula.ObjectPaths.Select(x => x.PathAsString).Contains(formulaUsablePath.PathAsString))
            return;

         formula.AddObjectPath(formulaUsablePath);
         context.PublishEvent(new AddedFormulaUsablePathEvent(formula, formulaUsablePath));
         context.PublishEvent(new FormulaChangedEvent(formula));
      }

      private FormulaUsablePath createPath(string moleculeName, IEnumerable<string> usedAliases, IMoBiContext context)
      {
         var dimensionRetriever = context.Resolve<IReactionDimensionRetriever>();
         var aliasCreator = context.Resolve<IAliasCreator>();
         var objectPathFactory = context.Resolve<IObjectPathFactory>();

         var pathToReferencedObject = new List<string> {ObjectPath.PARENT_CONTAINER, moleculeName};
         //in case of concentration, we need to add a reference to the concentration parameter
         if (dimensionRetriever.SelectedDimensionMode == ReactionDimensionMode.ConcentrationBased)
            pathToReferencedObject.Add(Constants.Parameters.CONCENTRATION);

         return objectPathFactory.CreateFormulaUsablePathFrom(pathToReferencedObject)
            .WithDimension(dimensionRetriever.MoleculeDimension)
            .WithAlias(aliasCreator.CreateAliasFrom(moleculeName, usedAliases));
      }
   }

   public abstract class AddPartnerToReactionCommand<T> : AddItemToReactionCommand<T> where T : ReactionPartnerBuilder
   {
      protected AddPartnerToReactionCommand(MoBiReactionBuildingBlock reactionBuildingBlock, T reactionPartnerBuilder, ReactionBuilder reactionBuilder, Func<ReactionBuilder, Action<T>> addMethod) :
         base(reactionBuildingBlock, reactionPartnerBuilder, reactionBuilder, addMethod)
      {
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new AddedReactionPartnerEvent(_itemToAdd, _parent));
      }

      protected override string MoleculeNameFrom(T reactionPartnerBuilder)
      {
         return reactionPartnerBuilder.MoleculeName;
      }
   }

   public class AddReactionPartnerToEductCollection : AddPartnerToReactionCommand<ReactionPartnerBuilder>
   {
      public AddReactionPartnerToEductCollection(MoBiReactionBuildingBlock reactionBuildingBlock, ReactionPartnerBuilder reactionPartnerBuilder, ReactionBuilder reactionBuilder) :
         base(reactionBuildingBlock, reactionPartnerBuilder, reactionBuilder, x => x.AddEduct)
      {
         ObjectType = ObjectTypes.Educt;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveReactionPartnerFromEductCollection(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _itemToAdd = _parent.Educts.First(rp => rp.MoleculeName.Equals(_moleculeName));
      }
   }

   public class AddReactionPartnerToProductCollection : AddPartnerToReactionCommand<ReactionPartnerBuilder>
   {
      public AddReactionPartnerToProductCollection(MoBiReactionBuildingBlock reactionBuildingBlock, ReactionPartnerBuilder reactionPartnerBuilder, ReactionBuilder reactionBuilder) :
         base(reactionBuildingBlock, reactionPartnerBuilder, reactionBuilder, x => x.AddProduct)
      {
         ObjectType = ObjectTypes.Product;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveReactionPartnerFromProductCollection(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _itemToAdd = _parent.Products.First(rp => rp.MoleculeName.Equals(_moleculeName));
      }
   }

   public class AddItemToModifierCollectionCommand : AddItemToReactionCommand<string>
   {
      public AddItemToModifierCollectionCommand(MoBiReactionBuildingBlock reactionBuildingBlock, string modifier, ReactionBuilder reactionBuilder) :
         base(reactionBuildingBlock, modifier, reactionBuilder, x => x.AddModifier)
      {
         ObjectType = ObjectTypes.Modifier;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         base.ExecuteWith(context);
         context.PublishEvent(new AddedReactionModifierEvent(_parent, _itemToAdd));
      }

      protected override string MoleculeNameFrom(string modifier)
      {
         return modifier;
      }

      protected override ICommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveItemFromModifierCollectionCommand(_parent, _itemToAdd, _buildingBlock).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         base.RestoreExecutionData(context);
         _itemToAdd = _moleculeName;
      }
   }
}