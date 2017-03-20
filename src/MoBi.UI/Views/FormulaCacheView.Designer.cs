using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class FormulaCacheView
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
         _gridBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         this.gridControl1 = new DevExpress.XtraGrid.GridControl();
         this.grdFormulaList = new MoBi.UI.Views.UxGridView();
         this.gridView2 = new MoBi.UI.Views.UxGridView();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdFormulaList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
         this.SuspendLayout();
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(889, 538);
         this.splitContainerControl1.SplitterPosition = 518;
         this.splitContainerControl1.TabIndex = 0;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // gridControl1
         // 
         this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.gridControl1.Location = new System.Drawing.Point(0, 3);
         this.gridControl1.MainView = this.grdFormulaList;
         this.gridControl1.Name = "gridControl1";
         this.gridControl1.Size = new System.Drawing.Size(518, 535);
         this.gridControl1.TabIndex = 0;
         this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdFormulaList,
            this.gridView2});
         // 
         // grdFormulaList
         // 
         this.grdFormulaList.GridControl = this.gridControl1;
         this.grdFormulaList.Name = "grdFormulaList";
         // 
         // gridView2
         // 
         this.gridView2.GridControl = this.gridControl1;
         this.gridView2.Name = "gridView2";
         // 
         // FormulaCacheView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.splitContainerControl1);
         this.Name = "FormulaCacheView";
         this.Size = new System.Drawing.Size(889, 538);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdFormulaList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
      private DevExpress.XtraGrid.GridControl gridControl1;
      private MoBi.UI.Views.UxGridView grdFormulaList;
      private MoBi.UI.Views.UxGridView gridView2;
   }
}
