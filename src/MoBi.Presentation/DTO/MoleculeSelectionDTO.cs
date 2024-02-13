using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class MoleculeSelectionDTO : SelectableDTO<MoleculeSelectionDTO, SelectMoleculesDTO>
   {
      public MoleculeSelectionDTO(MoleculeBuilder moleculeBuilder)
      {
         MoleculeBuilder = moleculeBuilder;
         Rules.Add(AllRules.SelectedMoleculesHaveUniqueNames);
      }

      public string Icon => MoleculeBuilder.Icon;
      public IBuildingBlock BuildingBlock => MoleculeBuilder.BuildingBlock;
      public string BuildingBlockDisplayName => BuildingBlock.ToString();
      public string MoleculeName => MoleculeBuilder.Name;
      public MoleculeBuilder MoleculeBuilder { get; }

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