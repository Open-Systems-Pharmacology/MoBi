using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionParameterToExpressionParameterDTOMapper : IMapper<ExpressionParameter, ExpressionParameterDTO>
   {
   }

   public class ExpressionParameterToExpressionParameterDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IExpressionParameterToExpressionParameterDTOMapper
   {
      private readonly IFormulaToValueFormulaDTOMapper _formulaMapper;

      public ExpressionParameterToExpressionParameterDTOMapper(IFormulaToValueFormulaDTOMapper formulaMapper)
      {
         _formulaMapper = formulaMapper;
      }
      public ExpressionParameterDTO MapFrom(ExpressionParameter expressionParameter)
      {
         return new ExpressionParameterDTO(expressionParameter)
         {
            Formula = _formulaMapper.MapFrom(expressionParameter.Formula)
         };
      }
   }
}