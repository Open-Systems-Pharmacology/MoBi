using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Commands.Core;
using libsbmlcs;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Builder;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using Model = libsbmlcs.Model;
using Reaction = libsbmlcs.Reaction;

namespace MoBi.Core.SBML
{
   public class ReactionImporter : SBMLImporter
   {
      internal readonly List<IReactionBuilder> ReactionBuilderList;
      private readonly List<ITransportBuilder> _passiveTransportList;
      private readonly IMoBiReactionBuildingBlock _reactionBuildingBlock;
      private readonly IPassiveTransportBuildingBlock _passiveTransportBuildingBlock;
      private readonly IDimensionFactory _dimensionFactory;

      public ReactionImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, IMoBiDimensionFactory moBiDimensionFactory, ASTHandler astHandler, IMoBiContext context, IReactionBuildingBlockFactory reactionBuildingBlockFactory)
          : base(objectPathFactory, objectBaseFactory, astHandler, context)
      {
         _dimensionFactory = moBiDimensionFactory;
         ReactionBuilderList = new List<IReactionBuilder>();
         _passiveTransportList = new List<ITransportBuilder>();
         _reactionBuildingBlock = reactionBuildingBlockFactory.Create().WithName(SBMLConstants.SBML_REACTION_BB);
         _passiveTransportBuildingBlock = ObjectBaseFactory.Create<IPassiveTransportBuildingBlock>()
             .WithName(SBMLConstants.SBML_PASSIVETRANSPORTS_BB);
      }

      /// <summary>
      ///     Imports all Reactions of the SBML Model.
      /// </summary>
      protected override void Import(Model model)
      {
         for (long i = 0; i < model.getNumReactions(); i++)
         {
            CreateReaction(model.getReaction(i), model);
         }
         AddToProject();
      }

      /// <summary>
      ///     Creates a MoBi reaction by a given SBML Reaction.
      /// </summary>
      internal void CreateReaction(Reaction sbmlReaction, Model model)
      {
         if (IsMultiCompartmentReaction(sbmlReaction, model))
            CreateMulticompartmentReaction(sbmlReaction, model);
         else
            CreateStandardReaction(sbmlReaction, model);
      }

      /// <summary>
      ///     Creates a multicompartment reaction.
      /// </summary>
      private void CreateMulticompartmentReaction(Reaction sbmlReaction, Model model)
      {
         if ((sbmlReaction.getNumReactants() + sbmlReaction.getNumProducts() == 2) && (IsPassiveTransport(sbmlReaction, model)))
         {
            CreatePassiveTransport(sbmlReaction, model);
         }
         else
            CreateGhostReaction(sbmlReaction, model);
      }

      /// <summary>
      ///     A passive Transport is created when two Species with the same name are reacting with 
      ///     each other. 
      /// </summary>
      private bool IsPassiveTransport(Reaction sbmlReaction, Model model)
      {
         var educt = sbmlReaction.getReactant(0).getSpecies();
         var product = sbmlReaction.getProduct(0).getSpecies();

         var eductSpecies = GetSpeciesById(educt, model);
         var productSpecies = GetSpeciesById(product, model);

         if (eductSpecies.getName() != productSpecies.getName()) return false;

         var molinfoEduct = _sbmlInformation.MoleculeInformation.FirstOrDefault(info => info.SpeciesIds.Any(s => s == educt));
         var molinfoProduct = _sbmlInformation.MoleculeInformation.FirstOrDefault(info => info.SpeciesIds.Any(s => s == product));
         if (molinfoEduct == null) return false;
         if (molinfoProduct == null) return false;

         var reactantMolecule = molinfoEduct.GetMoleculeBuilder();
         var productMolecule = molinfoProduct.GetMoleculeBuilder();

         return reactantMolecule == productMolecule;
      }

