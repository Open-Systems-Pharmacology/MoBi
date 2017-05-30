using MoBi.Assets;
using OSPSuite.Utility.Format;

namespace MoBi.Presentation.Formatters
{
   public class UnitFormatter : NumericFormatter<double>
   {
      private readonly string _unit;

      public UnitFormatter(string unit) : base(NumericFormatterOptions.Instance)
      {
         _unit = unit;
      }

      public override string Format(double valueToFormat)
      {
         if (double.IsNaN(valueToFormat))
            return AppConstants.NaN;

         var formattedValue = base.Format(valueToFormat);
         return $"{formattedValue} {_unit}";
      }
   }
}