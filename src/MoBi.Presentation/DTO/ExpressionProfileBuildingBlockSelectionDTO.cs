using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class ExpressionProfileBuildingBlockSelectionDTO : SelectableDTO<ExpressionProfileBuildingBlockSelectionDTO, IndividualExpressionAndFilePathDTO>
   {
      private readonly ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;

      public ExpressionProfileBuildingBlockSelectionDTO(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _expressionProfileBuildingBlock = expressionProfileBuildingBlock;
         Rules.Add(AllRules.SelectedMoleculesHaveUniqueNames);
      }
      
      public string DisplayName => _expressionProfileBuildingBlock.DisplayName;
      public ExpressionProfileBuildingBlock BuildingBlock => _expressionProfileBuildingBlock;
      public string Icon => _expressionProfileBuildingBlock.Icon;
      public string MoleculeName => _expressionProfileBuildingBlock.MoleculeName;

      private static class AllRules
      {
         public static IBusinessRule SelectedMoleculesHaveUniqueNames { get; } =
            CreateRule.For<ExpressionProfileBuildingBlockSelectionDTO>()
               .Property(x => x.Selected)
               .WithRule((dto, selected) => !selected || uniqueNamesSelected(dto))
               .WithError((dto, selected) => AppConstants.Validation.AnotherMoleculeNamedIsSelected(dto.MoleculeName));

         private static bool uniqueNamesSelected(ExpressionProfileBuildingBlockSelectionDTO dto) => !selectedMoleculesWithout(dto).Select(x => x.MoleculeName).Contains(dto.MoleculeName);

         private static IEnumerable<ExpressionProfileBuildingBlockSelectionDTO> selectedMoleculesWithout(ExpressionProfileBuildingBlockSelectionDTO dto) => dto.ParentDTO.SelectedExpressionProfiles.Except(new[] { dto });
      }
   }
}