using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Core.Services
{
   public interface IQuantityToParameterValueMapper : IMapper<IQuantity, ParameterValue>
   {
   }
   
   public class QuantityToParameterValueMapper : IQuantityToParameterValueMapper
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public QuantityToParameterValueMapper(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public ParameterValue MapFrom(IQuantity input)
      {
         var parameterValue = new ParameterValue
         {
            Path = _entityPathResolver.ObjectPathFor(input),
            Value = input.Value,
            Formula = input.Formula,
            Dimension = input.Dimension,
            DisplayUnit = input.DisplayUnit
         };

         parameterValue.UpdateValueOriginFrom(input.ValueOrigin);
         return parameterValue;
      }
   }
}