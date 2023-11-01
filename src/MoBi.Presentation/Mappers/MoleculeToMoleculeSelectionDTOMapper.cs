using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeToMoleculeSelectionDTOMapper : IMapper<MoleculeBuildingBlock, IReadOnlyList<MoleculeSelectionDTO>>
   {
   }

   public class MoleculeToMoleculeSelectionDTOMapper : IMoleculeToMoleculeSelectionDTOMapper
   {
      private MoleculeSelectionDTO createFrom(MoleculeBuilder molecule, string buildingBlockName)
      {
         return new MoleculeSelectionDTO
         {
            BuildingBlock = buildingBlockName,
            MoleculeBuilder = molecule
         };
      }

      public IReadOnlyList<MoleculeSelectionDTO> MapFrom(MoleculeBuildingBlock buildingBlock)
      {
         return buildingBlock.Select(x => createFrom(x, buildingBlock.ToString())).ToList();
      }
   }
}