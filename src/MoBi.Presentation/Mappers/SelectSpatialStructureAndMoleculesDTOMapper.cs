using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface ISelectSpatialStructureAndMoleculesDTOMapper
   {
      SelectSpatialStructureAndMoleculesDTO MapFrom(IEnumerable<MoleculeBuildingBlock> buildingBlocksToSelectFrom, MoBiSpatialStructure spatialStructure);
   }

   public class SelectSpatialStructureAndMoleculesDTOMapper : ISelectSpatialStructureAndMoleculesDTOMapper
   {
      private readonly IMoleculeToMoleculeSelectionDTOMapper _moleculeMapper;

      public SelectSpatialStructureAndMoleculesDTOMapper(IMoleculeToMoleculeSelectionDTOMapper moleculeMapper)
      {
         _moleculeMapper = moleculeMapper;
      }

      public SelectSpatialStructureAndMoleculesDTO MapFrom(IEnumerable<MoleculeBuildingBlock> buildingBlocksToSelectFrom, MoBiSpatialStructure spatialStructure)
      {
         var dto = new SelectSpatialStructureAndMoleculesDTO
         {
            SpatialStructure = spatialStructure,
         };

         dto.AddMolecules(buildingBlocksToSelectFrom.SelectMany(createMoleculeDTO).ToList());
         return dto;
      }

      private IReadOnlyList<MoleculeSelectionDTO> createMoleculeDTO(MoleculeBuildingBlock buildingBlock)
      {
         return _moleculeMapper.MapFrom(buildingBlock);
      }
   }
}