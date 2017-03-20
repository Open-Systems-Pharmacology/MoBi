namespace MoBi.UI.Views
{
   partial class EditConstantFormulaView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.valueEdit = new MoBi.UI.Views.ValueEdit();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemValue = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).BeginInit();
         this.SuspendLayout();
         // 
         // valueEdit
         // 
         this.valueEdit.Location = new System.Drawing.Point(94, 12);
         this.valueEdit.MaximumSize = new System.Drawing.Size(20000, 20);
         this.valueEdit.Name = "valueEdit";
         this.valueEdit.Size = new System.Drawing.Size(353, 20);
         this.valueEdit.TabIndex = 1;
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.valueEdit);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(459, 201);
         this.layoutControl1.TabIndex = 2;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValue});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(459, 201);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemValue
         // 
         this.layoutItemValue.Control = this.valueEdit;
         this.layoutItemValue.CustomizationFormText = "layoutItemValue";
         this.layoutItemValue.Location = new System.Drawing.Point(0, 0);
         this.layoutItemValue.Name = "layoutItemValue";
         this.layoutItemValue.Size = new System.Drawing.Size(439, 181);
         this.layoutItemValue.Text = "layoutItemValue";
         this.layoutItemValue.TextSize = new System.Drawing.Size(78, 13);
         // 
         // EditConstantFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "EditConstantFormulaView";
         this.Size = new System.Drawing.Size(459, 201);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private ValueEdit valueEdit;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValue;


   }
}
