using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class SplashScreen 
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
         this.progressBar = new DevExpress.XtraEditors.ProgressBarControl();
         this.lblCaption = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // progressBar
         // 
         this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.progressBar.EditValue = "0";
         this.progressBar.Location = new System.Drawing.Point(0, 118);
         this.progressBar.Name = "progressBar";
         this.progressBar.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
         this.progressBar.Properties.UseParentBackground = true;
         this.progressBar.Size = new System.Drawing.Size(451, 18);
         this.progressBar.TabIndex = 0;
         // 
         // lblCaption
         // 
         this.lblCaption.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblCaption.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.lblCaption.Location = new System.Drawing.Point(0, 102);
         this.lblCaption.Name = "lblCaption";
         this.lblCaption.Size = new System.Drawing.Size(56, 16);
         this.lblCaption.TabIndex = 1;
         this.lblCaption.Text = "lblCaption";
         // 
         // SplashScreen
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
         this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
         this.ClientSize = new System.Drawing.Size(451, 136);
         this.Controls.Add(this.lblCaption);
         this.Controls.Add(this.progressBar);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "SplashScreen";
         this.Opacity = 0.9;
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         ((System.ComponentModel.ISupportInitialize)(this.progressBar.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.ProgressBarControl progressBar;
      private DevExpress.XtraEditors.LabelControl lblCaption;
   }
}
