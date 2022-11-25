using OSPSuite.UI.Controls;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace MoBi.UI.Views
{
   partial class BaseStartValuesView<TStartValue,T> 
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
         _screenBinder.Dispose();
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
         this.checkFilterModified = new OSPSuite.UI.Controls.UxCheckEdit();
         this.panelRefreshStartValues = new DevExpress.XtraEditors.PanelControl();
         this.panelIsPresent = new DevExpress.XtraEditors.PanelControl();
         this.legendPanel = new DevExpress.XtraEditors.PanelControl();
         this.panelNegativeValuesAllowed = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemGridView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupPanel = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemRefreshStartValues = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemLegend = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCheckFilterModified = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDelete = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemIsPresent = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemNegativeValuesAllowed = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         this.checkFilterNew = new DevExpress.XtraEditors.CheckEdit();
         this.layoutItemCheckFilterNew = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelDeleteStartValues)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.checkFilterModified.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelRefreshStartValues)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRefreshStartValues)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLegend)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCheckFilterModified)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDelete)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.checkFilterNew.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCheckFilterNew)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.Location = new System.Drawing.Point(2, 99);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1623, 457);
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
         this.layoutControl.Controls.Add(this.checkFilterNew);
         this.layoutControl.Controls.Add(this.panelDeleteStartValues);
         this.layoutControl.Controls.Add(this.checkFilterModified);
         this.layoutControl.Controls.Add(this.panelRefreshStartValues);
         this.layoutControl.Controls.Add(this.panelIsPresent);
         this.layoutControl.Controls.Add(this.legendPanel);
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
         this.panelDeleteStartValues.Location = new System.Drawing.Point(863, 37);
         this.panelDeleteStartValues.Name = "panelDeleteStartValues";
         this.panelDeleteStartValues.Size = new System.Drawing.Size(373, 21);
         this.panelDeleteStartValues.TabIndex = 8;
         // 
         // checkFilterModified
         // 
         this.checkFilterModified.AllowClicksOutsideControlArea = false;
         this.checkFilterModified.Location = new System.Drawing.Point(14, 14);
         this.checkFilterModified.Name = "checkFilterModified";
         this.checkFilterModified.Properties.Caption = "checkFilterModified";
         this.checkFilterModified.Size = new System.Drawing.Size(196, 19);
         this.checkFilterModified.StyleController = this.layoutControl;
         this.checkFilterModified.TabIndex = 4;
         // 
         // panelRefreshStartValues
         // 
         this.panelRefreshStartValues.Location = new System.Drawing.Point(1240, 37);
         this.panelRefreshStartValues.Name = "panelRefreshStartValues";
         this.panelRefreshStartValues.Size = new System.Drawing.Size(373, 21);
         this.panelRefreshStartValues.TabIndex = 7;
         // 
         // panelIsPresent
         // 
         this.panelIsPresent.Location = new System.Drawing.Point(863, 62);
         this.panelIsPresent.Name = "panelIsPresent";
         this.panelIsPresent.Size = new System.Drawing.Size(373, 21);
         this.panelIsPresent.TabIndex = 6;
         // 
         // legendPanel
         // 
         this.legendPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.legendPanel.Location = new System.Drawing.Point(14, 37);
         this.legendPanel.Name = "legendPanel";
         this.legendPanel.Size = new System.Drawing.Size(386, 46);
         this.legendPanel.TabIndex = 5;
         // 
         // panelNegativeValuesAllowed
         // 
         this.panelNegativeValuesAllowed.Location = new System.Drawing.Point(1240, 62);
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
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(1627, 558);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemGridView
         // 
         this.layoutItemGridView.Control = this.gridControl;
         this.layoutItemGridView.CustomizationFormText = "layoutItemGridView";
         this.layoutItemGridView.Location = new System.Drawing.Point(0, 97);
         this.layoutItemGridView.Name = "layoutItemGridView";
         this.layoutItemGridView.Size = new System.Drawing.Size(1627, 461);
         this.layoutItemGridView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGridView.TextVisible = false;
         // 
         // layoutGroupPanel
         // 
         this.layoutGroupPanel.CustomizationFormText = "layoutGroupPanel";
         this.layoutGroupPanel.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemRefreshStartValues,
            this.layoutItemLegend,
            this.layoutItemCheckFilterModified,
            this.layoutItemDelete,
            this.layoutItemIsPresent,
            this.layoutItemNegativeValuesAllowed,
            this.emptySpaceItem,
            this.layoutItemCheckFilterNew,
            this.emptySpaceItem1});
         this.layoutGroupPanel.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupPanel.Name = "layoutGroupPanel";
         this.layoutGroupPanel.Size = new System.Drawing.Size(1627, 97);
         this.layoutGroupPanel.TextVisible = false;
         // 
         // layoutItemRefreshStartValues
         // 
         this.layoutItemRefreshStartValues.Control = this.panelRefreshStartValues;
         this.layoutItemRefreshStartValues.CustomizationFormText = "layoutItemRefreshStartValues";
         this.layoutItemRefreshStartValues.Location = new System.Drawing.Point(1226, 23);
         this.layoutItemRefreshStartValues.MaxSize = new System.Drawing.Size(377, 25);
         this.layoutItemRefreshStartValues.MinSize = new System.Drawing.Size(377, 25);
         this.layoutItemRefreshStartValues.Name = "layoutItemRefreshStartValues";
         this.layoutItemRefreshStartValues.Size = new System.Drawing.Size(377, 25);
         this.layoutItemRefreshStartValues.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemRefreshStartValues.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRefreshStartValues.TextVisible = false;
         // 
         // layoutItemLegend
         // 
         this.layoutItemLegend.Control = this.legendPanel;
         this.layoutItemLegend.CustomizationFormText = "layoutItemLegend";
         this.layoutItemLegend.Location = new System.Drawing.Point(0, 23);
         this.layoutItemLegend.MaxSize = new System.Drawing.Size(390, 50);
         this.layoutItemLegend.MinSize = new System.Drawing.Size(390, 50);
         this.layoutItemLegend.Name = "layoutItemLegend";
         this.layoutItemLegend.Size = new System.Drawing.Size(390, 50);
         this.layoutItemLegend.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemLegend.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemLegend.TextVisible = false;
         // 
         // layoutItemCheckFilterModified
         // 
         this.layoutItemCheckFilterModified.Control = this.checkFilterModified;
         this.layoutItemCheckFilterModified.CustomizationFormText = "layoutControlItem1";
         this.layoutItemCheckFilterModified.Location = new System.Drawing.Point(0, 0);
         this.layoutItemCheckFilterModified.Name = "layoutItemCheckFilterModified";
         this.layoutItemCheckFilterModified.Size = new System.Drawing.Size(200, 23);
         this.layoutItemCheckFilterModified.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCheckFilterModified.TextVisible = false;
         // 
         // layoutItemDelete
         // 
         this.layoutItemDelete.Control = this.panelDeleteStartValues;
         this.layoutItemDelete.Location = new System.Drawing.Point(849, 23);
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
         this.layoutItemIsPresent.Location = new System.Drawing.Point(849, 48);
         this.layoutItemIsPresent.MaxSize = new System.Drawing.Size(377, 25);
         this.layoutItemIsPresent.MinSize = new System.Drawing.Size(377, 25);
         this.layoutItemIsPresent.Name = "layoutItemIsPresent";
         this.layoutItemIsPresent.Size = new System.Drawing.Size(377, 25);
         this.layoutItemIsPresent.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemIsPresent.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIsPresent.TextVisible = false;
         // 
         // layoutItemNegativeValuesAllowed
         // 
         this.layoutItemNegativeValuesAllowed.Control = this.panelNegativeValuesAllowed;
         this.layoutItemNegativeValuesAllowed.CustomizationFormText = "layoutItemDelete";
         this.layoutItemNegativeValuesAllowed.Location = new System.Drawing.Point(1226, 48);
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
         this.emptySpaceItem.Location = new System.Drawing.Point(390, 23);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(459, 50);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // checkFilterNew
         // 
         this.checkFilterNew.Location = new System.Drawing.Point(214, 14);
         this.checkFilterNew.Name = "checkFilterNew";
         this.checkFilterNew.Properties.Caption = "checkFilterNew";
         this.checkFilterNew.Size = new System.Drawing.Size(196, 19);
         this.checkFilterNew.StyleController = this.layoutControl;
         this.checkFilterNew.TabIndex = 9;
         // 
         // layoutItemCheckFilterNew
         // 
         this.layoutItemCheckFilterNew.Control = this.checkFilterNew;
         this.layoutItemCheckFilterNew.Location = new System.Drawing.Point(200, 0);
         this.layoutItemCheckFilterNew.Name = "layoutItemCheckFilterNew";
         this.layoutItemCheckFilterNew.Size = new System.Drawing.Size(200, 23);
         this.layoutItemCheckFilterNew.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCheckFilterNew.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(400, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(1203, 23);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // BaseStartValuesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "BaseStartValuesView";
         this.Size = new System.Drawing.Size(1627, 558);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelDeleteStartValues)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.checkFilterModified.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelRefreshStartValues)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRefreshStartValues)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLegend)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCheckFilterModified)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDelete)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.checkFilterNew.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCheckFilterNew)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected DevExpress.XtraGrid.GridControl gridControl;
      protected MoBi.UI.Views.UxGridView gridView;
      protected OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemGridView;
      private DevExpress.XtraEditors.PanelControl legendPanel;
      private DevExpress.XtraEditors.PanelControl panelRefreshStartValues;
      protected DevExpress.XtraEditors.PanelControl panelIsPresent;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRefreshStartValues;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIsPresent;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCheckFilterModified;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLegend;
      private UxCheckEdit checkFilterModified;
      private DevExpress.XtraEditors.PanelControl panelDeleteStartValues;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDelete;
      protected DevExpress.XtraEditors.PanelControl panelNegativeValuesAllowed;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNegativeValuesAllowed;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
      private DevExpress.XtraEditors.CheckEdit checkFilterNew;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCheckFilterNew;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