      /// <summary>
      ///     Creates a "standard" MoBi reaction by a given one compartment reaction.
      /// </summary>
      private void CreateStandardReaction(Reaction sbmlReaction, Model model)
      {
         var reactionBuilder = ObjectBaseFactory.Create<IReactionBuilder>()
             .WithName(sbmlReaction.getId())
             .WithDescription(sbmlReaction.getNotesString());

         CreateModifiers(sbmlReaction, reactionBuilder, String.Empty, model);
         var parameters = CreateLocalParameters(sbmlReaction);
         if (parameters != null) parameters.ForEach(reactionBuilder.AddParameter);

         if (sbmlReaction.isSetKineticLaw()) CreateKineticLaw(sbmlReaction.getKineticLaw(), reactionBuilder, false);
         CreateProducts(sbmlReaction, reactionBuilder, model);
         CreateEducts(sbmlReaction, reactionBuilder, model);

         if (reactionBuilder != null)
            ReactionBuilderList.Add(reactionBuilder);
      }

      /// <summary>
      ///     Creates a Ghostreaction to import a SBML multicompartment reaction.
      /// </summary>
      private void CreateGhostReaction(Reaction sbmlReaction, Model model)
      {
         var eductCompartmentMoleculeDictionary = ComputeEducts(sbmlReaction, model);
         var productCompartmentMoleculeDictionary = ComputeProducts(sbmlReaction, model);

         CreateGhostReactions(sbmlReaction, eductCompartmentMoleculeDictionary, productCompartmentMoleculeDictionary, model);
      }

      /// <summary>
      ///     Saves educts of a SBML reaction into a dictionary, containing their compartment and SpeciesReference.
      /// </summary>
      private Dictionary<string, List<SpeciesReference>> ComputeEducts(Reaction sbmlReaction, Model model)
      {
         var eductCompartmentMoleculeDictionary = new Dictionary<string, List<SpeciesReference>>();
         for (long i = 0; i < sbmlReaction.getNumReactants(); i++)
         {
            var reactant = sbmlReaction.getReactant(i);
            var reactantSpecies = GetSpeciesById(reactant.getSpecies(), model);
            var compartment = reactantSpecies.getCompartment();
            if (_sbmlInformation.MoleculeInformation.All(info => info.SpeciesIds.TrueForAll(s => s != reactantSpecies.getId()))) continue;

            if (!eductCompartmentMoleculeDictionary.ContainsKey(compartment))
               eductCompartmentMoleculeDictionary[compartment] = new List<SpeciesReference> { reactant };
            else
               eductCompartmentMoleculeDictionary[compartment].Add(reactant);
         }
         return eductCompartmentMoleculeDictionary;
      }

      /// <summary>
      ///     Saves products of a SBML reaction into a dictionary, containing their compartment and SpeciesReference.
      /// </summary>
      private Dictionary<string, List<SpeciesReference>> ComputeProducts(Reaction sbmlReaction, Model model)
      {
         var productCompartmentMoleculeDictionary = new Dictionary<string, List<SpeciesReference>>();
         for (long i = 0; i < sbmlReaction.getNumProducts(); i++)
         {
            var product = sbmlReaction.getProduct(i);
            var productSpecies = GetSpeciesById(product.getSpecies(), model);
            var compartment = productSpecies.getCompartment();
            if (_sbmlInformation.MoleculeInformation.All(info => info.SpeciesIds.TrueForAll(s => s != productSpecies.getId()))) continue;

            if (!productCompartmentMoleculeDictionary.ContainsKey(compartment))
               productCompartmentMoleculeDictionary[compartment] = new List<SpeciesReference> { product };
            else
               productCompartmentMoleculeDictionary[compartment].Add(product);
         }
         return productCompartmentMoleculeDictionary;
      }


