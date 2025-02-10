namespace MoBi.UI.Views
{
   partial class SelectSingleView<T>
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
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.cbItems = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.layoutControlItemParameterValues = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbItems.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParameterValues)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.lblDescription);
         this.uxLayoutControl1.Controls.Add(this.cbItems);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(356, 150);
         this.uxLayoutControl1.TabIndex = 0;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemParameterValues,
            this.emptySpaceItem1,
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(356, 150);
         this.Root.TextVisible = false;
         // 
         // cbItems
         // 
         this.cbItems.Location = new System.Drawing.Point(62, 29);
         this.cbItems.Name = "cbItems";
         this.cbItems.Properties.AllowMouseWheel = false;
         this.cbItems.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbItems.Size = new System.Drawing.Size(282, 20);
         this.cbItems.StyleController = this.uxLayoutControl1;
         this.cbItems.TabIndex = 4;
         // 
         // layoutControlItemParameterValues
         // 
         this.layoutControlItemParameterValues.Control = this.cbItems;
         this.layoutControlItemParameterValues.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItemParameterValues.Name = "layoutControlItemParameterValues";
         this.layoutControlItemParameterValues.Size = new System.Drawing.Size(336, 24);
         this.layoutControlItemParameterValues.Text = "cbItems";
         this.layoutControlItemParameterValues.TextSize = new System.Drawing.Size(38, 13);
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 41);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(336, 89);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // lblDescription
         // 
         this.lblDescription.Location = new System.Drawing.Point(12, 12);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(63, 13);
         this.lblDescription.StyleController = this.uxLayoutControl1;
         this.lblDescription.TabIndex = 5;
         this.lblDescription.Text = "lblDescription";
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblDescription;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(336, 17);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // SelectSingleView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "SelectSingleView";
         this.Size = new System.Drawing.Size(356, 150);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbItems.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParameterValues)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbItems;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemParameterValues;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}
