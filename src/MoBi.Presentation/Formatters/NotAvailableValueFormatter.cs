using MoBi.Assets;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Formatters
{
   public class NotAvailableValueFormatter : NullableWithDisplayUnitFormatter
   {
      public override string Format(double? valueToFormat)
      {
         return !valueToFormat.HasValue || double.IsNaN(valueToFormat.Value) ? AppConstants.Captions.ValueNotAvailable : base.Format(valueToFormat);
      }

      public NotAvailableValueFormatter(IWithDisplayUnit withDisplayUnit) : base(withDisplayUnit)
      {
      }
   }
}