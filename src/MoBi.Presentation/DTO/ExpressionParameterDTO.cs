using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ExpressionParameterDTO : PathAndValueEntityDTO<ExpressionParameter>
   {
      public ExpressionParameterDTO(ExpressionParameter expressionParameter) : base(expressionParameter)
      {
      }
   }
}