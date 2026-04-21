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
         this.modelOverview = new OSPSuite.UI.Views.Diagram.BaseDiagramOverview();
         this.tabData = new DevExpress.XtraTab.XtraTabPage();
         this.tabChanges = new DevExpress.XtraTab.XtraTabPage();
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
         this.SuspendLayout();
         //
         // tabs
         //
         this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabs.Location = new System.Drawing.Point(0, 0);
         this.tabs.Name = "tabs";
         this.tabs.SelectedTabPage = this.tabSimulation;
         this.tabs.Size = new System.Drawing.Size(516, 389);
         this.tabs.TabIndex = 0;
         this.tabs.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabSimulation,
            this.tabData,
            this.tabChanges});
         //
         // tabSimulation
         //
         this.tabSimulation.Controls.Add(this.splitSimulationParameters);
         this.tabSimulation.Name = "tabSimulation";
         this.tabSimulation.Size = new System.Drawing.Size(514, 364);
         this.tabSimulation.Text = "Simulation";
         //
         // splitSimulationParameters
         //
         this.splitSimulationParameters.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitSimulationParameters.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitSimulationParameters.Location = new System.Drawing.Point(0, 0);
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
         this.splitSimulationParameters.Size = new System.Drawing.Size(514, 364);
         this.splitSimulationParameters.SplitterPosition = 257;
         this.splitSimulationParameters.TabIndex = 0;
         this.splitSimulationParameters.Text = "splitContainerControl1";
         //
         // tabsNavigation
         //
         this.tabsNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabsNavigation.Location = new System.Drawing.Point(0, 0);
         this.tabsNavigation.Name = "tabsNavigation";
         this.tabsNavigation.SelectedTabPage = this.tabTree;
         this.tabsNavigation.Size = new System.Drawing.Size(257, 364);
         this.tabsNavigation.TabIndex = 0;
         this.tabsNavigation.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabTree,
            this.tabDiagram});
         //
         // tabTree
         //
         this.tabTree.Name = "tabTree";
         this.tabTree.Size = new System.Drawing.Size(255, 339);
         this.tabTree.Text = "tabTree";
         //
         // tabDiagram
         //
         this.tabDiagram.Controls.Add(this.spliterDiagram);
         this.tabDiagram.Name = "tabDiagram";
         this.tabDiagram.Size = new System.Drawing.Size(255, 340);
         this.tabDiagram.Text = "tabDiagram";
         //
         // spliterDiagram
         //
         this.spliterDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
         this.spliterDiagram.Location = new System.Drawing.Point(0, 0);
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
         this.spliterDiagram.Size = new System.Drawing.Size(255, 340);
         this.spliterDiagram.SplitterPosition = 123;
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
         this.modelOverview.Name = "modelOverview";
         this.modelOverview.ShowsNegativeCoordinates = false;
         this.modelOverview.Size = new System.Drawing.Size(123, 340);
         this.modelOverview.TabIndex = 0;
         this.modelOverview.Text = "modelOverview";
         //
         // tabData
         //
         this.tabData.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
         this.tabData.Name = "tabData";
         this.tabData.Size = new System.Drawing.Size(514, 364);
         this.tabData.Text = "tabData";
         //
         // tabChanges
         //
         this.tabChanges.Name = "tabChanges";
         this.tabChanges.Size = new System.Drawing.Size(514, 364);
         this.tabChanges.Text = "tabChanges";
         //
         // EditSimulationView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(516, 389);
         this.Controls.Add(this.tabs);
         this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
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
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl tabs;
      private DevExpress.XtraTab.XtraTabPage tabSimulation;
      private SplitContainerControl splitSimulationParameters;
      private DevExpress.XtraTab.XtraTabControl tabsNavigation;
      private DevExpress.XtraTab.XtraTabPage tabTree;
      private DevExpress.XtraTab.XtraTabPage tabDiagram;
      private SplitContainerControl spliterDiagram;
      private OSPSuite.UI.Views.Diagram.BaseDiagramOverview modelOverview;
      private DevExpress.XtraTab.XtraTabPage tabData;
      private DevExpress.XtraTab.XtraTabPage tabChanges;
   }
}
