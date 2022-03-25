namespace MoBi.UI.Views
{
   partial class ObjectBaseSummaryView
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
            _screenBinder.Dispose();
            _gridViewBinder.Dispose();
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this._labelName = new System.Windows.Forms.Label();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.svgImageBox = new DevExpress.XtraEditors.SvgImageBox();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.svgImageBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.svgImageBox);
         this.layoutControl1.Controls.Add(this._labelName);
         this.layoutControl1.Controls.Add(this.gridControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(819, 385);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // _labelName
         // 
         this._labelName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
         this._labelName.Location = new System.Drawing.Point(42, 12);
         this._labelName.Name = "_labelName";
         this._labelName.Size = new System.Drawing.Size(765, 20);
         this._labelName.TabIndex = 6;
         this._labelName.Text = "label1";
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.Location = new System.Drawing.Point(12, 36);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(795, 337);
         this.gridControl.TabIndex = 5;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem1,
            this.layoutControlItem3});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(819, 385);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.gridControl;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(799, 341);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this._labelName;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(30, 0);
         this.layoutControlItem1.MinSize = new System.Drawing.Size(24, 24);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(769, 24);
         this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // svgImageBox
         // 
         this.svgImageBox.Location = new System.Drawing.Point(12, 12);
         this.svgImageBox.Name = "svgImageBox";
         this.svgImageBox.Size = new System.Drawing.Size(26, 20);
         this.svgImageBox.TabIndex = 9;
         this.svgImageBox.Text = "svgImageBox1";
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.svgImageBox;
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(30, 24);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // ObjectBaseSummaryView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "ObjectBaseSummaryView";
         this.Size = new System.Drawing.Size(819, 385);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.svgImageBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private System.Windows.Forms.Label _labelName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.SvgImageBox svgImageBox;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
   }
}
