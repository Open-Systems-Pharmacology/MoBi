using MoBi.Presentation.Presenter;

namespace MoBi.UI.Views
{
   partial class EditObserverBuilderView
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
      private void InitializeComponent()
      {
         this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.panelDescriptorConditionList = new DevExpress.XtraEditors.PanelControl();
         this.panelMoleculeList = new DevExpress.XtraEditors.PanelControl();
         this.cbDimension = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.barManager = new DevExpress.XtraBars.BarManager();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDimension = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupInContainerWith = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescriptorConditionList = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMoleculeList = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         this.tabFormula = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDescriptorConditionList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelMoleculeList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupInContainerWith)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescriptorConditionList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMoleculeList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // xtraTabControl1
         // 
         this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
         this.xtraTabControl1.Name = "xtraTabControl1";
         this.xtraTabControl1.SelectedTabPage = this.tabProperties;
         this.xtraTabControl1.Size = new System.Drawing.Size(741, 637);
         this.xtraTabControl1.TabIndex = 0;
         this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabFormula});
         // 
         // tabProperties
         // 
         this.tabProperties.Controls.Add(this.layoutControl1);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(735, 609);
         this.tabProperties.Text = "Properties";
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.htmlEditor);
         this.layoutControl1.Controls.Add(this.panelDescriptorConditionList);
         this.layoutControl1.Controls.Add(this.panelMoleculeList);
         this.layoutControl1.Controls.Add(this.cbDimension);
         this.layoutControl1.Controls.Add(this.btName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(712, 403, 522, 519);
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(735, 609);
         this.layoutControl1.TabIndex = 17;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(177, 577);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(546, 20);
         this.htmlEditor.StyleController = this.layoutControl1;
         this.htmlEditor.TabIndex = 14;
         // 
         // panelDescriptorConditionList
         // 
         this.panelDescriptorConditionList.Location = new System.Drawing.Point(189, 91);
         this.panelDescriptorConditionList.Name = "panelDescriptorConditionList";
         this.panelDescriptorConditionList.Size = new System.Drawing.Size(522, 185);
         this.panelDescriptorConditionList.TabIndex = 5;
         // 
         // panelMoleculeList
         // 
         this.panelMoleculeList.Location = new System.Drawing.Point(177, 297);
         this.panelMoleculeList.Name = "panelMoleculeList";
         this.panelMoleculeList.Size = new System.Drawing.Size(546, 276);
         this.panelMoleculeList.TabIndex = 4;
         // 
         // cbDimension
         // 
         this.cbDimension.Location = new System.Drawing.Point(177, 36);
         this.cbDimension.MenuManager = this.barManager;
         this.cbDimension.Name = "cbDimension";
         this.cbDimension.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbDimension.Size = new System.Drawing.Size(546, 20);
         this.cbDimension.StyleController = this.layoutControl1;
         this.cbDimension.TabIndex = 3;
         // 
         // barManager
         // 
         this.barManager.DockControls.Add(this.barDockControlTop);
         this.barManager.DockControls.Add(this.barDockControlBottom);
         this.barManager.DockControls.Add(this.barDockControlLeft);
         this.barManager.DockControls.Add(this.barDockControlRight);
         this.barManager.Form = this;
         this.barManager.MaxItemId = 0;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(741, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 637);
         this.barDockControlBottom.Size = new System.Drawing.Size(741, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 637);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(741, 0);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 637);
         // 
         // btName
         // 
         this.btName.Location = new System.Drawing.Point(177, 12);
         this.btName.MenuManager = this.barManager;
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(546, 20);
         this.btName.StyleController = this.layoutControl1;
         this.btName.TabIndex = 0;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "Root";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutItemDimension,
            this.layoutItemDescription,
            this.layoutGroupInContainerWith,
            this.layoutItemMoleculeList,
            this.splitterItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(735, 609);
         this.layoutControlGroup1.Text = "Root";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "layoutItemName";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(715, 24);
         this.layoutItemName.Text = "layoutItemName";
         this.layoutItemName.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutItemDimension
         // 
         this.layoutItemDimension.Control = this.cbDimension;
         this.layoutItemDimension.CustomizationFormText = "layoutItemDimension";
         this.layoutItemDimension.Location = new System.Drawing.Point(0, 24);
         this.layoutItemDimension.Name = "layoutItemDimension";
         this.layoutItemDimension.Size = new System.Drawing.Size(715, 24);
         this.layoutItemDimension.Text = "layoutItemDimension";
         this.layoutItemDimension.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "layoutItemDescription";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 565);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(715, 24);
         this.layoutItemDescription.Text = "layoutItemDescription";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutGroupInContainerWith
         // 
         this.layoutGroupInContainerWith.CustomizationFormText = "layoutGroupInContainerWith";
         this.layoutGroupInContainerWith.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDescriptorConditionList});
         this.layoutGroupInContainerWith.Location = new System.Drawing.Point(0, 48);
         this.layoutGroupInContainerWith.Name = "layoutGroupInContainerWith";
         this.layoutGroupInContainerWith.Size = new System.Drawing.Size(715, 232);
         this.layoutGroupInContainerWith.Text = "layoutGroupInContainerWith";
         // 
         // layoutItemDescriptorConditionList
         // 
         this.layoutItemDescriptorConditionList.Control = this.panelDescriptorConditionList;
         this.layoutItemDescriptorConditionList.CustomizationFormText = "layoutItemDescriptorConditionList";
         this.layoutItemDescriptorConditionList.Location = new System.Drawing.Point(0, 0);
         this.layoutItemDescriptorConditionList.Name = "layoutItemDescriptorConditionList";
         this.layoutItemDescriptorConditionList.Size = new System.Drawing.Size(691, 189);
         this.layoutItemDescriptorConditionList.Text = "layoutItemDescriptorConditionList";
         this.layoutItemDescriptorConditionList.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutItemMoleculeList
         // 
         this.layoutItemMoleculeList.Control = this.panelMoleculeList;
         this.layoutItemMoleculeList.CustomizationFormText = "layoutItemMoleculeList";
         this.layoutItemMoleculeList.Location = new System.Drawing.Point(0, 285);
         this.layoutItemMoleculeList.Name = "layoutItemMoleculeList";
         this.layoutItemMoleculeList.Size = new System.Drawing.Size(715, 280);
         this.layoutItemMoleculeList.Text = "layoutItemMoleculeList";
         this.layoutItemMoleculeList.TextSize = new System.Drawing.Size(162, 13);
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.CustomizationFormText = "splitterItem1";
         this.splitterItem1.Location = new System.Drawing.Point(0, 280);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(715, 5);
         // 
         // tabFormula
         // 
         this.tabFormula.Name = "tabFormula";
         this.tabFormula.Padding = new System.Windows.Forms.Padding(10);
         this.tabFormula.Size = new System.Drawing.Size(735, 609);
         this.tabFormula.Text = "Monitor";
         // 
         // EditObserverBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.xtraTabControl1);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "EditObserverBuilderView";
         this.Size = new System.Drawing.Size(741, 637);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
         this.xtraTabControl1.ResumeLayout(false);
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelDescriptorConditionList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelMoleculeList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupInContainerWith)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescriptorConditionList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMoleculeList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      protected DevExpress.XtraTab.XtraTabControl xtraTabControl1;
      protected DevExpress.XtraTab.XtraTabPage tabProperties;
      protected DevExpress.XtraBars.BarManager barManager;
      protected DevExpress.XtraBars.BarDockControl barDockControlTop;
      protected DevExpress.XtraBars.BarDockControl barDockControlBottom;
      protected DevExpress.XtraBars.BarDockControl barDockControlLeft;
      protected DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbDimension;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraTab.XtraTabPage tabFormula;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraEditors.PanelControl panelDescriptorConditionList;
      private DevExpress.XtraEditors.PanelControl panelMoleculeList;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDimension;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMoleculeList;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupInContainerWith;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescriptorConditionList;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
   }
}
