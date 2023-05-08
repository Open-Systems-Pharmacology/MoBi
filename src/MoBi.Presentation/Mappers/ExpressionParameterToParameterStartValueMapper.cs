using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionParameterToParameterValueMapper : IMapper<ExpressionParameter, ParameterValue>
   {
   }

   public class ExpressionParameterToParameterValueMapper : IExpressionParameterToParameterValueMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManager;

      public ExpressionParameterToParameterValueMapper(IObjectBaseFactory objectBaseFactory, ICloneManagerForModel cloneManager)
      {
         _objectBaseFactory = objectBaseFactory;
         _cloneManager = cloneManager;
      }

      public ParameterValue MapFrom(ExpressionParameter expressionParameter)
      {
         var parameterValue = _objectBaseFactory.Create<ParameterValue>();

         parameterValue.Path = expressionParameter.Path;
         parameterValue.Formula = _cloneManager.Clone(expressionParameter.Formula);
         parameterValue.Dimension = expressionParameter.Dimension;
         parameterValue.Value = expressionParameter.Value;
         parameterValue.Description = expressionParameter.Description;
         parameterValue.UpdateValueOriginFrom(expressionParameter.ValueOrigin);

         return parameterValue;
      }
   }
}