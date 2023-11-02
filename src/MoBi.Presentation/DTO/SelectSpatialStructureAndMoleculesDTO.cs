using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class SelectSpatialStructureAndMoleculesDTO : DxValidatableDTO
   {
      private readonly List<MoleculeSelectionDTO> _molecules = new List<MoleculeSelectionDTO>();
      private readonly List<MoleculeSelectionDTO> _selectedMolecules = new List<MoleculeSelectionDTO>();
      public MoBiSpatialStructure SpatialStructure { get; set; }
      public IReadOnlyList<MoleculeSelectionDTO> Molecules => _molecules;
      public IReadOnlyList<MoleculeSelectionDTO> SelectedMolecules => _selectedMolecules;

      public SelectSpatialStructureAndMoleculesDTO()
      {
         Rules.Add(AllRules.MoleculeSelected);
         Rules.Add(AllRules.SpatialStructureSelected);
      }

      private static class AllRules
      {
         public static IBusinessRule MoleculeSelected { get; } =
            CreateRule.For<SelectSpatialStructureAndMoleculesDTO>()
               .Property(x => x.Molecules)
               .WithRule((dto, moleculeBuildingBlock) => dto.Molecules != null && dto.Molecules.Any())
               .WithError(AppConstants.Validation.ExtendingRequiresMoleculeBuildingBlock);

         public static IBusinessRule SpatialStructureSelected { get; } =
            CreateRule.For<SelectSpatialStructureAndMoleculesDTO>()
               .Property(x => x.SpatialStructure)
               .WithRule((dto, spatialStructure) => dto.SpatialStructure != null)
               .WithError(AppConstants.Validation.ExtendingRequiresSpatialStructure);
      }

      private void addSelectedMolecule(MoleculeSelectionDTO moleculeSelectionDTO)
      {
         if (_selectedMolecules.Contains(moleculeSelectionDTO))
            return;

         _selectedMolecules.Add(moleculeSelectionDTO);
      }

      private void removeSelectedMolecule(MoleculeSelectionDTO moleculeSelectionDTO)
      {
         _selectedMolecules.Remove(moleculeSelectionDTO);
      }

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