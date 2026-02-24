using System;
using System.Linq;

namespace MoBi.Core.Extensions;

public static class ArrayExtensions
{
   public static bool HasConsistentLengthWith(this Array firstArray, params Array[] arrays) => arrays.All(x => x.Length == firstArray.Length);
}