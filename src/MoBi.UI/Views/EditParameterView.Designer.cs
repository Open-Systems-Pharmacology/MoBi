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
         this.panelOrigiView = new DevExpress.XtraEditors.PanelControl();
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
         this.layoutItemValueOrigin = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabTags = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlProperties)).BeginInit();
         this.layoutControlProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelOrigiView)).BeginInit();
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
         this.tabProperties.Size = new System.Drawing.Size(756, 415);
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
         this.layoutControlProperties.Size = new System.Drawing.Size(756, 415);
         this.layoutControlProperties.TabIndex = 25;
         this.layoutControlProperties.Text = "layoutControlProperties";
         // 
         // panelOrigiView
         // 
         this.panelOrigiView.Location = new System.Drawing.Point(133, 141);
         this.panelOrigiView.Name = "panelOrigiView";
         this.panelOrigiView.Size = new System.Drawing.Size(599, 19);
         this.panelOrigiView.TabIndex = 29;
         // 
         // chkIsFavorite
         // 
         this.chkIsFavorite.Location = new System.Drawing.Point(24, 164);
         this.chkIsFavorite.Name = "chkIsFavorite";
         this.chkIsFavorite.Properties.Caption = "chkIsFavorite";
         this.chkIsFavorite.Size = new System.Drawing.Size(174, 20);
         this.chkIsFavorite.StyleController = this.layoutControlProperties;
         this.chkIsFavorite.TabIndex = 27;
         // 
         // cbGroup
         // 
         this.cbGroup.Location = new System.Drawing.Point(133, 117);
         this.cbGroup.Name = "cbGroup";
         this.cbGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbGroup.Size = new System.Drawing.Size(599, 20);
         this.cbGroup.StyleController = this.layoutControlProperties;
         this.cbGroup.TabIndex = 26;
         // 
         // chkCanBeVariedInPopulation
         // 
         this.chkCanBeVariedInPopulation.AllowClicksOutsideControlArea = false;
         this.chkCanBeVariedInPopulation.Location = new System.Drawing.Point(558, 164);
         this.chkCanBeVariedInPopulation.Name = "chkCanBeVariedInPopulation";
         this.chkCanBeVariedInPopulation.Properties.Caption = "chkCanBeVariedInPopulation";
         this.chkCanBeVariedInPopulation.Size = new System.Drawing.Size(174, 20);
         this.chkCanBeVariedInPopulation.StyleController = this.layoutControlProperties;
         this.chkCanBeVariedInPopulation.TabIndex = 25;
         // 
         // chkPersistable
         // 
         this.chkPersistable.AllowClicksOutsideControlArea = false;
         this.chkPersistable.Location = new System.Drawing.Point(202, 164);
         this.chkPersistable.Name = "chkPersistable";
         this.chkPersistable.Properties.Caption = "chkPersistable";
         this.chkPersistable.Size = new System.Drawing.Size(174, 20);
         this.chkPersistable.StyleController = this.layoutControlProperties;
         this.chkPersistable.TabIndex = 24;
         // 
         // panelRHSFormula
         // 
         this.panelRHSFormula.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelRHSFormula.Location = new System.Drawing.Point(24, 365);
         this.panelRHSFormula.Name = "panelRHSFormula";
         this.panelRHSFormula.Size = new System.Drawing.Size(708, 2);
         this.panelRHSFormula.TabIndex = 21;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.htmlEditor.Location = new System.Drawing.Point(121, 383);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(623, 20);
         this.htmlEditor.StyleController = this.layoutControlProperties;
         this.htmlEditor.TabIndex = 22;
         // 
         // panelFormula
         // 
         this.panelFormula.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.panelFormula.Location = new System.Drawing.Point(24, 233);
         this.panelFormula.Name = "panelFormula";
         this.panelFormula.Size = new System.Drawing.Size(708, 49);
         this.panelFormula.TabIndex = 20;
         // 
         // cbDimension
         // 
         this.cbDimension.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.cbDimension.Location = new System.Drawing.Point(133, 93);
         this.cbDimension.Name = "cbDimension";
         this.cbDimension.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbDimension.Size = new System.Drawing.Size(599, 20);
         this.cbDimension.StyleController = this.layoutControlProperties;
         this.cbDimension.TabIndex = 19;
         // 
         // chkAdvancedParameter
         // 
         this.chkAdvancedParameter.AllowClicksOutsideControlArea = false;
         this.chkAdvancedParameter.Location = new System.Drawing.Point(380, 164);
         this.chkAdvancedParameter.Name = "chkAdvancedParameter";
         this.chkAdvancedParameter.Properties.Caption = "chkAdvancedParameter";
         this.chkAdvancedParameter.Size = new System.Drawing.Size(174, 20);
         this.chkAdvancedParameter.StyleController = this.layoutControlProperties;
         this.chkAdvancedParameter.TabIndex = 23;
         // 
         // cbParameterBuildMode
         // 
         this.cbParameterBuildMode.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.cbParameterBuildMode.Location = new System.Drawing.Point(133, 69);
         this.cbParameterBuildMode.Name = "cbParameterBuildMode";
         this.cbParameterBuildMode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbParameterBuildMode.Size = new System.Drawing.Size(599, 20);
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
         this.btName.Size = new System.Drawing.Size(623, 20);
         this.btName.StyleController = this.layoutControlProperties;
         this.btName.TabIndex = 0;
         // 
         // chkHasRHS
         // 
         this.chkHasRHS.AllowClicksOutsideControlArea = false;
         this.chkHasRHS.Location = new System.Drawing.Point(12, 308);
         this.chkHasRHS.Name = "chkHasRHS";
         this.chkHasRHS.Properties.Caption = "Parameter is state variable";
         this.chkHasRHS.Size = new System.Drawing.Size(732, 20);
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
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(756, 415);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "Name";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(736, 24);
         this.layoutItemName.Text = "Name:";
         this.layoutItemName.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutControlItemHasRHS
         // 
         this.layoutControlItemHasRHS.Control = this.chkHasRHS;
         this.layoutControlItemHasRHS.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItemHasRHS.Location = new System.Drawing.Point(0, 296);
         this.layoutControlItemHasRHS.Name = "layoutControlItemHasRHS";
         this.layoutControlItemHasRHS.Size = new System.Drawing.Size(736, 24);
         this.layoutControlItemHasRHS.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemHasRHS.TextVisible = false;
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "Description";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 371);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(736, 24);
         this.layoutItemDescription.Text = "Description:";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutGroupValue
         // 
         this.layoutGroupValue.CustomizationFormText = "Value";
         this.layoutGroupValue.ExpandButtonVisible = true;
         this.layoutGroupValue.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFormula});
         this.layoutGroupValue.Location = new System.Drawing.Point(0, 188);
         this.layoutGroupValue.Name = "layoutGroupValue";
         this.layoutGroupValue.Size = new System.Drawing.Size(736, 98);
         this.layoutGroupValue.Text = "Value";
         // 
         // layoutItemFormula
         // 
         this.layoutItemFormula.Control = this.panelFormula;
         this.layoutItemFormula.CustomizationFormText = "layoutItemFormula";
         this.layoutItemFormula.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFormula.Name = "layoutItemFormula";
         this.layoutItemFormula.Size = new System.Drawing.Size(712, 53);
         this.layoutItemFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemFormula.TextVisible = false;
         // 
         // layoutGroupRHSFormula
         // 
         this.layoutGroupRHSFormula.CustomizationFormText = "Right hand side";
         this.layoutGroupRHSFormula.ExpandButtonVisible = true;
         this.layoutGroupRHSFormula.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemRHSFormula});
         this.layoutGroupRHSFormula.Location = new System.Drawing.Point(0, 320);
         this.layoutGroupRHSFormula.Name = "layoutGroupRHSFormula";
         this.layoutGroupRHSFormula.Size = new System.Drawing.Size(736, 51);
         this.layoutGroupRHSFormula.Text = "Right hand side";
         // 
         // layoutItemRHSFormula
         // 
         this.layoutItemRHSFormula.Control = this.panelRHSFormula;
         this.layoutItemRHSFormula.CustomizationFormText = "layoutItemRHSFormula";
         this.layoutItemRHSFormula.Location = new System.Drawing.Point(0, 0);
         this.layoutItemRHSFormula.Name = "layoutItemRHSFormula";
         this.layoutItemRHSFormula.Size = new System.Drawing.Size(712, 6);
         this.layoutItemRHSFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRHSFormula.TextVisible = false;
         // 
         // splitterRHSFormula
         // 
         this.splitterRHSFormula.AllowHotTrack = true;
         this.splitterRHSFormula.CustomizationFormText = "splitterItem1";
         this.splitterRHSFormula.Location = new System.Drawing.Point(0, 286);
         this.splitterRHSFormula.Name = "splitterItem1";
         this.splitterRHSFormula.Size = new System.Drawing.Size(736, 10);
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
         this.layoutGroupProperties.Size = new System.Drawing.Size(736, 164);
         // 
         // layoutItemParameterType
         // 
         this.layoutItemParameterType.Control = this.cbParameterBuildMode;
         this.layoutItemParameterType.CustomizationFormText = "Parameter Type";
         this.layoutItemParameterType.Location = new System.Drawing.Point(0, 0);
         this.layoutItemParameterType.Name = "layoutItemParameterType";
         this.layoutItemParameterType.Size = new System.Drawing.Size(712, 24);
         this.layoutItemParameterType.Text = "Parameter type:";
         this.layoutItemParameterType.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutItemDimension
         // 
         this.layoutItemDimension.Control = this.cbDimension;
         this.layoutItemDimension.CustomizationFormText = "Dimension";
         this.layoutItemDimension.Location = new System.Drawing.Point(0, 24);
         this.layoutItemDimension.Name = "layoutItemDimension";
         this.layoutItemDimension.Size = new System.Drawing.Size(712, 24);
         this.layoutItemDimension.Text = "Dimension:";
         this.layoutItemDimension.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutItemGroup
         // 
         this.layoutItemGroup.Control = this.cbGroup;
         this.layoutItemGroup.CustomizationFormText = "layoutItemGroup";
         this.layoutItemGroup.Location = new System.Drawing.Point(0, 48);
         this.layoutItemGroup.Name = "layoutItemGroup";
         this.layoutItemGroup.Size = new System.Drawing.Size(712, 24);
         this.layoutItemGroup.TextSize = new System.Drawing.Size(106, 13);
         // 
         // layoutItemIsFavorite
         // 
         this.layoutItemIsFavorite.Control = this.chkIsFavorite;
         this.layoutItemIsFavorite.CustomizationFormText = "layoutItemIsFavorite";
         this.layoutItemIsFavorite.Location = new System.Drawing.Point(0, 95);
         this.layoutItemIsFavorite.Name = "layoutItemIsFavorite";
         this.layoutItemIsFavorite.Size = new System.Drawing.Size(178, 24);
         this.layoutItemIsFavorite.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemIsFavorite.TextVisible = false;
         // 
         // layoutControlItemPersistable
         // 
         this.layoutControlItemPersistable.Control = this.chkPersistable;
         this.layoutControlItemPersistable.CustomizationFormText = "layoutControlItemPersistable";
         this.layoutControlItemPersistable.Location = new System.Drawing.Point(178, 95);
         this.layoutControlItemPersistable.Name = "layoutControlItemPersistable";
         this.layoutControlItemPersistable.Size = new System.Drawing.Size(178, 24);
         this.layoutControlItemPersistable.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemPersistable.TextVisible = false;
         // 
         // layoutControlItemAdvancedParameter
         // 
         this.layoutControlItemAdvancedParameter.Control = this.chkAdvancedParameter;
         this.layoutControlItemAdvancedParameter.CustomizationFormText = "layoutControlItemAdvancedParameter";
         this.layoutControlItemAdvancedParameter.Location = new System.Drawing.Point(356, 95);
         this.layoutControlItemAdvancedParameter.Name = "layoutControlItemAdvancedParameter";
         this.layoutControlItemAdvancedParameter.Size = new System.Drawing.Size(178, 24);
         this.layoutControlItemAdvancedParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAdvancedParameter.TextVisible = false;
         // 
         // layoutControlItemCanBeVariedInPopulation
         // 
         this.layoutControlItemCanBeVariedInPopulation.Control = this.chkCanBeVariedInPopulation;
         this.layoutControlItemCanBeVariedInPopulation.CustomizationFormText = "layoutControlItemCanBeVariedInPopulation";
         this.layoutControlItemCanBeVariedInPopulation.Location = new System.Drawing.Point(534, 95);
         this.layoutControlItemCanBeVariedInPopulation.Name = "layoutControlItemCanBeVariedInPopulation";
         this.layoutControlItemCanBeVariedInPopulation.Size = new System.Drawing.Size(178, 24);
         this.layoutControlItemCanBeVariedInPopulation.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCanBeVariedInPopulation.TextVisible = false;
         // 
         // layoutItemValueOrigin
         // 
         this.layoutItemValueOrigin.Control = this.panelOrigiView;
         this.layoutItemValueOrigin.Location = new System.Drawing.Point(0, 72);
         this.layoutItemValueOrigin.Name = "layoutItemValueOrigin";
         this.layoutItemValueOrigin.Size = new System.Drawing.Size(712, 23);
         this.layoutItemValueOrigin.TextSize = new System.Drawing.Size(106, 13);
         // 
         // tabTags
         // 
         this.tabTags.Name = "tabTags";
         this.tabTags.Size = new System.Drawing.Size(756, 415);
         this.tabTags.Text = "tabTags";
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
         ((System.ComponentModel.ISupportInitialize)(this.panelOrigiView)).EndInit();
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
   }
}
