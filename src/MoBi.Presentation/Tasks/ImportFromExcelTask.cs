using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OSPSuite.Utility.Exceptions;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using MoBi.Presentation.Extensions;

namespace MoBi.Presentation.Tasks
{

   public interface IImportFromExcelTask
   {
      /// <summary>
      /// Retrieves all the appropriate sheet names from an excel file
      /// </summary>
      /// <param name="filename">The filename and path of the excel file</param>
      /// <param name="excludeEmptySheets">Excludes empty sheets from the list if set to true</param>
      /// <returns>The list of sheet names</returns>
      IReadOnlyList<string> RetrieveExcelSheets(string filename, bool excludeEmptySheets = false);

      /// <summary>
      /// Gets the DataTable for the filepath, and sheet indicated
      /// </summary>
      /// <param name="filePath">The path to the excel file</param>
      /// <param name="sheetName">the sheet in the file to convert</param>
      /// <param name="firstRowAsCaption">Whether or not the first row in the sheet should be interpreted as a caption</param>
      /// <returns>Data table corresponding to the file and sheet</returns>
      DataTable GetDataTables(string filePath, string sheetName, bool firstRowAsCaption);

      /// <summary>
      /// Gets all the DataTables from the Excel workbook at the specified filePath
      /// </summary>
      /// <param name="filePath">The path to the excel file</param>
      /// <param name="firstRowAsCaption">Whether or not the first row in the sheet should be interpreted as a caption</param>
      /// <returns>The data tables corresponding to the file</returns>
      IReadOnlyList<DataTable> GetAllDataTables(string filePath, bool firstRowAsCaption);
   }

   public class ImportFromExcelTask : IImportFromExcelTask
   {
      public IReadOnlyList<string> RetrieveExcelSheets(string filename, bool excludeEmptySheets = false)
      {
         try
         {
            return retrieveExcelSheets(filename, excludeEmptySheets).ToList();
         }
         catch
         {
            return Enumerable.Empty<string>().ToList();
         }
      }

      private static IEnumerable<string> retrieveExcelSheets(string filename, bool excludeEmptySheets)
      {
         if (string.IsNullOrEmpty(filename))
            yield break;

         if (!File.Exists(filename))
            yield break;

         using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
         {
            var workbook = WorkbookFactory.Create(fs);
            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
               if (!excludeEmptySheets || !workbook.GetSheetAt(i).IsCurrentSheetEmpty())
                  yield return workbook.GetSheetName(i);
            }
         }
      }

      public IReadOnlyList<DataTable> GetAllDataTables(string filePath, bool firstRowAsCaption)
      {
         return getDataTables(filePath, string.Empty, firstRowAsCaption);
      }

      public DataTable GetDataTables(string filePath, string sheetName, bool firstRowAsCaption)
      {
         return getDataTables(filePath, sheetName, firstRowAsCaption).FirstOrDefault();
      }

      private static IReadOnlyList<DataTable> getDataTables(string fileName, string sheetName, bool firstRowAsCaption)
      {
         try
         {
            return dataTables(fileName, sheetName, firstRowAsCaption).ToList();
         }
         catch (DuplicateNameException exception)
         {
            throw new OSPSuiteException(exception.Message);
         }
      }

      private static IEnumerable<DataTable> dataTables(string fileName, string sheetName, bool firstRowAsCaption)
      {
         using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
         {
            var workbook = WorkbookFactory.Create(fs);
            for (var i = 0; i < workbook.NumberOfSheets; i++)
            {
               if (!string.IsNullOrEmpty(sheetName) && !workbook.GetSheetAt(i).SheetName.Equals(sheetName)) continue;
               yield return workbook.GetSheetAt(i).ExportDataTable(sheetName, firstRowAsCaption);
            }
         }
      }
   }
}
