namespace MoBi.UI.Views
{
   partial class SelectNeighborPathView
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
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.tbContainerPath = new DevExpress.XtraEditors.TextEdit();
         this.layoutItemContainerPath = new DevExpress.XtraLayout.LayoutControlItem();
         this.panelContainerSelection = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbContainerPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerPath)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelContainerSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.panelContainerSelection);
         this.uxLayoutControl1.Controls.Add(this.tbContainerPath);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(492, 586);
         this.uxLayoutControl1.TabIndex = 0;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemContainerPath,
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(492, 586);
         this.Root.TextVisible = false;
         // 
         // tbContainerPath
         // 
         this.tbContainerPath.Location = new System.Drawing.Point(135, 2);
         this.tbContainerPath.Name = "tbContainerPath";
         this.tbContainerPath.Size = new System.Drawing.Size(355, 20);
         this.tbContainerPath.StyleController = this.uxLayoutControl1;
         this.tbContainerPath.TabIndex = 4;
         // 
         // layoutItemContainerPath
         // 
         this.layoutItemContainerPath.Control = this.tbContainerPath;
         this.layoutItemContainerPath.Location = new System.Drawing.Point(0, 0);
         this.layoutItemContainerPath.Name = "layoutItemContainerPath";
         this.layoutItemContainerPath.Size = new System.Drawing.Size(492, 24);
         this.layoutItemContainerPath.TextSize = new System.Drawing.Size(121, 13);
         // 
         // panelContainerSelection
         // 
         this.panelContainerSelection.Location = new System.Drawing.Point(2, 26);
         this.panelContainerSelection.Name = "panelContainerSelection";
         this.panelContainerSelection.Size = new System.Drawing.Size(488, 558);
         this.panelContainerSelection.TabIndex = 5;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.panelContainerSelection;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(492, 562);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // SelectNeighborPathView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "SelectNeighborPathView";
         this.Size = new System.Drawing.Size(492, 586);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbContainerPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerPath)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelContainerSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.PanelControl panelContainerSelection;
      private DevExpress.XtraEditors.TextEdit tbContainerPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemContainerPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}
