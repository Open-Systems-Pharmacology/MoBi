namespace MoBi.BatchTool.Views
{
   partial class BatchMainView
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
         this.btnLoadPkmlFilesFromFolder = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnLoadProjectFilesFromFolder = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnGenerateProjectOverview = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // btnLoadPkmlFilesFromFolder
         // 
         this.btnLoadPkmlFilesFromFolder.Location = new System.Drawing.Point(12, 12);
         this.btnLoadPkmlFilesFromFolder.Name = "btnLoadPkmlFilesFromFolder";
         this.btnLoadPkmlFilesFromFolder.Size = new System.Drawing.Size(458, 22);
         this.btnLoadPkmlFilesFromFolder.StyleController = this.layoutControl1;
         this.btnLoadPkmlFilesFromFolder.TabIndex = 0;
         this.btnLoadPkmlFilesFromFolder.Text = "btnLoadPkmlFilesFromFolder";
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.btnGenerateProjectOverview);
         this.layoutControl1.Controls.Add(this.btnLoadProjectFilesFromFolder);
         this.layoutControl1.Controls.Add(this.btnLoadPkmlFilesFromFolder);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(482, 383);
         this.layoutControl1.TabIndex = 1;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // btnLoadProjectFilesFromFolder
         // 
         this.btnLoadProjectFilesFromFolder.Location = new System.Drawing.Point(12, 38);
         this.btnLoadProjectFilesFromFolder.Name = "btnLoadProjectFilesFromFolder";
         this.btnLoadProjectFilesFromFolder.Size = new System.Drawing.Size(458, 22);
         this.btnLoadProjectFilesFromFolder.StyleController = this.layoutControl1;
         this.btnLoadProjectFilesFromFolder.TabIndex = 4;
         this.btnLoadProjectFilesFromFolder.Text = "btnLoadProjectFilesFromFolder";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(482, 383);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.btnLoadPkmlFilesFromFolder;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(462, 26);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.btnLoadProjectFilesFromFolder;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(462, 26);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // btnGenerateProjectOverview
         // 
         this.btnGenerateProjectOverview.Location = new System.Drawing.Point(12, 64);
         this.btnGenerateProjectOverview.Name = "btnGenerateProjectOverview";
         this.btnGenerateProjectOverview.Size = new System.Drawing.Size(458, 22);
         this.btnGenerateProjectOverview.StyleController = this.layoutControl1;
         this.btnGenerateProjectOverview.TabIndex = 5;
         this.btnGenerateProjectOverview.Text = "btnGenerateProjectOverview";
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.btnGenerateProjectOverview;
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 52);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(462, 311);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // BatchMainView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "BatchMainView";
         this.ClientSize = new System.Drawing.Size(482, 383);
         this.Controls.Add(this.layoutControl1);
         this.Name = "BatchMainView";
         this.Text = "BatchMainView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnLoadPkmlFilesFromFolder;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.SimpleButton btnLoadProjectFilesFromFolder;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.SimpleButton btnGenerateProjectOverview;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
   }
}