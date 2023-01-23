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

      public ParameterStartValuesBuildingBlock MapFrom(ExpressionProfileBuildingBlock input)
      {
         var buildingBlock = _objectBaseFactory.Create<ParameterStartValuesBuildingBlock>();

         buildingBlock.Name = input.Name;

         input.Each(expressionParameter => buildingBlock.Add(_expressionParameterToParameterStartValueMapper.MapFrom(expressionParameter)));

         buildingBlock.FormulaCache.AddRange(buildingBlock.UniqueFormulasByName());

         return buildingBlock;
      }
   }
}