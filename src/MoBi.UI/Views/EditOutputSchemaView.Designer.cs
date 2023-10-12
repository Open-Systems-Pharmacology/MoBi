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
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridViewIntervals = new MoBi.UI.Views.UxGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupIntervals = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemIntervals = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnAddOutputInterval = new OSPSuite.UI.Controls.UxSimpleButton();
         this.layoutControlItemAddInterval = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewIntervals)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupIntervals)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIntervals)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddInterval)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(5, 52);
         this.gridControl.MainView = this.gridViewIntervals;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(524, 359);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewIntervals});
         // 
         // gridViewIntervals
         // 
         this.gridViewIntervals.AllowsFiltering = true;
         this.gridViewIntervals.EnableColumnContextMenu = true;
         this.gridViewIntervals.GridControl = this.gridControl;
         this.gridViewIntervals.MultiSelect = false;
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
         this.layoutControl.Controls.Add(this.btnAddOutputInterval);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1520, 265, 1030, 730);
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
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(534, 416);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutGroupIntervals
         // 
         this.layoutGroupIntervals.CustomizationFormText = "layoutGroupIntervals";
         this.layoutGroupIntervals.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemIntervals,
            this.layoutControlItemAddInterval,
            this.emptySpaceItem1});
         this.layoutGroupIntervals.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupIntervals.Name = "layoutGroupIntervals";
         this.layoutGroupIntervals.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutGroupIntervals.Size = new System.Drawing.Size(534, 416);
         // 
         // layoutItemIntervals
         // 
         this.layoutItemIntervals.Control = this.gridControl;
         this.layoutItemIntervals.CustomizationFormText = "layoutItemIntervals";
         this.layoutItemIntervals.Location = new System.Drawing.Point(0, 26);
         this.layoutItemIntervals.Name = "layoutItemIntervals";
         this.layoutItemIntervals.Size = new System.Drawing.Size(528, 363);
         this.layoutItemIntervals.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIntervals.TextVisible = false;
         // 
         // btnAddOutputInterval
         // 
         this.btnAddOutputInterval.Location = new System.Drawing.Point(371, 26);
         this.btnAddOutputInterval.Manager = null;
         this.btnAddOutputInterval.Name = "btnAddOutputInterval";
         this.btnAddOutputInterval.Shortcut = System.Windows.Forms.Keys.None;
         this.btnAddOutputInterval.Size = new System.Drawing.Size(158, 22);
         this.btnAddOutputInterval.StyleController = this.layoutControl;
         this.btnAddOutputInterval.TabIndex = 4;
         this.btnAddOutputInterval.Text = "btnAddOutputInterval";
         // 
         // layoutControlItemAddInterval
         // 
         this.layoutControlItemAddInterval.Control = this.btnAddOutputInterval;
         this.layoutControlItemAddInterval.Location = new System.Drawing.Point(366, 0);
         this.layoutControlItemAddInterval.Name = "layoutControlItemAddInterval";
         this.layoutControlItemAddInterval.Size = new System.Drawing.Size(162, 26);
         this.layoutControlItemAddInterval.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddInterval.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(366, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddInterval)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private MoBi.UI.Views.UxGridView gridViewIntervals;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupIntervals;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIntervals;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private OSPSuite.UI.Controls.UxSimpleButton btnAddOutputInterval;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddInterval;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
   }
}
