using OSPSuite.Core.Domain;

namespace MoBi.Presentation.Formatters
{
   public class NullableWithDisplayUnitFormatter : NullableWithRetrievableDisplayUnitFormatter
   {
      public NullableWithDisplayUnitFormatter(IWithDisplayUnit withDisplayUnit)
         : base(() => withDisplayUnit.DisplayUnit)
      {
      }
   }
}