      /// <summary>
      ///     Creates for each compartment of the SBML Reaction one "Ghostreaction" to import multicompartment reactions.
      /// </summary>
      private void CreateGhostReactions(Reaction sbmlReaction, Dictionary<string, List<SpeciesReference>> eductCompartmentMoleculeDictionary, Dictionary<string, List<SpeciesReference>> productCompartmentMoleculeDictionary, Model model)
      {
         var usedProducts = new List<string>();
         
         foreach (var keyValuePair in eductCompartmentMoleculeDictionary)
         {
            var reactionBuilder = ObjectBaseFactory.Create<IReactionBuilder>()
                .WithName(sbmlReaction.getId() + "_" + keyValuePair.Key + "_ghostReaction")
                .WithDescription(sbmlReaction.getNotesString());

            CreateModifiers(sbmlReaction, reactionBuilder, keyValuePair.Key, model);
            var parameters = CreateLocalParameters(sbmlReaction);
            if (parameters != null) parameters.ForEach(reactionBuilder.AddParameter);
            CreateKineticLaw(sbmlReaction.getKineticLaw(), reactionBuilder, true);

            var compartmentName = keyValuePair.Key;
            if (!_sbmlInformation.DummyNameContainerDictionary.ContainsValue(compartmentName)) return;
            var dummyMolecule = _sbmlInformation.DummyNameContainerDictionary.FirstOrDefault(x => x.Value == compartmentName).Key;

            reactionBuilder.AddModifier(dummyMolecule);

            foreach (var species in keyValuePair.Value)
            {
               var reactionPartner = CreateReactionPartner(species, model);
               if (reactionPartner != null) reactionBuilder.AddEduct(reactionPartner);
            }

            if (productCompartmentMoleculeDictionary.ContainsKey(keyValuePair.Key))
            {
               usedProducts.Add(keyValuePair.Key);
               var productsInThisCompartment = productCompartmentMoleculeDictionary[keyValuePair.Key];
               foreach (var product in productsInThisCompartment)
               {
                  var reactionPartner = CreateReactionPartner(product, model);
                  if (reactionPartner != null) reactionBuilder.AddProduct(reactionPartner);
               }
            }
            ReactionBuilderList.Add(reactionBuilder);
         }


         foreach (var keyValuePair in productCompartmentMoleculeDictionary)
         {
            if (usedProducts.Contains(keyValuePair.Key)) continue;

            var reactionBuilder = ObjectBaseFactory.Create<IReactionBuilder>()
                .WithName(sbmlReaction.getId() + "_" + keyValuePair.Key + "_ghostReaction")
                .WithDescription(sbmlReaction.getNotesString());

            CreateModifiers(sbmlReaction, reactionBuilder, keyValuePair.Key, model);
            var parameters = CreateLocalParameters(sbmlReaction);
            if (parameters != null) parameters.ForEach(reactionBuilder.AddParameter);
            CreateKineticLaw(sbmlReaction.getKineticLaw(), reactionBuilder, true);

            var compartmentName = keyValuePair.Key;
            if (!_sbmlInformation.DummyNameContainerDictionary.ContainsValue(compartmentName)) return;
            var dummyMolecule = _sbmlInformation.DummyNameContainerDictionary.FirstOrDefault(x => x.Value == compartmentName).Key;

            reactionBuilder.AddModifier(dummyMolecule);

            foreach (var species in keyValuePair.Value)
            {
               var reactionPartner = CreateReactionPartner(species, model);
               if (reactionPartner != null) reactionBuilder.AddProduct(reactionPartner);
            }
            ReactionBuilderList.Add(reactionBuilder);
         }
      }

