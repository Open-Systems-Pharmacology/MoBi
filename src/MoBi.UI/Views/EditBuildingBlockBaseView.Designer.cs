namespace MoBi.UI.Views
{
   partial class EditBuildingBlockBaseView 
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
         this.tabPagesControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabEditBuildingBlock = new DevExpress.XtraTab.XtraTabPage();
         this.tabFormulaCache = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabPagesControl
         // 
         this.tabPagesControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabPagesControl.Location = new System.Drawing.Point(0, 0);
         this.tabPagesControl.Name = "tabPagesControl";
         this.tabPagesControl.SelectedTabPage = this.tabEditBuildingBlock;
         this.tabPagesControl.Size = new System.Drawing.Size(392, 297);
         this.tabPagesControl.TabIndex = 0;
         this.tabPagesControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabEditBuildingBlock,
            this.tabFormulaCache});
         // 
         // tabEditBuildingBlock
         // 
         this.tabEditBuildingBlock.Name = "tabEditBuildingBlock";
         this.tabEditBuildingBlock.Size = new System.Drawing.Size(386, 271);
         this.tabEditBuildingBlock.Text = "xtraTabPage1";
         // 
         // tabFormulaCache
         // 
         this.tabFormulaCache.Name = "tabFormulaCache";
         this.tabFormulaCache.Size = new System.Drawing.Size(294, 274);
         this.tabFormulaCache.Text = "Formulas";
         // 
         // BuidlingBlockWithFormulaCacheBaseView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(392, 297);
         this.Controls.Add(this.tabPagesControl);
         this.Name = "EditBuildingBlockBaseView";
         this.Text = "BuidlingBlockWithFormulaCacheBaseView";
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      protected DevExpress.XtraTab.XtraTabControl tabPagesControl;
      protected DevExpress.XtraTab.XtraTabPage tabEditBuildingBlock;
      protected DevExpress.XtraTab.XtraTabPage tabFormulaCache;
   }
}