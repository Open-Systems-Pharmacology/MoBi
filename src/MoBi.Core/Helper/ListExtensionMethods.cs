using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Helper
{
   public static class ListExtensionMethods
   {
      public static IList<T> AddUnique<T>(this IList<T> list, T newElement) where T : class
      {
         if (!list.Contains<T>(newElement))
         {
            list.Add(newElement);
         }
         return list;
      }

      public static void RemoveRange<T>(this IList<T> list, IEnumerable<T> itemsToRemove)
      {
         itemsToRemove.ToList().Each(t => list.Remove(t));
      }

      
   }
}