using System.Collections.Generic;

namespace MoBi.Core.Extensions;

public static class EnumerableExtensions
{
   public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
   {
      return [..source];
   }
}