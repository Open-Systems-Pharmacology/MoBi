namespace MoBi.UI.Views
{
   partial class SelectOrganAndProteinsView
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
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.organSelectionPanel = new DevExpress.XtraEditors.PanelControl();
         this.moleculeSelectionPanel = new DevExpress.XtraEditors.PanelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemMolecules = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemOrgan = new DevExpress.XtraLayout.LayoutControlItem();
         this.descriptionLabel = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.organSelectionPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.moleculeSelectionPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOrgan)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 593);
         this.tablePanel.Size = new System.Drawing.Size(914, 43);
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.descriptionLabel);
         this.uxLayoutControl1.Controls.Add(this.organSelectionPanel);
         this.uxLayoutControl1.Controls.Add(this.moleculeSelectionPanel);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1341, 332, 650, 400);
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(914, 593);
         this.uxLayoutControl1.TabIndex = 39;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // organSelectionPanel
         // 
         this.organSelectionPanel.Location = new System.Drawing.Point(12, 12);
         this.organSelectionPanel.Name = "organSelectionPanel";
         this.organSelectionPanel.Size = new System.Drawing.Size(434, 552);
         this.organSelectionPanel.TabIndex = 5;
         // 
         // moleculeSelectionPanel
         // 
         this.moleculeSelectionPanel.Location = new System.Drawing.Point(450, 12);
         this.moleculeSelectionPanel.Name = "moleculeSelectionPanel";
         this.moleculeSelectionPanel.Size = new System.Drawing.Size(452, 552);
         this.moleculeSelectionPanel.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemMolecules,
            this.layoutControlItemOrgan,
            this.layoutControlItem1});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(914, 593);
         this.Root.TextVisible = false;
         // 
         // layoutControlItemMolecules
         // 
         this.layoutControlItemMolecules.Control = this.moleculeSelectionPanel;
         this.layoutControlItemMolecules.Location = new System.Drawing.Point(438, 0);
         this.layoutControlItemMolecules.Name = "layoutControlItemMolecules";
         this.layoutControlItemMolecules.Size = new System.Drawing.Size(456, 556);
         this.layoutControlItemMolecules.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemMolecules.TextVisible = false;
         // 
         // layoutControlItemOrgan
         // 
         this.layoutControlItemOrgan.Control = this.organSelectionPanel;
         this.layoutControlItemOrgan.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemOrgan.Name = "layoutControlItemOrgan";
         this.layoutControlItemOrgan.Size = new System.Drawing.Size(438, 556);
         this.layoutControlItemOrgan.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemOrgan.TextVisible = false;
         // 
         // descriptionLabel
         // 
         this.descriptionLabel.Location = new System.Drawing.Point(12, 568);
         this.descriptionLabel.Name = "descriptionLabel";
         this.descriptionLabel.Size = new System.Drawing.Size(77, 13);
         this.descriptionLabel.StyleController = this.uxLayoutControl1;
         this.descriptionLabel.TabIndex = 6;
         this.descriptionLabel.Text = "descriptionLabel";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.descriptionLabel;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 556);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(894, 17);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // SelectOrganAndProteinsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(914, 636);
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "SelectOrganAndProteinsView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.uxLayoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.organSelectionPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.moleculeSelectionPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOrgan)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraEditors.PanelControl moleculeSelectionPanel;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMolecules;
      private DevExpress.XtraEditors.PanelControl organSelectionPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemOrgan;
      private DevExpress.XtraEditors.LabelControl descriptionLabel;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}
