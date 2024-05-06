namespace MoBi.UI.Views
{
   partial class OutputSelectionsView
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
         this.panel = new DevExpress.XtraEditors.PanelControl();
         this.btnLoadDefaults = new OSPSuite.UI.Controls.UxSimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panel)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
         this.tablePanel.Controls.Add(this.btnLoadDefaults);
         this.tablePanel.Location = new System.Drawing.Point(0, 526);
         this.tablePanel.Size = new System.Drawing.Size(745, 43);
         this.tablePanel.Controls.SetChildIndex(this.btnLoadDefaults, 0);
         // 
         // panel
         // 
         this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel.Location = new System.Drawing.Point(0, 0);
         this.panel.Name = "panel";
         this.panel.Size = new System.Drawing.Size(745, 569);
         this.panel.TabIndex = 38;
         // 
         // btnLoadDefaults
         // 
         this.tablePanel.SetColumn(this.btnLoadDefaults, 1);
         this.btnLoadDefaults.Location = new System.Drawing.Point(164, 10);
         this.btnLoadDefaults.Manager = null;
         this.btnLoadDefaults.Name = "btnLoadDefaults";
         this.tablePanel.SetRow(this.btnLoadDefaults, 0);
         this.btnLoadDefaults.Shortcut = System.Windows.Forms.Keys.None;
         this.btnLoadDefaults.Size = new System.Drawing.Size(140, 23);
         this.btnLoadDefaults.TabIndex = 3;
         this.btnLoadDefaults.Text = "btnLoadDefaults";
         // 
         // OutputSelectionsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SimulationSettingsView";
         this.ClientSize = new System.Drawing.Size(745, 569);
         this.Controls.Add(this.panel);
         this.Name = "OutputSelectionsView";
         this.Text = "SimulationSettingsView";
         this.Controls.SetChildIndex(this.panel, 0);
         this.Controls.SetChildIndex(this.tablePanel, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panel)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panel;
      private OSPSuite.UI.Controls.UxSimpleButton btnLoadDefaults;
   }
}