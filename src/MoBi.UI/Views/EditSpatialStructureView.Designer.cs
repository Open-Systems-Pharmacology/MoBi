using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MoBi.Presentation.Views;
using MoBi.UI.Views.SpaceDiagram;

namespace MoBi.UI.Views
{
   partial class EditSpatialStructureView
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
         this.splitHierarchyEdit = new DevExpress.XtraEditors.SplitContainerControl();
         this.tabsNavigation = new DevExpress.XtraTab.XtraTabControl();
         this.tabTree = new DevExpress.XtraTab.XtraTabPage();
         this.tabDiagram = new DevExpress.XtraTab.XtraTabPage();
         this.spliterDiagram = new DevExpress.XtraEditors.SplitContainerControl();
         this._diagramOverview = new Northwoods.Go.GoOverview();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         this.tabEditBuildingBlock.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitHierarchyEdit)).BeginInit();
         this.splitHierarchyEdit.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tabsNavigation)).BeginInit();
         this.tabsNavigation.SuspendLayout();
         this.tabDiagram.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram)).BeginInit();
         this.spliterDiagram.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabPagesControl
         // 
         this.tabPagesControl.Size = new System.Drawing.Size(813, 662);
         // 
         // tabEditBuildingBlock
         // 
         this.tabEditBuildingBlock.Controls.Add(this.splitHierarchyEdit);
         this.tabEditBuildingBlock.Size = new System.Drawing.Size(807, 634);
         // 
         // tabFormulaCache
         // 
         this.tabFormulaCache.Size = new System.Drawing.Size(386, 269);
         // 
         // splitHirarchieEdit
         // 
         this.splitHierarchyEdit.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitHierarchyEdit.Horizontal = false;
         this.splitHierarchyEdit.Location = new System.Drawing.Point(0, 0);
         this.splitHierarchyEdit.Name = "splitHierarchyEdit";
         this.splitHierarchyEdit.Panel1.Controls.Add(this.tabsNavigation);
         this.splitHierarchyEdit.Panel1.Text = "Panel1";
         this.splitHierarchyEdit.Panel2.Text = "Panel2";
         this.splitHierarchyEdit.Size = new System.Drawing.Size(807, 634);
         this.splitHierarchyEdit.SplitterPosition = 466;
         this.splitHierarchyEdit.TabIndex = 1;
         this.splitHierarchyEdit.Text = "splitHirarchieEdit";
         // 
         // tabsNavigation
         // 
         this.tabsNavigation.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabsNavigation.Location = new System.Drawing.Point(0, 0);
         this.tabsNavigation.Name = "tabsNavigation";
         this.tabsNavigation.SelectedTabPage = this.tabTree;
         this.tabsNavigation.Size = new System.Drawing.Size(807, 466);
         this.tabsNavigation.TabIndex = 0;
         this.tabsNavigation.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabTree,
            this.tabDiagram});
         // 
         // tabHirachic
         // 
         this.tabTree.Name = "tabTree";
         this.tabTree.Size = new System.Drawing.Size(801, 438);
         this.tabTree.Text = "xtraTabPage1";
         // 
         // tabDiagram
         // 
         this.tabDiagram.Controls.Add(this.spliterDiagram);
         this.tabDiagram.Name = "tabDiagram";
         this.tabDiagram.Size = new System.Drawing.Size(801, 438);
         this.tabDiagram.Text = "xtraTabPage2";
         // 
         // spliterDiagram
         // 
         this.spliterDiagram.Dock = System.Windows.Forms.DockStyle.Fill;
         this.spliterDiagram.Horizontal = false;
         this.spliterDiagram.Location = new System.Drawing.Point(0, 0);
         this.spliterDiagram.Name = "spliterDiagram";
         this.spliterDiagram.Panel1.Controls.Add(this._diagramOverview);
         this.spliterDiagram.Panel1.Text = "Panel1";
         this.spliterDiagram.Panel2.Text = "Panel2";
         this.spliterDiagram.Size = new System.Drawing.Size(801, 438);
         this.spliterDiagram.SplitterPosition = 96;
         this.spliterDiagram.TabIndex = 3;
         this.spliterDiagram.Text = "spliterDiagram";
         // 
         // _diagramOverview
         // 
         this._diagramOverview.AllowCopy = false;
         this._diagramOverview.AllowDelete = false;
         this._diagramOverview.AllowDragOut = false;
         this._diagramOverview.AllowDrop = false;
         this._diagramOverview.AllowEdit = false;
         this._diagramOverview.AllowInsert = false;
         this._diagramOverview.AllowLink = false;
         this._diagramOverview.ArrowMoveLarge = 10F;
         this._diagramOverview.ArrowMoveSmall = 1F;
         this._diagramOverview.BackColor = System.Drawing.Color.White;
         this._diagramOverview.Dock = System.Windows.Forms.DockStyle.Fill;
         this._diagramOverview.DocScale = 0.125F;
         this._diagramOverview.DragsRealtime = true;
         this._diagramOverview.Location = new System.Drawing.Point(0, 0);
         this._diagramOverview.Name = "_diagramOverview";
         this._diagramOverview.ShowsNegativeCoordinates = false;
         this._diagramOverview.Size = new System.Drawing.Size(801, 96);
         this._diagramOverview.TabIndex = 0;
         this._diagramOverview.Text = "goOverview1";
         // 
         // EditSpatialStructureView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "EditSpatialStructureView";
         this.ClientSize = new System.Drawing.Size(813, 662);
         this.Name = "EditSpatialStructureView";
         this.Text = "EditSpatialStructureView";
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         this.tabEditBuildingBlock.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitHierarchyEdit)).EndInit();
         this.splitHierarchyEdit.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tabsNavigation)).EndInit();
         this.tabsNavigation.ResumeLayout(false);
         this.tabDiagram.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.spliterDiagram)).EndInit();
         this.spliterDiagram.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SplitContainerControl splitHierarchyEdit;
      private XtraTabControl tabsNavigation;
      private XtraTabPage tabTree;
      private XtraTabPage tabDiagram;
      private SplitContainerControl spliterDiagram;
      private Northwoods.Go.GoOverview _diagramOverview;
   }
}