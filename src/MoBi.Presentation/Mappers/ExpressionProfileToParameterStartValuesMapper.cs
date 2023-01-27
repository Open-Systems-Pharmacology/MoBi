using MoBi.Core.Domain.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionProfileToParameterStartValuesMapper : IMapper<ExpressionProfileBuildingBlock, ParameterStartValuesBuildingBlock>
   {
   }

   public class ExpressionProfileToParameterStartValuesMapper : IExpressionProfileToParameterStartValuesMapper
   {
      private readonly IExpressionParameterToParameterStartValueMapper _expressionParameterToParameterStartValueMapper;
      private readonly IObjectBaseFactory _objectBaseFactory;

      public ExpressionProfileToParameterStartValuesMapper(IExpressionParameterToParameterStartValueMapper expressionParameterToParameterStartValueMapper, IObjectBaseFactory objectBaseFactory)
      {
         _expressionParameterToParameterStartValueMapper = expressionParameterToParameterStartValueMapper;
         _objectBaseFactory = objectBaseFactory;
      }

      public ParameterStartValuesBuildingBlock MapFrom(ExpressionProfileBuildingBlock expressionProfileBuildingBlock)
      {
         var buildingBlock = _objectBaseFactory.Create<ParameterStartValuesBuildingBlock>();
         buildingBlock.Name = expressionProfileBuildingBlock.Name;
         expressionProfileBuildingBlock.MapAllUsing(_expressionParameterToParameterStartValueMapper).Each(x => buildingBlock.Add(x));
         buildingBlock.FormulaCache.AddRange(buildingBlock.UniqueFormulasByName());

         return buildingBlock;
      }
   }
}