      /// <summary>
      ///     Imports a SBML Reaction by creating a passive Transport. 
      /// </summary>
      private void CreatePassiveTransport(Reaction sbmlReaction, Model model)
      {
         var reactant = sbmlReaction.getReactant(0).getSpecies();
         var product = sbmlReaction.getProduct(0).getSpecies();
         var reactantSpecies = GetSpeciesById(reactant, model);
         var productSpecies = GetSpeciesById(product, model);

         if (_sbmlInformation.MoleculeInformation.All(info => info.SpeciesIds.TrueForAll(s => s != reactant))) return;
         if (_sbmlInformation.MoleculeInformation.All(info => info.SpeciesIds.TrueForAll(s => s != product))) return;
         var molInfoReactant = _sbmlInformation.MoleculeInformation.FirstOrDefault(info => info.SpeciesIds.Contains(reactant));
         var molInfoProduct = _sbmlInformation.MoleculeInformation.FirstOrDefault(info => info.SpeciesIds.Contains(product));

         if (molInfoProduct == null) return;
         if (molInfoReactant == null) return;

         //must be the same Molecule
         if (molInfoReactant.GetMoleculeBuilder() != molInfoProduct.GetMoleculeBuilder()) CreateErrorMessage();

         var passiveTransport = ObjectBaseFactory.Create<ITransportBuilder>().WithName(sbmlReaction.getId());
         passiveTransport.ForAll = false;
         if (molInfoReactant.GetMoleculeBuilderName() == null) return;
         passiveTransport.MoleculeList.AddMoleculeName(molInfoReactant.GetMoleculeBuilderName());

         var reactantCompartment = GetContainerFromCompartment_(molInfoReactant.GetCompartment(reactantSpecies));
         var productCompartment = GetContainerFromCompartment_(molInfoProduct.GetCompartment(productSpecies));
         if (reactantCompartment != null && productCompartment != null)
         {
            var reactantMatchTag = new MatchTagCondition(reactantCompartment.Name);
            var productMatchTag = new MatchTagCondition(productCompartment.Name);
            passiveTransport.SourceCriteria.Add(reactantMatchTag);
            passiveTransport.TargetCriteria.Add(productMatchTag);
         }

         var parameters = CreateLocalParameters(sbmlReaction);
         if (parameters != null) parameters.ForEach(passiveTransport.AddParameter);
         CreateKinetic(sbmlReaction, passiveTransport);
         AddNeighbourhood(reactantCompartment, productCompartment, model);

         _passiveTransportBuildingBlock.Add(passiveTransport);
      }

      private void CreateErrorMessage()
      {
         var msg = new NotificationMessage(_reactionBuildingBlock, MessageOrigin.All, null, NotificationType.Warning)
         {
            Message = SBMLConstants.SBML_FEATURE_NOT_SUPPORTED + ": Reaction not supported."
         };
         _sbmlInformation.NotificationMessages.Add(msg);
      }

      /// <summary>
      ///     Creates the Kinetic Formula for a passive Transport.
      /// </summary>
      private void CreateKinetic(Reaction sbmlReaction, ITransportBuilder passiveTransport)
      {
         _astHandler.NeedAbsolutePath = true;
         var formula = sbmlReaction.getKineticLaw() == null
             ? ObjectBaseFactory.Create<ExplicitFormula>().WithFormulaString(String.Empty)
             : _astHandler.Parse(sbmlReaction.getKineticLaw().getMath(), sbmlReaction.getId(), _sbmlProject, _sbmlInformation);
         if (formula == null)
         {
            passiveTransport.Formula = ObjectBaseFactory.Create<ExplicitFormula>()
                .WithFormulaString(String.Empty)
                .WithName(SBMLConstants.DEFAULT_FORMULA_NAME);
         }
         else
         {
            passiveTransport.Formula = formula;
            _passiveTransportBuildingBlock.FormulaCache.Add(formula);
         }
      }

      /// <summary>
      ///     Adds the neighbourhood that is necessary for the Passive Transport.
      /// </summary>
      private void AddNeighbourhood(IContainer reactantCompartment, IContainer productCompartment, Model model)
      {
         var existant = false;
         if (GetMainSpatialStructure(model) == null) return;
         foreach (var n in GetMainSpatialStructure(model).Neighborhoods)
         {
            if (n.Name == (reactantCompartment.Id + "_" + productCompartment.Id))
               existant = true;
         }
         if (existant) return;
         var nbuilder = ObjectBaseFactory.Create<INeighborhoodBuilder>()
             .WithName(reactantCompartment.Id + "_" + productCompartment.Id)
             .WithFirstNeighbor(reactantCompartment)
             .WithSecondNeighbor(productCompartment);
         GetMainSpatialStructure(model).AddNeighborhood(nbuilder);
      }

