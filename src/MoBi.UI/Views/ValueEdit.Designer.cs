using DevExpress.XtraEditors;

namespace MoBi.UI.Views
{
   partial class  ValueEdit
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.fProperties = new DevExpress.XtraEditors.Repository.RepositoryItem();
         this.tbValue = new DevExpress.XtraEditors.TextEdit();
         this.cbUnit = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.warningProvider = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbUnit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.warningProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // fProperties
         // 
         this.fProperties.Name = "fProperties";
         // 
         // tbValue
         // 
         this.tbValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tablePanel.SetColumn(this.tbValue, 0);
         this.tbValue.Location = new System.Drawing.Point(0, 3);
         this.tbValue.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
         this.tbValue.Name = "tbValue";
         this.tablePanel.SetRow(this.tbValue, 0);
         this.tbValue.Size = new System.Drawing.Size(332, 20);
         this.tbValue.TabIndex = 1;
         // 
         // cbUnit
         // 
         this.cbUnit.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.tablePanel.SetColumn(this.cbUnit, 1);
         this.cbUnit.Location = new System.Drawing.Point(338, 3);
         this.cbUnit.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
         this.cbUnit.Name = "cbUnit";
         this.cbUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.tablePanel.SetRow(this.cbUnit, 0);
         this.cbUnit.Size = new System.Drawing.Size(332, 20);
         this.cbUnit.TabIndex = 2;
         // 
         // warningProvider
         // 
         this.warningProvider.ContainerControl = this;
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
         this.tablePanel.Controls.Add(this.cbUnit);
         this.tablePanel.Controls.Add(this.tbValue);
         this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tablePanel.Location = new System.Drawing.Point(0, 0);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(670, 26);
         this.tablePanel.TabIndex = 4;
         // 
         // ValueEdit
         // 

         this.Controls.Add(this.tablePanel);
         this.Name = "ValueEdit";
         this.Size = new System.Drawing.Size(670, 26);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.fProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbUnit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.warningProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private TextEdit tbValue;
      private DevExpress.XtraEditors.Repository.RepositoryItem fProperties;
      private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider warningProvider;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbUnit;
      private DevExpress.Utils.Layout.TablePanel tablePanel;
   }
}
