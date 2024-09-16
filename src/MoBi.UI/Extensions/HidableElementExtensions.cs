using MoBi.Presentation.Views;

namespace MoBi.UI.Extensions
{
   public static class HidableElementExtensions
   {
      public static bool IsSet(this HidablePathAndValuesViewElement elements, HidablePathAndValuesViewElement elementToCheck)
      {
         return (elements & elementToCheck) == elementToCheck;
      }
   }
}