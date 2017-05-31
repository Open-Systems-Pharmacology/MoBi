using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class EditParameterListView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkGroupBy = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkShowAdvancedParameter = new OSPSuite.UI.Controls.UxCheckEdit();
         this.btLoadParameter = new DevExpress.XtraEditors.SimpleButton();
         this.lblParentName = new DevExpress.XtraEditors.LabelControl();
         this.btAddParameter = new DevExpress.XtraEditors.SimpleButton();
         this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this._gridView = new MoBi.UI.Views.UxGridView();
         this.layoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemParentName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemParmaterLists = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemAddParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemLoadParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemShowAdvanceParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkGroupBy.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowAdvancedParameter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
         this.splitContainerControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParentName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParmaterLists)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoadParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemShowAdvanceParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.chkGroupBy);
         this.layoutControl.Controls.Add(this.chkShowAdvancedParameter);
         this.layoutControl.Controls.Add(this.btLoadParameter);
         this.layoutControl.Controls.Add(this.lblParentName);
         this.layoutControl.Controls.Add(this.btAddParameter);
         this.layoutControl.Controls.Add(this.splitContainerControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(608, 319, 250, 350);
         this.layoutControl.Root = this.layoutGroup;
         this.layoutControl.Size = new System.Drawing.Size(1072, 503);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // chkGroupBy
         // 
         this.chkGroupBy.AllowClicksOutsideControlArea = false;
         this.chkGroupBy.Location = new System.Drawing.Point(339, 15);
         this.chkGroupBy.Name = "chkGroupBy";
         this.chkGroupBy.Properties.Caption = "chkGroupBy";
         this.chkGroupBy.Size = new System.Drawing.Size(122, 19);
         this.chkGroupBy.StyleController = this.layoutControl;
         this.chkGroupBy.TabIndex = 11;
         // 
         // chkShowAdvancedParameter
         // 
         this.chkShowAdvancedParameter.AllowClicksOutsideControlArea = false;
         this.chkShowAdvancedParameter.Location = new System.Drawing.Point(173, 15);
         this.chkShowAdvancedParameter.Name = "chkShowAdvancedParameter";
         this.chkShowAdvancedParameter.Properties.Caption = "chkShowAdvancedParameter";
         this.chkShowAdvancedParameter.Size = new System.Drawing.Size(162, 19);
         this.chkShowAdvancedParameter.StyleController = this.layoutControl;
         this.chkShowAdvancedParameter.TabIndex = 10;
         // 
         // btLoadParameter
         // 
         this.btLoadParameter.Location = new System.Drawing.Point(565, 12);
         this.btLoadParameter.Name = "btLoadParameter";
         this.btLoadParameter.Size = new System.Drawing.Size(115, 22);
         this.btLoadParameter.StyleController = this.layoutControl;
         this.btLoadParameter.TabIndex = 9;
         this.btLoadParameter.Text = "btLoadParameter";
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
         // btAddParameter
         // 
         this.btAddParameter.Location = new System.Drawing.Point(684, 12);
         this.btAddParameter.Name = "btAddParameter";
         this.btAddParameter.Size = new System.Drawing.Size(376, 22);
         this.btAddParameter.StyleController = this.layoutControl;
         this.btAddParameter.TabIndex = 7;
         this.btAddParameter.Text = "btAddParameter";
         // 
         // splitContainerControl
         // 
         this.splitContainerControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.splitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitContainerControl.Location = new System.Drawing.Point(12, 38);
         this.splitContainerControl.Name = "splitContainerControl";
         this.splitContainerControl.Panel1.AutoScroll = true;
         this.splitContainerControl.Panel1.Controls.Add(this.gridControl);
         this.splitContainerControl.Panel1.Text = "Panel1";
         this.splitContainerControl.Panel2.Text = "Panel2";
         this.splitContainerControl.Size = new System.Drawing.Size(1048, 453);
         this.splitContainerControl.SplitterPosition = 522;
         this.splitContainerControl.TabIndex = 6;
         this.splitContainerControl.Text = "splitContainerControl1";
         // 
         // gridControl
         // 
         this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl.Location = new System.Drawing.Point(0, 0);
         this.gridControl.MainView = this._gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(522, 453);
         this.gridControl.TabIndex = 3;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this._gridView});
         // 
         // _gridView
         // 
         this._gridView.AllowsFiltering = true;
         this._gridView.EnableColumnContextMenu = true;
         this._gridView.GridControl = this.gridControl;
         this._gridView.MultiSelect = false;
         this._gridView.Name = "_gridView";
         this._gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this._gridView.OptionsNavigation.AutoFocusNewRow = true;
         this._gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this._gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this._gridView.OptionsView.ShowIndicator = false;
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
            this.layoutControlItem1,
            this.emptySpaceItem2});
         this.layoutGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutGroup.Name = "layoutControlGroup1";
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
         this.layoutControlItemParentName.Size = new System.Drawing.Size(73, 26);
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
         this.layoutControlItemParmaterLists.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemParmaterLists.Name = "layoutControlItem1";
         this.layoutControlItemParmaterLists.Size = new System.Drawing.Size(1052, 457);
         this.layoutControlItemParmaterLists.Text = "layoutControlItemLoadParameter";
         this.layoutControlItemParmaterLists.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemParmaterLists.TextVisible = false;
         // 
         // layoutControlItemAddParameter
         // 
         this.layoutControlItemAddParameter.Control = this.btAddParameter;
         this.layoutControlItemAddParameter.CustomizationFormText = "layoutControlItemAddParameter";
         this.layoutControlItemAddParameter.Location = new System.Drawing.Point(672, 0);
         this.layoutControlItemAddParameter.Name = "layoutControlItem2";
         this.layoutControlItemAddParameter.Size = new System.Drawing.Size(380, 26);
         this.layoutControlItemAddParameter.Text = "layoutControlItemAddParameter";
         this.layoutControlItemAddParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddParameter.TextVisible = false;
         // 
         // layoutControlItemLoadParameter
         // 
         this.layoutControlItemLoadParameter.Control = this.btLoadParameter;
         this.layoutControlItemLoadParameter.CustomizationFormText = "layoutControlItemLoadParameter";
         this.layoutControlItemLoadParameter.Location = new System.Drawing.Point(553, 0);
         this.layoutControlItemLoadParameter.Name = "layoutControlItem1";
         this.layoutControlItemLoadParameter.Size = new System.Drawing.Size(119, 26);
         this.layoutControlItemLoadParameter.Text = "layoutControlItemLoadParameter";
         this.layoutControlItemLoadParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemLoadParameter.TextVisible = false;
         // 
         // layoutControlItemShowAdvanceParameters
         // 
         this.layoutControlItemShowAdvanceParameters.Control = this.chkShowAdvancedParameter;
         this.layoutControlItemShowAdvanceParameters.CustomizationFormText = "layoutControlItemShowAdvanceParameters";
         this.layoutControlItemShowAdvanceParameters.Location = new System.Drawing.Point(161, 0);
         this.layoutControlItemShowAdvanceParameters.Name = "layoutControlItem1";
         this.layoutControlItemShowAdvanceParameters.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
         this.layoutControlItemShowAdvanceParameters.Size = new System.Drawing.Size(166, 26);
         this.layoutControlItemShowAdvanceParameters.Text = "layoutControlItemShowAdvanceParameters";
         this.layoutControlItemShowAdvanceParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemShowAdvanceParameters.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(453, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(100, 26);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkGroupBy;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(327, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
         this.layoutControlItem1.Size = new System.Drawing.Size(126, 26);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.CustomizationFormText = "emptySpaceItem2";
         this.emptySpaceItem2.Location = new System.Drawing.Point(73, 0);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(88, 26);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // EditParameterListView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditParameterListView";
         this.Size = new System.Drawing.Size(1072, 503);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkGroupBy.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowAdvancedParameter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
         this.splitContainerControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParentName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParmaterLists)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLoadParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemShowAdvanceParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btAddParameter;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
      private DevExpress.XtraGrid.GridControl gridControl;
      private UxGridView _gridView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemParmaterLists;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddParameter;
      private DevExpress.XtraEditors.LabelControl lblParentName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemParentName;
      private DevExpress.XtraEditors.SimpleButton btLoadParameter;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemLoadParameter;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemShowAdvanceParameters;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private UxCheckEdit chkShowAdvancedParameter;
      private UxCheckEdit chkGroupBy;



   }
}
