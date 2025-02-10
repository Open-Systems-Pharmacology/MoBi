using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Validation;

namespace MoBi.Presentation.DTO
{
   public class IndividualExpressionAndFilePathDTO : ContainsMultiSelectDTO<ExpressionProfileBuildingBlockSelectionDTO, IndividualExpressionAndFilePathDTO>, IWithName
   {
      public string FilePath { get; set; }
      public IndividualBuildingBlock IndividualBuildingBlock { get; set; }
      public string Name { get; set; }
      public ObjectPath ContainerPath { get; set; }
      public string Description => AppConstants.Captions.ExportContainerDescription(ContainerPath?.PathAsString);

      public IReadOnlyList<ExpressionProfileBuildingBlock> SelectedExpressionProfileBuildingBlocks => SelectedExpressionProfiles.Select(x => x.BuildingBlock).ToList();
      public IReadOnlyList<ExpressionProfileBuildingBlockSelectionDTO> SelectableExpressionProfiles => _selectableDTOs;
      public IReadOnlyList<ExpressionProfileBuildingBlockSelectionDTO> SelectedExpressionProfiles => _selectedDTOs;

      public IndividualExpressionAndFilePathDTO()
      {
         Rules.AddRange(AllRules.All);
      }

      public void AddSelectableExpressionProfile(ExpressionProfileBuildingBlock buildingBlock)
      {
         var dto = new ExpressionProfileBuildingBlockSelectionDTO(buildingBlock) { ParentDTO = this };
         AddSelectableDTO(dto);
      }

      private static class AllRules
      {
         public static IReadOnlyList<IBusinessRule> All { get; } = new IBusinessRule[]
         {
            CreateRule.For<IndividualExpressionAndFilePathDTO>()
               .Property(x => x.FilePath)
               .WithRule((dto, name) => name.StringIsNotEmpty())
               .WithError(Validation.ValueIsRequired)
         };
      }
   }
}