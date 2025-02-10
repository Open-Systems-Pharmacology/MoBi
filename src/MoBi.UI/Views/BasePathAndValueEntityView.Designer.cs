using OSPSuite.UI.Controls;

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
         this.components = new System.ComponentModel.Container();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.uxGridView1 = new OSPSuite.UI.Controls.UxGridView();
         this.ribbonPage = new DevExpress.XtraBars.Ribbon.RibbonPage();
         this.ribbonGroupEdit = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
         this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonGroupPresence = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnPresent = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotPresent = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonGroupNegativeValues = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
         this.btnAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemRibbon = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemGrid = new DevExpress.XtraLayout.LayoutControlItem();
         this.svgImageCollection = new DevExpress.Utils.SvgImageCollection(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRibbon)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection)).BeginInit();
         this.SuspendLayout();
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = null;
         this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.barDockControlTop.Size = new System.Drawing.Size(1627, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 1115);
         this.barDockControlBottom.Manager = null;
         this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.barDockControlBottom.Size = new System.Drawing.Size(1627, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Manager = null;
         this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 1115);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(1627, 0);
         this.barDockControlRight.Manager = null;
         this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 1115);
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.Location = new System.Drawing.Point(12, 145);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1603, 958);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView,
            this.uxGridView1});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.DetailHeight = 284;
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
         this.uxGridView1.DetailHeight = 284;
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
         // ribbonPage
         // 
         this.ribbonPage.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonGroupEdit,
            this.ribbonGroupPresence,
            this.ribbonGroupNegativeValues});
         this.ribbonPage.Name = "ribbonPage";
         // 
         // ribbonGroupEdit
         // 
         this.ribbonGroupEdit.ItemLinks.Add(this.btnDelete);
         this.ribbonGroupEdit.ItemLinks.Add(this.btnRefresh);
         this.ribbonGroupEdit.Name = "ribbonGroupEdit";
         this.ribbonGroupEdit.Text = "ribbonGroupEdit";
         // 
         // btnDelete
         // 
         this.btnDelete.Caption = "btnDelete";
         this.btnDelete.Id = 1;
         this.btnDelete.Name = "btnDelete";
         this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteClick);
         // 
         // btnRefresh
         // 
         this.btnRefresh.Caption = "btnRefresh";
         this.btnRefresh.Id = 2;
         this.btnRefresh.Name = "btnRefresh";
         // 
         // ribbonGroupPresence
         // 
         this.ribbonGroupPresence.ItemLinks.Add(this.btnPresent);
         this.ribbonGroupPresence.ItemLinks.Add(this.btnNotPresent);
         this.ribbonGroupPresence.Name = "ribbonGroupPresence";
         this.ribbonGroupPresence.Text = "ribbonGroupPresence";
         // 
         // btnPresent
         // 
         this.btnPresent.Caption = "btnPresent";
         this.btnPresent.Id = 3;
         this.btnPresent.Name = "btnPresent";
         // 
         // btnNotPresent
         // 
         this.btnNotPresent.Caption = "btnNotPresent";
         this.btnNotPresent.Id = 4;
         this.btnNotPresent.Name = "btnNotPresent";
         // 
         // ribbonGroupNegativeValues
         // 
         this.ribbonGroupNegativeValues.AllowTextClipping = false;
         this.ribbonGroupNegativeValues.ItemLinks.Add(this.btnAllowNegativeValues);
         this.ribbonGroupNegativeValues.ItemLinks.Add(this.btnNotAllowNegativeValues);
         this.ribbonGroupNegativeValues.Name = "ribbonGroupNegativeValues";
         this.ribbonGroupNegativeValues.Text = "ribbonGroupNegativeValues";
         // 
         // btnAllowNegativeValues
         // 
         this.btnAllowNegativeValues.Caption = "btnAllowNegativeValues";
         this.btnAllowNegativeValues.Id = 5;
         this.btnAllowNegativeValues.Name = "btnAllowNegativeValues";
         // 
         // btnNotAllowNegativeValues
         // 
         this.btnNotAllowNegativeValues.Caption = "btnNotAllowNegativeValues";
         this.btnNotAllowNegativeValues.Id = 6;
         this.btnNotAllowNegativeValues.Name = "btnNotAllowNegativeValues";
         // 
         // ribbonControl
         // 
         this.ribbonControl.AllowMinimizeRibbon = false;
         this.ribbonControl.AllowTrimPageText = false;
         this.ribbonControl.ApplicationButtonDropDownControl = this.barDockControlTop;
         this.ribbonControl.ApplicationButtonText = null;
         this.ribbonControl.AutoSizeItems = true;
         this.ribbonControl.Dock = System.Windows.Forms.DockStyle.None;
         this.ribbonControl.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(26, 24, 26, 24);
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.ExpandCollapseItem,
            this.ribbonControl.SearchEditItem,
            this.btnDelete,
            this.btnRefresh,
            this.btnPresent,
            this.btnNotPresent,
            this.btnAllowNegativeValues,
            this.btnNotAllowNegativeValues});
         this.ribbonControl.Location = new System.Drawing.Point(12, 12);
         this.ribbonControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.ribbonControl.MaxItemId = 8;
         this.ribbonControl.Name = "ribbonControl";
         this.ribbonControl.OptionsMenuMinWidth = 283;
         this.ribbonControl.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage});
         this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.MacOffice;
         this.ribbonControl.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowItemCaptionsInCaptionBar = true;
         this.ribbonControl.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
         this.ribbonControl.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
         this.ribbonControl.ShowQatLocationSelector = false;
         this.ribbonControl.ShowToolbarCustomizeItem = false;
         this.ribbonControl.Size = new System.Drawing.Size(1603, 83);
         this.ribbonControl.Toolbar.ShowCustomizeItem = false;
         this.ribbonControl.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.gridControl);
         this.uxLayoutControl1.Controls.Add(this.ribbonControl);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1603, 587, 1178, 843);
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(1627, 1115);
         this.uxLayoutControl1.TabIndex = 6;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemRibbon,
            this.layoutItemGrid});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1627, 1115);
         this.Root.TextVisible = false;
         // 
         // layoutItemRibbon
         // 
         this.layoutItemRibbon.Control = this.ribbonControl;
         this.layoutItemRibbon.Location = new System.Drawing.Point(0, 0);
         this.layoutItemRibbon.Name = "layoutItemRibbon";
         this.layoutItemRibbon.Size = new System.Drawing.Size(1607, 133);
         this.layoutItemRibbon.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment;
         this.layoutItemRibbon.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRibbon.TextVisible = false;
         // 
         // layoutItemGrid
         // 
         this.layoutItemGrid.Control = this.gridControl;
         this.layoutItemGrid.Location = new System.Drawing.Point(0, 133);
         this.layoutItemGrid.Name = "layoutItemGrid";
         this.layoutItemGrid.Size = new System.Drawing.Size(1607, 962);
         this.layoutItemGrid.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGrid.TextVisible = false;
         // 
         // svgImageCollection
         // 
         this.svgImageCollection.Add("actions_addcircled", "image://svgimages/icon builder/actions_addcircled.svg");
         this.svgImageCollection.Add("expandcollapse", "image://svgimages/outlook inspired/expandcollapse.svg");
         this.svgImageCollection.Add("actions_checkcircled", "image://svgimages/icon builder/actions_checkcircled.svg");
         this.svgImageCollection.Add("actions_deletecircled", "image://svgimages/icon builder/actions_deletecircled.svg");
         // 
         // BasePathAndValueEntityView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl1);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Margin = new System.Windows.Forms.Padding(4);
         this.Name = "BasePathAndValueEntityView";
         this.Size = new System.Drawing.Size(1627, 1115);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxGridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         this.uxLayoutControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRibbon)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection)).EndInit();
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
      private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage;
      protected DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupEdit;
      private DevExpress.XtraBars.BarButtonItem btnDelete;
      protected DevExpress.XtraBars.BarButtonItem btnRefresh;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupPresence;
      protected DevExpress.XtraBars.BarButtonItem btnPresent;
      protected DevExpress.XtraBars.BarButtonItem btnNotPresent;
      private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonGroupNegativeValues;
      protected DevExpress.XtraBars.BarButtonItem btnAllowNegativeValues;
      protected DevExpress.XtraBars.BarButtonItem btnNotAllowNegativeValues;
      protected DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
      private UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRibbon;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemGrid;
      private DevExpress.Utils.SvgImageCollection svgImageCollection;
   }
}
