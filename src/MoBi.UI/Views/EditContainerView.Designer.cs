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
         this.panelTags = new DevExpress.XtraEditors.PanelControl();
         this.cbContainerMode = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.cbContainerType = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemContainerType = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemContainerMode = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemTab = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabPagesControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelTags)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerMode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerType)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerMode)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTab)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
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
         this.layoutControl.Controls.Add(this.panelTags);
         this.layoutControl.Controls.Add(this.cbContainerMode);
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Controls.Add(this.btName);
         this.layoutControl.Controls.Add(this.cbContainerType);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(878, 592);
         this.layoutControl.TabIndex = 13;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelTags
         // 
         this.panelTags.Location = new System.Drawing.Point(10, 58);
         this.panelTags.Margin = new System.Windows.Forms.Padding(0);
         this.panelTags.Name = "panelTags";
         this.panelTags.Size = new System.Drawing.Size(858, 500);
         this.panelTags.TabIndex = 14;
         // 
         // cbContainerMode
         // 
         this.cbContainerMode.Location = new System.Drawing.Point(324, 36);
         this.cbContainerMode.Name = "cbContainerMode";
         this.cbContainerMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbContainerMode.Size = new System.Drawing.Size(542, 20);
         this.cbContainerMode.StyleController = this.layoutControl;
         this.cbContainerMode.TabIndex = 13;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(91, 560);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(775, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 12;
         // 
         // btName
         // 
         this.btName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btName.Location = new System.Drawing.Point(91, 12);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(775, 20);
         this.btName.StyleController = this.layoutControl;
         this.btName.TabIndex = 0;
         // 
         // cbContainerType
         // 
         this.cbContainerType.Location = new System.Drawing.Point(91, 36);
         this.cbContainerType.Name = "cbContainerType";
         this.cbContainerType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbContainerType.Size = new System.Drawing.Size(229, 20);
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
            this.layoutItemContainerType,
            this.layoutItemContainerMode,
            this.layoutItemName,
            this.layoutControlItemTab});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(878, 592);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "Description";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 548);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(858, 24);
         this.layoutItemDescription.Text = "Description:";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(76, 13);
         // 
         // layoutItemContainerType
         // 
         this.layoutItemContainerType.Control = this.cbContainerType;
         this.layoutItemContainerType.CustomizationFormText = "Container Type";
         this.layoutItemContainerType.Location = new System.Drawing.Point(0, 24);
         this.layoutItemContainerType.Name = "layoutItemContainerType";
         this.layoutItemContainerType.Size = new System.Drawing.Size(312, 24);
         this.layoutItemContainerType.Text = "Container type:";
         this.layoutItemContainerType.TextSize = new System.Drawing.Size(76, 13);
         // 
         // layoutItemContainerMode
         // 
         this.layoutItemContainerMode.Control = this.cbContainerMode;
         this.layoutItemContainerMode.CustomizationFormText = "layoutItemContainerMode";
         this.layoutItemContainerMode.Location = new System.Drawing.Point(312, 24);
         this.layoutItemContainerMode.Name = "layoutItemContainerMode";
         this.layoutItemContainerMode.Size = new System.Drawing.Size(546, 24);
         this.layoutItemContainerMode.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemContainerMode.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "Name";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(858, 24);
         this.layoutItemName.Text = "Name:";
         this.layoutItemName.TextSize = new System.Drawing.Size(76, 13);
         // 
         // layoutControlItemTab
         // 
         this.layoutControlItemTab.Control = this.panelTags;
         this.layoutControlItemTab.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItemTab.Name = "layoutControlItemTab";
         this.layoutControlItemTab.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlItemTab.Size = new System.Drawing.Size(858, 500);
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
         // EditContainerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabPagesControl);
         this.MinimumSize = new System.Drawing.Size(384, 139);
         this.Name = "EditContainerView";
         this.Size = new System.Drawing.Size(880, 617);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelTags)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerMode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbContainerType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerType)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerMode)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTab)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
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
      private ComboBoxEdit cbContainerMode;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemContainerType;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemContainerMode;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private PanelControl panelTags;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTab;
   }
}
