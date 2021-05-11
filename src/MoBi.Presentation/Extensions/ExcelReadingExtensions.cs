using System;
using System.Data;
using System.IO;
using NPOI.SS.UserModel;

namespace MoBi.Presentation.Extensions
{
   public static class ExcelReadingExtensions
   {
      /// <summary>
      /// Reads a workbook in readonly mode. This allows the worbook to be read while it's open in another process
      /// </summary>
      /// <param name="filePath">The path to the Excel file</param>
      public static DataTable ExportDataTable(this ISheet sheet, string sheetName, bool firstRowAsCaption)
      {
         var data = new DataTable {TableName = sheetName};
         var startRow = 0;
         if (sheet != null)
         {
            var firstRow = sheet.GetRow(0);
            if (firstRow == null)
               return data;
            int cellCount = firstRow.LastCellNum;
            startRow = firstRowAsCaption ? sheet.FirstRowNum + 1 : sheet.FirstRowNum;

            for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
            {
               var column = new DataColumn();
               if (firstRowAsCaption)
               {
                  var columnName = firstRow.GetCell(i).StringCellValue;
                  column = new DataColumn(columnName);
               }

               data.Columns.Add(column);
            }

            var rowCount = sheet.LastRowNum;
            for (var i = startRow; i <= rowCount; ++i)
            {
               var row = sheet.GetRow(i);
               if (row == null) continue;

               var dataRow = data.NewRow();
               for (int j = row.FirstCellNum; j < cellCount; ++j)
               {
                  if (row.GetCell(j) != null) // Similarly, no data are defaults to null cells
                     dataRow[j] = row.GetCell(j, MissingCellPolicy.RETURN_NULL_AND_BLANK).ToString();
               }

               data.Rows.Add(dataRow);
            }
         }

         return data;
      }
      public static bool IsCurrentSheetEmpty(this ISheet sheet)
      {

         return sheet.LastRowNum == -1 || sheet.GetRow(sheet.LastRowNum)?.GetCell(0) == null;
      }
   }
}