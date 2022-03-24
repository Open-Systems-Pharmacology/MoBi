namespace MoBi.UI.Views
{
   partial class NewFormulaView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.splitFormulaEditor = new DevExpress.XtraEditors.SplitContainerControl();
         this.txtName = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFormula = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitFormulaEditor)).BeginInit();
         this.splitFormulaEditor.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormula)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.splitFormulaEditor);
         this.layoutControl1.Controls.Add(this.txtName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(758, 524);
         this.layoutControl1.TabIndex = 38;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // splitFormulaEditor
         // 
         this.splitFormulaEditor.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitFormulaEditor.Location = new System.Drawing.Point(105, 36);
         this.splitFormulaEditor.Name = "splitFormulaEditor";
         this.splitFormulaEditor.Panel1.Text = "Panel1";
         this.splitFormulaEditor.Panel2.Text = "Panel2";
         this.splitFormulaEditor.Size = new System.Drawing.Size(641, 476);
         this.splitFormulaEditor.SplitterPosition = 317;
         this.splitFormulaEditor.TabIndex = 5;
         this.splitFormulaEditor.Text = "splitContainerControl1";
         // 
         // txtName
         // 
         this.txtName.Location = new System.Drawing.Point(105, 12);
         this.txtName.Name = "txtName";
         this.txtName.Size = new System.Drawing.Size(641, 20);
         this.txtName.StyleController = this.layoutControl1;
         this.txtName.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutItemFormula});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(758, 524);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.txtName;
         this.layoutItemName.CustomizationFormText = "layoutItemName";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutControlName";
         this.layoutItemName.Size = new System.Drawing.Size(738, 24);
         this.layoutItemName.Text = "layoutItemName";
         this.layoutItemName.TextSize = new System.Drawing.Size(90, 13);
         // 
         // layoutItemFormula
         // 
         this.layoutItemFormula.Control = this.splitFormulaEditor;
         this.layoutItemFormula.CustomizationFormText = "layoutItemFormula";
         this.layoutItemFormula.Location = new System.Drawing.Point(0, 24);
         this.layoutItemFormula.Name = "layoutItemFormula";
         this.layoutItemFormula.Size = new System.Drawing.Size(738, 480);
         this.layoutItemFormula.Text = "layoutItemFormula";
         this.layoutItemFormula.TextSize = new System.Drawing.Size(90, 13);
         // 
         // NewFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "NewFormulaView";
         this.ClientSize = new System.Drawing.Size(758, 570);
         this.Controls.Add(this.layoutControl1);
         this.Name = "NewFormulaView";
         this.Text = "NewFormulaView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitFormulaEditor)).EndInit();
         this.splitFormulaEditor.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormula)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.TextEdit txtName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraEditors.SplitContainerControl splitFormulaEditor;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormula;
   }
}