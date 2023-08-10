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

      public ParameterValue MapFrom(IQuantity quantity)
      {
         var parameterValue = new ParameterValue
         {
            Path = _entityPathResolver.ObjectPathFor(quantity),
            Value = quantity.Value,
            Formula = quantity.Formula,
            Dimension = quantity.Dimension,
            DisplayUnit = quantity.DisplayUnit
         };

         parameterValue.UpdateValueOriginFrom(quantity.ValueOrigin);
         return parameterValue;
      }
   }
}