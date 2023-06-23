using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper : IMapper<ExpressionProfileBuildingBlock, ExpressionProfileBuildingBlockDTO>
   {
   }

   public class ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper : IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper
   {
      private readonly IExpressionParameterToExpressionParameterDTOMapper _expressionParameterToExpressionParameterDTOMapper;

      public ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper(IExpressionParameterToExpressionParameterDTOMapper expressionParameterToExpressionParameterDTOMapper)
      {
         _expressionParameterToExpressionParameterDTOMapper = expressionParameterToExpressionParameterDTOMapper;
      }

      public ExpressionProfileBuildingBlockDTO MapFrom(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         return new ExpressionProfileBuildingBlockDTO(expressionProfileBuildingBlock)
         {
            ParameterDTOs = expressionProfileBuildingBlock.MapAllUsing(_expressionParameterToExpressionParameterDTOMapper)
         };
      }
   }
}