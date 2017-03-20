namespace MoBi.BatchTool.Views
{
   partial class FileFromFolderRunnerView
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
         this.panelLog = new DevExpress.XtraEditors.PanelControl();
         this.btnCalculate = new DevExpress.XtraEditors.SimpleButton();
         this.btnExit = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemButtonExit = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonCalculate = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnInputFolder = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutItemInputFolder = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelLog)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonExit)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonCalculate)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnInputFolder.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemInputFolder)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.btnInputFolder);
         this.layoutControl1.Controls.Add(this.panelLog);
         this.layoutControl1.Controls.Add(this.btnCalculate);
         this.layoutControl1.Controls.Add(this.btnExit);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(648, 560);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // panelLog
         // 
         this.panelLog.Location = new System.Drawing.Point(12, 36);
         this.panelLog.Name = "panelLog";
         this.panelLog.Size = new System.Drawing.Size(624, 486);
         this.panelLog.TabIndex = 6;
         // 
         // btnCalculate
         // 
         this.btnCalculate.Location = new System.Drawing.Point(443, 526);
         this.btnCalculate.Name = "btnCalculate";
         this.btnCalculate.Size = new System.Drawing.Size(193, 22);
         this.btnCalculate.StyleController = this.layoutControl1;
         this.btnCalculate.TabIndex = 5;
         this.btnCalculate.Text = "btnCalculate";
         // 
         // btnExit
         // 
         this.btnExit.Location = new System.Drawing.Point(12, 526);
         this.btnExit.Name = "btnExit";
         this.btnExit.Size = new System.Drawing.Size(208, 22);
         this.btnExit.StyleController = this.layoutControl1;
         this.btnExit.TabIndex = 4;
         this.btnExit.Text = "btnExit";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemButtonExit,
            this.layoutControlItem3,
            this.layoutItemInputFolder,
            this.layoutItemButtonCalculate,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(648, 560);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutItemButtonExit.Control = this.btnExit;
         this.layoutItemButtonExit.Location = new System.Drawing.Point(0, 514);
         this.layoutItemButtonExit.Name = "layoutItemButtonExit";
         this.layoutItemButtonExit.Size = new System.Drawing.Size(212, 26);
         this.layoutItemButtonExit.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonExit.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutItemButtonCalculate.Control = this.btnCalculate;
         this.layoutItemButtonCalculate.Location = new System.Drawing.Point(431, 514);
         this.layoutItemButtonCalculate.Name = "layoutItemButtonCalculate";
         this.layoutItemButtonCalculate.Size = new System.Drawing.Size(197, 26);
         this.layoutItemButtonCalculate.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonCalculate.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.panelLog;
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(628, 490);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // btnInputFolder
         // 
         this.btnInputFolder.Location = new System.Drawing.Point(123, 12);
         this.btnInputFolder.Name = "btnInputFolder";
         this.btnInputFolder.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btnInputFolder.Size = new System.Drawing.Size(513, 20);
         this.btnInputFolder.StyleController = this.layoutControl1;
         this.btnInputFolder.TabIndex = 7;
         // 
         // layoutItemInputFolder
         // 
         this.layoutItemInputFolder.Control = this.btnInputFolder;
         this.layoutItemInputFolder.Location = new System.Drawing.Point(0, 0);
         this.layoutItemInputFolder.Name = "layoutItemInputFolder";
         this.layoutItemInputFolder.Size = new System.Drawing.Size(628, 24);
         this.layoutItemInputFolder.TextSize = new System.Drawing.Size(108, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(212, 514);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(219, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // PkmlFileFromFolderRunnerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "PkmlFileFromFolderRunnerView";
         this.ClientSize = new System.Drawing.Size(648, 560);
         this.Controls.Add(this.layoutControl1);
         this.Name = "FileFromFolderRunnerView";
         this.Text = "PkmlFileFromFolderRunnerView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelLog)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonExit)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonCalculate)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnInputFolder.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemInputFolder)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraEditors.SimpleButton btnExit;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonExit;
      private DevExpress.XtraEditors.SimpleButton btnCalculate;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonCalculate;
      private DevExpress.XtraEditors.PanelControl panelLog;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraEditors.ButtonEdit btnInputFolder;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemInputFolder;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}