using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using libsbmlcs;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using Model = libsbmlcs.Model;
using Reaction = libsbmlcs.Reaction;

namespace MoBi.Core.SBML
{
   public class OneCompartmentReactionImporterTests : ContextForSBMLIntegration<ReactionImporter>
   {
      protected Model _sbmlModel;
      protected SBMLInformation _sbmlInformation;
      private Reaction _reaction;

      protected override void Context()
      {
         base.Context();
         _sbmlModel = new Model(3, 1);
         _sbmlInformation = new SBMLInformation();

         //one compartment reaction
         _reaction = _sbmlModel.createReaction();
         _reaction.setId("r1");

         //reaction partner
         var s1 = _sbmlModel.createSpecies();
         s1.setId("s1");
         s1.setCompartment("default");

         var s2 = _sbmlModel.createSpecies();
         s2.setId("s2");
         s2.setCompartment("default");

         var m = new MoleculeInformation(s1);
         var m2 = new MoleculeInformation(s2);
         _sbmlInformation.MoleculeInformation.Add(m);
         _sbmlInformation.MoleculeInformation.Add(m2);

         //SRef
         var s1Ref = _reaction.createReactant();
         s1Ref.setSpecies("s1");
         s1Ref.setStoichiometry(1);

         var s2Ref = _reaction.createProduct();
         s2Ref.setSpecies("s2");
         s2Ref.setStoichiometry(2);
         _reaction.addProduct(s2Ref);

         //Modifier
         var mod = _reaction.createModifier();
         mod.setId("mod");
         mod.setName("mod");
         mod.setSpecies("s1");

         //Kinetic Law
         var kl = _reaction.createKineticLaw();
         kl.setId("kl");
         kl.setMath(libsbml.parseFormula("2*3"));
      }

      protected override void Because()
      {
         sut.DoImport(_sbmlModel, new MoBiProject(), _sbmlInformation, new MoBiMacroCommand());
      }

      [Observation]
      public void ReactionCreationTest()
      {
         sut.ReactionBuilderList.ShouldNotBeNull();
         sut.ReactionBuilderList.Count.ShouldBeGreaterThan(0);
      }

      [Observation]
      public void KineticLawTest()
      {
         var reaction = sut.ReactionBuilderList.First();
         reaction.Formula.ShouldNotBeNull();
      }

      [Observation]
      public void ModifierTest()
      {
         var reaction = sut.ReactionBuilderList.First();
         reaction.ModifierNames.ShouldNotBeNull();
      }
   }

   public abstract class ReactionImporterSpecs : ContextForSBMLIntegration<ReactionImporter>
   {
   }

   public class MultiCompartmentReactionImporterTests : ReactionImporterSpecs
   {
      private IReactionBuilder _eductGhostReaction;
      private IReactionBuilder _productGhostReaction;
      private IMoBiReactionBuildingBlock _rbb;

      protected override void Context()
      {
         base.Context();
         _eductGhostReaction = null;
         _productGhostReaction = null;
         _fileName = Helper.TestFileFullPath("MulticompartmentReactionTest_simple.xml");
      }

      protected override void Because()
      {
         base.Because();
         _rbb = _moBiProject.ReactionBlockCollection.First();
         foreach (var reaction in _rbb)
         {
            if (reaction.Name.Contains("default")) _eductGhostReaction = reaction;
            if (reaction.Name.Contains("c1")) _productGhostReaction = reaction;
         }
      }

      [Observation]
      public void SimpleReactionCreationTest()
      {
         _moBiProject.ReactionBlockCollection.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void ReactionBuildingBlockTest()
      {
         _rbb.Name.ShouldBeEqualTo(SBMLConstants.SBML_REACTION_BB);
         _rbb.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void SimpleGhostReactionCreationTest()
      {
         _eductGhostReaction.ShouldNotBeNull();
         _productGhostReaction.ShouldNotBeNull();
      }

      [Observation]
      public void GhostReactionCreationTest()
      {
         //one Modifier each for the dummy species
         _eductGhostReaction.ModifierNames.Count().ShouldBeEqualTo(1);
         _productGhostReaction.ModifierNames.Count().ShouldBeEqualTo(1);

         _eductGhostReaction.Products.Count().ShouldBeEqualTo(0);
         _eductGhostReaction.Educts.Count().ShouldBeEqualTo(1);

         _productGhostReaction.Products.Count().ShouldBeEqualTo(1);
         _productGhostReaction.Educts.Count().ShouldBeEqualTo(0);
      }
   }

   public class PassiveTransportReactionImporterTests : ReactionImporterSpecs
   {
      private IPassiveTransportBuildingBlock _ptBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _fileName = Helper.TestFileFullPath("PassiveTransportTest_simple.xml");
      }

      protected override void Because()
      {
         base.Because();
         _ptBuildingBlock = _moBiProject.PassiveTransportCollection.FirstOrDefault();
      }

      [Observation]
      public void PassiveTransportNotNullTest()
      {
         _ptBuildingBlock.ShouldNotBeNull();
      }

      [Observation]
      public void NoReactionCreatedTest()
      {
         _moBiProject.ReactionBlockCollection.FirstOrDefault().FirstOrDefault().ShouldBeNull();
      }

      [Observation]
      public void SimplePassiveTransportTest()
      {
         _ptBuildingBlock.FirstOrDefault().ShouldNotBeNull();
         var pt = _ptBuildingBlock.FirstOrDefault();
         pt.MoleculeList.MoleculeNames.Find(name => name == "b_cat").ShouldNotBeNull();
         pt.SourceCriteria.Count().ShouldBeEqualTo(1);
         pt.TargetCriteria.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void LocalParameterCreationTest()
      {
         _ptBuildingBlock.FirstOrDefault().ShouldNotBeNull();
         var pt = _ptBuildingBlock.FirstOrDefault();
         pt.ShouldNotBeNull();
         pt.Parameters.Count().ShouldBeEqualTo(1);
         pt.Parameters.ExistsByName("lp1");
      }
   }
}