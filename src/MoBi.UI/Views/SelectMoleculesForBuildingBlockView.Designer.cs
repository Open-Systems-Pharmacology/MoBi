namespace MoBi.UI.Views
{
   partial class SelectMoleculesForBuildingBlockView
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
         _screenBinder.Dispose();
         _gridBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.gridControlMolecules = new OSPSuite.UI.Controls.UxGridControl();
         this.gridMolecules = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.layoutControlItemMolecules = new DevExpress.XtraLayout.LayoutControlItem();
         this.txtName = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.txtName);
         this.layoutControl1.Controls.Add(this.gridControlMolecules);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(448, 347);
         this.layoutControl1.TabIndex = 1;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemMolecules,
            this.layoutControlItemName});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(448, 347);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // gridControlMolecules
         // 
         this.gridControlMolecules.Location = new System.Drawing.Point(12, 52);
         this.gridControlMolecules.MainView = this.gridMolecules;
         this.gridControlMolecules.Name = "gridControlMolecules";
         this.gridControlMolecules.Size = new System.Drawing.Size(424, 283);
         this.gridControlMolecules.TabIndex = 4;
         this.gridControlMolecules.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridMolecules,
            this.gridView2});
         // 
         // gridMolecules
         // 
         this.gridMolecules.GridControl = this.gridControlMolecules;
         this.gridMolecules.Name = "gridMolecules";
         // 
         // gridView2
         // 
         this.gridView2.GridControl = this.gridControlMolecules;
         this.gridView2.Name = "gridView2";
         // 
         // layoutControlItemMolecules
         // 
         this.layoutControlItemMolecules.Control = this.gridControlMolecules;
         this.layoutControlItemMolecules.CustomizationFormText = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemMolecules.Name = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.Size = new System.Drawing.Size(428, 303);
         this.layoutControlItemMolecules.Text = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemMolecules.TextSize = new System.Drawing.Size(133, 13);
         // 
         // txtName
         // 
         this.txtName.Location = new System.Drawing.Point(149, 12);
         this.txtName.Name = "txtName";
         this.txtName.Size = new System.Drawing.Size(287, 20);
         this.txtName.StyleController = this.layoutControl1;
         this.txtName.TabIndex = 5;
         // 
         // layoutControlItemName
         // 
         this.layoutControlItemName.Control = this.txtName;
         this.layoutControlItemName.CustomizationFormText = "layoutControlItemName";
         this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemName.Name = "layoutControlItemName";
         this.layoutControlItemName.Size = new System.Drawing.Size(428, 24);
         this.layoutControlItemName.Text = "layoutControlItemName";
         this.layoutControlItemName.TextSize = new System.Drawing.Size(133, 13);
         // 
         // SelectMoleculesForBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(448, 393);
         this.Controls.Add(this.layoutControl1);
         this.Name = "SelectMoleculesForBuildingBlockView";
         this.Text = "SelectMoleculesForBuildingBlockView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.TextEdit txtName;
      private DevExpress.XtraGrid.GridControl gridControlMolecules;
      private DevExpress.XtraGrid.Views.Grid.GridView gridMolecules;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMolecules;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
   }
}