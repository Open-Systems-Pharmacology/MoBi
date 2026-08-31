namespace MoBi.UI.Views
{
   partial class SelectCommitTargetView
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.descriptionLabel = new DevExpress.XtraEditors.LabelControl();
         this.cmbModule = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.cmbParameterValues = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemModule = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemParameterValues = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cmbModule.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbParameterValues.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemModule)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameterValues)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         this.SuspendLayout();
         //
         // tablePanel
         //
         this.tablePanel.Location = new System.Drawing.Point(0, 157);
         this.tablePanel.Size = new System.Drawing.Size(584, 43);
         //
         // layoutControl
         //
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.descriptionLabel);
         this.layoutControl.Controls.Add(this.cmbModule);
         this.layoutControl.Controls.Add(this.cmbParameterValues);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(584, 157);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl";
         //
         // descriptionLabel
         //
         this.descriptionLabel.AllowHtmlString = true;
         this.descriptionLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this.descriptionLabel.Location = new System.Drawing.Point(12, 12);
         this.descriptionLabel.Name = "descriptionLabel";
         this.descriptionLabel.Size = new System.Drawing.Size(560, 13);
         this.descriptionLabel.StyleController = this.layoutControl;
         this.descriptionLabel.TabIndex = 4;
         //
         // cmbModule
         //
         this.cmbModule.Location = new System.Drawing.Point(180, 29);
         this.cmbModule.Name = "cmbModule";
         this.cmbModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbModule.Size = new System.Drawing.Size(392, 20);
         this.cmbModule.StyleController = this.layoutControl;
         this.cmbModule.TabIndex = 5;
         //
         // cmbParameterValues
         //
         this.cmbParameterValues.Location = new System.Drawing.Point(180, 53);
         this.cmbParameterValues.Name = "cmbParameterValues";
         this.cmbParameterValues.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbParameterValues.Size = new System.Drawing.Size(392, 20);
         this.cmbParameterValues.StyleController = this.layoutControl;
         this.cmbParameterValues.TabIndex = 6;
         //
         // layoutControlGroup
         //
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDescription,
            this.layoutItemModule,
            this.layoutItemParameterValues,
            this.emptySpaceItem});
         this.layoutControlGroup.Name = "Root";
         this.layoutControlGroup.Size = new System.Drawing.Size(584, 157);
         this.layoutControlGroup.TextVisible = false;
         //
         // layoutItemDescription
         //
         this.layoutItemDescription.Control = this.descriptionLabel;
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 0);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(564, 17);
         this.layoutItemDescription.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDescription.TextVisible = false;
         //
         // layoutItemModule
         //
         this.layoutItemModule.Control = this.cmbModule;
         this.layoutItemModule.Location = new System.Drawing.Point(0, 17);
         this.layoutItemModule.Name = "layoutItemModule";
         this.layoutItemModule.Size = new System.Drawing.Size(564, 24);
         this.layoutItemModule.TextSize = new System.Drawing.Size(164, 13);
         //
         // layoutItemParameterValues
         //
         this.layoutItemParameterValues.Control = this.cmbParameterValues;
         this.layoutItemParameterValues.Location = new System.Drawing.Point(0, 41);
         this.layoutItemParameterValues.Name = "layoutItemParameterValues";
         this.layoutItemParameterValues.Size = new System.Drawing.Size(564, 24);
         this.layoutItemParameterValues.TextSize = new System.Drawing.Size(164, 13);
         //
         // emptySpaceItem
         //
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(0, 65);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(564, 72);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         //
         // SelectCommitTargetView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SelectCommitTargetView";
         this.ClientSize = new System.Drawing.Size(584, 200);
         this.Name = "SelectCommitTargetView";
         this.Text = "SelectCommitTargetView";
         this.Controls.Add(this.layoutControl);
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cmbModule.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbParameterValues.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemModule)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameterValues)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraEditors.LabelControl descriptionLabel;
      private OSPSuite.UI.Controls.UxComboBoxEdit cmbModule;
      private OSPSuite.UI.Controls.UxComboBoxEdit cmbParameterValues;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemModule;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameterValues;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
   }
}
