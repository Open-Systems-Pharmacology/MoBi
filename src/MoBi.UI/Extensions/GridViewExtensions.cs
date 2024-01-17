using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
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
   }
}
