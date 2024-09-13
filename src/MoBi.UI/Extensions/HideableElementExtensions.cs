using MoBi.Presentation.Views;

namespace MoBi.UI.Extensions
{
   public static class HideableElementExtensions
   {
      public static bool IsSet(this HideableElement elements, HideableElement elementToCheck)
      {
         return (elements & elementToCheck) == elementToCheck;
      }
   }
}