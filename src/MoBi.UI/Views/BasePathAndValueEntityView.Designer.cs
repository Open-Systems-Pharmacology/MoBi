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
         this.panelNegativeValuesNotAllowed = new DevExpress.XtraEditors.PanelControl();
         this.panelNegativeValuesAllowed = new DevExpress.XtraEditors.PanelControl();
         this.panelIsNotPresent = new DevExpress.XtraEditors.PanelControl();
         this.panelDeleteStartValues = new DevExpress.XtraEditors.PanelControl();
         this.panelRefresh = new DevExpress.XtraEditors.PanelControl();
         this.panelIsPresent = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemGridView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupPanel = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDelete = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemIsPresent = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRefresh = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemIsNotPresent = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemNegativeValuesAllowed = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemNegativeValuesNotAllowed = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesNotAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsNotPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDeleteStartValues)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelRefresh)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDelete)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRefresh)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsNotPresent)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesNotAllowed)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
         this.gridControl.Location = new System.Drawing.Point(2, 112);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Margin = new System.Windows.Forms.Padding(4);
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1894, 573);
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
         this.layoutControl.Controls.Add(this.panelControl1);
         this.layoutControl.Controls.Add(this.panelNegativeValuesNotAllowed);
         this.layoutControl.Controls.Add(this.panelNegativeValuesAllowed);
         this.layoutControl.Controls.Add(this.panelIsNotPresent);
         this.layoutControl.Controls.Add(this.panelDeleteStartValues);
         this.layoutControl.Controls.Add(this.panelRefresh);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Controls.Add(this.panelIsPresent);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(4);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(309, -1010, 1628, 744);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(1898, 687);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelNegativeValuesNotAllowed
         // 
         this.panelNegativeValuesNotAllowed.Location = new System.Drawing.Point(894, 69);
         this.panelNegativeValuesNotAllowed.Margin = new System.Windows.Forms.Padding(4);
         this.panelNegativeValuesNotAllowed.Name = "panelNegativeValuesNotAllowed";
         this.panelNegativeValuesNotAllowed.Size = new System.Drawing.Size(436, 27);
         this.panelNegativeValuesNotAllowed.TabIndex = 10;
         // 
         // panelNegativeValuesAllowed
         // 
         this.panelNegativeValuesAllowed.Location = new System.Drawing.Point(894, 38);
         this.panelNegativeValuesAllowed.Margin = new System.Windows.Forms.Padding(4);
         this.panelNegativeValuesAllowed.Name = "panelNegativeValuesAllowed";
         this.panelNegativeValuesAllowed.Size = new System.Drawing.Size(436, 27);
         this.panelNegativeValuesAllowed.TabIndex = 10;
         // 
         // panelIsNotPresent
         // 
         this.panelIsNotPresent.Location = new System.Drawing.Point(454, 69);
         this.panelIsNotPresent.Margin = new System.Windows.Forms.Padding(4);
         this.panelIsNotPresent.Name = "panelIsNotPresent";
         this.panelIsNotPresent.Size = new System.Drawing.Size(436, 27);
         this.panelIsNotPresent.TabIndex = 9;
         // 
         // panelDeleteStartValues
         // 
         this.panelDeleteStartValues.Location = new System.Drawing.Point(14, 38);
         this.panelDeleteStartValues.Margin = new System.Windows.Forms.Padding(4);
         this.panelDeleteStartValues.Name = "panelDeleteStartValues";
         this.panelDeleteStartValues.Size = new System.Drawing.Size(436, 27);
         this.panelDeleteStartValues.TabIndex = 8;
         // 
         // panelRefresh
         // 
         this.panelRefresh.Location = new System.Drawing.Point(14, 69);
         this.panelRefresh.Margin = new System.Windows.Forms.Padding(4);
         this.panelRefresh.Name = "panelRefresh";
         this.panelRefresh.Size = new System.Drawing.Size(436, 27);
         this.panelRefresh.TabIndex = 6;
         // 
         // panelIsPresent
         // 
         this.panelIsPresent.Location = new System.Drawing.Point(454, 38);
         this.panelIsPresent.Margin = new System.Windows.Forms.Padding(4);
         this.panelIsPresent.Name = "panelIsPresent";
         this.panelIsPresent.Size = new System.Drawing.Size(436, 27);
         this.panelIsPresent.TabIndex = 8;
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
         this.layoutControlGroup1.Size = new System.Drawing.Size(1898, 687);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemGridView
         // 
         this.layoutItemGridView.Control = this.gridControl;
         this.layoutItemGridView.CustomizationFormText = "layoutItemGridView";
         this.layoutItemGridView.Location = new System.Drawing.Point(0, 110);
         this.layoutItemGridView.Name = "layoutItemGridView";
         this.layoutItemGridView.Size = new System.Drawing.Size(1898, 577);
         this.layoutItemGridView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGridView.TextVisible = false;
         // 
         // layoutGroupPanel
         // 
         this.layoutGroupPanel.CustomizationFormText = "layoutGroupPanel";
         this.layoutGroupPanel.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDelete,
            this.layoutItemIsPresent,
            this.layoutItemRefresh,
            this.layoutItemIsNotPresent,
            this.layoutItemNegativeValuesAllowed,
            this.layoutItemNegativeValuesNotAllowed,
            this.emptySpaceItem,
            this.layoutControlItem1});
         this.layoutGroupPanel.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupPanel.Name = "layoutGroupPanel";
         this.layoutGroupPanel.Size = new System.Drawing.Size(1898, 110);
         this.layoutGroupPanel.TextVisible = false;
         // 
         // layoutItemDelete
         // 
         this.layoutItemDelete.Control = this.panelDeleteStartValues;
         this.layoutItemDelete.Location = new System.Drawing.Point(0, 24);
         this.layoutItemDelete.MaxSize = new System.Drawing.Size(440, 31);
         this.layoutItemDelete.MinSize = new System.Drawing.Size(220, 31);
         this.layoutItemDelete.Name = "layoutItemDelete";
         this.layoutItemDelete.Size = new System.Drawing.Size(440, 31);
         this.layoutItemDelete.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemDelete.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDelete.TextVisible = false;
         // 
         // layoutItemIsPresent
         // 
         this.layoutItemIsPresent.Control = this.panelIsPresent;
         this.layoutItemIsPresent.CustomizationFormText = "layoutItemDelete";
         this.layoutItemIsPresent.Location = new System.Drawing.Point(440, 24);
         this.layoutItemIsPresent.MaxSize = new System.Drawing.Size(440, 31);
         this.layoutItemIsPresent.MinSize = new System.Drawing.Size(220, 31);
         this.layoutItemIsPresent.Name = "layoutItemIsPresent";
         this.layoutItemIsPresent.Size = new System.Drawing.Size(440, 31);
         this.layoutItemIsPresent.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemIsPresent.Text = "layoutItemDelete";
         this.layoutItemIsPresent.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIsPresent.TextVisible = false;
         // 
         // layoutItemRefresh
         // 
         this.layoutItemRefresh.Control = this.panelRefresh;
         this.layoutItemRefresh.CustomizationFormText = "layoutItemIsPresent";
         this.layoutItemRefresh.Location = new System.Drawing.Point(0, 55);
         this.layoutItemRefresh.MaxSize = new System.Drawing.Size(440, 31);
         this.layoutItemRefresh.MinSize = new System.Drawing.Size(220, 31);
         this.layoutItemRefresh.Name = "layoutItemRefresh";
         this.layoutItemRefresh.Size = new System.Drawing.Size(440, 31);
         this.layoutItemRefresh.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemRefresh.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRefresh.TextVisible = false;
         // 
         // layoutItemIsNotPresent
         // 
         this.layoutItemIsNotPresent.Control = this.panelIsNotPresent;
         this.layoutItemIsNotPresent.Location = new System.Drawing.Point(440, 55);
         this.layoutItemIsNotPresent.MaxSize = new System.Drawing.Size(440, 31);
         this.layoutItemIsNotPresent.MinSize = new System.Drawing.Size(220, 31);
         this.layoutItemIsNotPresent.Name = "layoutItemIsNotPresent";
         this.layoutItemIsNotPresent.Size = new System.Drawing.Size(440, 31);
         this.layoutItemIsNotPresent.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemIsNotPresent.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIsNotPresent.TextVisible = false;
         // 
         // layoutItemNegativeValuesAllowed
         // 
         this.layoutItemNegativeValuesAllowed.Control = this.panelNegativeValuesAllowed;
         this.layoutItemNegativeValuesAllowed.Location = new System.Drawing.Point(880, 24);
         this.layoutItemNegativeValuesAllowed.MaxSize = new System.Drawing.Size(440, 31);
         this.layoutItemNegativeValuesAllowed.MinSize = new System.Drawing.Size(220, 31);
         this.layoutItemNegativeValuesAllowed.Name = "layoutItemNegativeValuesAllowed";
         this.layoutItemNegativeValuesAllowed.Size = new System.Drawing.Size(440, 31);
         this.layoutItemNegativeValuesAllowed.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemNegativeValuesAllowed.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNegativeValuesAllowed.TextVisible = false;
         // 
         // layoutItemNegativeValuesNotAllowed
         // 
         this.layoutItemNegativeValuesNotAllowed.Control = this.panelNegativeValuesNotAllowed;
         this.layoutItemNegativeValuesNotAllowed.Location = new System.Drawing.Point(880, 55);
         this.layoutItemNegativeValuesNotAllowed.MaxSize = new System.Drawing.Size(440, 31);
         this.layoutItemNegativeValuesNotAllowed.MinSize = new System.Drawing.Size(220, 31);
         this.layoutItemNegativeValuesNotAllowed.Name = "layoutItemNegativeValuesNotAllowed";
         this.layoutItemNegativeValuesNotAllowed.Size = new System.Drawing.Size(440, 31);
         this.layoutItemNegativeValuesNotAllowed.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemNegativeValuesNotAllowed.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNegativeValuesNotAllowed.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(1320, 24);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(554, 62);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelControl1;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1874, 24);
         this.layoutControlItem1.Text = "Allowed actions on selected values";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(198, 16);
         // 
         // panelControl1
         // 
         this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelControl1.Location = new System.Drawing.Point(224, 14);
         this.panelControl1.Margin = new System.Windows.Forms.Padding(4);
         this.panelControl1.MinimumSize = new System.Drawing.Size(60, 20);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(1660, 20);
         this.panelControl1.TabIndex = 11;
         this.panelControl1.Visible = false;
         // 
         // BasePathAndValueEntityView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(5);
         this.Name = "BasePathAndValueEntityView";
         this.Size = new System.Drawing.Size(1898, 687);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesNotAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelNegativeValuesAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsNotPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDeleteStartValues)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelRefresh)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelIsPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDelete)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRefresh)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsNotPresent)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNegativeValuesNotAllowed)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      protected MoBi.UI.Views.UxGridView gridView;
      protected OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemGridView;
      protected DevExpress.XtraEditors.PanelControl panelRefresh;
      private DevExpress.XtraEditors.PanelControl panelDeleteStartValues;
      protected DevExpress.XtraEditors.PanelControl panelIsPresent;
      protected UxGridControl gridControl;
      protected DevExpress.XtraEditors.PanelControl panelIsNotPresent;
      protected DevExpress.XtraEditors.PanelControl panelNegativeValuesNotAllowed;
      protected DevExpress.XtraEditors.PanelControl panelNegativeValuesAllowed;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDelete;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIsPresent;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRefresh;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIsNotPresent;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNegativeValuesAllowed;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemNegativeValuesNotAllowed;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
      protected DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}
