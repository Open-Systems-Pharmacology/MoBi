using DevExpress.XtraEditors;

namespace MoBi.UI.Views
{
   partial class SelectAndEditContainerView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.legendPanel = new DevExpress.XtraEditors.PanelControl();
         this.btnAddBuildingBlockToProject = new DevExpress.XtraEditors.SimpleButton();
         this.lblEditName = new DevExpress.XtraEditors.LabelControl();
         this.panel = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemSaveAsBuildingBlock = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.legendControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.legendPanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveAsBuildingBlock)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendControlItem)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.legendPanel);
         this.layoutControl1.Controls.Add(this.btnAddBuildingBlockToProject);
         this.layoutControl1.Controls.Add(this.lblEditName);
         this.layoutControl1.Controls.Add(this.panel);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(433, 256, 508, 437);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(520, 466);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // legendPanel
         // 
         this.legendPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.legendPanel.Location = new System.Drawing.Point(24, 41);
         this.legendPanel.Name = "legendPanel";
         this.legendPanel.Size = new System.Drawing.Size(236, 22);
         this.legendPanel.TabIndex = 8;
         // 
         // btnAddBuildingBlockToProject
         // 
         this.btnAddBuildingBlockToProject.Location = new System.Drawing.Point(355, 41);
         this.btnAddBuildingBlockToProject.Name = "btnAddBuildingBlockToProject";
         this.btnAddBuildingBlockToProject.Size = new System.Drawing.Size(141, 22);
         this.btnAddBuildingBlockToProject.StyleController = this.layoutControl1;
         this.btnAddBuildingBlockToProject.TabIndex = 7;
         this.btnAddBuildingBlockToProject.Text = "btnSaveAsBuildingBlock";
         // 
         // lblEditName
         // 
         this.lblEditName.Location = new System.Drawing.Point(24, 24);
         this.lblEditName.Name = "lblEditName";
         this.lblEditName.Size = new System.Drawing.Size(55, 13);
         this.lblEditName.StyleController = this.layoutControl1;
         this.lblEditName.TabIndex = 5;
         this.lblEditName.Text = "lblEditName";
         // 
         // panel
         // 
         this.panel.Location = new System.Drawing.Point(12, 79);
         this.panel.Name = "panel";
         this.panel.Size = new System.Drawing.Size(496, 375);
         this.panel.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlGroup2});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(520, 466);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panel;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 67);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(500, 379);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemSaveAsBuildingBlock,
            this.layoutControlItem2,
            this.emptySpaceItem1,
            this.legendControlItem});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(500, 67);
         this.layoutControlGroup2.Text = "layoutControlGroup2";
         this.layoutControlGroup2.TextVisible = false;
         // 
         // layoutItemSaveAsBuildingBlock
         // 
         this.layoutItemSaveAsBuildingBlock.Control = this.btnAddBuildingBlockToProject;
         this.layoutItemSaveAsBuildingBlock.CustomizationFormText = "layoutItemSaveAsBuildingBlock";
         this.layoutItemSaveAsBuildingBlock.Location = new System.Drawing.Point(331, 17);
         this.layoutItemSaveAsBuildingBlock.Name = "layoutItemSaveAsBuildingBlock";
         this.layoutItemSaveAsBuildingBlock.Size = new System.Drawing.Size(145, 26);
         this.layoutItemSaveAsBuildingBlock.Text = "layoutItemSaveAsBuildingBlock";
         this.layoutItemSaveAsBuildingBlock.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSaveAsBuildingBlock.TextToControlDistance = 0;
         this.layoutItemSaveAsBuildingBlock.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.lblEditName;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(476, 17);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(240, 17);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(91, 26);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // legendControlItem
         // 
         this.legendControlItem.Control = this.legendPanel;
         this.legendControlItem.CustomizationFormText = "legendControlItem";
         this.legendControlItem.Location = new System.Drawing.Point(0, 17);
         this.legendControlItem.Name = "legendControlItem";
         this.legendControlItem.Size = new System.Drawing.Size(240, 26);
         this.legendControlItem.Text = "legendControlItem";
         this.legendControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.legendControlItem.TextToControlDistance = 0;
         this.legendControlItem.TextVisible = false;
         // 
         // SelectAndEditContainerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "SelectAndEditContainerView";
         this.Size = new System.Drawing.Size(520, 466);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.legendPanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveAsBuildingBlock)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendControlItem)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private LabelControl lblEditName;
      private PanelControl panel;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private SimpleButton btnAddBuildingBlockToProject;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSaveAsBuildingBlock;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private PanelControl legendPanel;
      private DevExpress.XtraLayout.LayoutControlItem legendControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;

   }
}
