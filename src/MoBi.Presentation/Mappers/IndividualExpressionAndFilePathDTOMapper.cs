using System.Collections.Generic;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IIndividualExpressionAndFilePathDTOMapper : IMapper<IEnumerable<ExpressionProfileBuildingBlock>, IndividualExpressionAndFilePathDTO>
   {

   }

   public class IndividualExpressionAndFilePathDTOMapper : IIndividualExpressionAndFilePathDTOMapper
   {
      public IndividualExpressionAndFilePathDTO MapFrom(IEnumerable<ExpressionProfileBuildingBlock> expressionProfileBuildingBlocks)
      {
         var dto = new IndividualExpressionAndFilePathDTO();
         expressionProfileBuildingBlocks.Each(x => createExpressionProfileDTO(x, dto));
         return dto;
      }


      private void createExpressionProfileDTO(ExpressionProfileBuildingBlock expressionProfile, IndividualExpressionAndFilePathDTO parentDTO)
      {
         parentDTO.AddSelectableExpressionProfile(expressionProfile);
      }
   }
}