namespace MoBi.UI.Views
{
   partial class ApplicationSettingsView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.buttonPKSimPath = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutItemPKSimPath = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.chkUseWatermark = new DevExpress.XtraEditors.CheckEdit();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.textWatermark = new DevExpress.XtraEditors.TextEdit();
         this.layoutItemTextWatermark = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonPKSimPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPKSimPath)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUseWatermark.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textWatermark.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTextWatermark)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.textWatermark);
         this.layoutControl.Controls.Add(this.chkUseWatermark);
         this.layoutControl.Controls.Add(this.buttonPKSimPath);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(402, 404);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemPKSimPath,
            this.emptySpaceItem1,
            this.layoutControlItem1,
            this.layoutItemTextWatermark});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(402, 404);
         this.layoutControlGroup.TextVisible = false;
         // 
         // buttonPKSimPath
         // 
         this.buttonPKSimPath.Location = new System.Drawing.Point(143, 12);
         this.buttonPKSimPath.Name = "buttonPKSimPath";
         this.buttonPKSimPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.buttonPKSimPath.Size = new System.Drawing.Size(247, 20);
         this.buttonPKSimPath.StyleController = this.layoutControl;
         this.buttonPKSimPath.TabIndex = 4;
         // 
         // layoutItemPKSimPath
         // 
         this.layoutItemPKSimPath.Control = this.buttonPKSimPath;
         this.layoutItemPKSimPath.Location = new System.Drawing.Point(0, 0);
         this.layoutItemPKSimPath.Name = "layoutItemPKSimPath";
         this.layoutItemPKSimPath.Size = new System.Drawing.Size(382, 24);
         this.layoutItemPKSimPath.TextSize = new System.Drawing.Size(127, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 71);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(382, 313);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // chkUseWatermark
         // 
         this.chkUseWatermark.Location = new System.Drawing.Point(12, 36);
         this.chkUseWatermark.Name = "chkUseWatermark";
         this.chkUseWatermark.Properties.Caption = "chkUseWatermark";
         this.chkUseWatermark.Size = new System.Drawing.Size(378, 19);
         this.chkUseWatermark.StyleController = this.layoutControl;
         this.chkUseWatermark.TabIndex = 5;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkUseWatermark;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(382, 23);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // textWatermark
         // 
         this.textWatermark.Location = new System.Drawing.Point(143, 59);
         this.textWatermark.Name = "textWatermark";
         this.textWatermark.Size = new System.Drawing.Size(247, 20);
         this.textWatermark.StyleController = this.layoutControl;
         this.textWatermark.TabIndex = 6;
         // 
         // layoutItemTextWatermark
         // 
         this.layoutItemTextWatermark.Control = this.textWatermark;
         this.layoutItemTextWatermark.Location = new System.Drawing.Point(0, 47);
         this.layoutItemTextWatermark.Name = "layoutItemTextWatermark";
         this.layoutItemTextWatermark.Size = new System.Drawing.Size(382, 24);
         this.layoutItemTextWatermark.TextSize = new System.Drawing.Size(127, 13);
         // 
         // ApplicationSettingsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ApplicationSettingsView";
         this.Size = new System.Drawing.Size(402, 404);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buttonPKSimPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPKSimPath)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUseWatermark.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textWatermark.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTextWatermark)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraEditors.ButtonEdit buttonPKSimPath;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPKSimPath;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.TextEdit textWatermark;
      private DevExpress.XtraEditors.CheckEdit chkUseWatermark;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTextWatermark;
   }
}
