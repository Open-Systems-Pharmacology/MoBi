namespace MoBi.UI.Views
{
   partial class SimulationUpdateView
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.descriptionLabel = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
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
         this.gridControl.Size = new System.Drawing.Size(524, 380);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         //
         // gridView
         //
         this.gridView.AllowsFiltering = false;
         this.gridView.EnableColumnContextMenu = false;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         //
         // descriptionLabel
         //
         this.descriptionLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this.descriptionLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.descriptionLabel.Location = new System.Drawing.Point(0, 380);
         this.descriptionLabel.Name = "descriptionLabel";
         this.descriptionLabel.Padding = new System.Windows.Forms.Padding(4);
         this.descriptionLabel.Size = new System.Drawing.Size(524, 40);
         this.descriptionLabel.TabIndex = 1;
         //
         // SimulationUpdateView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(524, 420);
         this.Controls.Add(this.descriptionLabel);
         this.Controls.Add(this.gridControl);
         this.MinimumSize = new System.Drawing.Size(400, 250);
         this.Name = "SimulationUpdateView";
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         this.ResumeLayout(false);
      }

      #endregion

      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private MoBi.UI.Views.UxGridView gridView;
      private DevExpress.XtraEditors.LabelControl descriptionLabel;
   }
}
