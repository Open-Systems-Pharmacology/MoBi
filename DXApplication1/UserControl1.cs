using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace DXApplication1
{
   public partial class UserControl1 : XtraUserControl
   {

      public UserControl1()
      {
         InitializeComponent();
         InitializeResources();
         setControlVisibility();
      }


      
      private void setControlVisibility()
      {
         layoutControl.SuspendLayout();
      
         RowFor(tablePanel, labelMinimum).Visible = false;
         RowFor(tablePanel, labelMaximum).Visible = false;

         RowFor(tablePanel, labelMean).Visible = false;
         RowFor(tablePanel, labelDeviation).Visible = true;
         RowFor(tablePanel, labelGeoStd).Visible = true;
         RowFor(tableProperties, labelPercentile).Visible = true;
      
         layoutControl.ResumeLayout();
      }

      public void InitializeResources()
      {
         var height = cbFormulaType.Height;
         adjustControlSize(tablePanel, veDeviation, height: height);
         adjustControlSize(tablePanel, veGeoStd, height: height);
         adjustControlSize(tablePanel, veMean, height: height);
         adjustControlSize(tablePanel, veMinimum, height: height);
         adjustControlSize(tablePanel, veMaximum, height: height);
         adjustControlSize(tableProperties, veValue, height: height);

         labelMean.Text = "Mean";
         labelDeviation.Text = "Standard Deviation";
         labelGeoStd.Text = "Geometric Deviation";
         labelMinimum.Text = "Minimum";
         labelMaximum.Text = "Maximum";
         labelName.Text = "Name";
         labelDimension.Text = "Dimension";
         labelValue.Text = "Value";
         labelPercentile.Text = "Percentile";
         labellDistribution.Text = "Distribution";
      }

      public static void adjustControlSize(TablePanel tablePanel, Control control, int? width = null, int? height = null)
      {
         var row = RowFor(tablePanel, control);
         var col = ColumnFor(tablePanel, control);
         if (width.HasValue)
         {
            col.Style = TablePanelEntityStyle.AutoSize;
            col.Width = width.Value + control.Margin.Horizontal;
            // control.MaximumSize = new Size(width.Value, control.Height);
            control.Width = width.Value;
         }

         if (height.HasValue)
         {
            row.Style = TablePanelEntityStyle.AutoSize;
            
            row.Height = height.Value + control.Margin.Vertical;
            // control.MaximumSize = new Size(control.Width, height.Value);
            control.Height = height.Value;
         }
      }

      /// <summary>
      ///    Returns the <see cref="TablePanelRow" /> where the <paramref name="control" /> is located
      /// </summary>
      public static TablePanelRow RowFor(TablePanel tablePanel, Control control)
      {
         return tablePanel.Rows[tablePanel.GetRow(control)];
      }

      /// <summary>
      ///    Returns the <see cref="TablePanelColumn" /> where the <paramref name="control" /> is located
      /// </summary>
      public static TablePanelColumn ColumnFor(TablePanel tablePanel, Control control)
      {
         return tablePanel.Columns[tablePanel.GetColumn(control)];
      }

      private void btName_Click(object sender, EventArgs e)
      {

      }
   }
}
