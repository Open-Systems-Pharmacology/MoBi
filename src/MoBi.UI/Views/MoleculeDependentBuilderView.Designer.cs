using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class MoleculeDependentBuilderView
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
         _gridViewIncludedBinder.Dispose();
         _gridViewExcludedBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnAddToExcludeList = new DevExpress.XtraEditors.SimpleButton();
         this.btnAddToIncludeList = new DevExpress.XtraEditors.SimpleButton();
         this.gridIncludedMolecules = new DevExpress.XtraGrid.GridControl();
         this.gridViewIncludedMolecules = new MoBi.UI.Views.UxGridView();
         this.gridExcludedMolecules = new DevExpress.XtraGrid.GridControl();
         this.gridViewExcludedMolecules = new MoBi.UI.Views.UxGridView();
         this.chkForAll = new UxCheckEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupMoleculeSelection = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layouytItemAddToIncludeList = new DevExpress.XtraLayout.LayoutControlItem();
         this.layouytItemAddToExcludeList = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutGroupInclude = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupExclude = new DevExpress.XtraLayout.LayoutControlGroup();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridIncludedMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewIncludedMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridExcludedMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewExcludedMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkForAll.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupMoleculeSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouytItemAddToIncludeList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouytItemAddToExcludeList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupInclude)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupExclude)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnAddToExcludeList);
         this.layoutControl.Controls.Add(this.btnAddToIncludeList);
         this.layoutControl.Controls.Add(this.gridIncludedMolecules);
         this.layoutControl.Controls.Add(this.gridExcludedMolecules);
         this.layoutControl.Controls.Add(this.chkForAll);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1080, 389, 430, 508);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(865, 400);
         this.layoutControl.TabIndex = 11;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnAddToExcludeList
         // 
         this.btnAddToExcludeList.Location = new System.Drawing.Point(583, 87);
         this.btnAddToExcludeList.Name = "btnAddToExcludeList";
         this.btnAddToExcludeList.Size = new System.Drawing.Size(256, 22);
         this.btnAddToExcludeList.StyleController = this.layoutControl;
         this.btnAddToExcludeList.TabIndex = 12;
         this.btnAddToExcludeList.Text = "btnAddToExcludeList";
         // 
         // btnAddToIncludeList
         // 
         this.btnAddToIncludeList.Location = new System.Drawing.Point(155, 87);
         this.btnAddToIncludeList.Name = "btnAddToIncludeList";
         this.btnAddToIncludeList.Size = new System.Drawing.Size(254, 22);
         this.btnAddToIncludeList.StyleController = this.layoutControl;
         this.btnAddToIncludeList.TabIndex = 11;
         this.btnAddToIncludeList.Text = "btnAddToIncludeList";
         // 
         // gridIncludedMolecules
         // 
         this.gridIncludedMolecules.Location = new System.Drawing.Point(26, 113);
         this.gridIncludedMolecules.MainView = this.gridViewIncludedMolecules;
         this.gridIncludedMolecules.Name = "gridIncludedMolecules";
         this.gridIncludedMolecules.Size = new System.Drawing.Size(383, 261);
         this.gridIncludedMolecules.TabIndex = 10;
         this.gridIncludedMolecules.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewIncludedMolecules});
         // 
         // gridViewIncludedMolecules
         // 
         this.gridViewIncludedMolecules.GridControl = this.gridIncludedMolecules;
         this.gridViewIncludedMolecules.Name = "gridViewIncludedMolecules";
         // 
         // gridExcludedMolecules
         // 
         this.gridExcludedMolecules.Location = new System.Drawing.Point(437, 113);
         this.gridExcludedMolecules.MainView = this.gridViewExcludedMolecules;
         this.gridExcludedMolecules.Name = "gridExcludedMolecules";
         this.gridExcludedMolecules.Size = new System.Drawing.Size(402, 261);
         this.gridExcludedMolecules.TabIndex = 9;
         this.gridExcludedMolecules.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewExcludedMolecules});
         // 
         // gridViewExcludedMolecules
         // 
         this.gridViewExcludedMolecules.GridControl = this.gridExcludedMolecules;
         this.gridViewExcludedMolecules.Name = "gridViewExcludedMolecules";
         // 
         // chkForAll
         // 
         this.chkForAll.Location = new System.Drawing.Point(14, 33);
         this.chkForAll.Name = "chkForAll";
         this.chkForAll.Properties.Caption = "All";
         this.chkForAll.Size = new System.Drawing.Size(837, 19);
         this.chkForAll.StyleController = this.layoutControl;
         this.chkForAll.TabIndex = 7;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupMoleculeSelection});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(865, 400);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         // 
         // layoutGroupMoleculeSelection
         // 
         this.layoutGroupMoleculeSelection.CustomizationFormText = "layoutControlGroup2";
         this.layoutGroupMoleculeSelection.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutGroupInclude,
            this.layoutGroupExclude});
         this.layoutGroupMoleculeSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupMoleculeSelection.Name = "layoutGroupMoleculeSelection";
         this.layoutGroupMoleculeSelection.Size = new System.Drawing.Size(865, 400);
         this.layoutGroupMoleculeSelection.Text = "layoutGroupMoleculeSelection";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkForAll;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(841, 23);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.gridExcludedMolecules;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(406, 265);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.gridIncludedMolecules;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(387, 265);
         this.layoutControlItem3.Text = "layoutControlItem3";
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextToControlDistance = 0;
         this.layoutControlItem3.TextVisible = false;
         // 
         // layouytItemAddToIncludeList
         // 
         this.layouytItemAddToIncludeList.Control = this.btnAddToIncludeList;
         this.layouytItemAddToIncludeList.CustomizationFormText = "layouytItemAddToIncludeList";
         this.layouytItemAddToIncludeList.Location = new System.Drawing.Point(129, 0);
         this.layouytItemAddToIncludeList.Name = "layouytItemAddToIncludeList";
         this.layouytItemAddToIncludeList.Size = new System.Drawing.Size(258, 26);
         this.layouytItemAddToIncludeList.Text = "layouytItemAddToIncludeList";
         this.layouytItemAddToIncludeList.TextSize = new System.Drawing.Size(0, 0);
         this.layouytItemAddToIncludeList.TextToControlDistance = 0;
         this.layouytItemAddToIncludeList.TextVisible = false;
         // 
         // layouytItemAddToExcludeList
         // 
         this.layouytItemAddToExcludeList.Control = this.btnAddToExcludeList;
         this.layouytItemAddToExcludeList.CustomizationFormText = "layouytItemAddToExcludeList";
         this.layouytItemAddToExcludeList.Location = new System.Drawing.Point(146, 0);
         this.layouytItemAddToExcludeList.Name = "layouytItemAddToExcludeList";
         this.layouytItemAddToExcludeList.Size = new System.Drawing.Size(260, 26);
         this.layouytItemAddToExcludeList.Text = "layouytItemAddToExcludeList";
         this.layouytItemAddToExcludeList.TextSize = new System.Drawing.Size(0, 0);
         this.layouytItemAddToExcludeList.TextToControlDistance = 0;
         this.layouytItemAddToExcludeList.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(129, 26);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
         this.emptySpaceItem2.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(146, 26);
         this.emptySpaceItem2.Text = "emptySpaceItem2";
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutGroupInclude
         // 
         this.layoutGroupInclude.CustomizationFormText = "layoutGroupInclude";
         this.layoutGroupInclude.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layouytItemAddToIncludeList,
            this.emptySpaceItem1,
            this.layoutControlItem3});
         this.layoutGroupInclude.Location = new System.Drawing.Point(0, 23);
         this.layoutGroupInclude.Name = "layoutGroupInclude";
         this.layoutGroupInclude.Size = new System.Drawing.Size(411, 334);
         this.layoutGroupInclude.Text = "layoutGroupInclude";
         // 
         // layoutGroupExclude
         // 
         this.layoutGroupExclude.CustomizationFormText = "layoutGroupExclude";
         this.layoutGroupExclude.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layouytItemAddToExcludeList,
            this.emptySpaceItem2});
         this.layoutGroupExclude.Location = new System.Drawing.Point(411, 23);
         this.layoutGroupExclude.Name = "layoutGroupExclude";
         this.layoutGroupExclude.Size = new System.Drawing.Size(430, 334);
         this.layoutGroupExclude.Text = "layoutGroupExclude";
         // 
         // MoleculeDependentBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "MoleculeDependentBuilderView";
         this.Size = new System.Drawing.Size(865, 400);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridIncludedMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewIncludedMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridExcludedMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewExcludedMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkForAll.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupMoleculeSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouytItemAddToIncludeList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouytItemAddToExcludeList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupInclude)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupExclude)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      protected DevExpress.XtraEditors.CheckEdit chkForAll;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupMoleculeSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraGrid.GridControl gridIncludedMolecules;
      private MoBi.UI.Views.UxGridView gridViewIncludedMolecules;
      private DevExpress.XtraGrid.GridControl gridExcludedMolecules;
      private MoBi.UI.Views.UxGridView gridViewExcludedMolecules;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraEditors.SimpleButton btnAddToExcludeList;
      private DevExpress.XtraEditors.SimpleButton btnAddToIncludeList;
      private DevExpress.XtraLayout.LayoutControlItem layouytItemAddToIncludeList;
      private DevExpress.XtraLayout.LayoutControlItem layouytItemAddToExcludeList;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupInclude;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupExclude;
   }
}
