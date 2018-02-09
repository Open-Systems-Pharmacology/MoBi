using MoBi.UI.Views.ReactionDiagram;

namespace MoBi.UI.Views
{
   partial class EditReactionBuildingBlockView
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
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         this.tabOverviewControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabFlowChart = new DevExpress.XtraTab.XtraTabPage();
         this.tabList = new DevExpress.XtraTab.XtraTabPage();
         this.tabFavorites = new DevExpress.XtraTab.XtraTabPage();
         this.tabUserDefined = new DevExpress.XtraTab.XtraTabPage();

         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         this.tabEditBuildingBlock.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tabOverviewControl)).BeginInit();
         this.tabOverviewControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabPagesControl
         // 
         this.tabPagesControl.Size = new System.Drawing.Size(857, 742);
         // 
         // tabEditBuildingBlock
         // 
         this.tabEditBuildingBlock.Controls.Add(this.splitContainerControl1);
         this.tabEditBuildingBlock.Size = new System.Drawing.Size(851, 714);
         // 
         // tabFormulaCache
         // 
         this.tabFormulaCache.Size = new System.Drawing.Size(386, 269);
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitContainerControl1.Horizontal = false;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Controls.Add(this.tabOverviewControl);
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(851, 714);
         this.splitContainerControl1.SplitterPosition = 357;
         this.splitContainerControl1.TabIndex = 0;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // tabOverviewControl
         // 
         this.tabOverviewControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabOverviewControl.Location = new System.Drawing.Point(0, 0);
         this.tabOverviewControl.Name = "tabOverviewControl";
         this.tabOverviewControl.SelectedTabPage = this.tabFlowChart;
         this.tabOverviewControl.Size = new System.Drawing.Size(851, 357);
         this.tabOverviewControl.TabIndex = 0;
         this.tabOverviewControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabFlowChart,
            this.tabList,
            this.tabFavorites,
            this.tabUserDefined
         });
         // 
         // tabFlowChart
         // 
         this.tabFlowChart.Name = "tabFlowChart";
         this.tabFlowChart.Size = new System.Drawing.Size(845, 329);
         this.tabFlowChart.Text = "tabFlowChart";
         // 
         // tabList
         // 
         this.tabList.Name = "tabList";
         this.tabList.Size = new System.Drawing.Size(845, 329);
         this.tabList.Text = "tabList";
         // 
         // tabFavorites
         // 
         this.tabFavorites.Name = "tabFavorites";
         this.tabFavorites.Size = new System.Drawing.Size(845, 329);
         this.tabFavorites.Text = "tabFavorites";
         // 
         // tabFavorites
         // 
         this.tabUserDefined.Name = "tabUserDefined";
         this.tabUserDefined.Size = new System.Drawing.Size(845, 329);
         this.tabUserDefined.Text = "tabUserDefined";

         // 
         // EditReactionBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "EditReactionBuildingBlockView";
         this.ClientSize = new System.Drawing.Size(857, 742);
         this.Name = "EditReactionBuildingBlockView";
         this.Text = "EditReactionBuildingBlockView";
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         this.tabEditBuildingBlock.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tabOverviewControl)).EndInit();
         this.tabOverviewControl.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
      private DevExpress.XtraTab.XtraTabControl tabOverviewControl;
      private DevExpress.XtraTab.XtraTabPage tabFlowChart;
      private DevExpress.XtraTab.XtraTabPage tabList;
      private DevExpress.XtraTab.XtraTabPage tabFavorites;
      private DevExpress.XtraTab.XtraTabPage tabUserDefined;
   }
}