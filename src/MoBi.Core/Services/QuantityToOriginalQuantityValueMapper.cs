using MoBi.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Core.Services
{
   public interface IQuantityToOriginalQuantityValueMapper : IMapper<IQuantity, OriginalQuantityValue>
   {
   }
   
   public class QuantityToOriginalQuantityValueMapper : IQuantityToOriginalQuantityValueMapper
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public QuantityToOriginalQuantityValueMapper(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public OriginalQuantityValue MapFrom(IQuantity quantity)
      {
         var parameterValue = new OriginalQuantityValue
         {
            Path = _entityPathResolver.ObjectPathFor(quantity),
            Value = quantity.Value,
            Dimension = quantity.Dimension,
            DisplayUnit = quantity.DisplayUnit
         };

         parameterValue.UpdateValueOriginFrom(quantity.ValueOrigin);
         return parameterValue;
      }
   }
}