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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.pnlValidationOptions = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupValidation = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemValidationOptions = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pnlValidationOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupValidation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValidationOptions)).BeginInit();
         this.SuspendLayout();
         //
         // layoutControl
         //
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.pnlValidationOptions);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(806, 355);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl";
         //
         // pnlValidationOptions
         //
         this.pnlValidationOptions.Location = new System.Drawing.Point(24, 43);
         this.pnlValidationOptions.Name = "pnlValidationOptions";
         this.pnlValidationOptions.Size = new System.Drawing.Size(758, 288);
         this.pnlValidationOptions.TabIndex = 4;
         //
         // layoutControlGroup
         //
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupValidation});
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(806, 355);
         this.layoutControlGroup.TextVisible = false;
         //
         // layoutGroupValidation
         //
         this.layoutGroupValidation.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValidationOptions});
         this.layoutGroupValidation.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupValidation.Name = "layoutGroupValidation";
         this.layoutGroupValidation.Size = new System.Drawing.Size(786, 335);
         //
         // layoutItemValidationOptions
         //
         this.layoutItemValidationOptions.Control = this.pnlValidationOptions;
         this.layoutItemValidationOptions.Location = new System.Drawing.Point(0, 0);
         this.layoutItemValidationOptions.Name = "layoutItemValidationOptions";
         this.layoutItemValidationOptions.Size = new System.Drawing.Size(762, 292);
         this.layoutItemValidationOptions.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemValidationOptions.TextVisible = false;
         //
         // FinalOptionsView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "FinalOptionsView";
         this.Size = new System.Drawing.Size(806, 355);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pnlValidationOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupValidation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValidationOptions)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraEditors.PanelControl pnlValidationOptions;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupValidation;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValidationOptions;
   }
}
