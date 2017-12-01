using System;
using System.IO;
using OSPSuite.Core.Domain;
using SmartXLS;

namespace MoBi.Presentation.Extensions
{
   public static class SmartXLSExtensions
   {
      /// <summary>
      /// Reads a workbook in readonly mode. This allows the worbook to be read while it's open in another process
      /// </summary>
      /// <param name="workBook">The workbook to be updated with the content of the excel file</param>
      /// <param name="filePath">The path to the Excel file</param>
      public static void ReadExcelFile(this WorkBook workBook, string filePath)
      {
         var extension = Path.GetExtension(filePath);

         if (!File.Exists(filePath)) return;

         using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            if (!string.IsNullOrEmpty(extension) && extension.Equals(Constants.Filter.XLSX_EXTENSION, StringComparison.CurrentCultureIgnoreCase))
               workBook.readXLSX(fs);
            else
               workBook.read(fs);
         }
      }

      public static bool IsCurrentSheetEmpty(this WorkBook workbook)
      {
         return workbook.LastCol == -1 || workbook.LastRow == -1;
      }
   }
}
