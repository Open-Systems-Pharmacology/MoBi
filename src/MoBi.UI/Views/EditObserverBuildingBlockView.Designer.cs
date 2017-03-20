using DevExpress.XtraEditors;

namespace MoBi.UI.Views
{
   partial class EditObserverBuildingBlockView
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
         this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
         this.tabControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabAmountObserverList = new DevExpress.XtraTab.XtraTabPage();
         this.tabContainerObserverList = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         this.tabEditBuildingBlock.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
         this.splitContainerControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
         this.tabControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabPagesControl
         // 
         this.tabPagesControl.Size = new System.Drawing.Size(1060, 448);
         // 
         // tabEditBuildingBlock
         // 
         this.tabEditBuildingBlock.Controls.Add(this.splitContainerControl);
         this.tabEditBuildingBlock.Size = new System.Drawing.Size(1054, 422);
         // 
         // tabFormulaCache
         // 
         this.tabFormulaCache.Size = new System.Drawing.Size(386, 271);
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl.Horizontal = true;
         this.splitContainerControl.FixedPanel = SplitFixedPanel.None;
         this.splitContainerControl.Location = new System.Drawing.Point(0, 0);
         this.splitContainerControl.Name = "splitContainerControl";
         this.splitContainerControl.Panel1.Controls.Add(this.tabControl);
         this.splitContainerControl.Panel1.Text = "Panel1";
         this.splitContainerControl.Panel2.Text = "Panel2";
         this.splitContainerControl.Size = new System.Drawing.Size(1054, 422);
         this.splitContainerControl.SplitterPosition = splitContainerControl.Width/3;
         this.splitContainerControl.TabIndex = 1;
         this.splitContainerControl.Text = "splitContainerControl1";
         // 
         // xtraTabControl1
         // 
         this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabControl.Location = new System.Drawing.Point(0, 0);
         this.tabControl.Name = "tabControl";
         this.tabControl.SelectedTabPage = this.tabAmountObserverList;
         this.tabControl.Size = new System.Drawing.Size(1054, 240);
         this.tabControl.TabIndex = 0;
         this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabAmountObserverList,
            this.tabContainerObserverList});
         this.tabControl.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.selectedPageChanged);
         // 
         // tabAmountObserverList
         // 
         this.tabAmountObserverList.Name = "tabAmountObserverList";
         this.tabAmountObserverList.Size = new System.Drawing.Size(1048, 214);
         this.tabContainerObserverList.Text = "tabAmountObserverList";
         // 
         // tabContainerObserverList
         // 
         this.tabContainerObserverList.Name = "tabContainerObserverList";
         this.tabContainerObserverList.Size = new System.Drawing.Size(1048, 214);
         this.tabContainerObserverList.Text = "tabContainerObserverList";
         // 
         // EditObserverBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1060, 448);
         this.Name = "EditObserverBuildingBlockView";
         this.Text = "EditObserverBuildingBlockView";
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         this.tabEditBuildingBlock.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
         this.splitContainerControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
         this.tabControl.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
      private DevExpress.XtraTab.XtraTabControl tabControl;
      private DevExpress.XtraTab.XtraTabPage tabAmountObserverList;
      private DevExpress.XtraTab.XtraTabPage tabContainerObserverList;

   }
}