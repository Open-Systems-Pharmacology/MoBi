namespace MoBi.UI.Views
{
   partial class SelectFolderAndIndividualFromProjectView
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

         disposeBinders();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnSelectFilePath = new DevExpress.XtraEditors.ButtonEdit();
         this.cmbSelectIndividual = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.descriptionLabel = new DevExpress.XtraEditors.LabelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItemSelectIndividual = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSelectFilePath = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.btnSelectFilePath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSelectIndividual.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectFilePath)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 115);
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.btnSelectFilePath);
         this.uxLayoutControl1.Controls.Add(this.cmbSelectIndividual);
         this.uxLayoutControl1.Controls.Add(this.descriptionLabel);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(580, 115);
         this.uxLayoutControl1.TabIndex = 39;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // btnSelectFilePath
         // 
         this.btnSelectFilePath.Location = new System.Drawing.Point(186, 36);
         this.btnSelectFilePath.Name = "btnSelectFilePath";
         this.btnSelectFilePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btnSelectFilePath.Size = new System.Drawing.Size(382, 20);
         this.btnSelectFilePath.StyleController = this.uxLayoutControl1;
         this.btnSelectFilePath.TabIndex = 10;
         this.btnSelectFilePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnSelectFilePathClick);
         // 
         // cmbSelectIndividual
         // 
         this.cmbSelectIndividual.Location = new System.Drawing.Point(186, 12);
         this.cmbSelectIndividual.Name = "cmbSelectIndividual";
         this.cmbSelectIndividual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbSelectIndividual.Size = new System.Drawing.Size(382, 20);
         this.cmbSelectIndividual.StyleController = this.uxLayoutControl1;
         this.cmbSelectIndividual.TabIndex = 8;
         // 
         // descriptionLabel
         // 
         this.descriptionLabel.AllowHtmlString = true;
         this.descriptionLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this.descriptionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.descriptionLabel.Location = new System.Drawing.Point(12, 60);
         this.descriptionLabel.Name = "descriptionLabel";
         this.descriptionLabel.Size = new System.Drawing.Size(556, 13);
         this.descriptionLabel.StyleController = this.uxLayoutControl1;
         this.descriptionLabel.TabIndex = 9;
         this.descriptionLabel.Text = "containerExportDescription";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItemSelectIndividual,
            this.layoutControlItemSelectFilePath});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(580, 115);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.descriptionLabel;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(560, 17);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 65);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(560, 30);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItemSelectIndividual
         // 
         this.layoutControlItemSelectIndividual.Control = this.cmbSelectIndividual;
         this.layoutControlItemSelectIndividual.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemSelectIndividual.Name = "layoutControlItemSelectIndividual";
         this.layoutControlItemSelectIndividual.Size = new System.Drawing.Size(560, 24);
         this.layoutControlItemSelectIndividual.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutControlItemSelectFilePath
         // 
         this.layoutControlItemSelectFilePath.Control = this.btnSelectFilePath;
         this.layoutControlItemSelectFilePath.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemSelectFilePath.Name = "layoutControlItemSelectFilePath";
         this.layoutControlItemSelectFilePath.Size = new System.Drawing.Size(560, 24);
         this.layoutControlItemSelectFilePath.TextSize = new System.Drawing.Size(162, 13);
         // 
         // SelectIndividualFromProjectView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SelectExportSettings";
         this.ClientSize = new System.Drawing.Size(580, 158);
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "SelectFolderAndIndividualFromProjectView";
         this.Text = "SelectExportSettings";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.uxLayoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.btnSelectFilePath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSelectIndividual.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectFilePath)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.LabelControl descriptionLabel;
      private OSPSuite.UI.Controls.UxComboBoxEdit cmbSelectIndividual;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelectIndividual;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.ButtonEdit btnSelectFilePath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelectFilePath;
   }
}