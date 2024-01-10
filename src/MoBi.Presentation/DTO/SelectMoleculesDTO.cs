using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class SelectMoleculesDTO : DxValidatableDTO
   {
      private readonly List<MoleculeSelectionDTO> _molecules = new List<MoleculeSelectionDTO>();
      private readonly List<MoleculeSelectionDTO> _selectedMolecules = new List<MoleculeSelectionDTO>();
      public IReadOnlyList<MoleculeSelectionDTO> Molecules => _molecules;
      public IReadOnlyList<MoleculeSelectionDTO> SelectedMolecules => _selectedMolecules;

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

      private void addSelectedMolecule(MoleculeSelectionDTO moleculeSelectionDTO)
      {
         if (_selectedMolecules.Contains(moleculeSelectionDTO))
            return;

         _selectedMolecules.Add(moleculeSelectionDTO);
      }

      private void removeSelectedMolecule(MoleculeSelectionDTO moleculeSelectionDTO) => _selectedMolecules.Remove(moleculeSelectionDTO);

      public void SelectionUpdated(MoleculeSelectionDTO moleculeSelectionDTO)
      {
         if (moleculeSelectionDTO.Selected)
            addSelectedMolecule(moleculeSelectionDTO);
         else
            removeSelectedMolecule(moleculeSelectionDTO);
      }

      public void AddMolecule(MoleculeSelectionDTO moleculeSelectionDTO)
      {
         moleculeSelectionDTO.ParentDTO = this;
         _molecules.Add(moleculeSelectionDTO);

         if (moleculeSelectionDTO.Selected)
            addSelectedMolecule(moleculeSelectionDTO);
      }
   }
}