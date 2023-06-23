using OSPSuite.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class SelectObjectPathView 
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
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 591);
         this.tablePanel.Size = new System.Drawing.Size(462, 43);
         // 
         // panelControl1
         // 
         this.panelControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControl.Location = new System.Drawing.Point(0, 0);
         this.panelControl.Name = "panelControl";
         this.panelControl.Size = new System.Drawing.Size(462, 591);
         this.panelControl.TabIndex = 39;
         // 
         // SelectObjectPathView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SelectEventAssignmentTargetView";
         this.ClientSize = new System.Drawing.Size(462, 634);
         this.Controls.Add(this.panelControl);
         this.Name = "SelectObjectPathView";
         this.Text = "SelectEventAssignmentTargetView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.panelControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl;
   }
}