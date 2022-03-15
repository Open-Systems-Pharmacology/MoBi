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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.valueEdit = new MoBi.UI.Views.ValueEdit();
         this.layoutItemValue = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.valueEdit);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(459, 90);
         this.layoutControl1.TabIndex = 2;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValue,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(459, 90);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 48);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(439, 22);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // valueEdit
         // 
         this.valueEdit.Caption = "";
         this.valueEdit.Location = new System.Drawing.Point(102, 12);
         this.valueEdit.Margin = new System.Windows.Forms.Padding(4);
         this.valueEdit.Name = "valueEdit";
         this.valueEdit.Size = new System.Drawing.Size(345, 44);
         this.valueEdit.TabIndex = 4;
         this.valueEdit.ToolTip = "";
         // 
         // layoutItemValue
         // 
         this.layoutItemValue.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
         this.layoutItemValue.Control = this.valueEdit;
         this.layoutItemValue.Location = new System.Drawing.Point(0, 0);
         this.layoutItemValue.MaxSize = new System.Drawing.Size(0, 48);
         this.layoutItemValue.MinSize = new System.Drawing.Size(232, 48);
         this.layoutItemValue.Name = "layoutItemValue";
         this.layoutItemValue.Size = new System.Drawing.Size(439, 48);
         this.layoutItemValue.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemValue.TextSize = new System.Drawing.Size(78, 13);
         // 
         // EditConstantFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "EditConstantFormulaView";
         this.Size = new System.Drawing.Size(459, 90);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private ValueEdit valueEdit;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValue;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
