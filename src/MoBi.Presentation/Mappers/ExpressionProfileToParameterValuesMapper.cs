using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionProfileToParameterValuesMapper : IMapper<ExpressionProfileBuildingBlock, ParameterValuesBuildingBlock>
   {
   }

   public class ExpressionProfileToParameterValuesMapper : IExpressionProfileToParameterValuesMapper
   {
      private readonly IPathAndValueEntityToParameterValueMapper _expressionParameterToParameterValueMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;

      public ExpressionProfileToParameterValuesMapper(IPathAndValueEntityToParameterValueMapper expressionParameterToParameterValueMapper, IObjectBaseFactory objectBaseFactory)
      {
         _expressionParameterToParameterValueMapper = expressionParameterToParameterValueMapper;
         _objectBaseFactory = objectBaseFactory;
      }

      public ParameterValuesBuildingBlock MapFrom(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         var buildingBlock = _objectBaseFactory.Create<ParameterValuesBuildingBlock>();
         buildingBlock.Name = expressionProfileBuildingBlock.Name;
         expressionProfileBuildingBlock.ExpressionParameters.MapAllUsing(_expressionParameterToParameterValueMapper).Each(x => buildingBlock.Add(x));
         buildingBlock.FormulaCache.AddRange(buildingBlock.UniqueFormulasByName());

         return buildingBlock;
      }
   }
}