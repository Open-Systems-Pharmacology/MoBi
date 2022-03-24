using OSPSuite.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class SelectEventAssignmentTargetView 
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
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl1
         // 
         this.panelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.panelControl1.Location = new System.Drawing.Point(0, 0);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(459, 599);
         this.panelControl1.TabIndex = 4;
         // 
         // SelectEventAssignmentTargetView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SelectEventAssignmentTargetView";
         this.ClientSize = new System.Drawing.Size(462, 634);
         this.Controls.Add(this.panelControl1);
         this.Name = "SelectEventAssignmentTargetView";
         this.Text = "SelectEventAssignmentTargetView";
         this.Controls.SetChildIndex(this.panelControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion


      private DevExpress.XtraEditors.PanelControl panelControl1;
   }
}