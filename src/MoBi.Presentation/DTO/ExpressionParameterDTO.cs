using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ExpressionParameterDTO : PathAndValueEntityDTO<ExpressionParameter>, IWithFormulaDTO
   {
      public ExpressionParameterDTO(ExpressionParameter expressionParameter) : base(expressionParameter)
      {
      }
   }
}