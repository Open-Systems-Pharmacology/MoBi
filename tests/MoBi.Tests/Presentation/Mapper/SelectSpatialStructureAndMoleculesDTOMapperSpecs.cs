using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mapper
{
   internal class concern_for_SelectSpatialStructureAndMoleculesDTOMapper : ContextSpecification<SelectSpatialStructureAndMoleculesDTOMapper>
   {
      protected IMoleculeToMoleculeSelectionDTOMapper _moleculeSelectionMapper;

      protected override void Context()
      {
         _moleculeSelectionMapper = A.Fake<IMoleculeToMoleculeSelectionDTOMapper>();
         sut = new SelectSpatialStructureAndMoleculesDTOMapper(_moleculeSelectionMapper);
      }
   }

   internal class When_mapping_from_molecules_and_spatial_structures_to_a_dto : concern_for_SelectSpatialStructureAndMoleculesDTOMapper
   {
      private SelectSpatialStructureAndMoleculesDTO _dto;
      private MoleculeBuildingBlock _buildingBlock;
      private MoleculeSelectionDTO _moleculeSelectionDTO;
      private MoBiSpatialStructure _spatialStructure;

      protected override void Context()
      {
         base.Context();
         _buildingBlock = new MoleculeBuildingBlock();
         _spatialStructure = new MoBiSpatialStructure();
         _moleculeSelectionDTO = new MoleculeSelectionDTO();
         A.CallTo(() => _moleculeSelectionMapper.MapFrom(_buildingBlock)).Returns(new[] { _moleculeSelectionDTO });
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
         _dto.Molecules.ShouldContain(_moleculeSelectionDTO);
      }
   }
}