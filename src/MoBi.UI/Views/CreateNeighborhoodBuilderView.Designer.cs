namespace MoBi.UI.Views
{
   partial class CreateNeighborhoodBuilderView
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
         _screenBinder.Dispose();
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelSecondNeighbor = new DevExpress.XtraEditors.PanelControl();
         this.panelFirstNeighbor = new DevExpress.XtraEditors.PanelControl();
         this.tbName = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemFirstNeighbor = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSecondNeighbor = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelSecondNeighbor)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFirstNeighbor)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFirstNeighbor)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSecondNeighbor)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.panelSecondNeighbor);
         this.layoutControl.Controls.Add(this.panelFirstNeighbor);
         this.layoutControl.Controls.Add(this.tbName);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(907, 571);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "uxLayoutControl1";
         // 
         // panelSecondNeighbor
         // 
         this.panelSecondNeighbor.Location = new System.Drawing.Point(453, 34);
         this.panelSecondNeighbor.Name = "panelSecondNeighbor";
         this.panelSecondNeighbor.Size = new System.Drawing.Size(444, 527);
         this.panelSecondNeighbor.TabIndex = 0;
         // 
         // panelFirstNeighbor
         // 
         this.panelFirstNeighbor.Location = new System.Drawing.Point(10, 34);
         this.panelFirstNeighbor.Name = "panelFirstNeighbor";
         this.panelFirstNeighbor.Size = new System.Drawing.Size(443, 527);
         this.panelFirstNeighbor.TabIndex = 5;
         // 
         // tbName
         // 
         this.tbName.Location = new System.Drawing.Point(103, 12);
         this.tbName.Name = "tbName";
         this.tbName.Size = new System.Drawing.Size(792, 20);
         this.tbName.StyleController = this.layoutControl;
         this.tbName.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFirstNeighbor,
            this.layoutItemName,
            this.layoutItemSecondNeighbor});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(907, 571);
         this.Root.TextVisible = false;
         // 
         // layoutItemFirstNeighbor
         // 
         this.layoutItemFirstNeighbor.Control = this.panelFirstNeighbor;
         this.layoutItemFirstNeighbor.Location = new System.Drawing.Point(0, 24);
         this.layoutItemFirstNeighbor.Name = "layoutItemFirstNeighbor";
         this.layoutItemFirstNeighbor.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutItemFirstNeighbor.Size = new System.Drawing.Size(443, 527);
         this.layoutItemFirstNeighbor.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemFirstNeighbor.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.tbName;
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(887, 24);
         this.layoutItemName.TextSize = new System.Drawing.Size(79, 13);
         // 
         // layoutItemSecondNeighbor
         // 
         this.layoutItemSecondNeighbor.Control = this.panelSecondNeighbor;
         this.layoutItemSecondNeighbor.Location = new System.Drawing.Point(443, 24);
         this.layoutItemSecondNeighbor.Name = "layoutItemSecondNeighbor";
         this.layoutItemSecondNeighbor.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutItemSecondNeighbor.Size = new System.Drawing.Size(444, 527);
         this.layoutItemSecondNeighbor.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSecondNeighbor.TextVisible = false;
         // 
         // EditNeighborhoodBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "CreateNeighborhoodBuilderView";
         this.Size = new System.Drawing.Size(907, 571);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelSecondNeighbor)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFirstNeighbor)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFirstNeighbor)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSecondNeighbor)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraEditors.TextEdit tbName;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraEditors.PanelControl panelSecondNeighbor;
      private DevExpress.XtraEditors.PanelControl panelFirstNeighbor;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFirstNeighbor;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSecondNeighbor;
   }
}
