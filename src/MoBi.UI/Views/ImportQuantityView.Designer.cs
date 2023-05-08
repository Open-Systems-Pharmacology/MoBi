using MoBi.Presentation.Views;
using OSPSuite.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class ImportQuantityView
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
         _excelSheetSelectionScreenBinder.Dispose();
         _importStartValuesDTOScreenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this._sheetNameComboBox = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.lblImportFileFormatHint = new System.Windows.Forms.Label();
         this.messageMemoEdit = new DevExpress.XtraEditors.MemoEdit();
         this.filePathButtonEdit = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.logEditControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._sheetNameComboBox.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.messageMemoEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.filePathButtonEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.logEditControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
        // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this._sheetNameComboBox);
         this.layoutControl1.Controls.Add(this.lblImportFileFormatHint);
         this.layoutControl1.Controls.Add(this.messageMemoEdit);
         this.layoutControl1.Controls.Add(this.filePathButtonEdit);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(684, 516);
         this.layoutControl1.TabIndex = 38;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // _sheetNameComboBox
         // 
         this._sheetNameComboBox.Location = new System.Drawing.Point(77, 36);
         this._sheetNameComboBox.Name = "_sheetNameComboBox";
         this._sheetNameComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this._sheetNameComboBox.Size = new System.Drawing.Size(595, 20);
         this._sheetNameComboBox.StyleController = this.layoutControl1;
         this._sheetNameComboBox.TabIndex = 39;
         // 
         // lblImportFileFormatHint
         // 
         this.lblImportFileFormatHint.Location = new System.Drawing.Point(12, 60);
         this.lblImportFileFormatHint.Name = "lblImportFileFormatHint";
         this.lblImportFileFormatHint.Size = new System.Drawing.Size(660, 93);
         this.lblImportFileFormatHint.TabIndex = 40;
         this.lblImportFileFormatHint.Text = "lblImportFileFormatHint";
         // 
         // messageMemoEdit
         // 
         this.messageMemoEdit.Location = new System.Drawing.Point(12, 157);
         this.messageMemoEdit.Name = "messageMemoEdit";
         this.messageMemoEdit.Size = new System.Drawing.Size(660, 347);
         this.messageMemoEdit.StyleController = this.layoutControl1;
         this.messageMemoEdit.TabIndex = 39;
         // 
         // filePathButtonEdit
         // 
         this.filePathButtonEdit.Location = new System.Drawing.Point(77, 12);
         this.filePathButtonEdit.Name = "filePathButtonEdit";
         this.filePathButtonEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.filePathButtonEdit.Size = new System.Drawing.Size(595, 20);
         this.filePathButtonEdit.StyleController = this.layoutControl1;
         this.filePathButtonEdit.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.logEditControlItem2,
            this.layoutControlItem2,
            this.layoutControlItem3});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(684, 516);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.filePathButtonEdit;
         this.layoutControlItem1.CustomizationFormText = "File Path:";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(664, 24);
         this.layoutControlItem1.Text = "File Path:";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(62, 13);
         // 
         // logEditControlItem2
         // 
         this.logEditControlItem2.Control = this.messageMemoEdit;
         this.logEditControlItem2.CustomizationFormText = "logEditControlItem2";
         this.logEditControlItem2.Location = new System.Drawing.Point(0, 145);
         this.logEditControlItem2.Name = "logEditControlItem2";
         this.logEditControlItem2.Size = new System.Drawing.Size(664, 351);
         this.logEditControlItem2.Text = "logEditControlItem2";
         this.logEditControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.logEditControlItem2.TextToControlDistance = 0;
         this.logEditControlItem2.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblImportFileFormatHint;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(664, 97);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this._sheetNameComboBox;
         this.layoutControlItem3.CustomizationFormText = "Sheet Name:";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(664, 24);
         this.layoutControlItem3.Text = "Sheet Name:";
         this.layoutControlItem3.TextSize = new System.Drawing.Size(62, 13);
         // 
         // ImportStartValuesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ImportParameterValuesView";
         this.ClientSize = new System.Drawing.Size(684, 562);
         this.Controls.Add(this.layoutControl1);
         this.Name = "ImportQuantityView";
         this.Text = "ImportParameterValuesView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._sheetNameComboBox.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.messageMemoEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.filePathButtonEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.logEditControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.MemoEdit messageMemoEdit;
      private DevExpress.XtraEditors.ButtonEdit filePathButtonEdit;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem logEditControlItem2;
      private System.Windows.Forms.Label lblImportFileFormatHint;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private OSPSuite.UI.Controls.UxComboBoxEdit _sheetNameComboBox;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
   }
}