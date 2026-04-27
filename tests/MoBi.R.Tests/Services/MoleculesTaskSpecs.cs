using System;
using MoBi.R.Services;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.R.Tests.Services;

internal abstract class concern_for_MoleculesTask : ContextForIntegration<IMoleculesTask>
{
   protected MoleculeBuildingBlock _buildingBlock;

   public override void GlobalContext()
   {
      base.GlobalContext();
      sut = Api.GetMoleculesTask();
   }

   protected override void Context()
   {
      base.Context();
      _buildingBlock = new MoleculeBuildingBlock().WithName("Molecules");
   }

   protected MoleculeBuilder addMolecule(string name, bool isFloating, QuantityType quantityType, bool isXenobiotic = true)
   {
      var molecule = new MoleculeBuilder
      {
         Name = name,
         IsFloating = isFloating,
         QuantityType = quantityType,
         IsXenobiotic = isXenobiotic
      };
      _buildingBlock.Add(molecule);
      return molecule;
   }
}

internal class When_getting_all_molecule_names_from_a_molecule_building_block : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme);
      addMolecule("Transporter", isFloating: false, QuantityType.Transporter);
   }

   protected override void Because()
   {
      _result = sut.AllMoleculeNames(_buildingBlock);
   }

   [Observation]
   public void should_return_all_molecule_names()
   {
      _result.ShouldOnlyContainInOrder("Drug", "Enzyme", "Transporter");
   }
}

internal class When_getting_all_molecule_names_from_an_empty_molecule_building_block : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Because()
   {
      _result = sut.AllMoleculeNames(_buildingBlock);
   }

   [Observation]
   public void should_return_an_empty_array()
   {
      _result.ShouldBeEmpty();
   }
}

internal class When_getting_all_floating_molecule_names : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug);
      addMolecule("Metabolite", isFloating: true, QuantityType.Metabolite);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme);
   }

   protected override void Because()
   {
      _result = sut.AllFloatingMoleculeNames(_buildingBlock);
   }

   [Observation]
   public void should_return_only_floating_molecule_names()
   {
      _result.ShouldOnlyContainInOrder("Drug", "Metabolite");
   }
}

internal class When_getting_all_stationary_molecule_names : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme);
      addMolecule("Transporter", isFloating: false, QuantityType.Transporter);
   }

   protected override void Because()
   {
      _result = sut.AllStationaryMoleculeNames(_buildingBlock);
   }

   [Observation]
   public void should_return_only_stationary_molecule_names()
   {
      _result.ShouldOnlyContainInOrder("Enzyme", "Transporter");
   }
}

internal class When_getting_all_molecule_names_of_type_protein : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug);
      addMolecule("Metabolite", isFloating: true, QuantityType.Metabolite);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme);
      addMolecule("Transporter", isFloating: false, QuantityType.Transporter);
      addMolecule("OtherProtein", isFloating: false, QuantityType.OtherProtein);
   }

   protected override void Because()
   {
      _result = sut.AllMoleculeNamesOfType(_buildingBlock, QuantityType.Protein);
   }

   [Observation]
   public void should_return_all_protein_molecule_names()
   {
      _result.ShouldOnlyContainInOrder("Enzyme", "Transporter", "OtherProtein");
   }
}

internal class When_getting_all_molecule_names_of_type_drug : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      addMolecule("Drug1", isFloating: true, QuantityType.Drug);
      addMolecule("Drug2", isFloating: true, QuantityType.Drug);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme);
   }

   protected override void Because()
   {
      _result = sut.AllMoleculeNamesOfType(_buildingBlock, QuantityType.Drug);
   }

   [Observation]
   public void should_return_only_drug_molecule_names()
   {
      _result.ShouldOnlyContainInOrder("Drug1", "Drug2");
   }
}

internal class When_getting_all_xenobiotic_floating_molecule_names : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug, isXenobiotic: true);
      addMolecule("Metabolite", isFloating: true, QuantityType.Metabolite, isXenobiotic: true);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme, isXenobiotic: false);
      addMolecule("FloatingEnzyme", isFloating: true, QuantityType.Enzyme, isXenobiotic: false);
      addMolecule("StationaryDrug", isFloating: false, QuantityType.Drug, isXenobiotic: true);
   }

   protected override void Because()
   {
      _result = sut.AllXenobioticFloatingMoleculeNames(_buildingBlock);
   }

   [Observation]
   public void should_return_only_xenobiotic_floating_molecule_names()
   {
      _result.ShouldOnlyContainInOrder("Drug", "Metabolite");
   }
}

internal class When_getting_all_endogenous_stationary_molecule_names : concern_for_MoleculesTask
{
   private string[] _result;

   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug, isXenobiotic: true);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme, isXenobiotic: false);
      addMolecule("Transporter", isFloating: false, QuantityType.Transporter, isXenobiotic: false);
      addMolecule("StationaryDrug", isFloating: false, QuantityType.Drug, isXenobiotic: true);
      addMolecule("FloatingEnzyme", isFloating: true, QuantityType.Enzyme, isXenobiotic: false);
   }

   protected override void Because()
   {
      _result = sut.AllEndogenousStationaryMoleculeNames(_buildingBlock);
   }

   [Observation]
   public void should_return_only_endogenous_stationary_molecule_names()
   {
      _result.ShouldOnlyContainInOrder("Enzyme", "Transporter");
   }
}

internal class When_getting_the_type_of_a_molecule : concern_for_MoleculesTask
{
   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug);
      addMolecule("Enzyme", isFloating: false, QuantityType.Enzyme);
   }

   [Observation]
   public void should_return_drug_for_a_drug_molecule()
   {
      sut.MoleculeTypeFor(_buildingBlock, "Drug").ShouldBeEqualTo("Drug");
   }

   [Observation]
   public void should_return_enzyme_for_an_enzyme_molecule()
   {
      sut.MoleculeTypeFor(_buildingBlock, "Enzyme").ShouldBeEqualTo("Enzyme");
   }
}

internal class When_getting_the_type_of_a_molecule_that_does_not_exist : concern_for_MoleculesTask
{
   protected override void Context()
   {
      base.Context();
      addMolecule("Drug", isFloating: true, QuantityType.Drug);
   }

   [Observation]
   public void should_throw_argument_exception()
   {
      The.Action(() => sut.MoleculeTypeFor(_buildingBlock, "NotThere")).ShouldThrowAn<ArgumentException>();
   }
}

internal class When_loading_molecules_from_pkml : concern_for_MoleculesTask
{
   private MoleculeBuildingBlock _result;

   protected override void Because()
   {
      _result = sut.LoadFromPKML(HelperForSpecs.DataTestFileFullPath("simulation with two modules.pkml"));
   }

   [Observation]
   public void should_return_the_molecule_building_block_from_the_file()
   {
      _result.ShouldNotBeNull();
      _result.Name.ShouldBeEqualTo("Molecules");
   }
}
