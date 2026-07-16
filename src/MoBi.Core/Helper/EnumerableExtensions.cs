using System.Collections.Generic;

namespace MoBi.Core.Helper
{
   public static class EnumerableExtensions
   {
      public static IEnumerable<TOut> ToEnumerable<TIn,TOut>(this IEnumerable<TIn> list) where TIn : TOut
      {
         foreach (var element in list)
         {
            yield return element;
         }
      }
   }
}