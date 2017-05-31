namespace MoBi.UI.Views
{
   partial class NotificationView
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
         this.components = new System.ComponentModel.Container();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.gridMessages = new OSPSuite.UI.Controls.UxGridControl();
         this.gridViewMessages = new MoBi.UI.Views.UxGridView();
         this._barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.menuBar = new DevExpress.XtraBars.Bar();
         this.btnErrors = new DevExpress.XtraBars.BarButtonItem();
         this.btnWarnings = new DevExpress.XtraBars.BarButtonItem();
         this.btnMessages = new DevExpress.XtraBars.BarButtonItem();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemGridView = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnDebug = new DevExpress.XtraBars.BarButtonItem();
         this.btnExportToFile = new DevExpress.XtraBars.BarButtonItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewMessages)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.gridMessages);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 29);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(707, 387);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // gridMessages
         // 
         this.gridMessages.Location = new System.Drawing.Point(2, 2);
         this.gridMessages.MainView = this.gridViewMessages;
         this.gridMessages.MenuManager = this._barManager;
         this.gridMessages.Name = "gridMessages";
         this.gridMessages.Size = new System.Drawing.Size(703, 383);
         this.gridMessages.TabIndex = 4;
         this.gridMessages.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMessages});
         // 
         // gridViewMessages
         // 
         this.gridViewMessages.GridControl = this.gridMessages;
         this.gridViewMessages.Name = "gridViewMessages";
         // 
         // _barManager
         // 
         this._barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.menuBar});
         this._barManager.DockControls.Add(this.barDockControlTop);
         this._barManager.DockControls.Add(this.barDockControlBottom);
         this._barManager.DockControls.Add(this.barDockControlLeft);
         this._barManager.DockControls.Add(this.barDockControlRight);
         this._barManager.Form = this;
         this._barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnErrors,
            this.btnWarnings,
            this.btnMessages,
            this.btnDebug,
            this.btnExportToFile});
         this._barManager.MaxItemId = 6;
         // 
         // menuBar
         // 
         this.menuBar.BarName = "Tools";
         this.menuBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
         this.menuBar.DockCol = 0;
         this.menuBar.DockRow = 0;
         this.menuBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
         this.menuBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnErrors),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnWarnings),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnMessages),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDebug),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportToFile)});
         this.menuBar.OptionsBar.AllowQuickCustomization = false;
         this.menuBar.OptionsBar.DisableClose = true;
         this.menuBar.OptionsBar.DisableCustomization = true;
         this.menuBar.OptionsBar.UseWholeRow = true;
         this.menuBar.Text = "Tools";
         // 
         // btnErrors
         // 
         this.btnErrors.Caption = "btnErrors";
         this.btnErrors.Id = 1;
         this.btnErrors.Name = "btnErrors";
         // 
         // btnWarnings
         // 
         this.btnWarnings.Caption = "btnWarnings";
         this.btnWarnings.Id = 2;
         this.btnWarnings.Name = "btnWarnings";
         // 
         // btnMessages
         // 
         this.btnMessages.Caption = "btnMessages";
         this.btnMessages.Id = 3;
         this.btnMessages.Name = "btnMessages";
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(707, 29);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 416);
         this.barDockControlBottom.Size = new System.Drawing.Size(707, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 387);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(707, 29);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 387);
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemGridView});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(707, 387);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemGridView
         // 
         this.layoutItemGridView.Control = this.gridMessages;
         this.layoutItemGridView.CustomizationFormText = "layoutItemGridView";
         this.layoutItemGridView.Location = new System.Drawing.Point(0, 0);
         this.layoutItemGridView.Name = "layoutItemGridView";
         this.layoutItemGridView.Size = new System.Drawing.Size(707, 387);
         this.layoutItemGridView.Text = "layoutItemGridView";
         this.layoutItemGridView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemGridView.TextToControlDistance = 0;
         this.layoutItemGridView.TextVisible = false;
         // 
         // btnDebug
         // 
         this.btnDebug.Caption = "btnDebug";
         this.btnDebug.Id = 4;
         this.btnDebug.Name = "barButtonItem1";
         // 
         // btnExportToFile
         // 
         this.btnExportToFile.Caption = "btnExportToFile";
         this.btnExportToFile.Id = 5;
         this.btnExportToFile.Name = "barButtonItem2";
         // 
         // NotificationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "NotificationView";
         this.Size = new System.Drawing.Size(707, 416);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridMessages)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewMessages)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGridView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraBars.BarManager _barManager;
      private DevExpress.XtraBars.Bar menuBar;
      private DevExpress.XtraBars.BarButtonItem btnErrors;
      private DevExpress.XtraBars.BarButtonItem btnWarnings;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraBars.BarButtonItem btnMessages;
      private DevExpress.XtraGrid.GridControl gridMessages;
      private MoBi.UI.Views.UxGridView gridViewMessages;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemGridView;
      private DevExpress.XtraBars.BarButtonItem btnDebug;
      private DevExpress.XtraBars.BarButtonItem btnExportToFile;
   }
}
