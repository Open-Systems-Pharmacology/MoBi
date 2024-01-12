using Antlr.Runtime.Misc;
using MoBi.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Format;

namespace MoBi.Presentation.Formatters
{
   public class NullableWithRetrievableDisplayUnitFormatter : NumericFormatter<double?>
   {
      protected readonly Func<Unit> _unitRetriever;

      public NullableWithRetrievableDisplayUnitFormatter(Func<Unit> unitRetriever) : base(NumericFormatterOptions.Instance)
      {
         _unitRetriever = unitRetriever;
      }

      public override string Format(double? valueToFormat)
      {
         var value = valueToFormat.GetValueOrDefault(double.NaN);
         if (double.IsNaN(value))
            return AppConstants.NaN;

         string formattedValue = base.Format(valueToFormat);
         return AppConstants.Commands.DisplayValue(formattedValue, _unitRetriever().Name);
      }
   }
}