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
         disposeBinders();
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         this.btnLoadFromDatabase = new OSPSuite.UI.Controls.UxSimpleButton();
         this.lblCategory = new DevExpress.XtraEditors.LabelControl();
         this.lblMoleculeName = new DevExpress.XtraEditors.LabelControl();
         this.lblSpecies = new DevExpress.XtraEditors.LabelControl();
         this.tbSpecies = new DevExpress.XtraEditors.TextEdit();
         this.tbMoleculeName = new DevExpress.XtraEditors.TextEdit();
         this.tbCategory = new DevExpress.XtraEditors.TextEdit();
         this.tbPKSimVersion = new DevExpress.XtraEditors.TextEdit();
         this.lblPKSimVersion = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbPKSimVersion.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.tablePanel.SetColumn(this.gridControl, 0);
         this.tablePanel.SetColumnSpan(this.gridControl, 3);
         this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl.Location = new System.Drawing.Point(3, 107);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.tablePanel.SetRow(this.gridControl, 4);
         this.gridControl.Size = new System.Drawing.Size(831, 478);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.ColumnPanelRowHeight = 0;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.FooterPanelHeight = 0;
         this.gridView.GridControl = this.gridControl;
         this.gridView.GroupRowHeight = 0;
         this.gridView.LevelIndent = 0;
         this.gridView.MultiSelect = true;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsSelection.MultiSelect = true;
         this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         this.gridView.PreviewIndent = 0;
         this.gridView.RowHeight = 0;
         this.gridView.ViewCaptionHeight = 0;
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 55F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 50F)});
         this.tablePanel.Controls.Add(this.lblPKSimVersion);
         this.tablePanel.Controls.Add(this.tbPKSimVersion);
         this.tablePanel.Controls.Add(this.btnLoadFromDatabase);
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
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(837, 588);
         this.tablePanel.TabIndex = 1;
         // 
         // btnLoadFromDatabase
         // 
         this.tablePanel.SetColumn(this.btnLoadFromDatabase, 2);
         this.btnLoadFromDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
         this.btnLoadFromDatabase.Location = new System.Drawing.Point(693, 3);
         this.btnLoadFromDatabase.Manager = null;
         this.btnLoadFromDatabase.MaximumSize = new System.Drawing.Size(141, 0);
         this.btnLoadFromDatabase.MinimumSize = new System.Drawing.Size(141, 0);
         this.btnLoadFromDatabase.Name = "btnLoadFromDatabase";
         this.tablePanel.SetRow(this.btnLoadFromDatabase, 0);
         this.btnLoadFromDatabase.Shortcut = System.Windows.Forms.Keys.None;
         this.btnLoadFromDatabase.Size = new System.Drawing.Size(141, 20);
         this.btnLoadFromDatabase.TabIndex = 9;
         this.btnLoadFromDatabase.Text = "Query Database";
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
         // tbSpecies
         // 
         this.tablePanel.SetColumn(this.tbSpecies, 1);
         this.tbSpecies.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbSpecies.Location = new System.Drawing.Point(87, 3);
         this.tbSpecies.MaximumSize = new System.Drawing.Size(600, 0);
         this.tbSpecies.Name = "tbSpecies";
         this.tablePanel.SetRow(this.tbSpecies, 0);
         this.tbSpecies.Size = new System.Drawing.Size(600, 20);
         this.tbSpecies.TabIndex = 3;
         // 
         // tbMoleculeName
         // 
         this.tablePanel.SetColumn(this.tbMoleculeName, 1);
         this.tablePanel.SetColumnSpan(this.tbMoleculeName, 2);
         this.tbMoleculeName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbMoleculeName.Location = new System.Drawing.Point(87, 29);
         this.tbMoleculeName.MaximumSize = new System.Drawing.Size(747, 0);
         this.tbMoleculeName.Name = "tbMoleculeName";
         this.tablePanel.SetRow(this.tbMoleculeName, 1);
         this.tbMoleculeName.Size = new System.Drawing.Size(747, 20);
         this.tbMoleculeName.TabIndex = 2;
         // 
         // tbCategory
         // 
         this.tablePanel.SetColumn(this.tbCategory, 1);
         this.tablePanel.SetColumnSpan(this.tbCategory, 2);
         this.tbCategory.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbCategory.Location = new System.Drawing.Point(87, 55);
         this.tbCategory.MaximumSize = new System.Drawing.Size(747, 0);
         this.tbCategory.Name = "tbCategory";
         this.tablePanel.SetRow(this.tbCategory, 2);
         this.tbCategory.Size = new System.Drawing.Size(747, 20);
         this.tbCategory.TabIndex = 1;
         // 
         // tbPKSimVersion
         // 
         this.tablePanel.SetColumn(this.tbPKSimVersion, 1);
         this.tablePanel.SetColumnSpan(this.tbPKSimVersion, 2);
         this.tbPKSimVersion.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbPKSimVersion.Location = new System.Drawing.Point(87, 81);
         this.tbPKSimVersion.MaximumSize = new System.Drawing.Size(747, 0);
         this.tbPKSimVersion.Name = "tbPKSimVersion";
         this.tablePanel.SetRow(this.tbPKSimVersion, 3);
         this.tbPKSimVersion.Size = new System.Drawing.Size(747, 20);
         this.tbPKSimVersion.TabIndex = 10;
         // 
         // lblPKSimVersion
         // 
         this.tablePanel.SetColumn(this.lblPKSimVersion, 0);
         this.lblPKSimVersion.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblPKSimVersion.Location = new System.Drawing.Point(3, 81);
         this.lblPKSimVersion.Name = "lblPKSimVersion";
         this.tablePanel.SetRow(this.lblPKSimVersion, 3);
         this.lblPKSimVersion.Size = new System.Drawing.Size(78, 20);
         this.lblPKSimVersion.TabIndex = 11;
         this.lblPKSimVersion.Text = "lblPKSimVersion";
         // 
         // ExpressionProfileBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tablePanel);
         this.Margin = new System.Windows.Forms.Padding(8);
         this.Name = "ExpressionProfileBuildingBlockView";
         this.Size = new System.Drawing.Size(837, 588);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.tablePanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbPKSimVersion.Properties)).EndInit();
         this.ResumeLayout(false);

      }

        #endregion
        private OSPSuite.UI.Controls.UxGridControl gridControl;
        private DevExpress.Utils.Layout.TablePanel tablePanel;
        private DevExpress.XtraEditors.TextEdit tbSpecies;
        private DevExpress.XtraEditors.TextEdit tbMoleculeName;
        private DevExpress.XtraEditors.TextEdit tbCategory;
        private OSPSuite.UI.Controls.UxGridView gridView;
        private DevExpress.XtraEditors.LabelControl lblCategory;
        private DevExpress.XtraEditors.LabelControl lblMoleculeName;
        private DevExpress.XtraEditors.LabelControl lblSpecies;
        private OSPSuite.UI.Controls.UxSimpleButton btnLoadFromDatabase;
      private DevExpress.XtraEditors.TextEdit tbPKSimVersion;
      private DevExpress.XtraEditors.LabelControl lblPKSimVersion;
   }
}
