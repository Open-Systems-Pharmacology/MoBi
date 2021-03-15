using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Presentation.Extensions;
using SmartXLS;
using OSPSuite.Utility.Exceptions;

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

         using (var workbook = new WorkBook())
         {
            workbook.ReadExcelFile(filename);
            for (var i = 0; i < workbook.NumSheets; i++)
            {
               workbook.Sheet = i;
               if (!excludeEmptySheets || !workbook.IsCurrentSheetEmpty())
                  yield return workbook.getSheetName(i);
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
         using (var workbook = new WorkBook())
         {
            workbook.ReadExcelFile(fileName);
            for (var i = 0; i < workbook.NumSheets; i++)
            {
               if (!String.IsNullOrEmpty(sheetName) && !workbook.getSheetName(i).Equals(sheetName)) continue;
               workbook.Sheet = i;
               //+1 because lastrow starts counting from 0, but export counts from 1
               yield return workbook.ExportDataTable(0, 0, workbook.LastRow + 1, workbook.LastCol + 1, firstRowAsCaption);
            }
         }
      }
   }
}
