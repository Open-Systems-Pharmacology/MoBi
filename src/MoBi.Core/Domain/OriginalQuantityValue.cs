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

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         ValueOrigin.UpdateFrom(sourceValueOrigin);
      }
   }
}