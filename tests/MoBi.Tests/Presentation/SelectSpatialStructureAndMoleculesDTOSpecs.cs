using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation
{
   public class concern_for_SelectSpatialStructureAndMoleculesDTO : ContextSpecification<SelectSpatialStructureAndMoleculesDTO>
   {
      protected override void Context()
      {
         sut = new SelectSpatialStructureAndMoleculesDTO(IsMoleculeRequired);
      }

      protected virtual bool IsMoleculeRequired => true;
   }

   public class When_validating_the_dto_when_building_blocks_are_selected : concern_for_SelectSpatialStructureAndMoleculesDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.Molecules = new MoleculeBuildingBlock();
         sut.SpatialStructure = new MoBiSpatialStructure();
      }

      protected override bool IsMoleculeRequired => false;

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_validating_the_dto_when_molecules_are_not_required : concern_for_SelectSpatialStructureAndMoleculesDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = new MoBiSpatialStructure();
         sut.Molecules = null;
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_validating_the_dto_when_no_molecules_are_selected : concern_for_SelectSpatialStructureAndMoleculesDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = new MoBiSpatialStructure();
         sut.Molecules = null;
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_validating_the_dto_when_no_spatial_structure_is_selected : concern_for_SelectSpatialStructureAndMoleculesDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.SpatialStructure = null;
         sut.Molecules = new MoleculeBuildingBlock();
      }

      [Observation]
      public void the_dto_is_valid()
      {
         sut.IsValid().ShouldBeFalse();
      }
   }
}
