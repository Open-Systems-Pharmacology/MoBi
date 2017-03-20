using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Extensions
{
   public static class StringExtensions
   {
      public static bool IsSpecialName(this string name)
      {
         return Constants.MOLECULE_PROPERTIES.Equals(name) || Constants.NEIGHBORHOODS.Equals(name) ;
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