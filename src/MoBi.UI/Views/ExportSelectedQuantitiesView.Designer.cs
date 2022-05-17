namespace MoBi.UI.Views
{
   partial class ExportSelectedQuantitiesView
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
         this.btnSelectReportPath = new DevExpress.XtraEditors.ButtonEdit();
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemPanel = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSelectionPath = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.btnSelectReportPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectionPath)).BeginInit();
         this.SuspendLayout();
        
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.btnSelectReportPath);
         this.layoutControl1.Controls.Add(this.panelControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControl;
         this.layoutControl1.Size = new System.Drawing.Size(499, 416);
         this.layoutControl1.TabIndex = 38;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // btnSelectReportPath
         // 
         this.btnSelectReportPath.Location = new System.Drawing.Point(132, 12);
         this.btnSelectReportPath.Name = "btnSelectReportPath";
         this.btnSelectReportPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btnSelectReportPath.Size = new System.Drawing.Size(355, 20);
         this.btnSelectReportPath.StyleController = this.layoutControl1;
         this.btnSelectReportPath.TabIndex = 0;
         // 
         // panelControl
         // 
         this.panelControl.Location = new System.Drawing.Point(12, 36);
         this.panelControl.Name = "panelControl";
         this.panelControl.Size = new System.Drawing.Size(475, 368);
         this.panelControl.TabIndex = 4;
         // 
         // layoutControl
         // 
         this.layoutControl.CustomizationFormText = "layoutControl";
         this.layoutControl.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControl.GroupBordersVisible = false;
         this.layoutControl.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemPanel,
            this.layoutItemSelectionPath});
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Size = new System.Drawing.Size(499, 416);
         this.layoutControl.Text = "layoutControl";
         this.layoutControl.TextVisible = false;
         // 
         // layoutItemPanel
         // 
         this.layoutItemPanel.Control = this.panelControl;
         this.layoutItemPanel.CustomizationFormText = "layoutItemPanel";
         this.layoutItemPanel.Location = new System.Drawing.Point(0, 24);
         this.layoutItemPanel.Name = "layoutItemPanel";
         this.layoutItemPanel.Size = new System.Drawing.Size(479, 372);
         this.layoutItemPanel.Text = "layoutItemPanel";
         this.layoutItemPanel.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemPanel.TextToControlDistance = 0;
         this.layoutItemPanel.TextVisible = false;
         // 
         // layoutItemSelectionPath
         // 
         this.layoutItemSelectionPath.Control = this.btnSelectReportPath;
         this.layoutItemSelectionPath.CustomizationFormText = "layoutItemSelectionPath";
         this.layoutItemSelectionPath.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSelectionPath.Name = "layoutItemSelectionPath";
         this.layoutItemSelectionPath.Size = new System.Drawing.Size(479, 24);
         this.layoutItemSelectionPath.Text = "layoutItemSelectionPath";
         this.layoutItemSelectionPath.TextSize = new System.Drawing.Size(117, 13);
         // 
         // ExportSelectedQuantitiesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ExportSelectedQuantitiesView";
         this.ClientSize = new System.Drawing.Size(499, 462);
         this.Controls.Add(this.layoutControl1);
         this.Name = "ExportSelectedQuantitiesView";
         this.Text = "ExportSelectedQuantitiesView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.btnSelectReportPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectionPath)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControl;
      private DevExpress.XtraEditors.PanelControl panelControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPanel;
      private DevExpress.XtraEditors.ButtonEdit btnSelectReportPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSelectionPath;
   }
}