      /// <summary>
      ///     Checks if all reaction partners are in the same compartment.
      /// </summary>
      private bool IsMultiCompartmentReaction(Reaction sbmlReaction, Model model)
      {
         var compartment = String.Empty;

         for (long i = 0; i < sbmlReaction.getNumReactants(); i++)
         {
            var x = sbmlReaction.getReactant(i).getSpecies();

            var species = GetSpeciesById(x, model);
            if (compartment == String.Empty)
               compartment = species.getCompartment();
            else
            {
               if (compartment != species.getCompartment())
                  return true;
            }
         }

         for (long i = 0; i < sbmlReaction.getNumProducts(); i++)
         {
            var x = sbmlReaction.getProduct(i).getSpecies();
            var species = GetSpeciesById(x, model);
            if (compartment == String.Empty)
               compartment = species.getCompartment();
            else
            {
               if (compartment != species.getCompartment())
                  return true;
            }
         }
         return false;
      }

      /// <summary>
      ///     Imports the SBML Modifiers to MoBi Modifiers. 
      /// </summary>
      private void CreateModifiers(Reaction sbmlReaction, IReactionBuilder reactionBuilder, string reactionCompartment, Model model)
      {
         for (long i = 0; i < sbmlReaction.getNumModifiers(); i++)
         {
            var modifier = sbmlReaction.getModifier(i);
            if (ModifierInDifferentCompartment(reactionCompartment, modifier, model)) continue;

            var modifierMolecule = _sbmlInformation.GetMoleculeBySBMLId(modifier.getSpecies());
            if (modifierMolecule != null)
               reactionBuilder.AddModifier(modifierMolecule.Name);
         }
      }

      /// <summary>
      ///     Checks if a given Modifier is present in the given compartment.
      /// </summary>
      private bool ModifierInDifferentCompartment(string reactionCompartment, ModifierSpeciesReference modifier, Model model)
      {
         var species = GetSpeciesById(modifier.getSpecies(), model);
         if (species == null) return false;

         return species.getCompartment() != reactionCompartment;
      }

      /// <summary>
      ///     Creates the MoBi Reaction Formula by the given SBML Kinetic Law. 
      /// </summary>
      private void CreateKineticLaw(KineticLaw kineticLaw, IReactionBuilder reactionBuilder, bool needAbsolutePath)
      {
         if (needAbsolutePath) _astHandler.NeedAbsolutePath = true;
         var formula = kineticLaw == null ? ObjectBaseFactory.Create<ExplicitFormula>().WithFormulaString(String.Empty).WithName(SBMLConstants.DEFAULT_FORMULA_NAME) : _astHandler.Parse(kineticLaw.getMath(), reactionBuilder, _sbmlProject, _sbmlInformation);
         if (formula == null)
         {
            reactionBuilder.Formula = ObjectBaseFactory.Create<ExplicitFormula>()
                .WithFormulaString(String.Empty)
                .WithName(SBMLConstants.DEFAULT_FORMULA_NAME);
         }
         else
         {
            reactionBuilder.Formula = formula;
            _reactionBuildingBlock.FormulaCache.Add(formula);
         }
      }

      /// <summary>
      ///     Creates the Educts of the MoBi reaction.
      /// </summary>
      private void CreateEducts(Reaction sbmlReaction, IReactionBuilder reactionBuilder, Model model)
      {
         for (long i = 0; i < sbmlReaction.getNumReactants(); i++)
         {
            var educt = CreateReactionPartner(sbmlReaction.getReactant(i), model);
            if (educt != null) reactionBuilder.AddEduct(educt);
         }
      }

      /// <summary>
      ///     Creates the Products of the MoBi reaction.
      /// </summary>
      private void CreateProducts(Reaction sbmlReaction, IReactionBuilder reactionBuilder, Model model)
      {
         for (long i = 0; i < sbmlReaction.getNumProducts(); i++)
         {
            var product = CreateReactionPartner(sbmlReaction.getProduct(i), model);
            if (product != null)
               reactionBuilder.AddProduct(product);
         }
      }

