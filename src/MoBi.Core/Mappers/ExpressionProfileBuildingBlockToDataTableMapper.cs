using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;

namespace MoBi.Core.Mappers
{
   public interface IExpressionProfileBuildingBlockToDataTableMapper : IMapper<ExpressionProfileBuildingBlock, List<DataTable>>
   {
   }

   public class ExpressionProfileBuildingBlockToDataTableMapper : IExpressionProfileBuildingBlockToDataTableMapper
   {
      private readonly IInitialConditionsBuildingBlockToDataTableMapper _initialConditionsMapper;
      private readonly IExpressionParametersToExpressionParametersDataTableMapper _expressionParametersMapper;

      public ExpressionProfileBuildingBlockToDataTableMapper(
         IInitialConditionsBuildingBlockToDataTableMapper initialConditionsMapper,
         IExpressionParametersToExpressionParametersDataTableMapper expressionParametersMapper)
      {
         _initialConditionsMapper = initialConditionsMapper;
         _expressionParametersMapper = expressionParametersMapper;
      }

      public List<DataTable> MapFrom(ExpressionProfileBuildingBlock input)
      {
         var dataTables = new List<DataTable>();

         dataTables.AddRange(_initialConditionsMapper.MapFrom(input.InitialConditions));
         dataTables.AddRange(_expressionParametersMapper.MapFrom(input.ExpressionParameters));

         return dataTables;
      }
   }
}