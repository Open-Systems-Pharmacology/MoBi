using DevExpress.XtraEditors;

namespace MoBi.UI.Views
{
   partial class EditNeighborhoodBuilderView
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
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemTab = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabPagesControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         this.tbFirstNeighborPath = new DevExpress.XtraEditors.TextEdit();
         this.layoutItemFirstNeighborPath = new DevExpress.XtraLayout.LayoutControlItem();
         this.tbSecondNeighborPath = new DevExpress.XtraEditors.TextEdit();
         this.layoutItemSecondNeighborPath = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelTags)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTab)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbFirstNeighborPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFirstNeighborPath)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSecondNeighborPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSecondNeighborPath)).BeginInit();
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
         this.layoutControl.Controls.Add(this.tbSecondNeighborPath);
         this.layoutControl.Controls.Add(this.tbFirstNeighborPath);
         this.layoutControl.Controls.Add(this.panelTags);
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Controls.Add(this.btName);
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
         this.panelTags.Location = new System.Drawing.Point(10, 82);
         this.panelTags.Margin = new System.Windows.Forms.Padding(0);
         this.panelTags.Name = "panelTags";
         this.panelTags.Size = new System.Drawing.Size(858, 476);
         this.panelTags.TabIndex = 14;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(176, 560);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(690, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 12;
         // 
         // btName
         // 
         this.btName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btName.Location = new System.Drawing.Point(176, 12);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(690, 20);
         this.btName.StyleController = this.layoutControl;
         this.btName.TabIndex = 0;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDescription,
            this.layoutItemName,
            this.layoutControlItemTab,
            this.layoutItemFirstNeighborPath,
            this.layoutItemSecondNeighborPath});
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
         this.layoutItemDescription.TextSize = new System.Drawing.Size(152, 13);
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "Name";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(858, 24);
         this.layoutItemName.Text = "Name:";
         this.layoutItemName.TextSize = new System.Drawing.Size(152, 13);
         // 
         // layoutControlItemTab
         // 
         this.layoutControlItemTab.Control = this.panelTags;
         this.layoutControlItemTab.Location = new System.Drawing.Point(0, 72);
         this.layoutControlItemTab.Name = "layoutControlItemTab";
         this.layoutControlItemTab.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlItemTab.Size = new System.Drawing.Size(858, 476);
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
         // tbFirstNeighborPath
         // 
         this.tbFirstNeighborPath.Location = new System.Drawing.Point(176, 36);
         this.tbFirstNeighborPath.Name = "tbFirstNeighborPath";
         this.tbFirstNeighborPath.Size = new System.Drawing.Size(690, 20);
         this.tbFirstNeighborPath.StyleController = this.layoutControl;
         this.tbFirstNeighborPath.TabIndex = 15;
         // 
         // layoutItemFirstNeighborPath
         // 
         this.layoutItemFirstNeighborPath.Control = this.tbFirstNeighborPath;
         this.layoutItemFirstNeighborPath.Location = new System.Drawing.Point(0, 24);
         this.layoutItemFirstNeighborPath.Name = "layoutItemFirstNeighborPath";
         this.layoutItemFirstNeighborPath.Size = new System.Drawing.Size(858, 24);
         this.layoutItemFirstNeighborPath.TextSize = new System.Drawing.Size(152, 13);
         // 
         // tbSecondNeighborPath
         // 
         this.tbSecondNeighborPath.Location = new System.Drawing.Point(176, 60);
         this.tbSecondNeighborPath.Name = "tbSecondNeighborPath";
         this.tbSecondNeighborPath.Size = new System.Drawing.Size(690, 20);
         this.tbSecondNeighborPath.StyleController = this.layoutControl;
         this.tbSecondNeighborPath.TabIndex = 16;
         // 
         // layoutItemSecondNeighborPath
         // 
         this.layoutItemSecondNeighborPath.Control = this.tbSecondNeighborPath;
         this.layoutItemSecondNeighborPath.Location = new System.Drawing.Point(0, 48);
         this.layoutItemSecondNeighborPath.Name = "layoutItemSecondNeighborPath";
         this.layoutItemSecondNeighborPath.Size = new System.Drawing.Size(858, 24);
         this.layoutItemSecondNeighborPath.TextSize = new System.Drawing.Size(152, 13);
         // 
         // EditNeighborhoodBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabPagesControl);
         this.Margin = new System.Windows.Forms.Padding(4);
         this.MinimumSize = new System.Drawing.Size(384, 139);
         this.Name = "EditNeighborhoodBuilderView";
         this.Size = new System.Drawing.Size(880, 617);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelTags)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTab)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbFirstNeighborPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFirstNeighborPath)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSecondNeighborPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSecondNeighborPath)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected DevExpress.XtraTab.XtraTabPage tabProperties;
      protected DevExpress.XtraTab.XtraTabControl tabPagesControl;
      protected DevExpress.XtraTab.XtraTabPage tabParameters;
      private ButtonEdit btName;
      private MemoExEdit htmlEditor;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private PanelControl panelTags;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTab;
      private TextEdit tbSecondNeighborPath;
      private TextEdit tbFirstNeighborPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFirstNeighborPath;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSecondNeighborPath;
   }
}
