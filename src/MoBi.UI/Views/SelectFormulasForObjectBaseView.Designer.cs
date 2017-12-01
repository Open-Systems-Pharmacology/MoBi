namespace MoBi.UI.Views
{
   partial class SelectFormulasForObjectBaseView
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
         this.gridControlSelections = new OSPSuite.UI.Controls.UxGridControl();
         this.grdSelections = new DevExpress.XtraGrid.Views.Grid.GridView();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlSelections)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdSelections)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControlSelections
         // 
         this.gridControlSelections.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControlSelections.Location = new System.Drawing.Point(0, 0);
         this.gridControlSelections.MainView = this.grdSelections;
         this.gridControlSelections.Name = "gridControlSelections";
         this.gridControlSelections.Size = new System.Drawing.Size(448, 347);
         this.gridControlSelections.TabIndex = 1;
         this.gridControlSelections.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdSelections});
         // 
         // grdSelections
         // 
         this.grdSelections.GridControl = this.gridControlSelections;
         this.grdSelections.Name = "grdSelections";
         // 
         // SelectFormulasForObjectBaseView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(448, 393);
         this.Controls.Add(this.gridControlSelections);
         this.Name = "SelectFormulasForObjectBaseView";
         this.Text = "SelectFormulasForObjectBaseView";
         this.Controls.SetChildIndex(this.gridControlSelections, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlSelections)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdSelections)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraGrid.GridControl gridControlSelections;
      private DevExpress.XtraGrid.Views.Grid.GridView grdSelections;
   }
}