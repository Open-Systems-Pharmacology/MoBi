using System.Collections.Generic;
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

      public MoleculeSelectionDTO(MoleculeBuilder moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
         Rules.Add(AllRules.SelectedMoleculesHaveUniqueNames);
      }

      public string Icon => MoleculeBuilder.Icon;
      public SelectMoleculesDTO ParentDTO { get; set; }
      public IBuildingBlock BuildingBlock => MoleculeBuilder.BuildingBlock;
      public string BuildingBlockDisplayName => BuildingBlock.ToString();
      public string MoleculeName => MoleculeBuilder.Name;
      public MoleculeBuilder MoleculeBuilder { get; }

      public bool Selected
      {
         get => _selected;
         set
         {
            _selected = value;
            ParentDTO?.SelectionUpdated(this);
         }
      }

      private static class AllRules
      {
         public static IBusinessRule SelectedMoleculesHaveUniqueNames { get; } =
            CreateRule.For<MoleculeSelectionDTO>()
               .Property(x => x.Selected)
               .WithRule((dto, selected) => !selected || uniqueNamesSelected(dto))
               .WithError((dto, selected) => AppConstants.Validation.AnotherMoleculeNamedIsSelected(dto.MoleculeName));

         private static bool uniqueNamesSelected(MoleculeSelectionDTO dto) => !selectedMoleculesWithout(dto).Select(x => x.MoleculeName).Contains(dto.MoleculeName);

         private static IEnumerable<MoleculeSelectionDTO> selectedMoleculesWithout(MoleculeSelectionDTO dto) => dto.ParentDTO.SelectedMolecules.Except(new[] { dto });
      }
   }
}