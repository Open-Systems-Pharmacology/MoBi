namespace MoBi.UI.Views
{
   partial class CreateObjectPathsFromReferencesView
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
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemBtnAdd = new DevExpress.XtraLayout.LayoutControlItem();
         this.referenceSelectionPanel = new OSPSuite.UI.Controls.UxPanelControl();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.memoEditObjectPaths = new DevExpress.XtraEditors.MemoEdit();
         this.layoutItemObjectPaths = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnAdd)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.referenceSelectionPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.memoEditObjectPaths.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemObjectPaths)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.memoEditObjectPaths);
         this.uxLayoutControl.Controls.Add(this.referenceSelectionPanel);
         this.uxLayoutControl.Controls.Add(this.btnAdd);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1250, 322, 1195, 925);
         this.uxLayoutControl.Root = this.Root;
         this.uxLayoutControl.Size = new System.Drawing.Size(712, 596);
         this.uxLayoutControl.TabIndex = 0;
         this.uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemBtnAdd,
            this.layoutControlItem4,
            this.emptySpaceItem2,
            this.emptySpaceItem3,
            this.layoutItemObjectPaths});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(712, 596);
         this.Root.TextVisible = false;
         // 
         // btnAdd
         // 
         this.btnAdd.Location = new System.Drawing.Point(307, 201);
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Size = new System.Drawing.Size(101, 22);
         this.btnAdd.StyleController = this.uxLayoutControl;
         this.btnAdd.TabIndex = 7;
         this.btnAdd.Text = "btnAdd";
         // 
         // layoutItemBtnAdd
         // 
         this.layoutItemBtnAdd.Control = this.btnAdd;
         this.layoutItemBtnAdd.Location = new System.Drawing.Point(295, 189);
         this.layoutItemBtnAdd.Name = "layoutItemBtnAdd";
         this.layoutItemBtnAdd.Size = new System.Drawing.Size(105, 26);
         this.layoutItemBtnAdd.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemBtnAdd.TextVisible = false;
         // 
         // referenceSelectionPanel
         // 
         this.referenceSelectionPanel.Location = new System.Drawing.Point(12, 12);
         this.referenceSelectionPanel.Name = "referenceSelectionPanel";
         this.referenceSelectionPanel.Size = new System.Drawing.Size(291, 572);
         this.referenceSelectionPanel.TabIndex = 9;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.referenceSelectionPanel;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(295, 576);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(295, 215);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(105, 361);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(295, 0);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(105, 189);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // memoEditObjectPaths
         // 
         this.memoEditObjectPaths.Location = new System.Drawing.Point(412, 12);
         this.memoEditObjectPaths.Name = "memoEditObjectPaths";
         this.memoEditObjectPaths.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
         this.memoEditObjectPaths.Size = new System.Drawing.Size(288, 572);
         this.memoEditObjectPaths.StyleController = this.uxLayoutControl;
         this.memoEditObjectPaths.TabIndex = 10;
         // 
         // layoutItemObjectPaths
         // 
         this.layoutItemObjectPaths.Control = this.memoEditObjectPaths;
         this.layoutItemObjectPaths.Location = new System.Drawing.Point(400, 0);
         this.layoutItemObjectPaths.Name = "layoutItemObjectPaths";
         this.layoutItemObjectPaths.Size = new System.Drawing.Size(292, 576);
         this.layoutItemObjectPaths.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemObjectPaths.TextVisible = false;
         // 
         // CreateObjectPathsFromReferencesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Name = "CreateObjectPathsFromReferencesView";
         this.Size = new System.Drawing.Size(712, 596);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnAdd)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.referenceSelectionPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.memoEditObjectPaths.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemObjectPaths)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.SimpleButton btnAdd;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemBtnAdd;
      private OSPSuite.UI.Controls.UxPanelControl referenceSelectionPanel;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraEditors.MemoEdit memoEditObjectPaths;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemObjectPaths;
   }
}
