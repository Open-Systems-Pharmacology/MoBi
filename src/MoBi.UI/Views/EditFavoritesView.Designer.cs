namespace MoBi.UI.Views
{
   partial class EditFavoritesView
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
         _gridViewBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this._gridControl = new DevExpress.XtraGrid.GridControl();
         this._gridView = new UxGridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._gridView)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl1
         // 
         this._gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this._gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this._gridControl.Location = new System.Drawing.Point(0, 0);
         this._gridControl.MainView = this._gridView;
         this._gridControl.Name = "_gridControl";
         this._gridControl.Size = new System.Drawing.Size(502, 305);
         this._gridControl.TabIndex = 0;
         this._gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this._gridView});
         // 
         // gridView1
         // 
         this._gridView.GridControl = this._gridControl;
         this._gridView.Name = "_gridView";
         // 
         // EditFavoriteView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this._gridControl);
         this.Name = "EditFavoritesView";
         this.Size = new System.Drawing.Size(502, 305);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._gridView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraGrid.GridControl _gridControl;
      private UxGridView _gridView;
   }
}
