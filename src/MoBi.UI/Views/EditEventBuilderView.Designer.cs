using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class EditEventBuilderView
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
         _gridBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.tabControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.gridControl1 = new OSPSuite.UI.Controls.UxGridControl();
         this.grdAssingments = new MoBi.UI.Views.UxGridView();
         this.btnAddAssignment = new DevExpress.XtraEditors.SimpleButton();
         this.splitContainerControl = new DevExpress.XtraEditors.SplitContainerControl();
         this.btnAddFormula = new DevExpress.XtraEditors.SimpleButton();
         this.cmbCondition = new DevExpress.XtraEditors.ComboBoxEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.chkOneTime = new UxCheckEdit();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemOneTime = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupCondition = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemCondition = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemAddFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.layoutGroupAssignment = new DevExpress.XtraLayout.LayoutControlGroup();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutControlItemAddAssignment = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
         this.tabControl.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdAssingments)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).BeginInit();
         this.splitContainerControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cmbCondition.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkOneTime.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOneTime)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupCondition)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCondition)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupAssignment)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddAssignment)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         this.SuspendLayout();
         // 
         // tabControl
         // 
         this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabControl.Location = new System.Drawing.Point(0, 0);
         this.tabControl.Name = "tabControl";
         this.tabControl.SelectedTabPage = this.tabProperties;
         this.tabControl.Size = new System.Drawing.Size(719, 586);
         this.tabControl.TabIndex = 0;
         this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabParameters});
         // 
         // tabProperties
         // 
         this.tabProperties.Controls.Add(this.layoutControl1);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(713, 558);
         this.tabProperties.Text = "Properties";
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.gridControl1);
         this.layoutControl1.Controls.Add(this.btnAddAssignment);
         this.layoutControl1.Controls.Add(this.splitContainerControl);
         this.layoutControl1.Controls.Add(this.btnAddFormula);
         this.layoutControl1.Controls.Add(this.cmbCondition);
         this.layoutControl1.Controls.Add(this.htmlEditor);
         this.layoutControl1.Controls.Add(this.chkOneTime);
         this.layoutControl1.Controls.Add(this.btName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1069, 197, 479, 642);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(713, 558);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // gridControl1
         // 
         this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.gridControl1.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl1.Location = new System.Drawing.Point(24, 420);
         this.gridControl1.MainView = this.grdAssingments;
         this.gridControl1.Name = "gridControl1";
         this.gridControl1.Size = new System.Drawing.Size(665, 90);
         this.gridControl1.TabIndex = 6;
         this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdAssingments});
         // 
         // grdAssingments
         // 
         this.grdAssingments.AllowsFiltering = true;
         this.grdAssingments.EnableColumnContextMenu = true;
         this.grdAssingments.GridControl = this.gridControl1;
         this.grdAssingments.Name = "grdAssingments";
         this.grdAssingments.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.grdAssingments.OptionsNavigation.AutoFocusNewRow = true;
         this.grdAssingments.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.grdAssingments.OptionsView.ShowGroupPanel = false;
         // 
         // btnAddAssignment
         // 
         this.btnAddAssignment.Location = new System.Drawing.Point(358, 394);
         this.btnAddAssignment.Name = "btnAddAssignment";
         this.btnAddAssignment.Size = new System.Drawing.Size(331, 22);
         this.btnAddAssignment.StyleController = this.layoutControl1;
         this.btnAddAssignment.TabIndex = 17;
         this.btnAddAssignment.Text = "btAddAssignment";
         // 
         // splitContainerControl
         // 
         this.splitContainerControl.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitContainerControl.Location = new System.Drawing.Point(24, 116);
         this.splitContainerControl.Name = "splitContainerControl";
         this.splitContainerControl.Panel1.Text = "Panel1";
         this.splitContainerControl.Panel2.Text = "Panel2";
         this.splitContainerControl.Size = new System.Drawing.Size(665, 226);
         this.splitContainerControl.SplitterPosition = 330;
         this.splitContainerControl.TabIndex = 16;
         this.splitContainerControl.Text = "splitContainerControl1";
         // 
         // btnAddFormula
         // 
         this.btnAddFormula.Location = new System.Drawing.Point(358, 90);
         this.btnAddFormula.Name = "btnAddFormula";
         this.btnAddFormula.Size = new System.Drawing.Size(331, 22);
         this.btnAddFormula.StyleController = this.layoutControl1;
         this.btnAddFormula.TabIndex = 15;
         this.btnAddFormula.Text = "btAddFormula";
         // 
         // cmbCondition
         // 
         this.cmbCondition.Location = new System.Drawing.Point(24, 90);
         this.cmbCondition.Name = "cmbCondition";
         this.cmbCondition.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbCondition.Size = new System.Drawing.Size(330, 20);
         this.cmbCondition.StyleController = this.layoutControl1;
         this.cmbCondition.TabIndex = 14;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Location = new System.Drawing.Point(155, 526);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Size = new System.Drawing.Size(546, 20);
         this.htmlEditor.StyleController = this.layoutControl1;
         this.htmlEditor.TabIndex = 13;
         // 
         // chkOneTime
         // 
         this.chkOneTime.Location = new System.Drawing.Point(12, 36);
         this.chkOneTime.Name = "chkOneTime";
         this.chkOneTime.Properties.Caption = "chkOneTime";
         this.chkOneTime.Size = new System.Drawing.Size(689, 19);
         this.chkOneTime.StyleController = this.layoutControl1;
         this.chkOneTime.TabIndex = 12;
         // 
         // btName
         // 
         this.btName.Location = new System.Drawing.Point(155, 12);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(546, 20);
         this.btName.StyleController = this.layoutControl1;
         this.btName.TabIndex = 9;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemName,
            this.layoutControlItemOneTime,
            this.layoutControlItemDescription,
            this.layoutGroupCondition,
            this.splitterItem1,
            this.layoutGroupAssignment});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(713, 558);
         this.layoutControlGroup1.Text = "Root";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemName
         // 
         this.layoutControlItemName.Control = this.btName;
         this.layoutControlItemName.CustomizationFormText = "layoutControlItemName";
         this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemName.Name = "layoutControlItemName";
         this.layoutControlItemName.Size = new System.Drawing.Size(693, 24);
         this.layoutControlItemName.Text = "layoutControlItemName";
         this.layoutControlItemName.TextSize = new System.Drawing.Size(140, 13);
         // 
         // layoutControlItemOneTime
         // 
         this.layoutControlItemOneTime.Control = this.chkOneTime;
         this.layoutControlItemOneTime.CustomizationFormText = "layoutControlItemOneTime";
         this.layoutControlItemOneTime.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemOneTime.Name = "layoutControlItem1";
         this.layoutControlItemOneTime.Size = new System.Drawing.Size(693, 23);
         this.layoutControlItemOneTime.Text = "layoutControlItemOneTime";
         this.layoutControlItemOneTime.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemOneTime.TextToControlDistance = 0;
         this.layoutControlItemOneTime.TextVisible = false;
         // 
         // layoutControlItemDescription
         // 
         this.layoutControlItemDescription.Control = this.htmlEditor;
         this.layoutControlItemDescription.CustomizationFormText = "layoutControlItemDescription";
         this.layoutControlItemDescription.Location = new System.Drawing.Point(0, 514);
         this.layoutControlItemDescription.Name = "layoutControlItemDescription";
         this.layoutControlItemDescription.Size = new System.Drawing.Size(693, 24);
         this.layoutControlItemDescription.Text = "layoutControlItemDescription";
         this.layoutControlItemDescription.TextSize = new System.Drawing.Size(140, 13);
         // 
         // layoutGroupCondition
         // 
         this.layoutGroupCondition.CustomizationFormText = "layoutGroupCondition";
         this.layoutGroupCondition.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemCondition,
            this.layoutControlItemAddFormula,
            this.layoutControlItem2});
         this.layoutGroupCondition.Location = new System.Drawing.Point(0, 47);
         this.layoutGroupCondition.Name = "layoutGroupCondition";
         this.layoutGroupCondition.Size = new System.Drawing.Size(693, 299);
         this.layoutGroupCondition.Text = "layoutGroupCondition";
         // 
         // layoutControlItemCondition
         // 
         this.layoutControlItemCondition.Control = this.cmbCondition;
         this.layoutControlItemCondition.CustomizationFormText = "layoutControlItemCondition";
         this.layoutControlItemCondition.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemCondition.Name = "layoutControlItemCondition";
         this.layoutControlItemCondition.Size = new System.Drawing.Size(334, 26);
         this.layoutControlItemCondition.Text = "layoutControlItemCondition";
         this.layoutControlItemCondition.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCondition.TextToControlDistance = 0;
         this.layoutControlItemCondition.TextVisible = false;
         // 
         // layoutControlItemAddFormula
         // 
         this.layoutControlItemAddFormula.Control = this.btnAddFormula;
         this.layoutControlItemAddFormula.CustomizationFormText = "layoutControlItemAddFormula";
         this.layoutControlItemAddFormula.Location = new System.Drawing.Point(334, 0);
         this.layoutControlItemAddFormula.Name = "layoutControlItemAddFormula";
         this.layoutControlItemAddFormula.Size = new System.Drawing.Size(335, 26);
         this.layoutControlItemAddFormula.Text = "layoutControlItemAddFormula";
         this.layoutControlItemAddFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddFormula.TextToControlDistance = 0;
         this.layoutControlItemAddFormula.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.splitContainerControl;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(669, 230);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.CustomizationFormText = "splitterItem1";
         this.splitterItem1.Location = new System.Drawing.Point(0, 346);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(693, 5);
         // 
         // layoutGroupAssignment
         // 
         this.layoutGroupAssignment.CustomizationFormText = "layoutGroupAssignment";
         this.layoutGroupAssignment.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1,
            this.layoutControlItemAddAssignment,
            this.layoutControlItem4});
         this.layoutGroupAssignment.Location = new System.Drawing.Point(0, 351);
         this.layoutGroupAssignment.Name = "layoutGroupAssignment";
         this.layoutGroupAssignment.Size = new System.Drawing.Size(693, 163);
         this.layoutGroupAssignment.Text = "layoutGroupAssignment";
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(334, 26);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutControlItemAddAssignment
         // 
         this.layoutControlItemAddAssignment.Control = this.btnAddAssignment;
         this.layoutControlItemAddAssignment.CustomizationFormText = "layoutControlItemAddAssignment";
         this.layoutControlItemAddAssignment.Location = new System.Drawing.Point(334, 0);
         this.layoutControlItemAddAssignment.Name = "layoutControlItemAddAssignment";
         this.layoutControlItemAddAssignment.Size = new System.Drawing.Size(335, 26);
         this.layoutControlItemAddAssignment.Text = "layoutControlItemAddAssignment";
         this.layoutControlItemAddAssignment.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddAssignment.TextToControlDistance = 0;
         this.layoutControlItemAddAssignment.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.gridControl1;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(669, 94);
         this.layoutControlItem4.Text = "layoutControlItem4";
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextToControlDistance = 0;
         this.layoutControlItem4.TextVisible = false;
         // 
         // tabParameters
         // 
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.Size = new System.Drawing.Size(713, 558);
         this.tabParameters.Text = "Parameter";
         // 
         // EditEventBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabControl);
         this.MinimumSize = new System.Drawing.Size(426, 368);
         this.Name = "EditEventBuilderView";
         this.Size = new System.Drawing.Size(719, 586);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
         this.tabControl.ResumeLayout(false);
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdAssingments)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl)).EndInit();
         this.splitContainerControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cmbCondition.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkOneTime.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemOneTime)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupCondition)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCondition)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupAssignment)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddAssignment)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl tabControl;
      private DevExpress.XtraTab.XtraTabPage tabProperties;
      private DevExpress.XtraTab.XtraTabPage tabParameters;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
      private DevExpress.XtraEditors.CheckEdit chkOneTime;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemOneTime;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDescription;
      private DevExpress.XtraGrid.GridControl gridControl1;
      private UxGridView grdAssingments;
      private DevExpress.XtraEditors.SimpleButton btnAddAssignment;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl;
      private DevExpress.XtraEditors.SimpleButton btnAddFormula;
      private DevExpress.XtraEditors.ComboBoxEdit cmbCondition;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCondition;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddAssignment;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupCondition;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupAssignment;
   }
}
