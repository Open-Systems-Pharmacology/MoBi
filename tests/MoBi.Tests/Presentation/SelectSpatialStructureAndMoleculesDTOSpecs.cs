using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_SelectSpatialStructureDTO : ContextSpecification<SelectSpatialStructureAndMoleculesDTO>
   {
      protected override void Context()
      {
         sut = new SelectSpatialStructureAndMoleculesDTO();
      }
   }

   public class When_validating_the_dto_when_two_molecules_with_the_same_name_and_both_are_selected : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.SpatialStructure = new MoBiSpatialStructure();
         
         sut.AddMolecule(new MoleculeSelectionDTO (new MoleculeBuilder().WithName("builder")) { Selected = true });
         sut.AddMolecule(new MoleculeSelectionDTO (new MoleculeBuilder().WithName("builder")) { Selected = true });
      }

      [Observation]
      public void the_dto_is_not_valid()
      {
         sut.Molecules.Any(x => x.IsValid()).ShouldBeFalse();
      }

      [Observation]
      public void both_molecules_should_be_selected()
      {
         sut.SelectedMolecules.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_selecting_a_molecule_that_makes_an_valid_result_become_invalid : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.SpatialStructure = new MoBiSpatialStructure();

         sut.AddMolecule(new MoleculeSelectionDTO (new MoleculeBuilder().WithName("builder")));
         sut.AddMolecule(new MoleculeSelectionDTO(new MoleculeBuilder().WithName("builder")) { Selected = true });
      }

      protected override void Because()
      {
         sut.Molecules.First().Selected = true;
      }

      [Observation]
      public void the_selected_molecule_should_be_added()
      {
         sut.SelectedMolecules.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_dto_is_invalid()
      {
         sut.Molecules.Any(x => x.IsValid()).ShouldBeFalse();
      }
   }

   public class When_unselecting_a_molecule_that_makes_an_invalid_result_become_valid : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.SpatialStructure = new MoBiSpatialStructure();

         sut.AddMolecule(new MoleculeSelectionDTO (new MoleculeBuilder().WithName("builder")) { Selected = true });
         sut.AddMolecule(new MoleculeSelectionDTO (new MoleculeBuilder().WithName("builder")) { Selected = true });
      }

      protected override void Because()
      {
         sut.Molecules.First().Selected = false;
      }

      [Observation]
      public void the_unselected_molecule_should_be_removed()
      {
         sut.SelectedMolecules.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.Molecules.Any(x => x.IsValid()).ShouldBeTrue();
      }
   }

   public class When_validating_the_dto_when_two_molecules_with_the_same_name_with_only_one_selected : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.SpatialStructure = new MoBiSpatialStructure();
         sut.AddMolecule(new MoleculeSelectionDTO (new MoleculeBuilder().WithName("builder")) { Selected = true });
         sut.AddMolecule(new MoleculeSelectionDTO(new MoleculeBuilder().WithName("builder")));
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_the_dto_when_building_blocks_are_selected : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.SpatialStructure = new MoBiSpatialStructure();
         sut.AddMolecule(new MoleculeSelectionDTO (new MoleculeBuilder().WithName("builder")));
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_the_dto_when_no_spatial_structure_is_selected : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
