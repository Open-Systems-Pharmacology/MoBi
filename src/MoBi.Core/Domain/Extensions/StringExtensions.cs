using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;

namespace MoBi.Core.Domain.Extensions
{
   public static class StringExtensions
   {
      public static bool IsSpecialName(this string name)
      {
         return name.IsOneOf(Constants.MOLECULE_PROPERTIES, Constants.NEIGHBORHOODS) ;
      }

      public static string TrimmedValueOf(this string valueToTrim)
      {
         if (!string.IsNullOrEmpty(valueToTrim))
            return valueToTrim.Trim();
         return valueToTrim;
      }

      public static bool IsNotEmpty(this string stringToCheck)
      {
         return !string.IsNullOrEmpty(stringToCheck.TrimmedValueOf());
      }
   }
}