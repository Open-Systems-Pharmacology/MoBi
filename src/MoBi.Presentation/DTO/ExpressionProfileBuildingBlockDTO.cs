using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ExpressionProfileBuildingBlockDTO
   {
      private readonly ExpressionProfileBuildingBlock _expressionProfileBuildingBlock;

      public ExpressionProfileBuildingBlockDTO(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         _expressionProfileBuildingBlock = expressionProfileBuildingBlock;
      }

      public IReadOnlyList<ExpressionParameterDTO> ExpressionParameters { get; set; }

      public string Species => _expressionProfileBuildingBlock.Species;
      public string Category => _expressionProfileBuildingBlock.Category;
      public string MoleculeName => _expressionProfileBuildingBlock.MoleculeName;
      public string NameType => _expressionProfileBuildingBlock.Icon;
   }
}