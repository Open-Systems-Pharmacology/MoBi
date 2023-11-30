using System.Collections.Generic;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface ISelectSpatialStructureAndMoleculesDTOMapper
   {
      SelectSpatialStructureAndMoleculesDTO MapFrom(IEnumerable<MoleculeBuildingBlock> moleculeBuildingBlocks, MoBiSpatialStructure spatialStructure);
   }

   public class SelectSpatialStructureAndMoleculesDTOMapper : ISelectSpatialStructureAndMoleculesDTOMapper
   {
      public SelectSpatialStructureAndMoleculesDTO MapFrom(IEnumerable<MoleculeBuildingBlock> moleculeBuildingBlocks, MoBiSpatialStructure spatialStructure)
      {
         var dto = new SelectSpatialStructureAndMoleculesDTO
         {
            SpatialStructure = spatialStructure,
         };

         moleculeBuildingBlocks.Each(x => createMoleculeDTOs(x, dto));
         return dto;
      }

      private void createMoleculeDTOs(MoleculeBuildingBlock buildingBlock, SelectSpatialStructureAndMoleculesDTO parentDTO)
      {
         buildingBlock.Each(x => createMoleculeDTO(x, parentDTO));
      }

      private void createMoleculeDTO(MoleculeBuilder molecule, SelectSpatialStructureAndMoleculesDTO parentDTO)
      {
         parentDTO.AddMolecule(new MoleculeSelectionDTO
         {
            MoleculeBuilder = molecule
         });
      }
   }
}