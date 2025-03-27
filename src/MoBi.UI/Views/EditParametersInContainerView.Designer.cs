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
         this.cbSelectIndividual = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.chkGroupBy = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkShowAdvancedParameter = new OSPSuite.UI.Controls.UxCheckEdit();
         this.btnLoadParameter = new DevExpress.XtraEditors.SimpleButton();
         this.lblParentName = new DevExpress.XtraEditors.LabelControl();
         this.btnAddParameter = new DevExpress.XtraEditors.SimpleButton();
         this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemParentName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemParmaterLists = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemAddParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemLoadParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemShowAdvanceParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItemGroupBy = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItemSelectIndividual = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbSelectIndividual.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroupBy.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowAdvancedParameter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel1)).BeginInit();
         this.splitContainerControl.Panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel2)).BeginInit();
         this.splitContainerControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParentName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParmaterLists)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoadParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemShowAdvanceParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGroupBy)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.cbSelectIndividual);
         this.layoutControl.Controls.Add(this.chkGroupBy);
         this.layoutControl.Controls.Add(this.chkShowAdvancedParameter);
         this.layoutControl.Controls.Add(this.btnLoadParameter);
         this.layoutControl.Controls.Add(this.lblParentName);
         this.layoutControl.Controls.Add(this.btnAddParameter);
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
         // cbSelectIndividual
         // 
         this.cbSelectIndividual.Location = new System.Drawing.Point(276, 12);
         this.cbSelectIndividual.Name = "cbSelectIndividual";
         this.cbSelectIndividual.Properties.AllowMouseWheel = false;
         this.cbSelectIndividual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbSelectIndividual.Size = new System.Drawing.Size(143, 20);
         this.cbSelectIndividual.StyleController = this.layoutControl;
         this.cbSelectIndividual.TabIndex = 1;
         // 
         // chkGroupBy
         // 
         this.chkGroupBy.AllowClicksOutsideControlArea = false;
         this.chkGroupBy.Location = new System.Drawing.Point(590, 15);
         this.chkGroupBy.Name = "chkGroupBy";
         this.chkGroupBy.Properties.Caption = "chkGroupBy";
         this.chkGroupBy.Size = new System.Drawing.Size(80, 20);
         this.chkGroupBy.StyleController = this.layoutControl;
         this.chkGroupBy.TabIndex = 11;
         // 
         // chkShowAdvancedParameter
         // 
         this.chkShowAdvancedParameter.AllowClicksOutsideControlArea = false;
         this.chkShowAdvancedParameter.Location = new System.Drawing.Point(423, 15);
         this.chkShowAdvancedParameter.Name = "chkShowAdvancedParameter";
         this.chkShowAdvancedParameter.Properties.Caption = "chkShowAdvancedParameter";
         this.chkShowAdvancedParameter.Size = new System.Drawing.Size(163, 20);
         this.chkShowAdvancedParameter.StyleController = this.layoutControl;
         this.chkShowAdvancedParameter.TabIndex = 10;
         // 
         // btnLoadParameter
         // 
         this.btnLoadParameter.Location = new System.Drawing.Point(867, 12);
         this.btnLoadParameter.Name = "btnLoadParameter";
         this.btnLoadParameter.Size = new System.Drawing.Size(90, 22);
         this.btnLoadParameter.StyleController = this.layoutControl;
         this.btnLoadParameter.TabIndex = 9;
         this.btnLoadParameter.Text = "btLoadParameter";
         // 
         // lblParentName
         // 
         this.lblParentName.Location = new System.Drawing.Point(12, 15);
         this.lblParentName.Name = "lblParentName";
         this.lblParentName.Size = new System.Drawing.Size(69, 13);
         this.lblParentName.StyleController = this.layoutControl;
         this.lblParentName.TabIndex = 8;
         this.lblParentName.Text = "lblParentName";
         // 
         // btnAddParameter
         // 
         this.btnAddParameter.Location = new System.Drawing.Point(961, 12);
         this.btnAddParameter.Name = "btnAddParameter";
         this.btnAddParameter.Size = new System.Drawing.Size(99, 22);
         this.btnAddParameter.StyleController = this.layoutControl;
         this.btnAddParameter.TabIndex = 7;
         this.btnAddParameter.Text = "btAddParameter";
         // 
         // splitContainerControl
         // 
         this.splitContainerControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.splitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitContainerControl.Location = new System.Drawing.Point(12, 39);
         this.splitContainerControl.Name = "splitContainerControl";
         // 
         // splitContainerControl.Panel1
         // 
         this.splitContainerControl.Panel1.AutoScroll = true;
         this.splitContainerControl.Panel1.Controls.Add(this.gridControl);
         this.splitContainerControl.Panel1.Text = "Panel1";
         // 
         // splitContainerControl.Panel2
         // 
         this.splitContainerControl.Panel2.Text = "Panel2";
         this.splitContainerControl.Size = new System.Drawing.Size(1048, 452);
         this.splitContainerControl.SplitterPosition = 522;
         this.splitContainerControl.TabIndex = 6;
         this.splitContainerControl.Text = "splitContainerControl1";
         // 
         // gridControl
         // 
         this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl.Location = new System.Drawing.Point(0, 0);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(522, 452);
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
         // layoutGroup
         // 
         this.layoutGroup.CustomizationFormText = "layoutGroup";
         this.layoutGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutGroup.GroupBordersVisible = false;
         this.layoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemParentName,
            this.layoutControlItemParmaterLists,
            this.layoutControlItemAddParameter,
            this.layoutControlItemLoadParameter,
            this.layoutControlItemShowAdvanceParameters,
            this.emptySpaceItem1,
            this.layoutControlItemGroupBy,
            this.emptySpaceItem2,
            this.layoutControlItemSelectIndividual});
         this.layoutGroup.Name = "Root";
         this.layoutGroup.Size = new System.Drawing.Size(1072, 503);
         this.layoutGroup.Text = "layoutGroup";
         this.layoutGroup.TextVisible = false;
         // 
         // layoutControlItemParentName
         // 
         this.layoutControlItemParentName.Control = this.lblParentName;
         this.layoutControlItemParentName.CustomizationFormText = "layoutControlItemParentName";
         this.layoutControlItemParentName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemParentName.Name = "layoutControlItem3";
         this.layoutControlItemParentName.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
         this.layoutControlItemParentName.Size = new System.Drawing.Size(73, 27);
         this.layoutControlItemParentName.Text = "layoutControlItemParentName";
         this.layoutControlItemParentName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutControlItemParentName.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemParentName.TextToControlDistance = 0;
         this.layoutControlItemParentName.TextVisible = false;
         // 
         // layoutControlItemParmaterLists
         // 
         this.layoutControlItemParmaterLists.Control = this.splitContainerControl;
         this.layoutControlItemParmaterLists.CustomizationFormText = "layoutControlItemLoadParameter";
         this.layoutControlItemParmaterLists.Location = new System.Drawing.Point(0, 27);
         this.layoutControlItemParmaterLists.Name = "layoutControlItem1";
         this.layoutControlItemParmaterLists.Size = new System.Drawing.Size(1052, 456);
         this.layoutControlItemParmaterLists.Text = "layoutControlItemLoadParameter";
         this.layoutControlItemParmaterLists.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemParmaterLists.TextVisible = false;
         // 
         // layoutControlItemAddParameter
         // 
         this.layoutControlItemAddParameter.Control = this.btnAddParameter;
         this.layoutControlItemAddParameter.CustomizationFormText = "layoutControlItemAddParameter";
         this.layoutControlItemAddParameter.Location = new System.Drawing.Point(949, 0);
         this.layoutControlItemAddParameter.Name = "layoutControlItem2";
         this.layoutControlItemAddParameter.Size = new System.Drawing.Size(103, 27);
         this.layoutControlItemAddParameter.Text = "layoutControlItemAddParameter";
         this.layoutControlItemAddParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddParameter.TextVisible = false;
         // 
         // layoutControlItemLoadParameter
         // 
         this.layoutControlItemLoadParameter.Control = this.btnLoadParameter;
         this.layoutControlItemLoadParameter.CustomizationFormText = "layoutControlItemLoadParameter";
         this.layoutControlItemLoadParameter.Location = new System.Drawing.Point(855, 0);
         this.layoutControlItemLoadParameter.Name = "item0";
         this.layoutControlItemLoadParameter.Size = new System.Drawing.Size(94, 27);
         this.layoutControlItemLoadParameter.Text = "layoutControlItemLoadParameter";
         this.layoutControlItemLoadParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemLoadParameter.TextVisible = false;
         // 
         // layoutControlItemShowAdvanceParameters
         // 
         this.layoutControlItemShowAdvanceParameters.Control = this.chkShowAdvancedParameter;
         this.layoutControlItemShowAdvanceParameters.CustomizationFormText = "layoutControlItemShowAdvanceParameters";
         this.layoutControlItemShowAdvanceParameters.Location = new System.Drawing.Point(411, 0);
         this.layoutControlItemShowAdvanceParameters.Name = "item1";
         this.layoutControlItemShowAdvanceParameters.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
         this.layoutControlItemShowAdvanceParameters.Size = new System.Drawing.Size(167, 27);
         this.layoutControlItemShowAdvanceParameters.Text = "layoutControlItemShowAdvanceParameters";
         this.layoutControlItemShowAdvanceParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemShowAdvanceParameters.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(662, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(193, 27);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItemGroupBy
         // 
         this.layoutControlItemGroupBy.Control = this.chkGroupBy;
         this.layoutControlItemGroupBy.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItemGroupBy.Location = new System.Drawing.Point(578, 0);
         this.layoutControlItemGroupBy.Name = "layoutControlItemGroupBy";
         this.layoutControlItemGroupBy.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
         this.layoutControlItemGroupBy.Size = new System.Drawing.Size(84, 27);
         this.layoutControlItemGroupBy.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemGroupBy.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
         this.emptySpaceItem2.Location = new System.Drawing.Point(73, 0);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(17, 27);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItemSelectIndividual
         // 
         this.layoutControlItemSelectIndividual.Control = this.cbSelectIndividual;
         this.layoutControlItemSelectIndividual.Location = new System.Drawing.Point(90, 0);
         this.layoutControlItemSelectIndividual.Name = "layoutControlItemSelectIndividual";
         this.layoutControlItemSelectIndividual.Size = new System.Drawing.Size(321, 27);
         this.layoutControlItemSelectIndividual.TextSize = new System.Drawing.Size(162, 13);
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
         ((System.ComponentModel.ISupportInitialize)(this.cbSelectIndividual.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroupBy.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowAdvancedParameter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel1)).EndInit();
         this.splitContainerControl.Panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl.Panel2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
         this.splitContainerControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParentName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParmaterLists)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoadParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemShowAdvanceParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGroupBy)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnAddParameter;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemParmaterLists;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddParameter;
      private DevExpress.XtraEditors.LabelControl lblParentName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemParentName;
      private DevExpress.XtraEditors.SimpleButton btnLoadParameter;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemLoadParameter;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemShowAdvanceParameters;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGroupBy;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private UxCheckEdit chkShowAdvancedParameter;
      private UxCheckEdit chkGroupBy;
      private UxGridControl gridControl;
      private UxComboBoxEdit cbSelectIndividual;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelectIndividual;
   }
}
