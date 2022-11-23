using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

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
         var expressionProfileDTO =  new ExpressionProfileBuildingBlockDTO(expressionProfileBuildingBlock)
         {
            ExpressionParameters = expressionProfileBuildingBlock.Select(x => _expressionParameterToExpressionParameterDTOMapper.MapFrom(x)).ToList()
         };

         return expressionProfileDTO;
      }
   }
}