﻿using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class MoleculeSelectionDTO : DxValidatableDTO
   {
      private bool _selected;

      public MoleculeSelectionDTO()
      {
         Rules.Add(AllRules.SelectedMoleculesHaveUniqueNames);
      }

      public string BuildingBlock { get; set; }

      public string MoleculeName => MoleculeBuilder?.Name;

      public MoleculeBuilder MoleculeBuilder { get; set; }

      public bool Selected {
         get => _selected;
         set
         {
            _selected = value;
            ParentDTO?.SelectionUpdated(this);
         }
      }

      public string Icon => MoleculeBuilder.Icon;
      public SelectSpatialStructureAndMoleculesDTO ParentDTO { get; set; }

      private static class AllRules
      {
         public static IBusinessRule SelectedMoleculesHaveUniqueNames { get; } =
            CreateRule.For<MoleculeSelectionDTO>()
               .Property(x => x.Selected)
               .WithRule((dto, selected) => !selected || uniqueNamesSelected(dto))
               .WithError((dto, selected) => AppConstants.Validation.AnotherMoleculeNamedIsSelected(dto.MoleculeName));

         private static bool uniqueNamesSelected(MoleculeSelectionDTO dto)
         {
            return !selectedMoleculesWithout(dto).Select(x => x.MoleculeName).Contains(dto.MoleculeName);
         }

         private static IEnumerable<MoleculeSelectionDTO> selectedMoleculesWithout(MoleculeSelectionDTO dto)
         {
            return dto.ParentDTO.SelectedMolecules.Except(new[] { dto });
         }
      }
   }
}