namespace MoBi.UI.Views
{
   partial class SelectSpatialStructureAndMoleculesView
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
         this.descriptionLabel = new DevExpress.XtraEditors.LabelControl();
         this.cmbMolecules = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.cmbSpatialStructure = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemMolecules = new DevExpress.XtraLayout.LayoutControlItem();
         this.descriptionLayoutControl = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSpatialStructure = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cmbMolecules.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSpatialStructure.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionLayoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSpatialStructure)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 98);
         this.tablePanel.Size = new System.Drawing.Size(457, 43);
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.descriptionLabel);
         this.layoutControl1.Controls.Add(this.cmbMolecules);
         this.layoutControl1.Controls.Add(this.cmbSpatialStructure);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1186, 175, 650, 400);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(457, 98);
         this.layoutControl1.TabIndex = 1;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // descriptionLabel
         // 
         this.descriptionLabel.AllowHtmlString = true;
         this.descriptionLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this.descriptionLabel.Location = new System.Drawing.Point(12, 60);
         this.descriptionLabel.Name = "descriptionLabel";
         this.descriptionLabel.Size = new System.Drawing.Size(433, 26);
         this.descriptionLabel.StyleController = this.layoutControl1;
         this.descriptionLabel.TabIndex = 6;
         this.descriptionLabel.Text = "Initial conditions will be created for all physical containers in the spatial str" +
    "ucture for each molecule";
         // 
         // cmbMolecules
         // 
         this.cmbMolecules.Location = new System.Drawing.Point(188, 12);
         this.cmbMolecules.Name = "cmbMolecules";
         this.cmbMolecules.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbMolecules.Size = new System.Drawing.Size(257, 20);
         this.cmbMolecules.StyleController = this.layoutControl1;
         this.cmbMolecules.TabIndex = 5;
         // 
         // cmbSpatialStructure
         // 
         this.cmbSpatialStructure.Location = new System.Drawing.Point(188, 36);
         this.cmbSpatialStructure.Name = "cmbSpatialStructure";
         this.cmbSpatialStructure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbSpatialStructure.Size = new System.Drawing.Size(257, 20);
         this.cmbSpatialStructure.StyleController = this.layoutControl1;
         this.cmbSpatialStructure.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemMolecules,
            this.descriptionLayoutControl,
            this.layoutControlItemSpatialStructure});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(457, 98);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemMolecules
         // 
         this.layoutControlItemMolecules.Control = this.cmbMolecules;
         this.layoutControlItemMolecules.CustomizationFormText = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemMolecules.Name = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.Size = new System.Drawing.Size(437, 24);
         this.layoutControlItemMolecules.TextSize = new System.Drawing.Size(164, 13);
         // 
         // descriptionLayoutControl
         // 
         this.descriptionLayoutControl.Control = this.descriptionLabel;
         this.descriptionLayoutControl.Location = new System.Drawing.Point(0, 48);
         this.descriptionLayoutControl.Name = "descriptionLayoutControl";
         this.descriptionLayoutControl.Size = new System.Drawing.Size(437, 30);
         this.descriptionLayoutControl.TextSize = new System.Drawing.Size(0, 0);
         this.descriptionLayoutControl.TextVisible = false;
         // 
         // layoutControlItemSpatialStructure
         // 
         this.layoutControlItemSpatialStructure.Control = this.cmbSpatialStructure;
         this.layoutControlItemSpatialStructure.CustomizationFormText = "layoutControlItemSpatialStructure";
         this.layoutControlItemSpatialStructure.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemSpatialStructure.Name = "layoutControlItemSpatialStructure";
         this.layoutControlItemSpatialStructure.Size = new System.Drawing.Size(437, 24);
         this.layoutControlItemSpatialStructure.TextSize = new System.Drawing.Size(164, 13);
         // 
         // SelectSpatialStructureAndMoleculesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "CreateStartValueView";
         this.ClientSize = new System.Drawing.Size(457, 141);
         this.Controls.Add(this.layoutControl1);
         this.Name = "SelectSpatialStructureAndMoleculesView";
         this.Text = "CreateStartValueView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cmbMolecules.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSpatialStructure.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionLayoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSpatialStructure)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private OSPSuite.UI.Controls.UxComboBoxEdit cmbMolecules;
      private OSPSuite.UI.Controls.UxComboBoxEdit cmbSpatialStructure;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSpatialStructure;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMolecules;
      private DevExpress.XtraEditors.LabelControl descriptionLabel;
      private DevExpress.XtraLayout.LayoutControlItem descriptionLayoutControl;
   }
}