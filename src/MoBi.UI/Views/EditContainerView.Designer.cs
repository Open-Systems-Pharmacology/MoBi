using DevExpress.XtraEditors;

namespace MoBi.UI.Views
{
   partial class EditContainerView 
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
      protected virtual void InitializeComponent()
      {
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btParentPath = new DevExpress.XtraEditors.ButtonEdit();
         this.panelTags = new DevExpress.XtraEditors.PanelControl();
         this.cbContainerMode = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.cbContainerType = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemTab = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabPagesControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         this.tbSource = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlItemSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnGoToSource = new OSPSuite.UI.Controls.UxSimpleButton();
         this.layoutControlItemGoToSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemContainerType = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemContainerMode = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemParentPath = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.btParentPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTags)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerMode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTab)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbSource.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGoToSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerType)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerMode)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParentPath)).BeginInit();
         this.SuspendLayout();
         // 
         // tabProperties
         // 
         this.tabProperties.Controls.Add(this.layoutControl);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(878, 592);
         this.tabProperties.Text = "Properties";
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.btnGoToSource);
         this.layoutControl.Controls.Add(this.tbSource);
         this.layoutControl.Controls.Add(this.btParentPath);
         this.layoutControl.Controls.Add(this.panelTags);
         this.layoutControl.Controls.Add(this.cbContainerMode);
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Controls.Add(this.btName);
         this.layoutControl.Controls.Add(this.cbContainerType);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1353, 324, 650, 400);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(878, 592);
         this.layoutControl.TabIndex = 13;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btParentPath
         // 
         this.btParentPath.Location = new System.Drawing.Point(141, 58);
         this.btParentPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.btParentPath.Name = "btParentPath";
         this.btParentPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btParentPath.Size = new System.Drawing.Size(726, 20);
         this.btParentPath.StyleController = this.layoutControl;
         this.btParentPath.TabIndex = 15;
         // 
         // panelTags
         // 
         this.panelTags.Location = new System.Drawing.Point(9, 106);
         this.panelTags.Margin = new System.Windows.Forms.Padding(0);
         this.panelTags.Name = "panelTags";
         this.panelTags.Size = new System.Drawing.Size(860, 454);
         this.panelTags.TabIndex = 14;
         // 
         // cbContainerMode
         // 
         this.cbContainerMode.Location = new System.Drawing.Point(324, 34);
         this.cbContainerMode.Name = "cbContainerMode";
         this.cbContainerMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbContainerMode.Size = new System.Drawing.Size(543, 20);
         this.cbContainerMode.StyleController = this.layoutControl;
         this.cbContainerMode.TabIndex = 13;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(141, 562);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(726, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 12;
         // 
         // btName
         // 
         this.btName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btName.Location = new System.Drawing.Point(141, 10);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(726, 20);
         this.btName.StyleController = this.layoutControl;
         this.btName.TabIndex = 0;
         // 
         // cbContainerType
         // 
         this.cbContainerType.Location = new System.Drawing.Point(141, 34);
         this.cbContainerType.Name = "cbContainerType";
         this.cbContainerType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbContainerType.Size = new System.Drawing.Size(179, 20);
         this.cbContainerType.StyleController = this.layoutControl;
         this.cbContainerType.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDescription,
            this.layoutControlItemTab,
            this.layoutControlItemSource,
            this.layoutControlItemGoToSource,
            this.layoutItemName,
            this.layoutItemContainerType,
            this.layoutItemContainerMode,
            this.layoutItemParentPath});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(878, 592);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "Description";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 552);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(860, 24);
         this.layoutItemDescription.Text = "Description:";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(120, 13);
         // 
         // layoutControlItemTab
         // 
         this.layoutControlItemTab.Control = this.panelTags;
         this.layoutControlItemTab.Location = new System.Drawing.Point(0, 98);
         this.layoutControlItemTab.Name = "layoutControlItemTab";
         this.layoutControlItemTab.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlItemTab.Size = new System.Drawing.Size(860, 454);
         this.layoutControlItemTab.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemTab.TextVisible = false;
         // 
         // tabPagesControl
         // 
         this.tabPagesControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabPagesControl.Location = new System.Drawing.Point(0, 0);
         this.tabPagesControl.Name = "tabPagesControl";
         this.tabPagesControl.SelectedTabPage = this.tabProperties;
         this.tabPagesControl.Size = new System.Drawing.Size(880, 617);
         this.tabPagesControl.TabIndex = 0;
         this.tabPagesControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabParameters});
         // 
         // tabParameters
         // 
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.Size = new System.Drawing.Size(878, 592);
         this.tabParameters.Text = "Parameters";
         // 
         // tbSource
         // 
         this.tbSource.Location = new System.Drawing.Point(141, 82);
         this.tbSource.Name = "tbSource";
         this.tbSource.Size = new System.Drawing.Size(641, 20);
         this.tbSource.StyleController = this.layoutControl;
         this.tbSource.TabIndex = 16;
         // 
         // layoutControlItemSource
         // 
         this.layoutControlItemSource.Control = this.tbSource;
         this.layoutControlItemSource.Location = new System.Drawing.Point(0, 72);
         this.layoutControlItemSource.Name = "layoutControlItemSource";
         this.layoutControlItemSource.Size = new System.Drawing.Size(775, 26);
         this.layoutControlItemSource.TextSize = new System.Drawing.Size(120, 13);
         // 
         // btnGoToSource
         // 
         this.btnGoToSource.Location = new System.Drawing.Point(786, 82);
         this.btnGoToSource.Manager = null;
         this.btnGoToSource.Name = "btnGoToSource";
         this.btnGoToSource.Shortcut = System.Windows.Forms.Keys.None;
         this.btnGoToSource.Size = new System.Drawing.Size(81, 22);
         this.btnGoToSource.StyleController = this.layoutControl;
         this.btnGoToSource.TabIndex = 17;
         this.btnGoToSource.Text = "btnGoToSource";
         // 
         // layoutControlItemGoToSource
         // 
         this.layoutControlItemGoToSource.Control = this.btnGoToSource;
         this.layoutControlItemGoToSource.Location = new System.Drawing.Point(775, 72);
         this.layoutControlItemGoToSource.Name = "layoutControlItemGoToSource";
         this.layoutControlItemGoToSource.Size = new System.Drawing.Size(85, 26);
         this.layoutControlItemGoToSource.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemGoToSource.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "Name";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(860, 24);
         this.layoutItemName.Text = "Name:";
         this.layoutItemName.TextSize = new System.Drawing.Size(120, 13);
         // 
         // layoutItemContainerType
         // 
         this.layoutItemContainerType.Control = this.cbContainerType;
         this.layoutItemContainerType.CustomizationFormText = "Container Type";
         this.layoutItemContainerType.Location = new System.Drawing.Point(0, 24);
         this.layoutItemContainerType.Name = "layoutItemContainerType";
         this.layoutItemContainerType.Size = new System.Drawing.Size(313, 24);
         this.layoutItemContainerType.Text = "Container type:";
         this.layoutItemContainerType.TextSize = new System.Drawing.Size(120, 13);
         // 
         // layoutItemContainerMode
         // 
         this.layoutItemContainerMode.Control = this.cbContainerMode;
         this.layoutItemContainerMode.CustomizationFormText = "layoutItemContainerMode";
         this.layoutItemContainerMode.Location = new System.Drawing.Point(313, 24);
         this.layoutItemContainerMode.Name = "layoutItemContainerMode";
         this.layoutItemContainerMode.Size = new System.Drawing.Size(547, 24);
         this.layoutItemContainerMode.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemContainerMode.TextVisible = false;
         // 
         // layoutItemParentPath
         // 
         this.layoutItemParentPath.Control = this.btParentPath;
         this.layoutItemParentPath.Location = new System.Drawing.Point(0, 48);
         this.layoutItemParentPath.Name = "layoutItemParentPath";
         this.layoutItemParentPath.Size = new System.Drawing.Size(860, 24);
         this.layoutItemParentPath.TextSize = new System.Drawing.Size(120, 13);
         // 
         // EditContainerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabPagesControl);
         this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.MinimumSize = new System.Drawing.Size(384, 139);
         this.Name = "EditContainerView";
         this.Size = new System.Drawing.Size(880, 617);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.btParentPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTags)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerMode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTab)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbSource.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGoToSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerType)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerMode)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParentPath)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected DevExpress.XtraTab.XtraTabPage tabProperties;
      protected DevExpress.XtraTab.XtraTabControl tabPagesControl;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbContainerType;
      protected DevExpress.XtraTab.XtraTabPage tabParameters;
      private ButtonEdit btName;
      private MemoExEdit htmlEditor;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private PanelControl panelTags;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTab;
      private ButtonEdit btParentPath;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbContainerMode;
      private OSPSuite.UI.Controls.UxSimpleButton btnGoToSource;
      private TextEdit tbSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGoToSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemContainerType;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemContainerMode;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParentPath;
   }
}
