using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class EditParameterView  
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
         this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControlProperties = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkIsFavorite = new DevExpress.XtraEditors.CheckEdit();
         this.cbGroup = new DevExpress.XtraEditors.ComboBoxEdit();
         this.chkCanBeVariedInPopulation = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkPersistable = new OSPSuite.UI.Controls.UxCheckEdit();
         this.panelRHSFormula = new DevExpress.XtraEditors.PanelControl();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.panelFormula = new DevExpress.XtraEditors.PanelControl();
         this.cbDimension = new DevExpress.XtraEditors.ComboBoxEdit();
         this.chkAdvancedParameter = new OSPSuite.UI.Controls.UxCheckEdit();
         this.cbParameterBuildMode = new DevExpress.XtraEditors.ComboBoxEdit();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.chkHasRHS = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemHasRHS = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupValue = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupRHSFormula = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemRHSFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterRHSFormula = new DevExpress.XtraLayout.SplitterItem();
         this.layoutGroupProperties = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemParameterType = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDimension = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemGroup = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemIsFavorite = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemPersistable = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemAdvancedParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCanBeVariedInPopulation = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabTags = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControlTags = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btAddTag = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl1 = new OSPSuite.UI.Controls.UxGridControl();
         this.gridViewTags = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemAddTag = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemTags = new DevExpress.XtraLayout.LayoutControlItem();
         this.panelOrigiView = new DevExpress.XtraEditors.PanelControl();
         this.layoutItemValueOrigin = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlProperties)).BeginInit();
         this.layoutControlProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkIsFavorite.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbGroup.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkCanBeVariedInPopulation.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkPersistable.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelRHSFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkAdvancedParameter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbParameterBuildMode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkHasRHS.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemHasRHS)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupValue)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRHSFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRHSFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterRHSFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameterType)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsFavorite)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPersistable)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAdvancedParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCanBeVariedInPopulation)).BeginInit();
         this.tabTags.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlTags)).BeginInit();
         this.layoutControlTags.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewTags)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddTag)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTags)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOrigiView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOrigin)).BeginInit();
         this.SuspendLayout();
         // 
         // xtraTabControl1
         // 
         this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
         this.xtraTabControl1.Name = "xtraTabControl1";
         this.xtraTabControl1.SelectedTabPage = this.tabProperties;
         this.xtraTabControl1.Size = new System.Drawing.Size(758, 440);
         this.xtraTabControl1.TabIndex = 0;
         this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabTags});
         // 
         // tabProperties
         // 
         this.tabProperties.Controls.Add(this.layoutControlProperties);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(752, 412);
         this.tabProperties.Text = "tabProperties";
         // 
         // layoutControlProperties
         // 
         this.layoutControlProperties.AllowCustomization = false;
         this.layoutControlProperties.Controls.Add(this.panelOrigiView);
         this.layoutControlProperties.Controls.Add(this.chkIsFavorite);
         this.layoutControlProperties.Controls.Add(this.cbGroup);
         this.layoutControlProperties.Controls.Add(this.chkCanBeVariedInPopulation);
         this.layoutControlProperties.Controls.Add(this.chkPersistable);
         this.layoutControlProperties.Controls.Add(this.panelRHSFormula);
         this.layoutControlProperties.Controls.Add(this.htmlEditor);
         this.layoutControlProperties.Controls.Add(this.panelFormula);
         this.layoutControlProperties.Controls.Add(this.cbDimension);
         this.layoutControlProperties.Controls.Add(this.chkAdvancedParameter);
         this.layoutControlProperties.Controls.Add(this.cbParameterBuildMode);
         this.layoutControlProperties.Controls.Add(this.btName);
         this.layoutControlProperties.Controls.Add(this.chkHasRHS);
         this.layoutControlProperties.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControlProperties.Location = new System.Drawing.Point(0, 0);
         this.layoutControlProperties.Name = "layoutControlProperties";
         this.layoutControlProperties.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(971, 370, 573, 607);
         this.layoutControlProperties.Root = this.layoutControlGroup1;
         this.layoutControlProperties.Size = new System.Drawing.Size(752, 412);
         this.layoutControlProperties.TabIndex = 25;
         this.layoutControlProperties.Text = "layoutControlProperties";
         // 
         // chkIsFavorite
         // 
         this.chkIsFavorite.Location = new System.Drawing.Point(24, 164);
         this.chkIsFavorite.Name = "chkIsFavorite";
         this.chkIsFavorite.Properties.Caption = "chkIsFavorite";
         this.chkIsFavorite.Size = new System.Drawing.Size(173, 19);
         this.chkIsFavorite.StyleController = this.layoutControlProperties;
         this.chkIsFavorite.TabIndex = 27;
         // 
         // cbGroup
         // 
         this.cbGroup.Location = new System.Drawing.Point(133, 114);
         this.cbGroup.Name = "cbGroup";
         this.cbGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbGroup.Size = new System.Drawing.Size(595, 20);
         this.cbGroup.StyleController = this.layoutControlProperties;
         this.cbGroup.TabIndex = 26;
         // 
         // chkCanBeVariedInPopulation
         // 
         this.chkCanBeVariedInPopulation.AllowClicksOutsideControlArea = false;
         this.chkCanBeVariedInPopulation.Location = new System.Drawing.Point(555, 164);
         this.chkCanBeVariedInPopulation.Name = "chkCanBeVariedInPopulation";
         this.chkCanBeVariedInPopulation.Properties.Caption = "chkCanBeVariedInPopulation";
         this.chkCanBeVariedInPopulation.Size = new System.Drawing.Size(173, 19);
         this.chkCanBeVariedInPopulation.StyleController = this.layoutControlProperties;
         this.chkCanBeVariedInPopulation.TabIndex = 25;
         // 
         // chkPersistable
         // 
         this.chkPersistable.AllowClicksOutsideControlArea = false;
         this.chkPersistable.Location = new System.Drawing.Point(201, 164);
         this.chkPersistable.Name = "chkPersistable";
         this.chkPersistable.Properties.Caption = "chkPersistable";
         this.chkPersistable.Size = new System.Drawing.Size(173, 19);
         this.chkPersistable.StyleController = this.layoutControlProperties;
         this.chkPersistable.TabIndex = 24;
         // 
         // panelRHSFormula
         // 
         this.panelRHSFormula.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelRHSFormula.Location = new System.Drawing.Point(24, 354);
         this.panelRHSFormula.Name = "panelRHSFormula";
         this.panelRHSFormula.Size = new System.Drawing.Size(704, 10);
         this.panelRHSFormula.TabIndex = 21;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.htmlEditor.Location = new System.Drawing.Point(121, 380);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(619, 20);
         this.htmlEditor.StyleController = this.layoutControlProperties;
         this.htmlEditor.TabIndex = 22;
         // 
         // panelFormula
         // 
         this.panelFormula.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelFormula.Location = new System.Drawing.Point(24, 229);
         this.panelFormula.Name = "panelFormula";
         this.panelFormula.Size = new System.Drawing.Size(704, 51);
         this.panelFormula.TabIndex = 20;
         // 
         // cbDimension
         // 
         this.cbDimension.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.cbDimension.Location = new System.Drawing.Point(133, 90);
         this.cbDimension.Name = "cbDimension";
         this.cbDimension.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbDimension.Size = new System.Drawing.Size(595, 20);
         this.cbDimension.StyleController = this.layoutControlProperties;
         this.cbDimension.TabIndex = 19;
         // 
         // chkAdvancedParameter
         // 
         this.chkAdvancedParameter.AllowClicksOutsideControlArea = false;
         this.chkAdvancedParameter.Location = new System.Drawing.Point(378, 164);
         this.chkAdvancedParameter.Name = "chkAdvancedParameter";
         this.chkAdvancedParameter.Properties.Caption = "chkAdvancedParameter";
         this.chkAdvancedParameter.Size = new System.Drawing.Size(173, 19);
         this.chkAdvancedParameter.StyleController = this.layoutControlProperties;
         this.chkAdvancedParameter.TabIndex = 23;
         // 
         // cbParameterBuildMode
         // 
         this.cbParameterBuildMode.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.cbParameterBuildMode.Location = new System.Drawing.Point(133, 66);
         this.cbParameterBuildMode.Name = "cbParameterBuildMode";
         this.cbParameterBuildMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbParameterBuildMode.Size = new System.Drawing.Size(595, 20);
         this.cbParameterBuildMode.StyleController = this.layoutControlProperties;
         this.cbParameterBuildMode.TabIndex = 17;
         // 
         // btName
         // 
         this.btName.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.btName.Location = new System.Drawing.Point(121, 12);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(619, 20);
         this.btName.StyleController = this.layoutControlProperties;
         this.btName.TabIndex = 0;
         // 
         // chkHasRHS
         // 
         this.chkHasRHS.AllowClicksOutsideControlArea = false;
         this.chkHasRHS.Location = new System.Drawing.Point(12, 301);
         this.chkHasRHS.Name = "chkHasRHS";
         this.chkHasRHS.Properties.Caption = "Parameter is state variable";
         this.chkHasRHS.Size = new System.Drawing.Size(728, 19);
         this.chkHasRHS.StyleController = this.layoutControlProperties;
         this.chkHasRHS.TabIndex = 14;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "Root";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutControlItemHasRHS,
            this.layoutItemDescription,
            this.layoutGroupValue,
            this.layoutGroupRHSFormula,
            this.splitterRHSFormula,
            this.layoutGroupProperties});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(752, 412);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "Name";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(732, 24);
         this.layoutItemName.Text = "Name:";
         this.layoutItemName.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutControlItemHasRHS
         // 
         this.layoutControlItemHasRHS.Control = this.chkHasRHS;
         this.layoutControlItemHasRHS.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItemHasRHS.Location = new System.Drawing.Point(0, 289);
         this.layoutControlItemHasRHS.Name = "layoutControlItemHasRHS";
         this.layoutControlItemHasRHS.Size = new System.Drawing.Size(732, 23);
         this.layoutControlItemHasRHS.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemHasRHS.TextVisible = false;
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "Description";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 368);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(732, 24);
         this.layoutItemDescription.Text = "Description:";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutGroupValue
         // 
         this.layoutGroupValue.CustomizationFormText = "Value";
         this.layoutGroupValue.ExpandButtonVisible = true;
         this.layoutGroupValue.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFormula});
         this.layoutGroupValue.Location = new System.Drawing.Point(0, 187);
         this.layoutGroupValue.Name = "layoutGroupValue";
         this.layoutGroupValue.Size = new System.Drawing.Size(732, 97);
         this.layoutGroupValue.Text = "Value";
         // 
         // layoutItemFormula
         // 
         this.layoutItemFormula.Control = this.panelFormula;
         this.layoutItemFormula.CustomizationFormText = "layoutItemFormula";
         this.layoutItemFormula.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFormula.Name = "layoutItemFormula";
         this.layoutItemFormula.Size = new System.Drawing.Size(708, 55);
         this.layoutItemFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemFormula.TextVisible = false;
         // 
         // layoutGroupRHSFormula
         // 
         this.layoutGroupRHSFormula.CustomizationFormText = "Right hand side";
         this.layoutGroupRHSFormula.ExpandButtonVisible = true;
         this.layoutGroupRHSFormula.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemRHSFormula});
         this.layoutGroupRHSFormula.Location = new System.Drawing.Point(0, 312);
         this.layoutGroupRHSFormula.Name = "layoutGroupRHSFormula";
         this.layoutGroupRHSFormula.Size = new System.Drawing.Size(732, 56);
         this.layoutGroupRHSFormula.Text = "Right hand side";
         // 
         // layoutItemRHSFormula
         // 
         this.layoutItemRHSFormula.Control = this.panelRHSFormula;
         this.layoutItemRHSFormula.CustomizationFormText = "layoutItemRHSFormula";
         this.layoutItemRHSFormula.Location = new System.Drawing.Point(0, 0);
         this.layoutItemRHSFormula.Name = "layoutItemRHSFormula";
         this.layoutItemRHSFormula.Size = new System.Drawing.Size(708, 14);
         this.layoutItemRHSFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRHSFormula.TextVisible = false;
         // 
         // splitterRHSFormula
         // 
         this.splitterRHSFormula.AllowHotTrack = true;
         this.splitterRHSFormula.CustomizationFormText = "splitterItem1";
         this.splitterRHSFormula.Location = new System.Drawing.Point(0, 284);
         this.splitterRHSFormula.Name = "splitterItem1";
         this.splitterRHSFormula.Size = new System.Drawing.Size(732, 5);
         // 
         // layoutGroupProperties
         // 
         this.layoutGroupProperties.ExpandButtonVisible = true;
         this.layoutGroupProperties.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemParameterType,
            this.layoutItemDimension,
            this.layoutItemGroup,
            this.layoutItemIsFavorite,
            this.layoutControlItemPersistable,
            this.layoutControlItemAdvancedParameter,
            this.layoutControlItemCanBeVariedInPopulation,
            this.layoutItemValueOrigin});
         this.layoutGroupProperties.Location = new System.Drawing.Point(0, 24);
         this.layoutGroupProperties.Name = "layoutGroupProperties";
         this.layoutGroupProperties.Size = new System.Drawing.Size(732, 163);
         // 
         // layoutItemParameterType
         // 
         this.layoutItemParameterType.Control = this.cbParameterBuildMode;
         this.layoutItemParameterType.CustomizationFormText = "Parameter Type";
         this.layoutItemParameterType.Location = new System.Drawing.Point(0, 0);
         this.layoutItemParameterType.Name = "layoutItemParameterType";
         this.layoutItemParameterType.Size = new System.Drawing.Size(708, 24);
         this.layoutItemParameterType.Text = "Parameter type:";
         this.layoutItemParameterType.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutItemDimension
         // 
         this.layoutItemDimension.Control = this.cbDimension;
         this.layoutItemDimension.CustomizationFormText = "Dimension";
         this.layoutItemDimension.Location = new System.Drawing.Point(0, 24);
         this.layoutItemDimension.Name = "layoutItemDimension";
         this.layoutItemDimension.Size = new System.Drawing.Size(708, 24);
         this.layoutItemDimension.Text = "Dimension:";
         this.layoutItemDimension.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutItemGroup
         // 
         this.layoutItemGroup.Control = this.cbGroup;
         this.layoutItemGroup.CustomizationFormText = "layoutItemGroup";
         this.layoutItemGroup.Location = new System.Drawing.Point(0, 48);
         this.layoutItemGroup.Name = "layoutItemGroup";
         this.layoutItemGroup.Size = new System.Drawing.Size(708, 24);
         this.layoutItemGroup.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutItemIsFavorite
         // 
         this.layoutItemIsFavorite.Control = this.chkIsFavorite;
         this.layoutItemIsFavorite.CustomizationFormText = "layoutItemIsFavorite";
         this.layoutItemIsFavorite.Location = new System.Drawing.Point(0, 98);
         this.layoutItemIsFavorite.Name = "layoutItemIsFavorite";
         this.layoutItemIsFavorite.Size = new System.Drawing.Size(177, 23);
         this.layoutItemIsFavorite.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIsFavorite.TextVisible = false;
         // 
         // layoutControlItemPersistable
         // 
         this.layoutControlItemPersistable.Control = this.chkPersistable;
         this.layoutControlItemPersistable.CustomizationFormText = "layoutControlItemPersistable";
         this.layoutControlItemPersistable.Location = new System.Drawing.Point(177, 98);
         this.layoutControlItemPersistable.Name = "layoutControlItemPersistable";
         this.layoutControlItemPersistable.Size = new System.Drawing.Size(177, 23);
         this.layoutControlItemPersistable.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemPersistable.TextVisible = false;
         // 
         // layoutControlItemAdvancedParameter
         // 
         this.layoutControlItemAdvancedParameter.Control = this.chkAdvancedParameter;
         this.layoutControlItemAdvancedParameter.CustomizationFormText = "layoutControlItemAdvancedParameter";
         this.layoutControlItemAdvancedParameter.Location = new System.Drawing.Point(354, 98);
         this.layoutControlItemAdvancedParameter.Name = "layoutControlItemAdvancedParameter";
         this.layoutControlItemAdvancedParameter.Size = new System.Drawing.Size(177, 23);
         this.layoutControlItemAdvancedParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAdvancedParameter.TextVisible = false;
         // 
         // layoutControlItemCanBeVariedInPopulation
         // 
         this.layoutControlItemCanBeVariedInPopulation.Control = this.chkCanBeVariedInPopulation;
         this.layoutControlItemCanBeVariedInPopulation.CustomizationFormText = "layoutControlItemCanBeVariedInPopulation";
         this.layoutControlItemCanBeVariedInPopulation.Location = new System.Drawing.Point(531, 98);
         this.layoutControlItemCanBeVariedInPopulation.Name = "layoutControlItemCanBeVariedInPopulation";
         this.layoutControlItemCanBeVariedInPopulation.Size = new System.Drawing.Size(177, 23);
         this.layoutControlItemCanBeVariedInPopulation.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCanBeVariedInPopulation.TextVisible = false;
         // 
         // tabTags
         // 
         this.tabTags.Controls.Add(this.layoutControlTags);
         this.tabTags.Name = "tabTags";
         this.tabTags.Size = new System.Drawing.Size(752, 412);
         this.tabTags.Text = "tabTags";
         // 
         // layoutControlTags
         // 
         this.layoutControlTags.AllowCustomization = false;
         this.layoutControlTags.Controls.Add(this.btAddTag);
         this.layoutControlTags.Controls.Add(this.gridControl1);
         this.layoutControlTags.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControlTags.Location = new System.Drawing.Point(0, 0);
         this.layoutControlTags.Name = "layoutControlTags";
         this.layoutControlTags.Root = this.layoutControlGroup2;
         this.layoutControlTags.Size = new System.Drawing.Size(752, 412);
         this.layoutControlTags.TabIndex = 0;
         this.layoutControlTags.Text = "layoutControlTags";
         // 
         // btAddTag
         // 
         this.btAddTag.Location = new System.Drawing.Point(12, 12);
         this.btAddTag.Name = "btAddTag";
         this.btAddTag.Size = new System.Drawing.Size(728, 22);
         this.btAddTag.StyleController = this.layoutControlTags;
         this.btAddTag.TabIndex = 4;
         this.btAddTag.Text = "simpleButton1";
         // 
         // gridControl1
         // 
         this.gridControl1.Location = new System.Drawing.Point(125, 38);
         this.gridControl1.MainView = this.gridViewTags;
         this.gridControl1.Name = "gridControl1";
         this.gridControl1.Size = new System.Drawing.Size(615, 362);
         this.gridControl1.TabIndex = 5;
         this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTags});
         // 
         // gridViewTags
         // 
         this.gridViewTags.GridControl = this.gridControl1;
         this.gridViewTags.Name = "gridViewTags";
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
         this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup2.GroupBordersVisible = false;
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemAddTag,
            this.layoutControlItemTags});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(752, 412);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // layoutControlItemAddTag
         // 
         this.layoutControlItemAddTag.Control = this.btAddTag;
         this.layoutControlItemAddTag.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItemAddTag.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemAddTag.Name = "layoutControlItemAddTag";
         this.layoutControlItemAddTag.Size = new System.Drawing.Size(732, 26);
         this.layoutControlItemAddTag.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddTag.TextVisible = false;
         // 
         // layoutControlItemTags
         // 
         this.layoutControlItemTags.Control = this.gridControl1;
         this.layoutControlItemTags.CustomizationFormText = "layoutControlItemTags";
         this.layoutControlItemTags.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemTags.Name = "layoutControlItem3";
         this.layoutControlItemTags.Size = new System.Drawing.Size(732, 366);
         this.layoutControlItemTags.Text = "layoutControlItemTags";
         this.layoutControlItemTags.TextSize = new System.Drawing.Size(110, 13);
         // 
         // panelControl1
         // 
         this.panelOrigiView.Location = new System.Drawing.Point(133, 138);
         this.panelOrigiView.Name = "panelOrigiView";
         this.panelOrigiView.Size = new System.Drawing.Size(595, 22);
         this.panelOrigiView.TabIndex = 29;
         // 
         // layoutItemValueOrigin
         // 
         this.layoutItemValueOrigin.Control = this.panelOrigiView;
         this.layoutItemValueOrigin.Location = new System.Drawing.Point(0, 72);
         this.layoutItemValueOrigin.Name = "layoutItemValueOrigin";
         this.layoutItemValueOrigin.Size = new System.Drawing.Size(708, 26);
         this.layoutItemValueOrigin.TextSize = new System.Drawing.Size(106, 13);
         // 
         // EditParameterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.xtraTabControl1);
         this.Name = "EditParameterView";
         this.Size = new System.Drawing.Size(758, 440);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
         this.xtraTabControl1.ResumeLayout(false);
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlProperties)).EndInit();
         this.layoutControlProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkIsFavorite.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbGroup.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkCanBeVariedInPopulation.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkPersistable.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelRHSFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkAdvancedParameter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbParameterBuildMode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkHasRHS.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemHasRHS)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupValue)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupRHSFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRHSFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterRHSFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameterType)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemIsFavorite)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPersistable)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAdvancedParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCanBeVariedInPopulation)).EndInit();
         this.tabTags.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlTags)).EndInit();
         this.layoutControlTags.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewTags)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddTag)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTags)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOrigiView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOrigin)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
      private DevExpress.XtraTab.XtraTabPage tabProperties;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControlProperties;
      private DevExpress.XtraEditors.PanelControl panelRHSFormula;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraEditors.PanelControl panelFormula;
      private DevExpress.XtraEditors.ComboBoxEdit cbDimension;
      private DevExpress.XtraEditors.ComboBoxEdit cbParameterBuildMode;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDimension;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameterType;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemHasRHS;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupValue;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormula;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupRHSFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRHSFormula;
      private DevExpress.XtraLayout.SplitterItem splitterRHSFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAdvancedParameter;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemPersistable;
      private DevExpress.XtraTab.XtraTabPage tabTags;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControlTags;
      private DevExpress.XtraEditors.SimpleButton btAddTag;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddTag;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewTags;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTags;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCanBeVariedInPopulation;
      private DevExpress.XtraEditors.ComboBoxEdit cbGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemGroup;
      private DevExpress.XtraEditors.CheckEdit chkIsFavorite;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemIsFavorite;
      private UxCheckEdit chkPersistable;
      private UxCheckEdit chkAdvancedParameter;
      private UxCheckEdit chkHasRHS;
      private UxCheckEdit chkCanBeVariedInPopulation;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupProperties;
      private DevExpress.XtraEditors.PanelControl panelOrigiView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValueOrigin;
      private UxGridControl gridControl1;
   }
}