      /// <summary>
      ///     Creates the Local Parameters of the MoBi Reaction by the given local Parameters
      ///     of the SBML Reaction.
      /// </summary>
      private List<IParameter> CreateLocalParameters(Reaction sbmlReaction)
      {
         if (sbmlReaction.getKineticLaw() == null) return null;
         if (sbmlReaction.getKineticLaw().getNumLocalParameters() <= 0) return null;
         var parameter = new List<IParameter>();

         for (long i = 0; i < sbmlReaction.getKineticLaw().getNumLocalParameters(); i++)
         {
            var p = sbmlReaction.getKineticLaw().getLocalParameter(i);
            var formula = ObjectBaseFactory.Create<ConstantFormula>()
                .WithValue(p.getValue());
            var localParameter = ObjectBaseFactory.Create<IParameter>()
                .WithName(p.getId())
                .WithDescription(p.getNotesString())
                .WithFormula(formula);

            var dim = GetDimension(p);
            if (dim != null) localParameter.Dimension = dim;

            if (localParameter != null)
               parameter.Add(localParameter);
         }
         return parameter;
      }

      /// <summary>
      ///     Gets the MoBi Dimension of a SBML Local Parameter.
      /// </summary>
      /// <returns> The Dimension, if it was found, or null. </returns>
      private IDimension GetDimension(LocalParameter localParameter)
      {
         if (!localParameter.isSetUnits())
            return _dimensionFactory.Dimension(Constants.Dimension.DIMENSIONLESS);
         if (_sbmlInformation.MobiDimension.ContainsKey(localParameter.getUnits()))
            return _sbmlInformation.MobiDimension[localParameter.getUnits()];
         return null;
      }

      /// <summary>
      ///     Creates a MoBi ReactionPartner by a given SBML Species Reference
      /// </summary>
      private IReactionPartnerBuilder CreateReactionPartner(SpeciesReference speciesReference, Model model)
      {
         var molecule = _sbmlInformation.GetMoleculeBySBMLId(speciesReference.getSpecies());
         if (molecule == null) return null;

         IReactionPartnerBuilder productBuilder = new ReactionPartnerBuilder
         {
            MoleculeName = molecule.Name,
         };

         if (speciesReference.isSetStoichiometryMath()) CreateStoichiometryErrorMsg(speciesReference.getSpecies());
         productBuilder.StoichiometricCoefficient = speciesReference.isSetStoichiometry() ? speciesReference.getStoichiometry() : SBMLConstants.SBML_STOICHIOMETRY_DEFAULT;

         var species = GetSpeciesById(speciesReference.getSpecies(), model);
         if (species == null) return productBuilder;
         if (species.getConstant() && species.getBoundaryCondition() == false)
            productBuilder.StoichiometricCoefficient = 0;

         return productBuilder;
      }

      /// <summary>
      ///     Stoichiometry formulas are not supported in MoBi.
      /// </summary>
      private void CreateStoichiometryErrorMsg(string speciesId)
      {
         var msg = new NotificationMessage(_reactionBuildingBlock, MessageOrigin.All, null, NotificationType.Warning)
         {
            Message = SBMLConstants.SBML_FEATURE_NOT_SUPPORTED + ": Stoichiometry formula not supported. Affected species: " + speciesId
         };
         _sbmlInformation.NotificationMessages.Add(msg);
      }

      /// <summary>
      ///     Gets a SBML species by it's Id. 
      /// </summary>
      private Species GetSpeciesById(string speciesId, Model model)
      {
         for (long i = 0; i < model.getNumSpecies(); i++)
         {
            if (model.getSpecies(i).getId() == speciesId)
               return model.getSpecies(i);
         }
         return null;
      }

      /// <summary>
      ///     Adds all reactionBuilders to the Reaction Building Block and adds the Reaction Building 
      ///     Block to the MoBi Project.
      /// </summary>
      public override void AddToProject()
      {
         foreach (var reaction in ReactionBuilderList)
            _reactionBuildingBlock.Add(reaction);

         foreach (var passiveTransport in _passiveTransportList)
            _passiveTransportBuildingBlock.Add(passiveTransport);

         _context.AddToHistory(new AddBuildingBlockCommand<IMoBiReactionBuildingBlock>(_reactionBuildingBlock).Run(_context));
         _context.AddToHistory(new AddBuildingBlockCommand<IPassiveTransportBuildingBlock>(_passiveTransportBuildingBlock).Run(_context));
      }
   }
}
