using MoBi.Assets;
using OSPSuite.Utility.Format;
using MoBi.Presentation.DTO;

namespace MoBi.Presentation.Formatters
{
   public class ValuePointFormatter : NumericFormatter<double>
   {
      private readonly ValuePointParameterDTO _dtoValuePointParameter;

      public ValuePointFormatter(ValuePointParameterDTO dtoValuePointParameter)
         : base(NumericFormatterOptions.Instance)
      {
         _dtoValuePointParameter = dtoValuePointParameter;
      }

      public override string Format(double valueToFormat)
      {
         if (double.IsNaN(valueToFormat))
            return AppConstants.NaN;

         var formattedValue = base.Format(valueToFormat);
         return $"{formattedValue} {_dtoValuePointParameter.DisplayUnit}";
      }
   }
}