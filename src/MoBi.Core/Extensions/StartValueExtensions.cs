using System.Globalization;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Extensions
{
   public static class StartValueExtensions
   {
      public static string GetStartValueAsDisplayString(this PathAndValueEntity startValue)
      {
         return string.Format("{0} {1}",
            startValue.ConvertToDisplayUnit(startValue.Value).ToString(CultureInfo.InvariantCulture),
            startValue.DisplayUnit
            );
      }   
   }
}