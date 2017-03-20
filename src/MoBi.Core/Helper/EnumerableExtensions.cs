using System;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Core.Helper
{
   public static class EnumerableExtensions
   {
      public static IEnumerable<TIn> DistinctBy<TIn, TCompare>(this IEnumerable<TIn> list, Func<TIn, TCompare> func)
      {
         return list.GroupBy(func).Select(grouping => grouping.First());
      }

      public static IEnumerable<TOut> ToEnumerable<TIn,TOut>(this IEnumerable<TIn> list) where TIn : TOut
      {
         foreach (var element in list)
         {
            yield return element;
         }
      }
   }
}