using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace MoBi.Core.Domain
{
   public class OriginalQuantityValue : IWithValueOrigin, IWithDisplayUnit
   {
      public string Path { get; set; }
      public double Value { get; set; }
      public ValueOrigin ValueOrigin { get; } = new ValueOrigin();
      public Unit DisplayUnit { get; set; }
      public IDimension Dimension { get; set; }
      
      public OriginalQuantityValue WithPropertiesFrom(OriginalQuantityValue sourceOriginalQuantityValue)
      {
         Path = sourceOriginalQuantityValue.Path;
         Value = sourceOriginalQuantityValue.Value;
         ValueOrigin.UpdateFrom(sourceOriginalQuantityValue.ValueOrigin);
         DisplayUnit = sourceOriginalQuantityValue.DisplayUnit;
         Dimension = sourceOriginalQuantityValue.Dimension;
         return this;
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         ValueOrigin.UpdateFrom(sourceValueOrigin);
      }
   }
}