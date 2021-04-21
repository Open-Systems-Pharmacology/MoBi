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
         this.fProperties = new DevExpress.XtraEditors.Repository.RepositoryItem();
         this.tbValue = new DevExpress.XtraEditors.TextEdit();
         this.cbUnit = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.warningProvider = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbUnit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.warningProvider)).BeginInit();
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
         this.tbValue.Location = new System.Drawing.Point(0, 0);
         this.tbValue.Name = "tbValue";
         this.tbValue.Size = new System.Drawing.Size(151, 20);
         this.tbValue.TabIndex = 1;
         // 
         // cbUnit
         // 
         this.cbUnit.Dock = System.Windows.Forms.DockStyle.Right;
         this.cbUnit.Location = new System.Drawing.Point(157, 0);
         this.cbUnit.Name = "cbUnit";
         this.cbUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbUnit.Size = new System.Drawing.Size(72, 20);
         this.cbUnit.TabIndex = 2;
         // 
         // warningProvider
         // 
         this.warningProvider.ContainerControl = this;
         // 
         // ValueEdit
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.Controls.Add(this.tbValue);
         this.Controls.Add(this.cbUnit);
         this.MaximumSize = new System.Drawing.Size(200000, 20);
         this.Name = "ValueEdit";
         this.Size = new System.Drawing.Size(229, 20);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.fProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbUnit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.warningProvider)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private TextEdit tbValue;
      private ComboBoxEdit cbUnit;
      private DevExpress.XtraEditors.Repository.RepositoryItem fProperties;
      private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider warningProvider;
   }
}
