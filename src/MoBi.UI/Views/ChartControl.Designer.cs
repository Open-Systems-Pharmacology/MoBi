using DevExpress.XtraEditors;

namespace MoBi.UI.Views
{
   partial class ChartControl
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
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.FixedPanel = SplitFixedPanel.None;
         this.splitContainerControl1.Size = new System.Drawing.Size(352, 214);
         this.splitContainerControl1.SplitterPosition = 176;
         this.splitContainerControl1.TabIndex = 0;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // ChartControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.splitContainerControl1);
         this.Name = "ChartControl";
         this.Size = new System.Drawing.Size(352, 214);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
   }
}
