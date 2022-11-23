namespace MoBi.UI.Views
{
   partial class ExpressionProfileBuildingBlockView
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
         this.uxGridView1 = new OSPSuite.UI.Controls.UxGridView();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.uxGridView2 = new OSPSuite.UI.Controls.UxGridView();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         this.tbCategory = new DevExpress.XtraEditors.TextEdit();
         this.tbMoleculeName = new DevExpress.XtraEditors.TextEdit();
         this.tbSpecies = new DevExpress.XtraEditors.TextEdit();
         this.lblSpecies = new DevExpress.XtraEditors.LabelControl();
         this.lblMoleculeName = new DevExpress.XtraEditors.LabelControl();
         this.lblCategory = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // uxGridView1
         // 
         this.uxGridView1.AllowsFiltering = true;
         this.uxGridView1.EnableColumnContextMenu = true;
         this.uxGridView1.GridControl = this.gridControl;
         this.uxGridView1.MultiSelect = true;
         this.uxGridView1.Name = "uxGridView1";
         this.uxGridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.uxGridView1.OptionsNavigation.AutoFocusNewRow = true;
         this.uxGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.uxGridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.uxGridView1.OptionsSelection.MultiSelect = true;
         this.uxGridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // expressionParametersGridControl
         // 
         this.tablePanel.SetColumn(this.gridControl, 0);
         this.tablePanel.SetColumnSpan(this.gridControl, 2);
         this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl.Location = new System.Drawing.Point(3, 81);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.tablePanel.SetRow(this.gridControl, 3);
         this.gridControl.Size = new System.Drawing.Size(831, 504);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // uxGridView2
         // 
         this.uxGridView2.AllowsFiltering = true;
         this.uxGridView2.EnableColumnContextMenu = true;
         this.uxGridView2.GridControl = this.gridControl;
         this.uxGridView2.MultiSelect = true;
         this.uxGridView2.Name = "uxGridView2";
         this.uxGridView2.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.uxGridView2.OptionsNavigation.AutoFocusNewRow = true;
         this.uxGridView2.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.uxGridView2.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.uxGridView2.OptionsSelection.MultiSelect = true;
         this.uxGridView2.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = true;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsSelection.MultiSelect = true;
         this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 55F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 50F)});
         this.tablePanel.Controls.Add(this.lblCategory);
         this.tablePanel.Controls.Add(this.lblMoleculeName);
         this.tablePanel.Controls.Add(this.lblSpecies);
         this.tablePanel.Controls.Add(this.tbSpecies);
         this.tablePanel.Controls.Add(this.tbMoleculeName);
         this.tablePanel.Controls.Add(this.tbCategory);
         this.tablePanel.Controls.Add(this.gridControl);
         this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tablePanel.Location = new System.Drawing.Point(0, 0);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(837, 588);
         this.tablePanel.TabIndex = 1;
         // 
         // tbCategory
         // 
         this.tablePanel.SetColumn(this.tbCategory, 1);
         this.tbCategory.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbCategory.Location = new System.Drawing.Point(87, 55);
         this.tbCategory.Name = "tbCategory";
         this.tablePanel.SetRow(this.tbCategory, 2);
         this.tbCategory.Size = new System.Drawing.Size(747, 20);
         this.tbCategory.TabIndex = 1;
         // 
         // tbMoleculeName
         // 
         this.tablePanel.SetColumn(this.tbMoleculeName, 1);
         this.tbMoleculeName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbMoleculeName.Location = new System.Drawing.Point(87, 29);
         this.tbMoleculeName.Name = "tbMoleculeName";
         this.tablePanel.SetRow(this.tbMoleculeName, 1);
         this.tbMoleculeName.Size = new System.Drawing.Size(747, 20);
         this.tbMoleculeName.TabIndex = 2;
         // 
         // tbSpecies
         // 
         this.tablePanel.SetColumn(this.tbSpecies, 1);
         this.tbSpecies.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbSpecies.Location = new System.Drawing.Point(87, 3);
         this.tbSpecies.Name = "tbSpecies";
         this.tablePanel.SetRow(this.tbSpecies, 0);
         this.tbSpecies.Size = new System.Drawing.Size(747, 20);
         this.tbSpecies.TabIndex = 3;
         // 
         // lblSpecies
         // 
         this.tablePanel.SetColumn(this.lblSpecies, 0);
         this.lblSpecies.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblSpecies.Location = new System.Drawing.Point(3, 3);
         this.lblSpecies.Name = "lblSpecies";
         this.tablePanel.SetRow(this.lblSpecies, 0);
         this.lblSpecies.Size = new System.Drawing.Size(78, 20);
         this.lblSpecies.TabIndex = 4;
         this.lblSpecies.Text = "lblSpecies";
         // 
         // lblMoleculeName
         // 
         this.tablePanel.SetColumn(this.lblMoleculeName, 0);
         this.lblMoleculeName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblMoleculeName.Location = new System.Drawing.Point(3, 29);
         this.lblMoleculeName.Name = "lblMoleculeName";
         this.tablePanel.SetRow(this.lblMoleculeName, 1);
         this.lblMoleculeName.Size = new System.Drawing.Size(78, 20);
         this.lblMoleculeName.TabIndex = 5;
         this.lblMoleculeName.Text = "lblMoleculeName";
         // 
         // lblCategory
         // 
         this.tablePanel.SetColumn(this.lblCategory, 0);
         this.lblCategory.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblCategory.Location = new System.Drawing.Point(3, 55);
         this.lblCategory.Name = "lblCategory";
         this.tablePanel.SetRow(this.lblCategory, 2);
         this.lblCategory.Size = new System.Drawing.Size(78, 20);
         this.lblCategory.TabIndex = 6;
         this.lblCategory.Text = "lblCategory";
         // 
         // ExpressionProfileBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tablePanel);
         this.Name = "ExpressionProfileBuildingBlockView";
         this.Size = new System.Drawing.Size(837, 588);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.tablePanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).EndInit();
         this.ResumeLayout(false);

      }

        #endregion

        private OSPSuite.UI.Controls.UxGridView uxGridView1;
        private OSPSuite.UI.Controls.UxGridControl gridControl;
        private DevExpress.Utils.Layout.TablePanel tablePanel;
        private DevExpress.XtraEditors.TextEdit tbSpecies;
        private DevExpress.XtraEditors.TextEdit tbMoleculeName;
        private DevExpress.XtraEditors.TextEdit tbCategory;
        private OSPSuite.UI.Controls.UxGridView gridView;
        private OSPSuite.UI.Controls.UxGridView uxGridView2;
        private DevExpress.XtraEditors.LabelControl lblCategory;
        private DevExpress.XtraEditors.LabelControl lblMoleculeName;
        private DevExpress.XtraEditors.LabelControl lblSpecies;
    }
}
