namespace MoBi.UI.Views
{
   partial class SelectMoleculesView
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
         disposeBinders();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl
         // 
         this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl.Location = new System.Drawing.Point(0, 0);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(552, 315);
         this.gridControl.TabIndex = 40;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         // 
         // SelectMoleculesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.gridControl);
         this.Name = "SelectMoleculesView";
         this.Size = new System.Drawing.Size(552, 315);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private UxGridView gridView;
   }
}
