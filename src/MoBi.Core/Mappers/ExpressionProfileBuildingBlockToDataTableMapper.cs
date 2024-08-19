using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Mappers
{
   public interface IExpressionProfileBuildingBlockToDataTableMapper : IMapper<ExpressionProfileBuildingBlock, List<DataTable>>
   {
   }

   public class ExpressionProfileBuildingBlockToDataTableMapper : IExpressionProfileBuildingBlockToDataTableMapper
   {
      private readonly IInitialConditionsBuildingBlockToDataTableMapper _initialConditionsMapper;
      private readonly IParameterValueBuildingBlockToParameterValuesDataTableMapper _parameterValuesMapper;

      public ExpressionProfileBuildingBlockToDataTableMapper(
         IInitialConditionsBuildingBlockToDataTableMapper initialConditionsMapper,
         IParameterValueBuildingBlockToParameterValuesDataTableMapper parameterValuesMapper)
      {
         _initialConditionsMapper = initialConditionsMapper;
         _parameterValuesMapper = parameterValuesMapper;
      }

      public List<DataTable> MapFrom(ExpressionProfileBuildingBlock input)
      {
         var dataTables = new List<DataTable>();

         var initialConditionsBuildingBlock = new InitialConditionsBuildingBlock();
         input.InitialConditions.Each(initialConditionsBuildingBlock.Add);

         var initialConditionsTable = _initialConditionsMapper.MapFrom(initialConditionsBuildingBlock);
         dataTables.AddRange(initialConditionsTable);

         var parameterValuesBuildingBlock = new ParameterValuesBuildingBlock();
         input.ExpressionParameters.Each(parameterValuesBuildingBlock.Add);

         var parameterValuesTable = _parameterValuesMapper.MapFrom(parameterValuesBuildingBlock);
         dataTables.AddRange(parameterValuesTable);

         return dataTables;
      }
   }
}