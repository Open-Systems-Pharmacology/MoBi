using System;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Core.Domain;
using OSPSuite.UI.Controls;

namespace MoBi.UI.Extensions
{
   public static class GridViewExtensions
   {
      public static void ConfigureGridForCheckBoxSelect(this UxGridView gridView, string selectColumnName)
      {
         gridView.MultiSelect = true;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
         gridView.OptionsSelection.CheckBoxSelectorField = selectColumnName;
         gridView.OptionsSelection.ShowCheckBoxSelectorInGroupRow = DefaultBoolean.True;

         gridView.OptionsCustomization.AllowQuickHideColumns = false;
      }

      public static void DisableGrouping(this UxGridView gridView)
      {
         gridView.OptionsCustomization.AllowGroup = false;
         gridView.OptionsView.ShowGroupPanel = false;
      }

      /// <summary>
      /// Creates a ToolTipControlInfo for the <paramref name="toolTipSubject"/> using the <paramref name="toolTipGenerator"/> function.
      /// If a column can be determined from <paramref name="point"/>, then the column caption and cell display text are used with <paramref name="toolTipGenerator"/>.
      /// </summary>
      public static ToolTipControlInfo CreateToolTipControlInfoFor<T>(this GridView gridView, T toolTipSubject, Point point, Func<T, string, string, SuperToolTip> toolTipGenerator) where T : IWithName
      {
         var columnName = string.Empty;
         var cellValue = string.Empty;

         var gridHitInfo = gridView.CalcHitInfo(point);
         if (gridHitInfo.Column != null)
         {
            columnName = gridHitInfo.Column.Caption;
            cellValue = gridView.GetRowCellDisplayText(gridHitInfo.RowHandle, gridHitInfo.Column);
         }

         // Adds a super-tooltip for the current active object. Uniquely identified by the combination of toolTipSubject.Name and columnName.
         return new ToolTipControlInfo(toolTipSubject.Name + columnName, string.Empty)
         {
            SuperTip = toolTipGenerator(toolTipSubject, columnName, cellValue), 
            ToolTipType = ToolTipType.SuperTip
         };
      }
   }
}