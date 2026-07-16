using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.DTO
{
   public class ExpressionParameterDTO : ParameterValueWithInitialStateDTO<ExpressionParameter, ExpressionParameterDTO>
   {
      public ExpressionParameterDTO(ExpressionParameter expressionParameter) : base(expressionParameter)
      {
      }
   }
}