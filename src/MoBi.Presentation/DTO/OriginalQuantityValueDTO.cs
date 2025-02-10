using MoBi.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace MoBi.Presentation.DTO
{
   public class OriginalQuantityValueDTO
   {
      public OriginalQuantityValueDTO(OriginalQuantityValue originalQuantityValue, double currentBaseValue)
      {
         Dimension = originalQuantityValue.Dimension;
         Path = originalQuantityValue.Path;
         OriginalValue = Dimension.BaseUnitValueToUnitValue(originalQuantityValue.DisplayUnit, originalQuantityValue.Value);
         DisplayUnit = originalQuantityValue.DisplayUnit;
         Type = originalQuantityValue.Type.ToString().SplitToUpperCase();
         CurrentValue = Dimension.BaseUnitValueToUnitValue(DisplayUnit, currentBaseValue);
      }

      public string Type { get; private set; }
      public string Path { get; private set; }
      public IDimension Dimension {  get; set;}

      public double? CurrentValue { get; private set; }

      public Unit DisplayUnit { get; set; }
      public double? OriginalValue { get; private set; }
   }
}