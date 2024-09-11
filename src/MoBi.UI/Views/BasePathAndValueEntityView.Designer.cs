using OSPSuite.UI.Controls;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using MoBi.UI.Properties;

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
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.uxGridView1 = new OSPSuite.UI.Controls.UxGridView();
         this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
         this.ribbonGroupEdit = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
         this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonGroupPresence = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnPresent = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotPresent = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonGroupNegativeValues = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.lc1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.layoutItemGridView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.lc1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = null;
         this.barDockControlTop.Size = new System.Drawing.Size(1898, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 687);
         this.barDockControlBottom.Manager = null;
         this.barDockControlBottom.Size = new System.Drawing.Size(1898, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Manager = null;
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 687);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(1898, 0);
         this.barDockControlRight.Manager = null;
         this.barDockControlRight.Size = new System.Drawing.Size(0, 687);
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
         this.gridControl.Location = new System.Drawing.Point(2, 129);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Margin = new System.Windows.Forms.Padding(4);
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1894, 556);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView,
            this.uxGridView1});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = true;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsSelection.MultiSelect = true;
         // 
         // uxGridView1
         // 
         this.uxGridView1.AllowsFiltering = true;
         this.uxGridView1.EnableColumnContextMenu = true;
         this.uxGridView1.GridControl = this.gridControl;
         this.uxGridView1.MultiSelect = true;
         this.uxGridView1.Name = "uxGridView1";
         this.uxGridView1.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.uxGridView1.OptionsNavigation.AutoFocusNewRow = true;
         this.uxGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.uxGridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.uxGridView1.OptionsSelection.MultiSelect = true;
         this.uxGridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // ribbonPage1
         // 
         this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonGroupEdit,
            this.ribbonGroupPresence,
            this.ribbonGroupNegativeValues});
         this.ribbonPage1.Name = "ribbonPage1";
         // 
         // ribbonGroupEdit
         // 
         this.ribbonGroupEdit.ItemLinks.Add(this.btnDelete);
         this.ribbonGroupEdit.ItemLinks.Add(this.btnRefresh);
         this.ribbonGroupEdit.Name = "ribbonGroupEdit";
         this.ribbonGroupEdit.Text = "Edit";
         // 
         // btnDelete
         // 
         this.btnDelete.Caption = "Delete";
         this.btnDelete.Id = 1;
         this.btnDelete.Name = "btnDelete";
         this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
         // 
         // btnRefresh
         // 
         this.btnRefresh.Caption = "Refresh";
         this.btnRefresh.Id = 2;
         this.btnRefresh.Name = "btnRefresh";
         this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
         // 
         // ribbonGroupPresence
         // 
         this.ribbonGroupPresence.ItemLinks.Add(this.btnPresent);
         this.ribbonGroupPresence.ItemLinks.Add(this.btnNotPresent);
         this.ribbonGroupPresence.Name = "ribbonGroupPresence";
         this.ribbonGroupPresence.Text = "Presence";
         // 
         // btnPresent
         // 
         this.btnPresent.Caption = "Mark as Present";
         this.btnPresent.Id = 3;
         this.btnPresent.Name = "btnPresent";
         this.btnPresent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPresent_ItemClick);
         // 
         // btnNotPresent
         // 
         this.btnNotPresent.Caption = "Mark as Not Present";
         this.btnNotPresent.Id = 4;
         this.btnNotPresent.Name = "btnNotPresent";
         this.btnNotPresent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNotPresent_ItemClick);
         // 
         // ribbonGroupNegativeValues
         // 
         this.ribbonGroupNegativeValues.AllowTextClipping = false;
         this.ribbonGroupNegativeValues.ItemLinks.Add(this.btnAllowNegativeValues);
         this.ribbonGroupNegativeValues.ItemLinks.Add(this.btnNotAllowNegativeValues);
         this.ribbonGroupNegativeValues.Name = "ribbonGroupNegativeValues";
         this.ribbonGroupNegativeValues.Text = "Negative Values";
         // 
         // btnAllowNegativeValues
         // 
         this.btnAllowNegativeValues.Caption = "Allow";
         this.btnAllowNegativeValues.Id = 5;
         this.btnAllowNegativeValues.Name = "btnAllowNegativeValues";
         this.btnAllowNegativeValues.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAllowNegativeValues_ItemClick);
         // 
         // btnNotAllowNegativeValues
         // 
         this.btnNotAllowNegativeValues.Caption = "Dissallow";
         this.btnNotAllowNegativeValues.Id = 6;
         this.btnNotAllowNegativeValues.Name = "btnNotAllowNegativeValues";
         this.btnNotAllowNegativeValues.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNotAllowNegativeValues_ItemClick);
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "Root";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutItemGridView});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(1898, 687);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "MenuRoot";
         this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup2.GroupBordersVisible = false;
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lc1});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "MenuRoot";
         this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup2.Size = new System.Drawing.Size(1898, 127);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // lc1
         // 
         this.lc1.Control = this.ribbonControl1;
         this.lc1.Location = new System.Drawing.Point(0, 0);
         this.lc1.Name = "lc1";
         this.lc1.Size = new System.Drawing.Size(1894, 123);
         this.lc1.TextSize = new System.Drawing.Size(0, 0);
         this.lc1.TextVisible = false;
         // 
         // ribbonControl1
         // 
         this.ribbonControl1.AllowMinimizeRibbon = false;
         this.ribbonControl1.AllowTrimPageText = false;
         this.ribbonControl1.ApplicationButtonText = null;
         this.ribbonControl1.AutoSizeItems = true;
         this.ribbonControl1.ButtonGroupsLayout = DevExpress.XtraBars.ButtonGroupsLayout.TwoRows;
         this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
         this.ribbonControl1.ExpandCollapseItem.Id = 0;
         this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.btnDelete,
            this.btnRefresh,
            this.btnPresent,
            this.btnNotPresent,
            this.btnAllowNegativeValues,
            this.btnNotAllowNegativeValues});
         this.ribbonControl1.Location = new System.Drawing.Point(4, 4);
         this.ribbonControl1.MaxItemId = 8;
         this.ribbonControl1.Name = "ribbonControl1";
         this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
         this.ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
         this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl1.ShowItemCaptionsInCaptionBar = true;
         this.ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
         this.ribbonControl1.ShowQatLocationSelector = false;
         this.ribbonControl1.ShowToolbarCustomizeItem = false;
         this.ribbonControl1.Size = new System.Drawing.Size(1890, 122);
         this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
         this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
         // 
         // layoutItemGridView
         // 
         this.layoutItemGridView.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.layoutItemGridView.ContentVertAlignment = DevExpress.Utils.VertAlignment.Top;
         this.layoutItemGridView.Control = this.gridControl;
         this.layoutItemGridView.CustomizationFormText = "layoutItemGridView";
         this.layoutItemGridView.Location = new System.Drawing.Point(0, 127);
         this.layoutItemGridView.Name = "layoutItemGridView";
         this.layoutItemGridView.Size = new System.Drawing.Size(1898, 560);
         this.layoutItemGridView.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment;
         this.layoutItemGridView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGridView.TextVisible = false;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Controls.Add(this.ribbonControl1);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(4);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(379, -1065, 1628, 744);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(1898, 687);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // BasePathAndValueEntityView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Margin = new System.Windows.Forms.Padding(5);
         this.Name = "BasePathAndValueEntityView";
         this.Size = new System.Drawing.Size(1898, 687);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.lc1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         this.layoutControl.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      protected UxGridControl gridControl;
      protected OSPSuite.UI.Controls.UxGridView gridView;
      private OSPSuite.UI.Controls.UxGridView uxGridView1;
      private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupEdit;
      private DevExpress.XtraBars.BarButtonItem btnDelete;
      private DevExpress.XtraBars.BarButtonItem btnRefresh;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupPresence;
      private DevExpress.XtraBars.BarButtonItem btnPresent;
      private DevExpress.XtraBars.BarButtonItem btnNotPresent;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupNegativeValues;
      private DevExpress.XtraBars.BarButtonItem btnAllowNegativeValues;
      private DevExpress.XtraBars.BarButtonItem btnNotAllowNegativeValues;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemGridView;
      private DevExpress.XtraLayout.LayoutControlItem lc1;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      protected UxLayoutControl layoutControl;
   }
}
