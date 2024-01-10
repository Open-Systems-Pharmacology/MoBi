using System.Collections.Generic;
using Antlr.Runtime.Misc;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface ISelectMoleculesDTOMapper : IMapper<IEnumerable<MoleculeBuildingBlock>, SelectMoleculesDTO>
   {
      /// <summary>
      ///    Returns a SelectMoleculesDTO with molecules from the <paramref name="moleculeBuildingBlocks" /> collection.
      ///    The <paramref name="canSelect" /> predicate is used to determine whether a molecule can be selected.
      /// </summary>
      SelectMoleculesDTO MapFrom(IEnumerable<MoleculeBuildingBlock> moleculeBuildingBlocks, Func<MoleculeBuilder, bool> canSelect);
   }

   public class SelectMoleculesDTOMapper : ISelectMoleculesDTOMapper
   {
      public SelectMoleculesDTO MapFrom(IEnumerable<MoleculeBuildingBlock> moleculeBuildingBlocks, Func<MoleculeBuilder, bool> canSelect)
      {
         var dto = new SelectMoleculesDTO();
         moleculeBuildingBlocks.Each(x => createMoleculeDTOs(x, dto, canSelect));
         return dto;
      }

      private void createMoleculeDTOs(MoleculeBuildingBlock buildingBlock, SelectMoleculesDTO parentDTO, Func<MoleculeBuilder, bool> canSelect) => buildingBlock.Each(x => createMoleculeDTO(x, parentDTO, canSelect));

      private void createMoleculeDTO(MoleculeBuilder molecule, SelectMoleculesDTO parentDTO, Func<MoleculeBuilder, bool> canSelect)
      {
         if (canSelect == null || canSelect(molecule))
            parentDTO.AddMolecule(new MoleculeSelectionDTO(molecule));
      }

      public SelectMoleculesDTO MapFrom(IEnumerable<MoleculeBuildingBlock> moleculeBuildingBlocks) => MapFrom(moleculeBuildingBlocks, x => true);
   }
}