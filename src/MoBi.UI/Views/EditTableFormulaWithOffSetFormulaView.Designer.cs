﻿namespace MoBi.UI.Views
{
   partial class EditTableFormulaWithOffsetFormulaView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.btEditOffsetObjectPath = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlItemOffsetObjectPath = new DevExpress.XtraLayout.LayoutControlItem();
         this.btEditTableObjectPath = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlItemTableObjectPath = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditOffsetObjectPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOffsetObjectPath)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditTableObjectPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTableObjectPath)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.btEditTableObjectPath);
         this.layoutControl1.Controls.Add(this.btEditOffsetObjectPath);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(334, 150);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemOffsetObjectPath,
            this.layoutControlItemTableObjectPath});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(334, 150);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // btEditOffsetObjectPath
         // 
         this.btEditOffsetObjectPath.Location = new System.Drawing.Point(188, 36);
         this.btEditOffsetObjectPath.Name = "btEditOffsetObjectPath";
         this.btEditOffsetObjectPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btEditOffsetObjectPath.Size = new System.Drawing.Size(134, 20);
         this.btEditOffsetObjectPath.StyleController = this.layoutControl1;
         this.btEditOffsetObjectPath.TabIndex = 4;
         // 
         // layoutControlItemOffsetObjectPath
         // 
         this.layoutControlItemOffsetObjectPath.Control = this.btEditOffsetObjectPath;
         this.layoutControlItemOffsetObjectPath.CustomizationFormText = "layoutControlItemOffsetObjectPath";
         this.layoutControlItemOffsetObjectPath.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemOffsetObjectPath.Name = "layoutControlItemOffsetObjectPath";
         this.layoutControlItemOffsetObjectPath.Size = new System.Drawing.Size(314, 106);
         this.layoutControlItemOffsetObjectPath.Text = "layoutControlItemOffsetObjectPath";
         this.layoutControlItemOffsetObjectPath.TextSize = new System.Drawing.Size(172, 13);
         // 
         // btEditTableObjectPath
         // 
         this.btEditTableObjectPath.Location = new System.Drawing.Point(188, 12);
         this.btEditTableObjectPath.Name = "btEditTableObjectPath";
         this.btEditTableObjectPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btEditTableObjectPath.Size = new System.Drawing.Size(134, 20);
         this.btEditTableObjectPath.StyleController = this.layoutControl1;
         this.btEditTableObjectPath.TabIndex = 5;
         // 
         // layoutControlItemTableObjectPath
         // 
         this.layoutControlItemTableObjectPath.Control = this.btEditTableObjectPath;
         this.layoutControlItemTableObjectPath.CustomizationFormText = "layoutControlItemTableObjectPath";
         this.layoutControlItemTableObjectPath.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemTableObjectPath.Name = "layoutControlItemTableObjectPath";
         this.layoutControlItemTableObjectPath.Size = new System.Drawing.Size(314, 24);
         this.layoutControlItemTableObjectPath.Text = "layoutControlItemTableObjectPath";
         this.layoutControlItemTableObjectPath.TextSize = new System.Drawing.Size(172, 13);
         // 
         // EditTableFormulaWithOffSetFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "EditTableFormulaWithOffsetFormulaView";
         this.Size = new System.Drawing.Size(334, 150);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditOffsetObjectPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOffsetObjectPath)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditTableObjectPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTableObjectPath)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.ButtonEdit btEditTableObjectPath;
      private DevExpress.XtraEditors.ButtonEdit btEditOffsetObjectPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemOffsetObjectPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTableObjectPath;
   }
}
