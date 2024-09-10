using OSPSuite.UI.Controls;
using DevExpress.XtraLayout.Utils;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain.Builder;
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
         this.components = new System.ComponentModel.Container();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemGridView = new DevExpress.XtraLayout.LayoutControlItem();
         this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
         this.bar1 = new DevExpress.XtraBars.Bar();
         this.btnDelete = new DevExpress.XtraBars.BarButtonItem();
         this.btnRefresh = new DevExpress.XtraBars.BarLargeButtonItem();
         this.btnPresent = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotPresent = new DevExpress.XtraBars.BarButtonItem();
         this.btnAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.btnNotAllowNegativeValues = new DevExpress.XtraBars.BarButtonItem();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5);
         this.gridControl.Location = new System.Drawing.Point(2, 2);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Margin = new System.Windows.Forms.Padding(4);
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(1894, 658);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
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
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 25);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(4);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(309, -1010, 1628, 744);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(1898, 662);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "Root";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemGridView});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(1898, 662);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemGridView
         // 
         this.layoutItemGridView.Control = this.gridControl;
         this.layoutItemGridView.CustomizationFormText = "layoutItemGridView";
         this.layoutItemGridView.Location = new System.Drawing.Point(0, 0);
         this.layoutItemGridView.Name = "layoutItemGridView";
         this.layoutItemGridView.Size = new System.Drawing.Size(1898, 662);
         this.layoutItemGridView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGridView.TextVisible = false;
         // 
         // barManager1
         // 
         this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
         this.barManager1.DockControls.Add(this.barDockControlTop);
         this.barManager1.DockControls.Add(this.barDockControlBottom);
         this.barManager1.DockControls.Add(this.barDockControlLeft);
         this.barManager1.DockControls.Add(this.barDockControlRight);
         this.barManager1.Form = this;
         this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnDelete,
            this.btnRefresh,
            this.btnPresent,
            this.btnNotPresent,
            this.btnAllowNegativeValues,
            this.btnNotAllowNegativeValues});
         this.barManager1.MaxItemId = 6;
         // 
         // bar1
         // 
         this.bar1.BarName = "Tools";
         this.bar1.DockCol = 0;
         this.bar1.DockRow = 0;
         this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
         this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDelete, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnRefresh, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnPresent, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnNotPresent, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAllowNegativeValues, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnNotAllowNegativeValues, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
         this.bar1.OptionsBar.AllowQuickCustomization = false;
         this.bar1.OptionsBar.DisableClose = true;
         this.bar1.OptionsBar.DisableCustomization = true;
         this.bar1.OptionsBar.UseWholeRow = true;
         this.bar1.Text = "Tools";
         // 
         // btnDelete
         // 
         this.btnDelete.Caption = "Delete";
         this.btnDelete.Id = 0;
         this.btnDelete.Name = "btnDelete";
         this.btnDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDelete_ItemClick);
         // 
         // btnRefresh
         // 
         this.btnRefresh.Caption = "Refresh";
         this.btnRefresh.CaptionAlignment = DevExpress.XtraBars.BarItemCaptionAlignment.Right;
         this.btnRefresh.Id = 1;
         this.btnRefresh.Name = "btnRefresh";
         this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
         // 
         // btnPresent
         // 
         this.btnPresent.Caption = "Mark as Present";
         this.btnPresent.Id = 2;
         this.btnPresent.Name = "btnPresent";
         this.btnPresent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnPresent_ItemClick);
         // 
         // btnNotPresent
         // 
         this.btnNotPresent.Caption = "Mark as not present";
         this.btnNotPresent.Id = 3;
         this.btnNotPresent.Name = "btnNotPresent";
         this.btnNotPresent.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNotPresent_ItemClick);
         // 
         // btnAllowNegativeValues
         // 
         this.btnAllowNegativeValues.Caption = "Allow Negativa Values";
         this.btnAllowNegativeValues.Id = 4;
         this.btnAllowNegativeValues.Name = "btnAllowNegativeValues";
         this.btnAllowNegativeValues.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAllowNegativeValues_ItemClick);
         // 
         // btnNotAllowNegativeValues
         // 
         this.btnNotAllowNegativeValues.Caption = "Not Allow Negative Values";
         this.btnNotAllowNegativeValues.Id = 5;
         this.btnNotAllowNegativeValues.Name = "btnNotAllowNegativeValues";
         this.btnNotAllowNegativeValues.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnNotAllowNegativeValues_ItemClick);
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = this.barManager1;
         this.barDockControlTop.Size = new System.Drawing.Size(1898, 25);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 687);
         this.barDockControlBottom.Manager = this.barManager1;
         this.barDockControlBottom.Size = new System.Drawing.Size(1898, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 25);
         this.barDockControlLeft.Manager = this.barManager1;
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 662);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(1898, 25);
         this.barDockControlRight.Manager = this.barManager1;
         this.barDockControlRight.Size = new System.Drawing.Size(0, 662);
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion
      protected OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemGridView;
      protected UxGridControl gridControl;
      protected DevExpress.XtraBars.BarManager barManager1;
      protected DevExpress.XtraBars.Bar bar1;
      protected DevExpress.XtraBars.BarButtonItem btnDelete;
      protected DevExpress.XtraBars.BarLargeButtonItem btnRefresh;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      protected DevExpress.XtraBars.BarButtonItem btnPresent;
      protected DevExpress.XtraBars.BarButtonItem btnNotPresent;
      protected DevExpress.XtraBars.BarButtonItem btnAllowNegativeValues;
      protected DevExpress.XtraBars.BarButtonItem btnNotAllowNegativeValues;
      protected OSPSuite.UI.Controls.UxGridView gridView;
   }
}
