using MoBi.Assets;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Formatters
{
   public class StartValueFormatter : NullableWithDisplayUnitFormatter
   {
      public override string Format(double? valueToFormat)
      {
         return !valueToFormat.HasValue || double.IsNaN(valueToFormat.Value) ? AppConstants.Captions.StartValueNotAvailable : base.Format(valueToFormat);
      }

      public StartValueFormatter(IWithDisplayUnit moleculeStartValue)
         : base(moleculeStartValue)
      {

      }
   }
}