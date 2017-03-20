using System.Data;
using System.Text;
using OSPSuite.Utility.Exceptions;

namespace MoBi.Presentation.Mappers
{
   public class ImportQuantityDTOsFromDataTablesMapperException : OSPSuiteException
   {
      protected DataRow _row;
      protected string _suggestion;
      private readonly int _rowIndex;

      /// <summary>
      /// Constructs the exception
      /// </summary>
      /// <param name="row">The table being converted</param>
      /// <param name="rowIndex">Give the index of the datarow that's causing the exception to be thrown</param>
      /// <param name="suggestion">Give a suggestion how this data row could be fixed</param>
      public ImportQuantityDTOsFromDataTablesMapperException(DataRow row, int rowIndex, string suggestion)
      {
         _row = row;
         _rowIndex = rowIndex;
         _suggestion = suggestion;
      }

      public override string Message
      {
         get
         {
            var sb = new StringBuilder();

            // +2 to make the message a little more friendly for the user (+1=convert from 0-base to 1-base, +1=header row)
            sb.Append(string.Format("row {0} ", _rowIndex + 2)); 
            return _suggestion + " in " + sb.ToString().Trim(',');
         }
      }
   }
}