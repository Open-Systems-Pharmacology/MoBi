using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ExpressionParameterDTO : PathWithValueEntityDTO<ExpressionParameter>, IWithFormulaDTO
   {
      public ExpressionParameterDTO(ExpressionParameter expressionParameter) : base(expressionParameter)
      {
         
      }
   }
}