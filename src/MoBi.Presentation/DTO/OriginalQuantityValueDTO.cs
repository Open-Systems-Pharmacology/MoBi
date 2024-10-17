using MoBi.Core.Domain;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Presentation.DTO
{
   public class OriginalQuantityValueDTO
   {
      public OriginalQuantityValueDTO(OriginalQuantityValue originalQuantityValue)
      {
         Dimension = originalQuantityValue.Dimension;
         Path = originalQuantityValue.Path;
         OriginalValue = Dimension.BaseUnitValueToUnitValue(originalQuantityValue.DisplayUnit, originalQuantityValue.Value);
         OriginalDisplayUnit = originalQuantityValue.DisplayUnit;
      }

      public string Path { get; private set; }
      public IDimension Dimension {  get; set;}

      public double? CurrentValue { get; private set; }
      public Unit CurrentDisplayUnit { get; private set; }

      public Unit OriginalDisplayUnit { get; set; }
      public double? OriginalValue { get; private set; }

      public OriginalQuantityValueDTO WithCurrentQuantity(IQuantity quantity)
      {
         CurrentValue = Dimension.BaseUnitValueToUnitValue(quantity.DisplayUnit, quantity.Value);
         CurrentDisplayUnit = quantity.DisplayUnit;
         return this;
      }
   }
}