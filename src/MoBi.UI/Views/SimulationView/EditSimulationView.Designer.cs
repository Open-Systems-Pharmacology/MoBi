using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MoBi.UI.Views.SimulationView
{
   partial class EditSimulationView 
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
         tabs.TabPages.Clear();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.tabs = new DevExpress.XtraTab.XtraTabControl();
         this.tabSimulation = new DevExpress.XtraTab.XtraTabPage();
         this.splitSimulationParameters = new DevExpress.XtraEditors.SplitContainerControl();
         this.tabsNavigation = new DevExpress.XtraTab.XtraTabControl();
         this.tabTree = new DevExpress.XtraTab.XtraTabPage();
         this.tabDiagram = new DevExpress.XtraTab.XtraTabPage();
         this.spliterDiagram = new DevExpress.XtraEditors.SplitContainerControl();
         this.modelOverview = new Northwoods.Go.GoOverview();
         this.tabResults = new DevExpress.XtraTab.XtraTabPage();
         this.plotTabs = new DevExpress.XtraTab.XtraTabControl();
         this.tabData = new DevExpress.XtraTab.XtraTabPage();
         this.tabPredVsObs = new DevExpress.XtraTab.XtraTabPage();
         this.tabResidVsTime = new DevExpress.XtraTab.XtraTabPage();
         this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
         this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
         this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabs)).BeginInit();
         this.tabs.SuspendLayout();
         this.tabSimulation.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitSimulationParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitSimulationParameters.Panel1)).BeginInit();
         this.splitSimulationParameters.Panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitSimulationParameters.Panel2)).BeginInit();
         this.splitSimulationParameters.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tabsNavigation)).BeginInit();
         this.tabsNavigation.SuspendLayout();
         this.tabDiagram.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram.Panel1)).BeginInit();
         this.spliterDiagram.Panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram.Panel2)).BeginInit();
         this.spliterDiagram.SuspendLayout();
         this.tabResults.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.plotTabs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabs
         // 
         this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabs.Location = new System.Drawing.Point(0, 0);
         this.tabs.Margin = new System.Windows.Forms.Padding(8);
         this.tabs.Name = "tabs";
         this.tabs.SelectedTabPage = this.tabSimulation;
         this.tabs.Size = new System.Drawing.Size(1290, 987);
         this.tabs.TabIndex = 0;
         this.tabs.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabSimulation,
            this.tabResults,
            this.tabData,
            this.tabPredVsObs,
            this.tabResidVsTime});
         // 
         // tabSimulation
         // 
         this.tabSimulation.Controls.Add(this.splitSimulationParameters);
         this.tabSimulation.Margin = new System.Windows.Forms.Padding(8);
         this.tabSimulation.Name = "tabSimulation";
         this.tabSimulation.Size = new System.Drawing.Size(1286, 926);
         this.tabSimulation.Text = "Simulation";
         // 
         // splitSimulationParameters
         // 
         this.splitSimulationParameters.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitSimulationParameters.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitSimulationParameters.Location = new System.Drawing.Point(0, 0);
         this.splitSimulationParameters.Margin = new System.Windows.Forms.Padding(8);
         this.splitSimulationParameters.Name = "splitSimulationParameters";
         // 
         // splitSimulationParameters.Panel1
         // 
         this.splitSimulationParameters.Panel1.Controls.Add(this.tabsNavigation);
         this.splitSimulationParameters.Panel1.Text = "Panel1";
         // 
         // splitSimulationParameters.Panel2
         // 
         this.splitSimulationParameters.Panel2.Text = "Panel2";
         this.splitSimulationParameters.Size = new System.Drawing.Size(1286, 926);
         this.splitSimulationParameters.SplitterPosition = 643;
         this.splitSimulationParameters.TabIndex = 0;
         this.splitSimulationParameters.Text = "splitContainerControl1";
         // 
         // tabsNavigation
         // 
         this.tabsNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabsNavigation.Location = new System.Drawing.Point(0, 0);
         this.tabsNavigation.Margin = new System.Windows.Forms.Padding(8);
         this.tabsNavigation.Name = "tabsNavigation";
         this.tabsNavigation.SelectedTabPage = this.tabTree;
         this.tabsNavigation.Size = new System.Drawing.Size(643, 926);
         this.tabsNavigation.TabIndex = 0;
         this.tabsNavigation.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabTree,
            this.tabDiagram});
         // 
         // tabTree
         // 
         this.tabTree.Margin = new System.Windows.Forms.Padding(8);
         this.tabTree.Name = "tabTree";
         this.tabTree.Size = new System.Drawing.Size(639, 865);
         this.tabTree.Text = "tabTree";
         // 
         // tabDiagram
         // 
         this.tabDiagram.Controls.Add(this.spliterDiagram);
         this.tabDiagram.Margin = new System.Windows.Forms.Padding(8);
         this.tabDiagram.Name = "tabDiagram";
         this.tabDiagram.Size = new System.Drawing.Size(639, 865);
         this.tabDiagram.Text = "tabDiagram";
         // 
         // spliterDiagram
         // 
         this.spliterDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
         this.spliterDiagram.Location = new System.Drawing.Point(0, 0);
         this.spliterDiagram.Margin = new System.Windows.Forms.Padding(8);
         this.spliterDiagram.Name = "spliterDiagram";
         // 
         // spliterDiagram.Panel1
         // 
         this.spliterDiagram.Panel1.Controls.Add(this.modelOverview);
         this.spliterDiagram.Panel1.Text = "Panel1";
         // 
         // spliterDiagram.Panel2
         // 
         this.spliterDiagram.Panel2.Text = "Panel2";
         this.spliterDiagram.Size = new System.Drawing.Size(639, 865);
         this.spliterDiagram.SplitterPosition = 307;
         this.spliterDiagram.TabIndex = 1;
         this.spliterDiagram.Text = "splitContainerControl2";
         // 
         // modelOverview
         // 
         this.modelOverview.AllowCopy = false;
         this.modelOverview.AllowDelete = false;
         this.modelOverview.AllowDragOut = false;
         this.modelOverview.AllowDrop = false;
         this.modelOverview.AllowEdit = false;
         this.modelOverview.AllowInsert = false;
         this.modelOverview.AllowLink = false;
         this.modelOverview.ArrowMoveLarge = 10F;
         this.modelOverview.ArrowMoveSmall = 1F;
         this.modelOverview.BackColor = System.Drawing.Color.White;
         this.modelOverview.Dock = System.Windows.Forms.DockStyle.Fill;
         this.modelOverview.DocScale = 0.125F;
         this.modelOverview.DragsRealtime = true;
         this.modelOverview.Location = new System.Drawing.Point(0, 0);
         this.modelOverview.Margin = new System.Windows.Forms.Padding(8);
         this.modelOverview.Name = "modelOverview";
         this.modelOverview.ShowsNegativeCoordinates = false;
         this.modelOverview.Size = new System.Drawing.Size(307, 865);
         this.modelOverview.TabIndex = 0;
         this.modelOverview.Text = "modelOverview";
         // 
         // tabResults
         // 
         this.tabResults.Controls.Add(this.plotTabs);
         this.tabResults.Margin = new System.Windows.Forms.Padding(8);
         this.tabResults.Name = "tabResults";
         this.tabResults.Size = new System.Drawing.Size(1286, 926);
         this.tabResults.Text = "tabResults";
         // 
         // plotTabs
         // 
         this.plotTabs.Dock = System.Windows.Forms.DockStyle.Fill;
         this.plotTabs.Location = new System.Drawing.Point(0, 0);
         this.plotTabs.Name = "plotTabs";
         this.plotTabs.Size = new System.Drawing.Size(1286, 926);
         this.plotTabs.TabIndex = 0;
         // 
         // tabData
         // 
         this.tabData.Name = "tabData";
         this.tabData.Size = new System.Drawing.Size(1286, 926);
         this.tabData.Text = "tabData";
         // 
         // tabPredVsObs
         // 
         this.tabPredVsObs.Name = "tabPredVsObs";
         this.tabPredVsObs.Size = new System.Drawing.Size(1286, 926);
         this.tabPredVsObs.Text = "tabPredVsObs";
         // 
         // tabResidVsTime
         // 
         this.tabResidVsTime.Name = "tabResidVsTime";
         this.tabResidVsTime.Size = new System.Drawing.Size(1286, 926);
         this.tabResidVsTime.Text = "tabResidVsTime";
         // 
         // xtraTabPage2
         // 
         this.xtraTabPage2.Name = "xtraTabPage2";
         this.xtraTabPage2.Size = new System.Drawing.Size(296, 239);
         this.xtraTabPage2.Text = "xtraTabPage2";
         // 
         // xtraTabPage1
         // 
         this.xtraTabPage1.Name = "xtraTabPage1";
         this.xtraTabPage1.Size = new System.Drawing.Size(296, 239);
         this.xtraTabPage1.Text = "xtraTabPage1";
         // 
         // xtraTabControl1
         // 
         this.xtraTabControl1.Location = new System.Drawing.Point(256, 17);
         this.xtraTabControl1.Name = "xtraTabControl1";
         this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
         this.xtraTabControl1.Size = new System.Drawing.Size(300, 300);
         this.xtraTabControl1.TabIndex = 2;
         this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
         // 
         // EditSimulationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1290, 987);
         this.Controls.Add(this.tabs);
         this.Margin = new System.Windows.Forms.Padding(20);
         this.Name = "EditSimulationView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabs)).EndInit();
         this.tabs.ResumeLayout(false);
         this.tabSimulation.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitSimulationParameters.Panel1)).EndInit();
         this.splitSimulationParameters.Panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitSimulationParameters.Panel2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitSimulationParameters)).EndInit();
         this.splitSimulationParameters.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tabsNavigation)).EndInit();
         this.tabsNavigation.ResumeLayout(false);
         this.tabDiagram.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram.Panel1)).EndInit();
         this.spliterDiagram.Panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram.Panel2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram)).EndInit();
         this.spliterDiagram.ResumeLayout(false);
         this.tabResults.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.plotTabs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
         this.xtraTabControl1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl tabs;
      private DevExpress.XtraTab.XtraTabPage tabSimulation;
      private DevExpress.XtraTab.XtraTabPage tabResults;
      private SplitContainerControl splitSimulationParameters;
      private DevExpress.XtraTab.XtraTabControl tabsNavigation;
      private DevExpress.XtraTab.XtraTabPage tabTree;
      private DevExpress.XtraTab.XtraTabPage tabDiagram;
      private SplitContainerControl spliterDiagram;
      private Northwoods.Go.GoOverview modelOverview;
      private DevExpress.XtraTab.XtraTabControl plotTabs;
      private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
      private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
      private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
      private DevExpress.XtraTab.XtraTabPage tabData;
      private DevExpress.XtraTab.XtraTabPage tabPredVsObs;
      private DevExpress.XtraTab.XtraTabPage tabResidVsTime;
   }
}
