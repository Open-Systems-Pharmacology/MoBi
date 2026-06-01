using System;
using System.Collections.Generic;
using System.Linq;

namespace MoBi.Core.Domain.Extensions
{
   public class IndexedNameComparer : IComparer<string>
   {
      public int Compare(string x, string y)
      {
         if (x == null && y == null) return 0;
         if (x == null) return -1;
         if (y == null) return 1;

         var xParts = x.Split('_');
         var yParts = y.Split('_');

         var prefixX = string.Join("_", xParts.Take(xParts.Length - 1));
         var prefixY = string.Join("_", yParts.Take(yParts.Length - 1));

         var prefixComparison = string.Compare(prefixX, prefixY, StringComparison.Ordinal);

         // if the prefix strings are not equal then use the string comparison
         if (prefixComparison != 0)
            return prefixComparison;

         var suffixX = xParts.Last();
         var suffixY = yParts.Last();

         // both have a numeric suffix, compare them as numbers
         if (int.TryParse(suffixX, out var numX) && int.TryParse(suffixY, out var numY))
            return numX.CompareTo(numY);

         // fall back to string comparison if either suffix is not numeric
         return string.Compare(suffixX, suffixY, StringComparison.Ordinal);
      }
   }
}