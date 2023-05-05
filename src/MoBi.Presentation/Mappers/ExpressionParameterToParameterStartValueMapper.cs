using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Presentation.Mappers
{
   public interface IExpressionParameterToParameterStartValueMapper : IMapper<ExpressionParameter, ParameterValue>
   {
   }

   public class ExpressionParameterToParameterStartValueMapper : IExpressionParameterToParameterStartValueMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly ICloneManagerForModel _cloneManager;

      public ExpressionParameterToParameterStartValueMapper(IObjectBaseFactory objectBaseFactory, ICloneManagerForModel cloneManager)
      {
         _objectBaseFactory = objectBaseFactory;
         _cloneManager = cloneManager;
      }

      public ParameterValue MapFrom(ExpressionParameter expressionParameter)
      {
         var parameterStartValue = _objectBaseFactory.Create<ParameterValue>();

         parameterStartValue.Path = expressionParameter.Path;
         parameterStartValue.Formula = _cloneManager.Clone(expressionParameter.Formula);
         parameterStartValue.Dimension = expressionParameter.Dimension;
         parameterStartValue.Value = expressionParameter.Value;
         parameterStartValue.Description = expressionParameter.Description;
         parameterStartValue.UpdateValueOriginFrom(expressionParameter.ValueOrigin);

         return parameterStartValue;
      }
   }
}