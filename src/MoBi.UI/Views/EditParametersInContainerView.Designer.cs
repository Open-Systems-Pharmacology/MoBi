using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class EditParametersInContainerView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
         this.panel1LayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnAddParameter = new DevExpress.XtraEditors.SimpleButton();
         this.btnLoadParameter = new DevExpress.XtraEditors.SimpleButton();
         this.tbContainerPath = new DevExpress.XtraEditors.TextEdit();
         this.chkGroupBy = new OSPSuite.UI.Controls.UxCheckEdit();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.chkShowAdvancedParameter = new OSPSuite.UI.Controls.UxCheckEdit();
         this.cbSelectIndividual = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemGrid = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSelectIndividual = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemShowAdvanceParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemGroupBy = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemContainerPath = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemLoadParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemAddParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemParmaterLists = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel1)).BeginInit();
         this.splitContainerControl.Panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel2)).BeginInit();
         this.splitContainerControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panel1LayoutControl)).BeginInit();
         this.panel1LayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbContainerPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroupBy.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowAdvancedParameter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbSelectIndividual.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemShowAdvanceParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGroupBy)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemContainerPath)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoadParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParmaterLists)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.splitContainerControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(608, 319, 940, 954);
         this.layoutControl.Root = this.layoutGroup;
         this.layoutControl.Size = new System.Drawing.Size(1072, 503);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // splitContainerControl
         // 
         this.splitContainerControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.splitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitContainerControl.Location = new System.Drawing.Point(12, 12);
         this.splitContainerControl.Name = "splitContainerControl";
         // 
         // splitContainerControl.Panel1
         // 
         this.splitContainerControl.Panel1.AutoScroll = true;
         this.splitContainerControl.Panel1.Controls.Add(this.panel1LayoutControl);
         this.splitContainerControl.Panel1.Text = "Panel1";
         // 
         // splitContainerControl.Panel2
         // 
         this.splitContainerControl.Panel2.Text = "Panel2";
         this.splitContainerControl.Size = new System.Drawing.Size(1048, 479);
         this.splitContainerControl.SplitterPosition = 522;
         this.splitContainerControl.TabIndex = 6;
         this.splitContainerControl.Text = "splitContainerControl1";
         // 
         // panel1LayoutControl
         // 
         this.panel1LayoutControl.AllowCustomization = false;
         this.panel1LayoutControl.Controls.Add(this.btnAddParameter);
         this.panel1LayoutControl.Controls.Add(this.btnLoadParameter);
         this.panel1LayoutControl.Controls.Add(this.tbContainerPath);
         this.panel1LayoutControl.Controls.Add(this.chkGroupBy);
         this.panel1LayoutControl.Controls.Add(this.gridControl);
         this.panel1LayoutControl.Controls.Add(this.chkShowAdvancedParameter);
         this.panel1LayoutControl.Controls.Add(this.cbSelectIndividual);
         this.panel1LayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1LayoutControl.Location = new System.Drawing.Point(0, 0);
         this.panel1LayoutControl.Name = "panel1LayoutControl";
         this.panel1LayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1008, 199, 650, 400);
         this.panel1LayoutControl.Root = this.Root;
         this.panel1LayoutControl.Size = new System.Drawing.Size(522, 479);
         this.panel1LayoutControl.TabIndex = 4;
         this.panel1LayoutControl.Text = "panel1LayoutControl";
         // 
         // btnAddParameter
         // 
         this.btnAddParameter.Location = new System.Drawing.Point(424, 2);
         this.btnAddParameter.Name = "btnAddParameter";
         this.btnAddParameter.Size = new System.Drawing.Size(86, 22);
         this.btnAddParameter.StyleController = this.panel1LayoutControl;
         this.btnAddParameter.TabIndex = 7;
         this.btnAddParameter.Text = "btAddParameter";
         // 
         // btnLoadParameter
         // 
         this.btnLoadParameter.Location = new System.Drawing.Point(2, 2);
         this.btnLoadParameter.Name = "btnLoadParameter";
         this.btnLoadParameter.Size = new System.Drawing.Size(418, 22);
         this.btnLoadParameter.StyleController = this.panel1LayoutControl;
         this.btnLoadParameter.TabIndex = 9;
         this.btnLoadParameter.Text = "btLoadParameter";
         // 
         // tbContainerPath
         // 
         this.tbContainerPath.Location = new System.Drawing.Point(176, 52);
         this.tbContainerPath.Name = "tbContainerPath";
         this.tbContainerPath.Size = new System.Drawing.Size(344, 20);
         this.tbContainerPath.StyleController = this.panel1LayoutControl;
         this.tbContainerPath.TabIndex = 12;
         // 
         // chkGroupBy
         // 
         this.chkGroupBy.AllowClicksOutsideControlArea = false;
         this.chkGroupBy.Location = new System.Drawing.Point(263, 76);
         this.chkGroupBy.Name = "chkGroupBy";
         this.chkGroupBy.Properties.Caption = "chkGroupBy";
         this.chkGroupBy.Size = new System.Drawing.Size(257, 20);
         this.chkGroupBy.StyleController = this.panel1LayoutControl;
         this.chkGroupBy.TabIndex = 11;
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(2, 100);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(518, 377);
         this.gridControl.TabIndex = 3;
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
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsView.ShowIndicator = false;
         // 
         // chkShowAdvancedParameter
         // 
         this.chkShowAdvancedParameter.AllowClicksOutsideControlArea = false;
         this.chkShowAdvancedParameter.Location = new System.Drawing.Point(2, 76);
         this.chkShowAdvancedParameter.Name = "chkShowAdvancedParameter";
         this.chkShowAdvancedParameter.Properties.Caption = "chkShowAdvancedParameter";
         this.chkShowAdvancedParameter.Size = new System.Drawing.Size(257, 20);
         this.chkShowAdvancedParameter.StyleController = this.panel1LayoutControl;
         this.chkShowAdvancedParameter.TabIndex = 10;
         // 
         // cbSelectIndividual
         // 
         this.cbSelectIndividual.Location = new System.Drawing.Point(176, 28);
         this.cbSelectIndividual.Name = "cbSelectIndividual";
         this.cbSelectIndividual.Properties.AllowMouseWheel = false;
         this.cbSelectIndividual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbSelectIndividual.Size = new System.Drawing.Size(344, 20);
         this.cbSelectIndividual.StyleController = this.panel1LayoutControl;
         this.cbSelectIndividual.TabIndex = 1;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemGrid,
            this.layoutControlItemSelectIndividual,
            this.layoutControlItemShowAdvanceParameters,
            this.layoutControlItemGroupBy,
            this.layoutControlItemContainerPath,
            this.layoutControlItemLoadParameter,
            this.layoutControlItemAddParameter,
            this.emptySpaceItem1});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(522, 479);
         this.Root.TextVisible = false;
         // 
         // layoutControlItemGrid
         // 
         this.layoutControlItemGrid.Control = this.gridControl;
         this.layoutControlItemGrid.Location = new System.Drawing.Point(0, 98);
         this.layoutControlItemGrid.Name = "layoutControlItemGrid";
         this.layoutControlItemGrid.Size = new System.Drawing.Size(522, 381);
         this.layoutControlItemGrid.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemGrid.TextVisible = false;
         // 
         // layoutControlItemSelectIndividual
         // 
         this.layoutControlItemSelectIndividual.Control = this.cbSelectIndividual;
         this.layoutControlItemSelectIndividual.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemSelectIndividual.Name = "layoutControlItemSelectIndividual";
         this.layoutControlItemSelectIndividual.Size = new System.Drawing.Size(522, 24);
         this.layoutControlItemSelectIndividual.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutControlItemShowAdvanceParameters
         // 
         this.layoutControlItemShowAdvanceParameters.Control = this.chkShowAdvancedParameter;
         this.layoutControlItemShowAdvanceParameters.Location = new System.Drawing.Point(0, 74);
         this.layoutControlItemShowAdvanceParameters.Name = "layoutControlItemShowAdvanceParameters";
         this.layoutControlItemShowAdvanceParameters.Size = new System.Drawing.Size(261, 24);
         this.layoutControlItemShowAdvanceParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemShowAdvanceParameters.TextVisible = false;
         // 
         // layoutControlItemGroupBy
         // 
         this.layoutControlItemGroupBy.Control = this.chkGroupBy;
         this.layoutControlItemGroupBy.Location = new System.Drawing.Point(261, 74);
         this.layoutControlItemGroupBy.Name = "layoutControlItemGroupBy";
         this.layoutControlItemGroupBy.Size = new System.Drawing.Size(261, 24);
         this.layoutControlItemGroupBy.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemGroupBy.TextVisible = false;
         // 
         // layoutControlItemContainerPath
         // 
         this.layoutControlItemContainerPath.Control = this.tbContainerPath;
         this.layoutControlItemContainerPath.Location = new System.Drawing.Point(0, 50);
         this.layoutControlItemContainerPath.Name = "layoutControlItemContainerPath";
         this.layoutControlItemContainerPath.Size = new System.Drawing.Size(522, 24);
         this.layoutControlItemContainerPath.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutControlItemLoadParameter
         // 
         this.layoutControlItemLoadParameter.Control = this.btnLoadParameter;
         this.layoutControlItemLoadParameter.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemLoadParameter.Name = "layoutControlItemLoadParameter";
         this.layoutControlItemLoadParameter.Size = new System.Drawing.Size(422, 26);
         this.layoutControlItemLoadParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemLoadParameter.TextVisible = false;
         // 
         // layoutControlItemAddParameter
         // 
         this.layoutControlItemAddParameter.Control = this.btnAddParameter;
         this.layoutControlItemAddParameter.Location = new System.Drawing.Point(422, 0);
         this.layoutControlItemAddParameter.Name = "layoutControlItemAddParameter";
         this.layoutControlItemAddParameter.Size = new System.Drawing.Size(90, 26);
         this.layoutControlItemAddParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddParameter.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(512, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(10, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutGroup
         // 
         this.layoutGroup.CustomizationFormText = "layoutGroup";
         this.layoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutGroup.GroupBordersVisible = false;
         this.layoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemParmaterLists});
         this.layoutGroup.Name = "Root";
         this.layoutGroup.Size = new System.Drawing.Size(1072, 503);
         this.layoutGroup.Text = "layoutGroup";
         this.layoutGroup.TextVisible = false;
         // 
         // layoutControlItemParmaterLists
         // 
         this.layoutControlItemParmaterLists.Control = this.splitContainerControl;
         this.layoutControlItemParmaterLists.CustomizationFormText = "layoutControlItemLoadParameter";
         this.layoutControlItemParmaterLists.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemParmaterLists.Name = "layoutControlItem1";
         this.layoutControlItemParmaterLists.Size = new System.Drawing.Size(1052, 483);
         this.layoutControlItemParmaterLists.Text = "layoutControlItemLoadParameter";
         this.layoutControlItemParmaterLists.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemParmaterLists.TextVisible = false;
         // 
         // EditParametersInContainerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditParametersInContainerView";
         this.Size = new System.Drawing.Size(1072, 503);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel1)).EndInit();
         this.splitContainerControl.Panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
         this.splitContainerControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panel1LayoutControl)).EndInit();
         this.panel1LayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbContainerPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroupBy.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowAdvancedParameter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbSelectIndividual.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemShowAdvanceParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGroupBy)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemContainerPath)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoadParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParmaterLists)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnAddParameter;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemParmaterLists;
      private DevExpress.XtraEditors.SimpleButton btnLoadParameter;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private UxCheckEdit chkShowAdvancedParameter;
      private UxCheckEdit chkGroupBy;
      private UxGridControl gridControl;
      private UxComboBoxEdit cbSelectIndividual;
      private UxLayoutControl panel1LayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGrid;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelectIndividual;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemShowAdvanceParameters;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGroupBy;
      private DevExpress.XtraEditors.TextEdit tbContainerPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemContainerPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemLoadParameter;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddParameter;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
