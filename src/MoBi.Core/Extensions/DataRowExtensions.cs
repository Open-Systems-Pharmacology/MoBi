using System.Data;
using System.Text;
using OSPSuite.Utility.Extensions;

namespace MoBi.Core.Extensions
{
   public static class DataRowExtensions
   {
      /// <summary>
      /// Prints a version of the row data by converting each argument to a string by calling ToString and enclosing it in square brackets
      /// </summary>
      /// <param name="row">The row being converted</param>
      /// <returns>A version of the row as a string: [row0],[row1],[row2] etc.</returns>
      public static string ToNiceString(this DataRow row)
      {
         var sb = new StringBuilder();
         row.ItemArray.Each(x => sb.Append(string.Format("[{0}],", x.ToString())));
         return sb.ToString().Trim(',');
      }
   }
}