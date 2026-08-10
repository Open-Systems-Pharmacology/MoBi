namespace MoBi.UI.Views
{
   partial class ParameterReferencesView
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
         _gridViewBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.btnBack = new DevExpress.XtraEditors.SimpleButton();
         this.lblBreadcrumb = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemBack = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemBreadcrumb = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemReferences = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemBack)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemBreadcrumb)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemReferences)).BeginInit();
         this.SuspendLayout();
         //
         // layoutControl
         //
         this.layoutControl.Controls.Add(this.btnBack);
         this.layoutControl.Controls.Add(this.lblBreadcrumb);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(984, 515);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl";
         //
         // btnBack
         //
         this.btnBack.Location = new System.Drawing.Point(12, 12);
         this.btnBack.Name = "btnBack";
         this.btnBack.Size = new System.Drawing.Size(96, 22);
         this.btnBack.StyleController = this.layoutControl;
         this.btnBack.TabIndex = 5;
         this.btnBack.Text = "btnBack";
         //
         // lblBreadcrumb
         //
         this.lblBreadcrumb.Location = new System.Drawing.Point(112, 17);
         this.lblBreadcrumb.Name = "lblBreadcrumb";
         this.lblBreadcrumb.Size = new System.Drawing.Size(860, 13);
         this.lblBreadcrumb.StyleController = this.layoutControl;
         this.lblBreadcrumb.TabIndex = 6;
         this.lblBreadcrumb.Text = "lblBreadcrumb";
         //
         // gridControl
         //
         this.gridControl.Location = new System.Drawing.Point(12, 38);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(960, 465);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         //
         // gridView
         //
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         this.gridView.RowsInsertable = false;
         //
         // layoutControlGroup
         //
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemBack,
            this.layoutControlItemBreadcrumb,
            this.layoutControlItemReferences});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(984, 515);
         this.layoutControlGroup.Text = "layoutControlGroup";
         this.layoutControlGroup.TextVisible = false;
         //
         // layoutControlItemBack
         //
         this.layoutControlItemBack.Control = this.btnBack;
         this.layoutControlItemBack.CustomizationFormText = "layoutControlItemBack";
         this.layoutControlItemBack.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemBack.Name = "layoutControlItemBack";
         this.layoutControlItemBack.Size = new System.Drawing.Size(100, 26);
         this.layoutControlItemBack.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutControlItemBack.Text = "layoutControlItemBack";
         this.layoutControlItemBack.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemBack.TextToControlDistance = 0;
         this.layoutControlItemBack.TextVisible = false;
         //
         // layoutControlItemBreadcrumb
         //
         this.layoutControlItemBreadcrumb.Control = this.lblBreadcrumb;
         this.layoutControlItemBreadcrumb.CustomizationFormText = "layoutControlItemBreadcrumb";
         this.layoutControlItemBreadcrumb.Location = new System.Drawing.Point(100, 0);
         this.layoutControlItemBreadcrumb.Name = "layoutControlItemBreadcrumb";
         this.layoutControlItemBreadcrumb.Size = new System.Drawing.Size(864, 26);
         this.layoutControlItemBreadcrumb.Text = "layoutControlItemBreadcrumb";
         this.layoutControlItemBreadcrumb.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemBreadcrumb.TextToControlDistance = 0;
         this.layoutControlItemBreadcrumb.TextVisible = false;
         //
         // layoutControlItemReferences
         //
         this.layoutControlItemReferences.Control = this.gridControl;
         this.layoutControlItemReferences.CustomizationFormText = "layoutControlItemReferences";
         this.layoutControlItemReferences.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemReferences.Name = "layoutControlItemReferences";
         this.layoutControlItemReferences.Size = new System.Drawing.Size(964, 469);
         this.layoutControlItemReferences.Text = "layoutControlItemReferences";
         this.layoutControlItemReferences.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemReferences.TextToControlDistance = 0;
         this.layoutControlItemReferences.TextVisible = false;
         //
         // ParameterReferencesView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(984, 561);
         this.Controls.Add(this.layoutControl);
         this.Name = "ParameterReferencesView";
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemBack)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemBreadcrumb)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemReferences)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraEditors.SimpleButton btnBack;
      private DevExpress.XtraEditors.LabelControl lblBreadcrumb;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemBack;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemBreadcrumb;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemReferences;
   }
}
