namespace MoBi.UI.Views
{
   partial class EditMoleculesBuildingBlockView
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
         this.moleculeErrorProvider = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
         this.splitContainer = new DevExpress.XtraEditors.SplitContainerControl();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         this.tabEditBuildingBlock.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.moleculeErrorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
         this.splitContainer.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabPagesControl
         // 
         this.tabPagesControl.Size = new System.Drawing.Size(814, 444);
         // 
         // tabEditBuildingBlock
         // 
         this.tabEditBuildingBlock.Controls.Add(this.splitContainer);
         this.tabEditBuildingBlock.Size = new System.Drawing.Size(808, 418);
         // 
         // tabFormulaCache
         // 
         this.tabFormulaCache.Size = new System.Drawing.Size(386, 271);
         // 
         // dxErrorProvider1
         // 
         this.moleculeErrorProvider.ContainerControl = this;
         // 
         // splitContainer
         // 
         this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer.Location = new System.Drawing.Point(0, 0);
         this.splitContainer.Name = "splitContainer";
         this.splitContainer.Panel1.Text = "Panel1";
         this.splitContainer.Panel2.Text = "Panel2";
         this.splitContainer.Size = new System.Drawing.Size(808, 418);
         this.splitContainer.SplitterPosition = 398;
         this.splitContainer.TabIndex = 2;
         this.splitContainer.Text = "splitContainerControl2";
         // 
         // EditMoleculesBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(814, 444);
         this.Name = "EditMoleculesBuildingBlockView";
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         this.tabEditBuildingBlock.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.moleculeErrorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
         this.splitContainer.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider moleculeErrorProvider;
      private DevExpress.XtraEditors.SplitContainerControl splitContainer;
   }
}