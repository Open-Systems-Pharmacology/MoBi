using MoBi.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace MoBi.Core.Services
{
   public interface IQuantityToOriginalQuantityValueMapper : IMapper<IQuantity, OriginalQuantityValue>, IMapper<MoleculeAmount, OriginalQuantityValue>
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
         var quantityValue = new OriginalQuantityValue
         {
            Path = _entityPathResolver.ObjectPathFor(quantity),
            Value = quantity.Value,
            Dimension = quantity.Dimension,
            DisplayUnit = quantity.DisplayUnit,
            Type = OriginalQuantityValue.Types.Quantity
         };

         quantityValue.UpdateValueOriginFrom(quantity.ValueOrigin);
         return quantityValue;
      }

      public OriginalQuantityValue MapFrom(MoleculeAmount moleculeAmount)
      {
         return new OriginalQuantityValue
         {
            Value = moleculeAmount.ScaleDivisor,
            Path = _entityPathResolver.ObjectPathFor(moleculeAmount),
            Type = OriginalQuantityValue.Types.ScaleDivisor,
            Dimension = Constants.Dimension.NO_DIMENSION,
            DisplayUnit = Constants.Dimension.NO_DIMENSION.DefaultUnit
         };
      }
   }
}