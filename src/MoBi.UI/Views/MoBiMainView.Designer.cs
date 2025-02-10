using DevExpress.XtraEditors;


namespace MoBi.UI.Views
{
   partial class MoBiMainView
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
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MoBiMainView));
         this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
         this.panelContainer3 = new DevExpress.XtraBars.Docking.DockPanel();
         this._panelSearch = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer3 = new DevExpress.XtraBars.Docking.ControlContainer();
         this._panelJournal = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer7 = new DevExpress.XtraBars.Docking.ControlContainer();
         this.panelContainer1 = new DevExpress.XtraBars.Docking.DockPanel();
         this._panelModuleExplorer = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer8 = new DevExpress.XtraBars.Docking.ControlContainer();
         this._panelSimulationExplorer = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer1 = new DevExpress.XtraBars.Docking.ControlContainer();
         this.panelContainer2 = new DevExpress.XtraBars.Docking.DockPanel();
         this._panelJournalDiagram = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer6 = new DevExpress.XtraBars.Docking.ControlContainer();
         this._panelHistoryBrowser = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer2 = new DevExpress.XtraBars.Docking.ControlContainer();
         this._panelWarningList = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer4 = new DevExpress.XtraBars.Docking.ControlContainer();
         this._panelComparison = new OSPSuite.UI.Controls.UxDockPanel();
         this.controlContainer5 = new DevExpress.XtraBars.Docking.ControlContainer();
         this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
         this.ribbonControl = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.applicationMenu = new DevExpress.XtraBars.Ribbon.ApplicationMenu(this.components);
         this.rightPaneAppMenu = new DevExpress.XtraBars.PopupControlContainer(this.components);
         this.panelControlAppMenuFileLabels = new DevExpress.XtraEditors.PanelControl();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
         this.panelContainer3.SuspendLayout();
         this._panelSearch.SuspendLayout();
         this._panelJournal.SuspendLayout();
         this.panelContainer1.SuspendLayout();
         this._panelModuleExplorer.SuspendLayout();
         this._panelSimulationExplorer.SuspendLayout();
         this.panelContainer2.SuspendLayout();
         this._panelJournalDiagram.SuspendLayout();
         this._panelHistoryBrowser.SuspendLayout();
         this._panelWarningList.SuspendLayout();
         this._panelComparison.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rightPaneAppMenu)).BeginInit();
         this.rightPaneAppMenu.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelControlAppMenuFileLabels)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.SuspendLayout();
         // 
         // dockManager
         // 
         this.dockManager.Form = this;
         this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.panelContainer3,
            this.panelContainer1,
            this.panelContainer2});
         this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
         // 
         // panelContainer3
         // 
         this.panelContainer3.ActiveChild = this._panelSearch;
         this.panelContainer3.Controls.Add(this._panelJournal);
         this.panelContainer3.Controls.Add(this._panelSearch);
         this.panelContainer3.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
         this.panelContainer3.ID = new System.Guid("bc71e420-92ce-49f2-82a9-a99eea92af7f");
         this.panelContainer3.Location = new System.Drawing.Point(955, 58);
         this.panelContainer3.Name = "panelContainer3";
         this.panelContainer3.OriginalSize = new System.Drawing.Size(272, 200);
         this.panelContainer3.Size = new System.Drawing.Size(272, 751);
         this.panelContainer3.Tabbed = true;
         this.panelContainer3.Text = "panelContainer3";
         // 
         // _panelSearch
         // 
         this._panelSearch.Controls.Add(this.controlContainer3);
         this._panelSearch.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelSearch.ID = new System.Guid("e6d3b85b-98f4-46b5-94f7-b1e106c56515");
         this._panelSearch.Location = new System.Drawing.Point(4, 26);
         this._panelSearch.Name = "_panelSearch";
         this._panelSearch.OriginalSize = new System.Drawing.Size(264, 699);
         this._panelSearch.Size = new System.Drawing.Size(265, 696);
         this._panelSearch.Text = "_panelSearch";
         // 
         // controlContainer3
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer3, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer3.Location = new System.Drawing.Point(0, 0);
         this.controlContainer3.Name = "controlContainer3";
         this.controlContainer3.Size = new System.Drawing.Size(265, 696);
         this.controlContainer3.TabIndex = 0;
         // 
         // _panelJournal
         // 
         this._panelJournal.Controls.Add(this.controlContainer7);
         this._panelJournal.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelJournal.ID = new System.Guid("ddba7258-95af-4806-86de-7b6330f476ac");
         this._panelJournal.Location = new System.Drawing.Point(4, 26);
         this._panelJournal.Name = "_panelJournal";
         this._panelJournal.OriginalSize = new System.Drawing.Size(264, 699);
         this._panelJournal.Size = new System.Drawing.Size(265, 696);
         this._panelJournal.Text = "_panelJournal";
         // 
         // controlContainer7
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer7, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer7.Location = new System.Drawing.Point(0, 0);
         this.controlContainer7.Name = "controlContainer7";
         this.controlContainer7.Size = new System.Drawing.Size(265, 696);
         this.controlContainer7.TabIndex = 0;
         // 
         // panelContainer1
         // 
         this.panelContainer1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
         this.panelContainer1.Appearance.Options.UseBackColor = true;
         this.panelContainer1.Controls.Add(this._panelModuleExplorer);
         this.panelContainer1.Controls.Add(this._panelSimulationExplorer);
         this.panelContainer1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
         this.panelContainer1.ID = new System.Guid("38102c06-df38-46bd-96b7-ca9a31525bba");
         this.panelContainer1.Location = new System.Drawing.Point(0, 58);
         this.panelContainer1.Name = "panelContainer1";
         this.panelContainer1.OriginalSize = new System.Drawing.Size(400, 200);
         this.panelContainer1.Size = new System.Drawing.Size(400, 751);
         this.panelContainer1.Text = "panelContainer1";
         // 
         // _panelModuleExplorer
         // 
         this._panelModuleExplorer.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
         this._panelModuleExplorer.Appearance.Options.UseBackColor = true;
         this._panelModuleExplorer.Controls.Add(this.controlContainer8);
         this._panelModuleExplorer.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelModuleExplorer.ID = new System.Guid("a18c4d16-e6c8-46db-bcf9-e4a27c56f346");
         this._panelModuleExplorer.Location = new System.Drawing.Point(0, 0);
         this._panelModuleExplorer.Name = "_panelModuleExplorer";
         this._panelModuleExplorer.OriginalSize = new System.Drawing.Size(200, 250);
         this._panelModuleExplorer.Size = new System.Drawing.Size(400, 375);
         this._panelModuleExplorer.Text = "_panelModuleExplorer";
         // 
         // controlContainer8
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer8, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer8.Location = new System.Drawing.Point(3, 26);
         this.controlContainer8.Name = "controlContainer8";
         this.controlContainer8.Size = new System.Drawing.Size(393, 345);
         this.controlContainer8.TabIndex = 0;
         // 
         // _panelSimulationExplorer
         // 
         this._panelSimulationExplorer.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
         this._panelSimulationExplorer.Appearance.Options.UseBackColor = true;
         this._panelSimulationExplorer.Controls.Add(this.controlContainer1);
         this._panelSimulationExplorer.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelSimulationExplorer.ID = new System.Guid("82f18ed9-e0cb-4b39-9b61-baa05d838a8c");
         this._panelSimulationExplorer.Location = new System.Drawing.Point(0, 375);
         this._panelSimulationExplorer.Name = "_panelSimulationExplorer";
         this._panelSimulationExplorer.OriginalSize = new System.Drawing.Size(200, 251);
         this._panelSimulationExplorer.Size = new System.Drawing.Size(400, 376);
         this._panelSimulationExplorer.Text = "_panelSimulationExplorer";
         // 
         // controlContainer1
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer1, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer1.Location = new System.Drawing.Point(3, 26);
         this.controlContainer1.Name = "controlContainer1";
         this.controlContainer1.Size = new System.Drawing.Size(393, 347);
         this.controlContainer1.TabIndex = 0;
         // 
         // panelContainer2
         // 
         this.panelContainer2.ActiveChild = this._panelJournalDiagram;
         this.panelContainer2.Controls.Add(this._panelHistoryBrowser);
         this.panelContainer2.Controls.Add(this._panelWarningList);
         this.panelContainer2.Controls.Add(this._panelComparison);
         this.panelContainer2.Controls.Add(this._panelJournalDiagram);
         this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
         this.panelContainer2.FloatVertical = true;
         this.panelContainer2.ID = new System.Guid("6dced7fd-e36c-4393-b426-bcaf8926773c");
         this.panelContainer2.Location = new System.Drawing.Point(400, 609);
         this.panelContainer2.Name = "panelContainer2";
         this.panelContainer2.OriginalSize = new System.Drawing.Size(200, 200);
         this.panelContainer2.Size = new System.Drawing.Size(555, 200);
         this.panelContainer2.Tabbed = true;
         this.panelContainer2.Text = "panelContainer2";
         // 
         // _panelJournalDiagram
         // 
         this._panelJournalDiagram.Controls.Add(this.controlContainer6);
         this._panelJournalDiagram.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelJournalDiagram.FloatVertical = true;
         this._panelJournalDiagram.ID = new System.Guid("d848fbb3-7f05-4dff-bf90-562918f8d25d");
         this._panelJournalDiagram.Location = new System.Drawing.Point(3, 27);
         this._panelJournalDiagram.Name = "_panelJournalDiagram";
         this._panelJournalDiagram.OriginalSize = new System.Drawing.Size(749, 144);
         this._panelJournalDiagram.Size = new System.Drawing.Size(549, 144);
         this._panelJournalDiagram.Text = "_panelJournalDiagram";
         // 
         // controlContainer6
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer6, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer6.Location = new System.Drawing.Point(0, 0);
         this.controlContainer6.Name = "controlContainer6";
         this.controlContainer6.Size = new System.Drawing.Size(549, 144);
         this.controlContainer6.TabIndex = 0;
         // 
         // _panelHistoryBrowser
         // 
         this._panelHistoryBrowser.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
         this._panelHistoryBrowser.Appearance.Options.UseBackColor = true;
         this._panelHistoryBrowser.Controls.Add(this.controlContainer2);
         this._panelHistoryBrowser.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelHistoryBrowser.FloatVertical = true;
         this._panelHistoryBrowser.ID = new System.Guid("7d442645-69c2-404d-aefc-4b4c044c3a48");
         this._panelHistoryBrowser.Location = new System.Drawing.Point(3, 27);
         this._panelHistoryBrowser.Name = "_panelHistoryBrowser";
         this._panelHistoryBrowser.OriginalSize = new System.Drawing.Size(749, 144);
         this._panelHistoryBrowser.Size = new System.Drawing.Size(549, 144);
         this._panelHistoryBrowser.Text = "_panelHistoryBrowser";
         // 
         // controlContainer2
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer2, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer2.Location = new System.Drawing.Point(0, 0);
         this.controlContainer2.Name = "controlContainer2";
         this.controlContainer2.Size = new System.Drawing.Size(549, 144);
         this.controlContainer2.TabIndex = 0;
         // 
         // _panelWarningList
         // 
         this._panelWarningList.Controls.Add(this.controlContainer4);
         this._panelWarningList.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelWarningList.ID = new System.Guid("1f0ce5a0-aede-4240-b15c-7191ac1dc039");
         this._panelWarningList.Location = new System.Drawing.Point(3, 27);
         this._panelWarningList.Name = "_panelWarningList";
         this._panelWarningList.OriginalSize = new System.Drawing.Size(749, 144);
         this._panelWarningList.Size = new System.Drawing.Size(549, 144);
         this._panelWarningList.Text = "_panelWarningList";
         // 
         // controlContainer4
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer4, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer4.Location = new System.Drawing.Point(0, 0);
         this.controlContainer4.Name = "controlContainer4";
         this.controlContainer4.Size = new System.Drawing.Size(549, 144);
         this.controlContainer4.TabIndex = 0;
         // 
         // _panelComparison
         // 
         this._panelComparison.Controls.Add(this.controlContainer5);
         this._panelComparison.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
         this._panelComparison.FloatVertical = true;
         this._panelComparison.ID = new System.Guid("4885d682-375e-43bc-befc-dda4c7d6dd87");
         this._panelComparison.Location = new System.Drawing.Point(3, 27);
         this._panelComparison.Name = "_panelComparison";
         this._panelComparison.OriginalSize = new System.Drawing.Size(749, 144);
         this._panelComparison.Size = new System.Drawing.Size(549, 144);
         this._panelComparison.Text = "_panelComparison";
         // 
         // controlContainer5
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.controlContainer5, DevExpress.Utils.DefaultBoolean.Default);
         this.controlContainer5.Location = new System.Drawing.Point(0, 0);
         this.controlContainer5.Name = "controlContainer5";
         this.controlContainer5.Size = new System.Drawing.Size(549, 144);
         this.controlContainer5.TabIndex = 0;
         // 
         // ribbonStatusBar1
         // 
         this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 809);
         this.ribbonStatusBar1.Name = "ribbonStatusBar1";
         this.ribbonStatusBar1.Ribbon = this.ribbonControl;
         this.ribbonStatusBar1.Size = new System.Drawing.Size(1227, 24);
         // 
         // ribbonControl
         // 
         this.ribbonControl.AllowTrimPageText = false;
         this.ribbonControl.ApplicationButtonDropDownControl = this.applicationMenu;
         this.ribbonControl.ApplicationButtonImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("ribbonControl.ApplicationButtonImageOptions.Image")));
         this.ribbonControl.ApplicationButtonText = null;
         this.ribbonControl.AutoSizeItems = true;
         this.ribbonControl.ExpandCollapseItem.Id = 0;
         this.ribbonControl.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl.SearchEditItem,
            this.ribbonControl.ExpandCollapseItem});
         this.ribbonControl.Location = new System.Drawing.Point(0, 0);
         this.ribbonControl.MaxItemId = 1;
         this.ribbonControl.Name = "ribbonControl";
         this.ribbonControl.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.Office2010;
         this.ribbonControl.Size = new System.Drawing.Size(1227, 58);
         this.ribbonControl.StatusBar = this.ribbonStatusBar1;
         // 
         // applicationMenu
         // 
         this.applicationMenu.Name = "applicationMenu";
         this.applicationMenu.Ribbon = this.ribbonControl;
         this.applicationMenu.RightPaneControlContainer = this.rightPaneAppMenu;
         this.applicationMenu.ShowRightPane = true;
         // 
         // rightPaneAppMenu
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.rightPaneAppMenu, DevExpress.Utils.DefaultBoolean.Default);
         this.rightPaneAppMenu.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.rightPaneAppMenu.Appearance.Options.UseBackColor = true;
         this.rightPaneAppMenu.AutoSize = true;
         this.rightPaneAppMenu.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.rightPaneAppMenu.Controls.Add(this.panelControlAppMenuFileLabels);
         this.rightPaneAppMenu.Controls.Add(this.labelControl1);
         this.rightPaneAppMenu.Controls.Add(this.panelControl1);
         this.rightPaneAppMenu.Location = new System.Drawing.Point(345, 223);
         this.rightPaneAppMenu.Name = "rightPaneAppMenu";
         this.rightPaneAppMenu.Ribbon = this.ribbonControl;
         this.rightPaneAppMenu.Size = new System.Drawing.Size(310, 162);
         this.rightPaneAppMenu.TabIndex = 7;
         this.rightPaneAppMenu.Visible = false;
         // 
         // panelControlAppMenuFileLabels
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.panelControlAppMenuFileLabels, DevExpress.Utils.DefaultBoolean.Default);
         this.panelControlAppMenuFileLabels.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelControlAppMenuFileLabels.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelControlAppMenuFileLabels.Location = new System.Drawing.Point(10, 19);
         this.panelControlAppMenuFileLabels.Name = "panelControlAppMenuFileLabels";
         this.panelControlAppMenuFileLabels.Size = new System.Drawing.Size(300, 143);
         this.panelControlAppMenuFileLabels.TabIndex = 2;
         // 
         // labelControl1
         // 
         this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
         this.labelControl1.Appearance.Options.UseFont = true;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Bottom;
         this.labelControl1.LineVisible = true;
         this.labelControl1.Location = new System.Drawing.Point(10, 0);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(300, 19);
         this.labelControl1.TabIndex = 0;
         this.labelControl1.Text = "Recent Documents:";
         // 
         // panelControl1
         // 
         this.defaultToolTipController.SetAllowHtmlText(this.panelControl1, DevExpress.Utils.DefaultBoolean.Default);
         this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
         this.panelControl1.Location = new System.Drawing.Point(0, 0);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(10, 162);
         this.panelControl1.TabIndex = 1;
         // 
         // MoBiMainView
         // 
         this.defaultToolTipController.SetAllowHtmlText(this, DevExpress.Utils.DefaultBoolean.Default);
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "MoBi";
         this.ClientSize = new System.Drawing.Size(1227, 833);
         this.Controls.Add(this.rightPaneAppMenu);
         this.Controls.Add(this.panelContainer2);
         this.Controls.Add(this.panelContainer1);
         this.Controls.Add(this.panelContainer3);
         this.Controls.Add(this.ribbonStatusBar1);
         this.Controls.Add(this.ribbonControl);
         this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("MoBiMainView.IconOptions.Icon")));
         this.Name = "MoBiMainView";
         this.Ribbon = this.ribbonControl;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
         this.StatusBar = this.ribbonStatusBar1;
         this.Text = "MoBi";
         this.Controls.SetChildIndex(this.ribbonControl, 0);
         this.Controls.SetChildIndex(this.ribbonStatusBar1, 0);
         this.Controls.SetChildIndex(this.panelContainer3, 0);
         this.Controls.SetChildIndex(this.panelContainer1, 0);
         this.Controls.SetChildIndex(this.panelContainer2, 0);
         this.Controls.SetChildIndex(this.rightPaneAppMenu, 0);
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabbedMdiManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
         this.panelContainer3.ResumeLayout(false);
         this._panelSearch.ResumeLayout(false);
         this._panelJournal.ResumeLayout(false);
         this.panelContainer1.ResumeLayout(false);
         this._panelModuleExplorer.ResumeLayout(false);
         this._panelSimulationExplorer.ResumeLayout(false);
         this.panelContainer2.ResumeLayout(false);
         this._panelJournalDiagram.ResumeLayout(false);
         this._panelHistoryBrowser.ResumeLayout(false);
         this._panelWarningList.ResumeLayout(false);
         this._panelComparison.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ribbonControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.applicationMenu)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rightPaneAppMenu)).EndInit();
         this.rightPaneAppMenu.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelControlAppMenuFileLabels)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraBars.Docking.DockManager dockManager;
      private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
      private DevExpress.XtraBars.Ribbon.ApplicationMenu applicationMenu;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl;
      private DevExpress.XtraBars.PopupControlContainer rightPaneAppMenu;
      private PanelControl panelControlAppMenuFileLabels;
      private LabelControl labelControl1;
      private PanelControl panelControl1;
      private DevExpress.XtraBars.Docking.DockPanel panelContainer1;
      private OSPSuite.UI.Controls.UxDockPanel _panelSimulationExplorer;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer1;
      private OSPSuite.UI.Controls.UxDockPanel _panelHistoryBrowser;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer2;
      private OSPSuite.UI.Controls.UxDockPanel _panelSearch;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer3;
      private OSPSuite.UI.Controls.UxDockPanel _panelWarningList;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer4;
      private DevExpress.XtraBars.Docking.DockPanel panelContainer2;
      private OSPSuite.UI.Controls.UxDockPanel _panelComparison;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer5;
      private OSPSuite.UI.Controls.UxDockPanel _panelJournalDiagram;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer6;
      private DevExpress.XtraBars.Docking.DockPanel panelContainer3;
      private OSPSuite.UI.Controls.UxDockPanel _panelJournal;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer7;
      private OSPSuite.UI.Controls.UxDockPanel _panelModuleExplorer;
      private DevExpress.XtraBars.Docking.ControlContainer controlContainer8;
   }
}