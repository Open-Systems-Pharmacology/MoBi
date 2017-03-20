using MoBi.Assets;
using OSPSuite.Utility.Format;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Formatters
{
   public class NullableWithDisplayUnitFormatter : NumericFormatter<double?>
   {
      private readonly IWithDisplayUnit _withDisplayUnit;

      public NullableWithDisplayUnitFormatter(IWithDisplayUnit withDisplayUnit)
         : base(NumericFormatterOptions.Instance)
      {
         _withDisplayUnit = withDisplayUnit;
      }

      public override string Format(double? valueToFormat)
      {
         var value = valueToFormat.GetValueOrDefault(double.NaN);
         if (double.IsNaN(value))
            return AppConstants.NaN;

         string formattedValue = base.Format(valueToFormat);
         return AppConstants.Commands.DisplayValue(formattedValue, _withDisplayUnit.DisplayUnit.Name);
      }
   }
}