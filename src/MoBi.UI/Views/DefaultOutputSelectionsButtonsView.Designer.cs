namespace MoBi.UI.Views
{
   partial class DefaultOutputSelectionsButtonsView
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
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         this.btnLoadProjectDefaults = new OSPSuite.UI.Controls.UxSimpleButton();
         this.btnMakeProjectDefaults = new OSPSuite.UI.Controls.UxSimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.AutoSize = true;
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
         this.tablePanel.Controls.Add(this.btnLoadProjectDefaults);
         this.tablePanel.Controls.Add(this.btnMakeProjectDefaults);
         this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tablePanel.Location = new System.Drawing.Point(0, 0);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(321, 29);
         this.tablePanel.TabIndex = 0;
         // 
         // btnLoadProjectDefaults
         // 
         this.tablePanel.SetColumn(this.btnLoadProjectDefaults, 1);
         this.btnLoadProjectDefaults.Dock = System.Windows.Forms.DockStyle.Top;
         this.btnLoadProjectDefaults.Location = new System.Drawing.Point(164, 3);
         this.btnLoadProjectDefaults.Manager = null;
         this.btnLoadProjectDefaults.Name = "btnLoadProjectDefaults";
         this.tablePanel.SetRow(this.btnLoadProjectDefaults, 0);
         this.btnLoadProjectDefaults.Shortcut = System.Windows.Forms.Keys.None;
         this.btnLoadProjectDefaults.Size = new System.Drawing.Size(155, 23);
         this.btnLoadProjectDefaults.TabIndex = 1;
         this.btnLoadProjectDefaults.Text = "btnLoadProjectDefaults";
         // 
         // btnMakeProjectDefaults
         // 
         this.tablePanel.SetColumn(this.btnMakeProjectDefaults, 0);
         this.btnMakeProjectDefaults.Dock = System.Windows.Forms.DockStyle.Top;
         this.btnMakeProjectDefaults.Location = new System.Drawing.Point(3, 3);
         this.btnMakeProjectDefaults.Manager = null;
         this.btnMakeProjectDefaults.Name = "btnMakeProjectDefaults";
         this.tablePanel.SetRow(this.btnMakeProjectDefaults, 0);
         this.btnMakeProjectDefaults.Shortcut = System.Windows.Forms.Keys.None;
         this.btnMakeProjectDefaults.Size = new System.Drawing.Size(155, 23);
         this.btnMakeProjectDefaults.TabIndex = 0;
         this.btnMakeProjectDefaults.Text = "btnMakeProjectDefaults";
         // 
         // DefaultOutputSelectionsButtonsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tablePanel);
         this.Name = "DefaultOutputSelectionsButtonsView";
         this.Size = new System.Drawing.Size(321, 29);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.Utils.Layout.TablePanel tablePanel;
      private OSPSuite.UI.Controls.UxSimpleButton btnLoadProjectDefaults;
      private OSPSuite.UI.Controls.UxSimpleButton btnMakeProjectDefaults;
   }
}
