namespace MoBi.UI.Views
{
   partial class EditExpressionProfileBuildingBlockView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.tabEditInitialConditions = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         this.SuspendLayout();
         // 
         // tabPagesControl
         // 
         this.tabPagesControl.SelectedTabPage = this.tabEditBuildingBlock;
         this.tabPagesControl.Size = new System.Drawing.Size(750, 521);
         this.tabPagesControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabEditInitialConditions});
         this.tabPagesControl.Controls.SetChildIndex(this.tabFormulaCache, 0);
         this.tabPagesControl.Controls.SetChildIndex(this.tabEditInitialConditions, 0);
         this.tabPagesControl.Controls.SetChildIndex(this.tabEditBuildingBlock, 0);
         // 
         // tabEditBuildingBlock
         // 
         this.tabEditBuildingBlock.Size = new System.Drawing.Size(748, 496);
         // 
         // tabFormulaCache
         // 
         this.tabFormulaCache.Size = new System.Drawing.Size(748, 496);
         // 
         // tabEditInitialConditions
         // 
         this.tabEditInitialConditions.Name = "tabEditInitialConditions";
         this.tabEditInitialConditions.Size = new System.Drawing.Size(748, 496);
         this.tabEditInitialConditions.Text = "tabEditInitialConditions";
         // 
         // EditExpressionProfileBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(750, 521);
         this.Name = "EditExpressionProfileBuildingBlockView";
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabPage tabEditInitialConditions;
   }
}
