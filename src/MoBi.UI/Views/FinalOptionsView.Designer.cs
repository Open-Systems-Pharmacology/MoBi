using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class FinalOptionsView
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
         this.pnlValidationOptions = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupValidation = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemValidationOptions = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pnlValidationOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupValidation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValidationOptions)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.pnlValidationOptions);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(806, 355);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // pnlValidationOptions
         // 
         this.pnlValidationOptions.Location = new System.Drawing.Point(162, 43);
         this.pnlValidationOptions.Name = "pnlValidationOptions";
         this.pnlValidationOptions.Size = new System.Drawing.Size(620, 288);
         this.pnlValidationOptions.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupValidation});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(806, 355);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutGroupValidation
         // 
         this.layoutGroupValidation.CustomizationFormText = "layoutGroupValidation";
         this.layoutGroupValidation.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValidationOptions});
         this.layoutGroupValidation.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupValidation.Name = "layoutGroupValidation";
         this.layoutGroupValidation.Size = new System.Drawing.Size(786, 335);
         this.layoutGroupValidation.Text = "layoutGroupValidation";
         // 
         // layoutItemValidationOptions
         // 
         this.layoutItemValidationOptions.Control = this.pnlValidationOptions;
         this.layoutItemValidationOptions.CustomizationFormText = "layoutItemValidationOptions";
         this.layoutItemValidationOptions.Location = new System.Drawing.Point(0, 0);
         this.layoutItemValidationOptions.Name = "layoutControlItem1";
         this.layoutItemValidationOptions.Size = new System.Drawing.Size(762, 292);
         this.layoutItemValidationOptions.Text = "layoutItemValidationOptions";
         this.layoutItemValidationOptions.TextSize = new System.Drawing.Size(135, 13);
         // 
         // FinalOptionsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "FinalOptionsView";
         this.Size = new System.Drawing.Size(806, 355);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pnlValidationOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupValidation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValidationOptions)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraEditors.PanelControl pnlValidationOptions;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValidationOptions;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupValidation;

   }
}
