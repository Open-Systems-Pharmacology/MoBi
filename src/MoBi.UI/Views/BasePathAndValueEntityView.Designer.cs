using OSPSuite.UI.Controls;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace MoBi.UI.Views
{
   partial class BasePathAndValueEntityView<TPathAndValueEntity,T> 
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
         _valueOriginBinder.Dispose();
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
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelDeleteStartValues = new DevExpress.XtraEditors.PanelControl();
         this.panelIsPresent = new DevExpress.XtraEditors.PanelControl();
         this.panelNegativeValuesAllowed = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemGridView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupPanel = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDelete = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemIsPresent = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemNegativeValuesAllowed = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelDeleteStartValues)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDelete)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.Location = new System.Drawing.Point(2, 76);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1623, 480);
         this.gridControl.TabIndex = 0;
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
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.panelDeleteStartValues);
         this.layoutControl.Controls.Add(this.panelIsPresent);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Controls.Add(this.panelNegativeValuesAllowed);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1195, 272, 412, 432);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(1627, 558);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelDeleteStartValues
         // 
         this.panelDeleteStartValues.Location = new System.Drawing.Point(14, 14);
         this.panelDeleteStartValues.Name = "panelDeleteStartValues";
         this.panelDeleteStartValues.Size = new System.Drawing.Size(373, 21);
         this.panelDeleteStartValues.TabIndex = 8;
         // 
         // panelIsPresent
         // 
         this.panelIsPresent.Location = new System.Drawing.Point(14, 39);
         this.panelIsPresent.Name = "panelIsPresent";
         this.panelIsPresent.Size = new System.Drawing.Size(373, 21);
         this.panelIsPresent.TabIndex = 6;
         // 
         // panelNegativeValuesAllowed
         // 
         this.panelNegativeValuesAllowed.Location = new System.Drawing.Point(391, 14);
         this.panelNegativeValuesAllowed.Name = "panelNegativeValuesAllowed";
         this.panelNegativeValuesAllowed.Size = new System.Drawing.Size(373, 21);
         this.panelNegativeValuesAllowed.TabIndex = 8;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "Root";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemGridView,
            this.layoutGroupPanel});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(1627, 558);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemGridView
         // 
         this.layoutItemGridView.Control = this.gridControl;
         this.layoutItemGridView.CustomizationFormText = "layoutItemGridView";
         this.layoutItemGridView.Location = new System.Drawing.Point(0, 74);
         this.layoutItemGridView.Name = "layoutItemGridView";
         this.layoutItemGridView.Size = new System.Drawing.Size(1627, 484);
         this.layoutItemGridView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGridView.TextVisible = false;
         // 
         // layoutGroupPanel
         // 
         this.layoutGroupPanel.CustomizationFormText = "layoutGroupPanel";
         this.layoutGroupPanel.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDelete,
            this.layoutItemIsPresent,
            this.layoutItemNegativeValuesAllowed,
            this.emptySpaceItem});
         this.layoutGroupPanel.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupPanel.Name = "layoutGroupPanel";
         this.layoutGroupPanel.Size = new System.Drawing.Size(1627, 74);
         this.layoutGroupPanel.TextVisible = false;
         // 
         // layoutItemDelete
         // 
         this.layoutItemDelete.Control = this.panelDeleteStartValues;
         this.layoutItemDelete.Location = new System.Drawing.Point(0, 0);
         this.layoutItemDelete.MaxSize = new System.Drawing.Size(377, 25);
         this.layoutItemDelete.MinSize = new System.Drawing.Size(377, 25);
         this.layoutItemDelete.Name = "layoutItemDelete";
         this.layoutItemDelete.Size = new System.Drawing.Size(377, 25);
         this.layoutItemDelete.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemDelete.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDelete.TextVisible = false;
         // 
         // layoutItemIsPresent
         // 
         this.layoutItemIsPresent.Control = this.panelIsPresent;
         this.layoutItemIsPresent.CustomizationFormText = "layoutItemIsPresent";
         this.layoutItemIsPresent.Location = new System.Drawing.Point(0, 25);
         this.layoutItemIsPresent.MaxSize = new System.Drawing.Size(377, 25);
         this.layoutItemIsPresent.MinSize = new System.Drawing.Size(377, 25);
         this.layoutItemIsPresent.Name = "layoutItemIsPresent";
         this.layoutItemIsPresent.Size = new System.Drawing.Size(754, 25);
         this.layoutItemIsPresent.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemIsPresent.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIsPresent.TextVisible = false;
         // 
         // layoutItemNegativeValuesAllowed
         // 
         this.layoutItemNegativeValuesAllowed.Control = this.panelNegativeValuesAllowed;
         this.layoutItemNegativeValuesAllowed.CustomizationFormText = "layoutItemDelete";
         this.layoutItemNegativeValuesAllowed.Location = new System.Drawing.Point(377, 0);
         this.layoutItemNegativeValuesAllowed.MaxSize = new System.Drawing.Size(377, 25);
         this.layoutItemNegativeValuesAllowed.MinSize = new System.Drawing.Size(377, 25);
         this.layoutItemNegativeValuesAllowed.Name = "layoutItemNegativeValuesAllowed";
         this.layoutItemNegativeValuesAllowed.Size = new System.Drawing.Size(377, 25);
         this.layoutItemNegativeValuesAllowed.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemNegativeValuesAllowed.Text = "layoutItemDelete";
         this.layoutItemNegativeValuesAllowed.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNegativeValuesAllowed.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(754, 0);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(849, 50);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // BasePathAndValueEntityView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "BasePathAndValueEntityView";
         this.Size = new System.Drawing.Size(1627, 558);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelDeleteStartValues)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDelete)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      protected MoBi.UI.Views.UxGridView gridView;
      protected OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemGridView;
      protected DevExpress.XtraEditors.PanelControl panelIsPresent;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIsPresent;
      private DevExpress.XtraEditors.PanelControl panelDeleteStartValues;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDelete;
      protected DevExpress.XtraEditors.PanelControl panelNegativeValuesAllowed;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNegativeValuesAllowed;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
      protected UxGridControl gridControl;
   }
}
