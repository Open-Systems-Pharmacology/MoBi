using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Domain
{
   public class OriginalQuantityValue : IWithValueOrigin, IWithDisplayUnit
   {
      public enum Types
      {
         Quantity,
         ScaleDivisor
      }

      public string Path { get; set; }
      public double Value { get; set; }
      public ValueOrigin ValueOrigin { get; } = new ValueOrigin();
      public Unit DisplayUnit { get; set; }
      public IDimension Dimension { get; set; }
      public Types Type { get; set; }
      public string Id => new ObjectPath(Path.ToPathArray().Append(Type.ToString()));
      public bool IsQuantityChange => Type == Types.Quantity;
      public bool IsScaleChange => Type == Types.ScaleDivisor;

      public OriginalQuantityValue WithPropertiesFrom(OriginalQuantityValue sourceOriginalQuantityValue)
      {
         Path = sourceOriginalQuantityValue.Path;
         Value = sourceOriginalQuantityValue.Value;
         ValueOrigin.UpdateFrom(sourceOriginalQuantityValue.ValueOrigin);
         DisplayUnit = sourceOriginalQuantityValue.DisplayUnit;
         Dimension = sourceOriginalQuantityValue.Dimension;
         Type = sourceOriginalQuantityValue.Type;
         return this;
      }

      public void UpdateValueOriginFrom(ValueOrigin sourceValueOrigin)
      {
         ValueOrigin.UpdateFrom(sourceValueOrigin);
      }
   }
}