using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionParameterToExpressionParameterDTOMapper : IMapper<ExpressionParameter, ExpressionParameterDTO>
   {

   }

   public class ExpressionParameterToExpressionParameterDTOMapper : PathWithValueToDTOMapper<ExpressionParameter, ExpressionParameterDTO>, IExpressionParameterToExpressionParameterDTOMapper
   {
      public ExpressionParameterToExpressionParameterDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper) : base(formulaMapper)
      {
         
      }

      protected override ExpressionParameterDTO DTOFor(ExpressionParameter inputParameter)
      {
         return new ExpressionParameterDTO(inputParameter);
      }
   }
}