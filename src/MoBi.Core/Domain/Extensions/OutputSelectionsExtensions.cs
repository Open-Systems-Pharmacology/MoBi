using System.Linq;
using OSPSuite.Core.Domain;

namespace MoBi.Core.Domain.Extensions
{
   public static class OutputSelectionsExtensions
   {
      /// <summary>
      /// Returns true if one item selected in the first output selection is not selected in the second or vice versa
      /// </summary>
      public static bool DiffersFrom(this OutputSelections first, OutputSelections second)
      {
         if (second == null)
            return true;

         if (first.Count() != second.Count())
            return true;

         foreach (var firstOutput in first.AllOutputs)
         {
            var secondOutput = second.AllOutputs.FirstOrDefault(x => string.Equals(x.Path, firstOutput.Path));

            if (secondOutput == null)
               return true;
         }

         return false;
      }
   }
}