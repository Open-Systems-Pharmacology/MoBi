using System;
using System.Data;
using System.Linq;
using OSPSuite.TeXReporting.Data;

namespace MoBi.Core.Extensions
{
   public static class DataViewExtensions
   {
      /// <summary>
      ///    Checks wether all values in the column named <paramref name="columnName"/> are empty and hides that column then.
      /// </summary>
      public static void CheckColumnVisibility(this DataView view, string columnName)
      {
         checkColumnVisibility(view, columnName, cellValue => string.IsNullOrEmpty(cellValue.ToString()));
      }

      /// <summary>
      ///    Checks wether all values in the column named <paramref name="columnName"/> do have the specified <paramref name="value"/> and hides that column then.
      /// </summary>
      public static void CheckColumnVisibility(this DataView view, string columnName, object value)
      {
         checkColumnVisibility(view, columnName, cellValue => Equals(cellValue, value));
      }

      private static void checkColumnVisibility(this DataView view, string columnName, Func<object, bool> cellValueCheckerFunc)
      {
         if (!view.Table.Columns.Contains(columnName))
            return;

         var columnHidden = view.Cast<DataRowView>().All(row => cellValueCheckerFunc(row[columnName]));
         view.Table.Columns[columnName].SetHidden(columnHidden);
      }

      /// <summary>
      ///    Create a table for the view which preserves all child relations of the base table.
      /// </summary>
      public static DataTable CreateDataTableWithPreservedChildRelations(this DataView view)
      {
         var dt = view.ToTable();
         var viewTable = view.Table;
         if (viewTable == null)
            return dt;
         if (viewTable.DataSet == null)
            return dt;

         var ds = new DataSet();
         ds.Tables.Add(dt);
         foreach (DataRelation relation in viewTable.ChildRelations)
         {
            var childTable = relation.ChildTable.Copy();
            ds.Tables.Add(childTable);
            var childColumns = (from col in relation.ChildColumns where childTable.Columns.Contains(col.ColumnName) select childTable.Columns[col.ColumnName]).ToArray();
            var parentColumns = (from col in relation.ParentColumns where dt.Columns.Contains(col.ColumnName) select dt.Columns[col.ColumnName]).ToArray();
            ds.Relations.Add(parentColumns, childColumns);
         }
         return dt;
      }
   }
}