using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper : IMapper<ExpressionProfileBuildingBlock, ExpressionProfileBuildingBlockDTO>
   {
   }

   public class ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper :
      PathAndValueEntityBuildingBlockToPathAndValueEntityBuildingBlockDTOMapper<ExpressionProfileBuildingBlock, ExpressionParameter, ExpressionProfileBuildingBlockDTO, ExpressionParameterDTO>,
      IExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper
   {
      private readonly IExpressionParameterToExpressionParameterDTOMapper _expressionParameterToExpressionParameterDTOMapper;
      private readonly IInitialConditionToInitialConditionDTOMapper _initialConditionsMapper;

      public ExpressionProfileBuildingBlockToExpressionProfileBuildingBlockDTOMapper(IExpressionParameterToExpressionParameterDTOMapper expressionParameterToExpressionParameterDTOMapper, IInitialConditionToInitialConditionDTOMapper initialConditionsMapper)
      {
         _expressionParameterToExpressionParameterDTOMapper = expressionParameterToExpressionParameterDTOMapper;
         _initialConditionsMapper = initialConditionsMapper;
      }

      protected override ExpressionParameterDTO BuilderDTOFor(ExpressionParameter pathAndValueEntity, ExpressionProfileBuildingBlock buildingBlock)
      {
         return _expressionParameterToExpressionParameterDTOMapper.MapFrom(pathAndValueEntity);
      }

      protected override ExpressionProfileBuildingBlockDTO MapBuildingBlockDTO(ExpressionProfileBuildingBlock buildingBlock, List<ExpressionParameterDTO> parameterDTOs)
      {
         return new ExpressionProfileBuildingBlockDTO(buildingBlock)
         {
            ParameterDTOs = parameterDTOs,
            InitialConditionDTOs = buildingBlock.InitialConditions.Select(x => _initialConditionsMapper.MapFrom(x, buildingBlock)).ToList()
         };
      }
   }
}