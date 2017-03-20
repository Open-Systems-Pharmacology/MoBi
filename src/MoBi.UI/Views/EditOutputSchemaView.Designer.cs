namespace MoBi.UI.Views
{
   partial class EditOutputSchemaView
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
         this.gridControl = new DevExpress.XtraGrid.GridControl();
         this.gridViewIntervals = new MoBi.UI.Views.UxGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupIntervals = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemIntervals = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewIntervals)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupIntervals)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIntervals)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(5, 24);
         this.gridControl.MainView = this.gridViewIntervals;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(524, 387);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewIntervals});
         // 
         // gridViewIntervals
         // 
         this.gridViewIntervals.AllowsFiltering = true;
         this.gridViewIntervals.GridControl = this.gridControl;
         this.gridViewIntervals.Name = "gridViewIntervals";
         this.gridViewIntervals.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridViewIntervals.OptionsNavigation.AutoFocusNewRow = true;
         this.gridViewIntervals.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridViewIntervals.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridViewIntervals.OptionsView.ShowIndicator = false;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(327, 206, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(534, 416);
         this.layoutControl.TabIndex = 2;
         this.layoutControl.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupIntervals});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(534, 416);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutGroupIntervals
         // 
         this.layoutGroupIntervals.CustomizationFormText = "layoutGroupIntervals";
         this.layoutGroupIntervals.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemIntervals});
         this.layoutGroupIntervals.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupIntervals.Name = "layoutGroupIntervals";
         this.layoutGroupIntervals.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutGroupIntervals.Size = new System.Drawing.Size(534, 416);
         this.layoutGroupIntervals.Text = "layoutGroupIntervals";
         // 
         // layoutItemIntervals
         // 
         this.layoutItemIntervals.Control = this.gridControl;
         this.layoutItemIntervals.CustomizationFormText = "layoutItemIntervals";
         this.layoutItemIntervals.Location = new System.Drawing.Point(0, 0);
         this.layoutItemIntervals.Name = "layoutItemIntervals";
         this.layoutItemIntervals.Size = new System.Drawing.Size(528, 391);
         this.layoutItemIntervals.Text = "layoutItemIntervals";
         this.layoutItemIntervals.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIntervals.TextToControlDistance = 0;
         this.layoutItemIntervals.TextVisible = false;
         // 
         // EditOutputSchemaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditOutputSchemaView";
         this.Size = new System.Drawing.Size(534, 416);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewIntervals)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupIntervals)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIntervals)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraGrid.GridControl gridControl;
      private MoBi.UI.Views.UxGridView gridViewIntervals;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupIntervals;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIntervals;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;

   }
}
