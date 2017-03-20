namespace MoBi.UI.Views
{
   partial class EditApplicationBuilderView
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
         _gridMoleculesBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
         this.tabMolecules = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.panelDescriptorCriteria = new DevExpress.XtraEditors.PanelControl();
         this.gridControlMolecules = new DevExpress.XtraGrid.GridControl();
         this.grdMoleculeBuilder = new MoBi.UI.Views.UxGridView();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.cbApplicatedMoleculeName = new DevExpress.XtraEditors.ComboBoxEdit();
         this.barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layouItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupApplicationBuilder = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupContainer = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabParameter = new DevExpress.XtraTab.XtraTabPage();
         this.layoutItemMolecule = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.tabMolecules.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelDescriptorCriteria)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlMolecules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdMoleculeBuilder)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbApplicatedMoleculeName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupApplicationBuilder)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupContainer)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMolecule)).BeginInit();
         this.SuspendLayout();
         // 
         // xtraTabControl1
         // 
         this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
         this.xtraTabControl1.Name = "xtraTabControl1";
         this.xtraTabControl1.SelectedTabPage = this.tabMolecules;
         this.xtraTabControl1.Size = new System.Drawing.Size(579, 498);
         this.xtraTabControl1.TabIndex = 1;
         this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabMolecules,
            this.tabParameter});
         // 
         // tabMolecules
         // 
         this.tabMolecules.Controls.Add(this.layoutControl);
         this.tabMolecules.Name = "tabMolecules";
         this.tabMolecules.Size = new System.Drawing.Size(573, 470);
         this.tabMolecules.Text = "Administered Molecules";
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.panelDescriptorCriteria);
         this.layoutControl.Controls.Add(this.gridControlMolecules);
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Controls.Add(this.cbApplicatedMoleculeName);
         this.layoutControl.Controls.Add(this.btName);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(673, 381, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(573, 470);
         this.layoutControl.TabIndex = 22;
         this.layoutControl.Text = "layoutControl1";
         // 
         // panelDescriptorCriteria
         // 
         this.panelDescriptorCriteria.Location = new System.Drawing.Point(24, 273);
         this.panelDescriptorCriteria.Name = "panelDescriptorCriteria";
         this.panelDescriptorCriteria.Size = new System.Drawing.Size(525, 149);
         this.panelDescriptorCriteria.TabIndex = 19;
         // 
         // gridControlMolecules
         // 
         this.gridControlMolecules.Location = new System.Drawing.Point(24, 90);
         this.gridControlMolecules.MainView = this.grdMoleculeBuilder;
         this.gridControlMolecules.Name = "gridControlMolecules";
         this.gridControlMolecules.Size = new System.Drawing.Size(525, 137);
         this.gridControlMolecules.TabIndex = 6;
         this.gridControlMolecules.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdMoleculeBuilder});
         // 
         // grdMoleculeBuilder
         // 
         this.grdMoleculeBuilder.AllowsFiltering = true;
         this.grdMoleculeBuilder.EnableColumnContextMenu = true;
         this.grdMoleculeBuilder.GridControl = this.gridControlMolecules;
         this.grdMoleculeBuilder.MultiSelect = false;
         this.grdMoleculeBuilder.Name = "grdMoleculeBuilder";
         this.grdMoleculeBuilder.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.grdMoleculeBuilder.OptionsNavigation.AutoFocusNewRow = true;
         this.grdMoleculeBuilder.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.grdMoleculeBuilder.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.grdMoleculeBuilder.OptionsView.ShowGroupPanel = false;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(108, 438);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(453, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 18;
         // 
         // cbApplicatedMoleculeName
         // 
         this.cbApplicatedMoleculeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cbApplicatedMoleculeName.Location = new System.Drawing.Point(108, 36);
         this.cbApplicatedMoleculeName.MenuManager = this.barManager;
         this.cbApplicatedMoleculeName.Name = "cbApplicatedMoleculeName";
         this.cbApplicatedMoleculeName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbApplicatedMoleculeName.Size = new System.Drawing.Size(453, 20);
         this.cbApplicatedMoleculeName.StyleController = this.layoutControl;
         this.cbApplicatedMoleculeName.TabIndex = 15;
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
         this.barDockControlTop.Size = new System.Drawing.Size(579, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 498);
         this.barDockControlBottom.Size = new System.Drawing.Size(579, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 498);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(579, 0);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 498);
         // 
         // btName
         // 
         this.btName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btName.Location = new System.Drawing.Point(108, 12);
         this.btName.MenuManager = this.barManager;
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(453, 20);
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
            this.layouItemName,
            this.layoutGroupApplicationBuilder,
            this.layoutGroupContainer,
            this.layoutItemMolecule});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(573, 470);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "Description";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 426);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(553, 24);
         this.layoutItemDescription.Text = "Description:";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(93, 13);
         // 
         // layouItemName
         // 
         this.layouItemName.Control = this.btName;
         this.layouItemName.CustomizationFormText = "Name";
         this.layouItemName.Location = new System.Drawing.Point(0, 0);
         this.layouItemName.Name = "layouItemName";
         this.layouItemName.Size = new System.Drawing.Size(553, 24);
         this.layouItemName.Text = "Name:";
         this.layouItemName.TextSize = new System.Drawing.Size(93, 13);
         // 
         // layoutGroupApplicationBuilder
         // 
         this.layoutGroupApplicationBuilder.CustomizationFormText = "layoutGroupApplicationBuilder";
         this.layoutGroupApplicationBuilder.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
         this.layoutGroupApplicationBuilder.Location = new System.Drawing.Point(0, 48);
         this.layoutGroupApplicationBuilder.Name = "layoutGroupApplicationBuilder";
         this.layoutGroupApplicationBuilder.Size = new System.Drawing.Size(553, 183);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.gridControlMolecules;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(529, 141);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutGroupContainer
         // 
         this.layoutGroupContainer.CustomizationFormText = "layoutGroupContainer";
         this.layoutGroupContainer.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
         this.layoutGroupContainer.Location = new System.Drawing.Point(0, 231);
         this.layoutGroupContainer.Name = "layoutGroupContainer";
         this.layoutGroupContainer.Size = new System.Drawing.Size(553, 195);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.panelDescriptorCriteria;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(529, 153);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // tabParameter
         // 
         this.tabParameter.Name = "tabParameter";
         this.tabParameter.Size = new System.Drawing.Size(573, 470);
         this.tabParameter.Text = "Parameters";
         // 
         // layoutItemMolecule
         // 
         this.layoutItemMolecule.Control = this.cbApplicatedMoleculeName;
         this.layoutItemMolecule.CustomizationFormText = "layoutItemMolecule";
         this.layoutItemMolecule.Location = new System.Drawing.Point(0, 24);
         this.layoutItemMolecule.Name = "layoutItemMolecule";
         this.layoutItemMolecule.Size = new System.Drawing.Size(553, 24);
         this.layoutItemMolecule.TextSize = new System.Drawing.Size(93, 13);
         // 
         // EditApplicationBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.xtraTabControl1);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "EditApplicationBuilderView";
         this.Size = new System.Drawing.Size(579, 498);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
         this.xtraTabControl1.ResumeLayout(false);
         this.tabMolecules.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelDescriptorCriteria)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlMolecules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdMoleculeBuilder)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbApplicatedMoleculeName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupApplicationBuilder)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupContainer)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMolecule)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
      private DevExpress.XtraTab.XtraTabPage tabParameter;
      private DevExpress.XtraTab.XtraTabPage tabMolecules;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarManager barManager;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private DevExpress.XtraEditors.ComboBoxEdit cbApplicatedMoleculeName;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraGrid.GridControl gridControlMolecules;
      private UxGridView grdMoleculeBuilder;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlItem layouItemName;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupApplicationBuilder;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupContainer;
      private DevExpress.XtraEditors.PanelControl panelDescriptorCriteria;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMolecule;
   }
}
