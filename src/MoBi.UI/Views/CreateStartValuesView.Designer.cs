namespace MoBi.UI.Views
{
   partial class CreateStartValuesView
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
         this.cmbSpatialStructure = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlItemSpatialStructure = new DevExpress.XtraLayout.LayoutControlItem();
         this.cmbMolecules = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlItemMolecules = new DevExpress.XtraLayout.LayoutControlItem();
         this.txtName = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSpatialStructure.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSpatialStructure)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbMolecules.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.txtName);
         this.layoutControl1.Controls.Add(this.cmbMolecules);
         this.layoutControl1.Controls.Add(this.cmbSpatialStructure);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(457, 277);
         this.layoutControl1.TabIndex = 1;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemSpatialStructure,
            this.layoutControlItemMolecules,
            this.layoutControlItemName});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(457, 277);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // cmbSpatialStructure
         // 
         this.cmbSpatialStructure.Location = new System.Drawing.Point(180, 60);
         this.cmbSpatialStructure.Name = "cmbSpatialStructure";
         this.cmbSpatialStructure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbSpatialStructure.Size = new System.Drawing.Size(265, 20);
         this.cmbSpatialStructure.StyleController = this.layoutControl1;
         this.cmbSpatialStructure.TabIndex = 4;
         // 
         // layoutControlItemSpatialStructure
         // 
         this.layoutControlItemSpatialStructure.Control = this.cmbSpatialStructure;
         this.layoutControlItemSpatialStructure.CustomizationFormText = "layoutControlItemSpatialStructure";
         this.layoutControlItemSpatialStructure.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItemSpatialStructure.Name = "layoutControlItemSpatialStructure";
         this.layoutControlItemSpatialStructure.Size = new System.Drawing.Size(437, 209);
         this.layoutControlItemSpatialStructure.Text = "layoutControlItemSpatialStructure";
         this.layoutControlItemSpatialStructure.TextSize = new System.Drawing.Size(164, 13);
         // 
         // cmbMolecules
         // 
         this.cmbMolecules.Location = new System.Drawing.Point(180, 36);
         this.cmbMolecules.Name = "cmbMolecules";
         this.cmbMolecules.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbMolecules.Size = new System.Drawing.Size(265, 20);
         this.cmbMolecules.StyleController = this.layoutControl1;
         this.cmbMolecules.TabIndex = 5;
         // 
         // layoutControlItemMolecules
         // 
         this.layoutControlItemMolecules.Control = this.cmbMolecules;
         this.layoutControlItemMolecules.CustomizationFormText = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemMolecules.Name = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.Size = new System.Drawing.Size(437, 24);
         this.layoutControlItemMolecules.Text = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.TextSize = new System.Drawing.Size(164, 13);
         // 
         // txtName
         // 
         this.txtName.Location = new System.Drawing.Point(180, 12);
         this.txtName.Name = "txtName";
         this.txtName.Size = new System.Drawing.Size(265, 20);
         this.txtName.StyleController = this.layoutControl1;
         this.txtName.TabIndex = 6;
         // 
         // layoutControlItemName
         // 
         this.layoutControlItemName.Control = this.txtName;
         this.layoutControlItemName.CustomizationFormText = "layoutControlItemName";
         this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemName.Name = "layoutControlItemName";
         this.layoutControlItemName.Size = new System.Drawing.Size(437, 24);
         this.layoutControlItemName.Text = "layoutControlItemName";
         this.layoutControlItemName.TextSize = new System.Drawing.Size(164, 13);
         // 
         // CreateStartValuesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(457, 323);
         this.Controls.Add(this.layoutControl1);
         this.Name = "CreateStartValuesView";
         this.Text = "CreateStartValueView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSpatialStructure.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSpatialStructure)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbMolecules.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.TextEdit txtName;
      private DevExpress.XtraEditors.ComboBoxEdit cmbMolecules;
      private DevExpress.XtraEditors.ComboBoxEdit cmbSpatialStructure;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSpatialStructure;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMolecules;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
   }
}