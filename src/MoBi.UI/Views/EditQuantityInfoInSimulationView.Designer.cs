namespace MoBi.UI.Views
{
   partial class EditQuantityInfoInSimulationView
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
         _screenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.htmlEditor = new DevExpress.XtraEditors.MemoEdit();
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnGoToSource = new OSPSuite.UI.Controls.UxSimpleButton();
         this.tbSource = new DevExpress.XtraEditors.TextEdit();
         this.tbName = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.nameLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.descriptionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemGoToSource = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbSource.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionLayoutControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGoToSource)).BeginInit();
         this.SuspendLayout();
         // 
         // htmlEditor
         // 
         this.htmlEditor.Location = new System.Drawing.Point(12, 78);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Size = new System.Drawing.Size(374, 144);
         this.htmlEditor.StyleController = this.uxLayoutControl;
         this.htmlEditor.TabIndex = 4;
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.btnGoToSource);
         this.uxLayoutControl.Controls.Add(this.tbSource);
         this.uxLayoutControl.Controls.Add(this.htmlEditor);
         this.uxLayoutControl.Controls.Add(this.tbName);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.Root = this.Root;
         this.uxLayoutControl.Size = new System.Drawing.Size(398, 234);
         this.uxLayoutControl.TabIndex = 8;
         this.uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // btnGoToSource
         // 
         this.btnGoToSource.Location = new System.Drawing.Point(299, 36);
         this.btnGoToSource.Manager = null;
         this.btnGoToSource.Name = "btnGoToSource";
         this.btnGoToSource.Shortcut = System.Windows.Forms.Keys.None;
         this.btnGoToSource.Size = new System.Drawing.Size(87, 22);
         this.btnGoToSource.StyleController = this.uxLayoutControl;
         this.btnGoToSource.TabIndex = 9;
         this.btnGoToSource.Text = "btnGoToSource";
         // 
         // tbSource
         // 
         this.tbSource.Location = new System.Drawing.Point(166, 36);
         this.tbSource.Name = "tbSource";
         this.tbSource.Size = new System.Drawing.Size(129, 20);
         this.tbSource.StyleController = this.uxLayoutControl;
         this.tbSource.TabIndex = 5;
         // 
         // tbName
         // 
         this.tbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tbName.Location = new System.Drawing.Point(166, 12);
         this.tbName.Name = "tbName";
         this.tbName.Size = new System.Drawing.Size(220, 20);
         this.tbName.StyleController = this.uxLayoutControl;
         this.tbName.TabIndex = 0;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.nameLayoutControlItem,
            this.descriptionLayoutControlItem,
            this.layoutControlItemSource,
            this.layoutControlItemGoToSource});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(398, 234);
         this.Root.TextVisible = false;
         // 
         // nameLayoutControlItem
         // 
         this.nameLayoutControlItem.Control = this.tbName;
         this.nameLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         this.nameLayoutControlItem.Name = "nameLayoutControlItem";
         this.nameLayoutControlItem.Size = new System.Drawing.Size(378, 24);
         this.nameLayoutControlItem.TextSize = new System.Drawing.Size(142, 13);
         // 
         // descriptionLayoutControlItem
         // 
         this.descriptionLayoutControlItem.Control = this.htmlEditor;
         this.descriptionLayoutControlItem.Location = new System.Drawing.Point(0, 50);
         this.descriptionLayoutControlItem.Name = "descriptionLayoutControlItem";
         this.descriptionLayoutControlItem.Size = new System.Drawing.Size(378, 164);
         this.descriptionLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         this.descriptionLayoutControlItem.TextSize = new System.Drawing.Size(142, 13);
         // 
         // sourceLayoutControlItem
         // 
         this.layoutControlItemSource.Control = this.tbSource;
         this.layoutControlItemSource.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemSource.Name = "layoutControlItemSource";
         this.layoutControlItemSource.Size = new System.Drawing.Size(287, 26);
         this.layoutControlItemSource.TextSize = new System.Drawing.Size(142, 13);
         // 
         // goToSourceLayoutControlItem
         // 
         this.layoutControlItemGoToSource.Control = this.btnGoToSource;
         this.layoutControlItemGoToSource.Location = new System.Drawing.Point(287, 24);
         this.layoutControlItemGoToSource.Name = "layoutControlItemGoToSource";
         this.layoutControlItemGoToSource.Size = new System.Drawing.Size(91, 26);
         this.layoutControlItemGoToSource.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemGoToSource.TextVisible = false;
         // 
         // EditQuantityInfoInSimulationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Name = "EditQuantityInfoInSimulationView";
         this.Size = new System.Drawing.Size(398, 234);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbSource.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.nameLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionLayoutControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGoToSource)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraEditors.MemoEdit htmlEditor;
      private DevExpress.XtraEditors.TextEdit tbName;
      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem nameLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem descriptionLayoutControlItem;
      private DevExpress.XtraEditors.TextEdit tbSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSource;
      private OSPSuite.UI.Controls.UxSimpleButton btnGoToSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGoToSource;
   }
}
