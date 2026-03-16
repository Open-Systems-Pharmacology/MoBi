namespace MoBi.UI.Views
{
   partial class EditMoleculeCalculationMethodsView
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         treeViewPanel = new OSPSuite.UI.Controls.UxPanelControl();
         calculationMethodGrid = new OSPSuite.UI.Controls.UxGridControl();
         gridViewCalculationMethods = new OSPSuite.UI.Controls.UxGridView();
         Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemCalculationMethods = new DevExpress.XtraLayout.LayoutControlItem();
         layoutControlItemMolecules = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
         ((System.ComponentModel.ISupportInitialize)uxLayoutControl1).BeginInit();
         uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)treeViewPanel).BeginInit();
         ((System.ComponentModel.ISupportInitialize)calculationMethodGrid).BeginInit();
         ((System.ComponentModel.ISupportInitialize)gridViewCalculationMethods).BeginInit();
         ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
         ((System.ComponentModel.ISupportInitialize)this.layoutControlItemCalculationMethods).BeginInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItemMolecules).BeginInit();
         SuspendLayout();
         // 
         // uxLayoutControl1
         // 
         uxLayoutControl1.AllowCustomization = false;
         uxLayoutControl1.Controls.Add(treeViewPanel);
         uxLayoutControl1.Controls.Add(calculationMethodGrid);
         uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         uxLayoutControl1.Name = "uxLayoutControl1";
         uxLayoutControl1.Root = Root;
         uxLayoutControl1.Size = new System.Drawing.Size(638, 409);
         uxLayoutControl1.TabIndex = 0;
         uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // treeViewPanel
         // 
         treeViewPanel.Location = new System.Drawing.Point(12, 12);
         treeViewPanel.Name = "treeViewPanel";
         treeViewPanel.Size = new System.Drawing.Size(149, 385);
         treeViewPanel.TabIndex = 5;
         // 
         // calculationMethodGrid
         // 
         calculationMethodGrid.Location = new System.Drawing.Point(165, 12);
         calculationMethodGrid.MainView = gridViewCalculationMethods;
         calculationMethodGrid.Name = "calculationMethodGrid";
         calculationMethodGrid.Size = new System.Drawing.Size(461, 385);
         calculationMethodGrid.TabIndex = 4;
         calculationMethodGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridViewCalculationMethods });
         // 
         // gridViewCalculationMethods
         // 
         gridViewCalculationMethods.AllowsFiltering = true;
         gridViewCalculationMethods.EnableColumnContextMenu = true;
         gridViewCalculationMethods.GridControl = calculationMethodGrid;
         gridViewCalculationMethods.MultiSelect = true;
         gridViewCalculationMethods.Name = "gridViewCalculationMethods";
         gridViewCalculationMethods.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         gridViewCalculationMethods.OptionsNavigation.AutoFocusNewRow = true;
         gridViewCalculationMethods.OptionsSelection.EnableAppearanceFocusedCell = false;
         gridViewCalculationMethods.OptionsSelection.EnableAppearanceFocusedRow = false;
         gridViewCalculationMethods.OptionsSelection.MultiSelect = true;
         gridViewCalculationMethods.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // Root
         // 
         Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         Root.GroupBordersVisible = false;
         Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { this.layoutControlItemCalculationMethods, layoutControlItemMolecules });
         Root.Name = "Root";
         Root.Size = new System.Drawing.Size(638, 409);
         Root.TextVisible = false;
         // 
         // layoutControlItemCalculationMethods
         // 
         this.layoutControlItemCalculationMethods.Control = calculationMethodGrid;
         this.layoutControlItemCalculationMethods.Location = new System.Drawing.Point(153, 0);
         this.layoutControlItemCalculationMethods.Name = "layoutControlItemCalculationMethods";
         this.layoutControlItemCalculationMethods.Size = new System.Drawing.Size(465, 389);
         this.layoutControlItemCalculationMethods.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCalculationMethods.TextVisible = false;
         // 
         // layoutControlItemMolecules
         // 
         layoutControlItemMolecules.Control = treeViewPanel;
         layoutControlItemMolecules.Location = new System.Drawing.Point(0, 0);
         layoutControlItemMolecules.Name = "layoutControlItemMolecules";
         layoutControlItemMolecules.Size = new System.Drawing.Size(153, 389);
         layoutControlItemMolecules.TextSize = new System.Drawing.Size(0, 0);
         layoutControlItemMolecules.TextVisible = false;
         // 
         // EditMoleculeCalculationMethodsView
         // 
         AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         Controls.Add(uxLayoutControl1);
         Name = "EditMoleculeCalculationMethodsView";
         Size = new System.Drawing.Size(638, 409);
         ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
         ((System.ComponentModel.ISupportInitialize)uxLayoutControl1).EndInit();
         uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)treeViewPanel).EndInit();
         ((System.ComponentModel.ISupportInitialize)calculationMethodGrid).EndInit();
         ((System.ComponentModel.ISupportInitialize)gridViewCalculationMethods).EndInit();
         ((System.ComponentModel.ISupportInitialize)Root).EndInit();
         ((System.ComponentModel.ISupportInitialize)this.layoutControlItemCalculationMethods).EndInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItemMolecules).EndInit();
         ResumeLayout(false);
      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private OSPSuite.UI.Controls.UxGridControl calculationMethodGrid;
      private OSPSuite.UI.Controls.UxGridView gridViewCalculationMethods;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCalculationMethods;
      private OSPSuite.UI.Controls.UxPanelControl treeViewPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMolecules;
   }
}
