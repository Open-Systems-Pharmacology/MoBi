using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class SelectMoleculesDTO : ContainsMultiSelectDTO<MoleculeSelectionDTO, SelectMoleculesDTO>
   {
      public IReadOnlyList<MoleculeSelectionDTO> Molecules => _selectableDTOs;
      public IReadOnlyList<MoleculeSelectionDTO> SelectedMolecules => _selectedDTOs;

      public SelectMoleculesDTO()
      {
         Rules.Add(AllRules.MoleculeSelected);
      }

      private static class AllRules
      {
         public static IBusinessRule MoleculeSelected { get; } =
            CreateRule.For<SelectMoleculesDTO>()
               .Property(x => x.Molecules)
               .WithRule((dto, moleculeBuildingBlock) => dto.Molecules != null && dto.Molecules.Any())
               .WithError(AppConstants.Validation.ExtendingRequiresMoleculeBuildingBlock);
      }

      public void AddMolecule(MoleculeSelectionDTO moleculeSelectionDTO)
      {
         moleculeSelectionDTO.ParentDTO = this;
         AddSelectableDTO(moleculeSelectionDTO);
      }
   }
}