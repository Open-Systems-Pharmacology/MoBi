using MoBi.Assets;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Formatters
{
   public class ValueAllowingNaNFormatter : NullableWithDisplayUnitFormatter
   {
      public override string Format(double? valueToFormat)
      {
         return !valueToFormat.HasValue || double.IsNaN(valueToFormat.Value) ? AppConstants.Captions.ValueNotAvailable : base.Format(valueToFormat);
      }

      public ValueAllowingNaNFormatter(IWithDisplayUnit withDisplayUnit) : base(withDisplayUnit)
      {
      }
   }
}