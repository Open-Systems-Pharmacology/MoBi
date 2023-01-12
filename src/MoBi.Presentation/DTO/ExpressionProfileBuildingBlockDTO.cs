using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ExpressionProfileBuildingBlockDTO
   {
      private readonly ExpressionProfileBuildingBlock _buildingBlock;

      public ExpressionProfileBuildingBlockDTO(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _buildingBlock = expressionProfileBuildingBlock;
      }

      public IReadOnlyList<ExpressionParameterDTO> ParameterDTOs { get; set; }

      public string Species => _buildingBlock.Species;
      public string Category => _buildingBlock.Category;
      public string MoleculeName => _buildingBlock.MoleculeName;
      public string NameType => _buildingBlock.Type.DisplayName;
   }
}