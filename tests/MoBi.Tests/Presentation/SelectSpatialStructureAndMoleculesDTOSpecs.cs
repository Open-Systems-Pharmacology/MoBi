using System.Collections.Generic;
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
         sut.AddMolecules(new List<MoleculeSelectionDTO>
         {
            new MoleculeSelectionDTO { MoleculeBuilder = new MoleculeBuilder().WithName("builder"), Selected = true },
            new MoleculeSelectionDTO { MoleculeBuilder = new MoleculeBuilder().WithName("builder"), Selected = true }
         });
      }

      [Observation]
      public void the_dto_is_not_valid()
      {
         sut.Molecules.Any(x => x.IsValid()).ShouldBeFalse();
      }
   }

   public class When_validating_the_dto_when_two_molecules_with_the_same_name_with_only_one_selected : concern_for_SelectSpatialStructureDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.SpatialStructure = new MoBiSpatialStructure();
         sut.AddMolecules(new List<MoleculeSelectionDTO>()
         {
            new MoleculeSelectionDTO { MoleculeBuilder = new MoleculeBuilder().WithName("builder"), Selected = true},
            new MoleculeSelectionDTO { MoleculeBuilder = new MoleculeBuilder().WithName("builder") }
         });
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
         sut.AddMolecules(new List<MoleculeSelectionDTO> {new MoleculeSelectionDTO { MoleculeBuilder = new MoleculeBuilder().WithName("builder")}});
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
