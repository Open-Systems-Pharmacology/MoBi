using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mapper
{
   internal class concern_for_SelectSpatialStructureAndMoleculesDTOMapper : ContextSpecification<SelectSpatialStructureAndMoleculesDTOMapper>
   {
      protected override void Context()
      {
         sut = new SelectSpatialStructureAndMoleculesDTOMapper();
      }
   }

   internal class When_mapping_from_molecules_and_spatial_structures_to_a_dto : concern_for_SelectSpatialStructureAndMoleculesDTOMapper
   {
      private SelectSpatialStructureAndMoleculesDTO _dto;
      private MoleculeBuildingBlock _buildingBlock;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoleculeBuildingBlock { new MoleculeBuilder() };
         _spatialStructure = new MoBiSpatialStructure();
      }

      protected override void Because()
      {
         _dto = sut.MapFrom(new[] { _buildingBlock }, _spatialStructure);
      }

      [Observation]
      public void should_map_the_spatial_structure()
      {
         _dto.SpatialStructure.ShouldBeEqualTo(_spatialStructure);
      }

      [Observation]
      public void should_map_the_molecules()
      {
         _dto.Molecules.Count.ShouldBeEqualTo(1);
      }
   }
}