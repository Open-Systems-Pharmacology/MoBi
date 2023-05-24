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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnLoadFromDatabase = new OSPSuite.UI.Controls.UxSimpleButton();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.tbPKSimVersion = new DevExpress.XtraEditors.TextEdit();
         this.tbCategory = new DevExpress.XtraEditors.TextEdit();
         this.tbMoleculeName = new DevExpress.XtraEditors.TextEdit();
         this.tbSpecies = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.speciesControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.moleculeControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.categoryControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.pkSimVersionControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbPKSimVersion.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.speciesControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.moleculeControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.categoryControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pkSimVersionControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.btnLoadFromDatabase);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Controls.Add(this.tbPKSimVersion);
         this.layoutControl.Controls.Add(this.tbCategory);
         this.layoutControl.Controls.Add(this.tbMoleculeName);
         this.layoutControl.Controls.Add(this.tbSpecies);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(1324, 588);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "uxLayoutControl1";
         // 
         // btnLoadFromDatabase
         // 
         this.btnLoadFromDatabase.Location = new System.Drawing.Point(1164, 36);
         this.btnLoadFromDatabase.Manager = null;
         this.btnLoadFromDatabase.MaximumSize = new System.Drawing.Size(150, 0);
         this.btnLoadFromDatabase.Name = "btnLoadFromDatabase";
         this.btnLoadFromDatabase.Shortcut = System.Windows.Forms.Keys.None;
         this.btnLoadFromDatabase.Size = new System.Drawing.Size(148, 22);
         this.btnLoadFromDatabase.StyleController = this.layoutControl;
         this.btnLoadFromDatabase.TabIndex = 9;
         this.btnLoadFromDatabase.Text = "btnLoadFromDatabase";
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(12, 110);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1300, 466);
         this.gridControl.TabIndex = 8;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
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
         // tbPKSimVersion
         // 
         this.tbPKSimVersion.Location = new System.Drawing.Point(143, 86);
         this.tbPKSimVersion.Name = "tbPKSimVersion";
         this.tbPKSimVersion.Size = new System.Drawing.Size(1169, 20);
         this.tbPKSimVersion.StyleController = this.layoutControl;
         this.tbPKSimVersion.TabIndex = 7;
         // 
         // tbCategory
         // 
         this.tbCategory.Location = new System.Drawing.Point(143, 62);
         this.tbCategory.Name = "tbCategory";
         this.tbCategory.Size = new System.Drawing.Size(1169, 20);
         this.tbCategory.StyleController = this.layoutControl;
         this.tbCategory.TabIndex = 6;
         // 
         // tbMoleculeName
         // 
         this.tbMoleculeName.Location = new System.Drawing.Point(143, 36);
         this.tbMoleculeName.Name = "tbMoleculeName";
         this.tbMoleculeName.Size = new System.Drawing.Size(1017, 20);
         this.tbMoleculeName.StyleController = this.layoutControl;
         this.tbMoleculeName.TabIndex = 5;
         // 
         // tbSpecies
         // 
         this.tbSpecies.Location = new System.Drawing.Point(143, 12);
         this.tbSpecies.Name = "tbSpecies";
         this.tbSpecies.Size = new System.Drawing.Size(1169, 20);
         this.tbSpecies.StyleController = this.layoutControl;
         this.tbSpecies.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.speciesControlItem,
            this.moleculeControlItem,
            this.categoryControlItem,
            this.pkSimVersionControlItem,
            this.layoutControlItem5,
            this.layoutControlItem6});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1324, 588);
         this.Root.TextVisible = false;
         // 
         // speciesControlItem
         // 
         this.speciesControlItem.Control = this.tbSpecies;
         this.speciesControlItem.Location = new System.Drawing.Point(0, 0);
         this.speciesControlItem.Name = "speciesControlItem";
         this.speciesControlItem.Size = new System.Drawing.Size(1304, 24);
         this.speciesControlItem.TextSize = new System.Drawing.Size(119, 13);
         // 
         // moleculeControlItem
         // 
         this.moleculeControlItem.Control = this.tbMoleculeName;
         this.moleculeControlItem.Location = new System.Drawing.Point(0, 24);
         this.moleculeControlItem.Name = "moleculeControlItem";
         this.moleculeControlItem.Size = new System.Drawing.Size(1152, 26);
         this.moleculeControlItem.TextSize = new System.Drawing.Size(119, 13);
         // 
         // categoryControlItem
         // 
         this.categoryControlItem.Control = this.tbCategory;
         this.categoryControlItem.Location = new System.Drawing.Point(0, 50);
         this.categoryControlItem.Name = "categoryControlItem";
         this.categoryControlItem.Size = new System.Drawing.Size(1304, 24);
         this.categoryControlItem.TextSize = new System.Drawing.Size(119, 13);
         // 
         // pkSimVersionControlItem
         // 
         this.pkSimVersionControlItem.Control = this.tbPKSimVersion;
         this.pkSimVersionControlItem.Location = new System.Drawing.Point(0, 74);
         this.pkSimVersionControlItem.Name = "pkSimVersionControlItem";
         this.pkSimVersionControlItem.Size = new System.Drawing.Size(1304, 24);
         this.pkSimVersionControlItem.TextSize = new System.Drawing.Size(119, 13);
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.gridControl;
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 98);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(1304, 470);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // layoutControlItem6
         // 
         this.layoutControlItem6.Control = this.btnLoadFromDatabase;
         this.layoutControlItem6.Location = new System.Drawing.Point(1152, 24);
         this.layoutControlItem6.Name = "layoutControlItem6";
         this.layoutControlItem6.Size = new System.Drawing.Size(152, 26);
         this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem6.TextVisible = false;
         // 
         // ExpressionProfileBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ExpressionProfileBuildingBlockView";
         this.Size = new System.Drawing.Size(1324, 588);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbPKSimVersion.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.speciesControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.moleculeControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.categoryControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pkSimVersionControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private OSPSuite.UI.Controls.UxSimpleButton btnLoadFromDatabase;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private OSPSuite.UI.Controls.UxGridView gridView;
      private DevExpress.XtraEditors.TextEdit tbPKSimVersion;
      private DevExpress.XtraEditors.TextEdit tbCategory;
      private DevExpress.XtraEditors.TextEdit tbMoleculeName;
      private DevExpress.XtraEditors.TextEdit tbSpecies;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem speciesControlItem;
      private DevExpress.XtraLayout.LayoutControlItem moleculeControlItem;
      private DevExpress.XtraLayout.LayoutControlItem categoryControlItem;
      private DevExpress.XtraLayout.LayoutControlItem pkSimVersionControlItem;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
   }
}
