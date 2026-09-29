using OSPSuite.UI.Controls;

namespace MoBi.UI.Views.BaseDiagram
{
   partial class DiagramOptionsView
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
          _colorBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.chkMoleculePropertiesVisible = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.coeReactionPortModifier = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeReactionLinkModifier = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeReactionPortEduct = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeReactionNode = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeReactionLinkProduct = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeReactionPortProduct = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.chkSnapGridVisible = new OSPSuite.UI.Controls.UxCheckEdit();
         this.coeReactionLinkEduct = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.cbeDefaultNodeSizeReaction = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.cbeDefaultNodeSizeMolecule = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.coeContainerLogical = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeMoleculeNode = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeContainerPhysical = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.txtContainerOpacity = new DevExpress.XtraEditors.TextEdit();
         this.coeNeighborhoodLink = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeNeighborhoodNode = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.coeNeighborhoodPort = new OSPSuite.UI.Controls.UxColorPickEditWithHistory();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.defaultSizeOfNewMoleculeControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.defaultSizeOfNewReactionControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.showSnapGridControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.colorsLayoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.containerLogicalControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.moleculeNodeControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.containerPhysicalControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.containerOpacityControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.neighborhoodLinkControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.neighborhoodNodeControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.neighborhoodPortControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.reactionNodeControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.reactionPortEductControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.reactionLinkEductControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.reactionPortProductControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.reactionLinkProductControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.reactionPortModifierControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.reactionLinkModifierControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.timer1 = new System.Windows.Forms.Timer(this.components);
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkMoleculePropertiesVisible.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionPortModifier.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionLinkModifier.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionPortEduct.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionNode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionLinkProduct.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionPortProduct.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkSnapGridVisible.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionLinkEduct.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbeDefaultNodeSizeReaction.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbeDefaultNodeSizeMolecule.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeContainerLogical.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeMoleculeNode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeContainerPhysical.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtContainerOpacity.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeNeighborhoodLink.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeNeighborhoodNode.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeNeighborhoodPort.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultSizeOfNewMoleculeControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultSizeOfNewReactionControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.showSnapGridControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorsLayoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.containerLogicalControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.moleculeNodeControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.containerPhysicalControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.containerOpacityControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.neighborhoodLinkControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.neighborhoodNodeControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.neighborhoodPortControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionNodeControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionPortEductControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionLinkEductControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionPortProductControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionLinkProductControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionPortModifierControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionLinkModifierControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         this.SuspendLayout();
         // 
         // chkMoleculePropertiesVisible
         // 
         this.chkMoleculePropertiesVisible.AllowClicksOutsideControlArea = false;
         this.chkMoleculePropertiesVisible.EditValue = true;
         this.chkMoleculePropertiesVisible.Location = new System.Drawing.Point(14, 37);
         this.chkMoleculePropertiesVisible.Name = "chkMoleculePropertiesVisible";
         this.chkMoleculePropertiesVisible.Properties.Caption = "Show Molecule Properties";
         this.chkMoleculePropertiesVisible.Size = new System.Drawing.Size(248, 19);
         this.chkMoleculePropertiesVisible.StyleController = this.layoutControl;
         this.chkMoleculePropertiesVisible.TabIndex = 100;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.coeReactionPortModifier);
         this.layoutControl.Controls.Add(this.coeReactionLinkModifier);
         this.layoutControl.Controls.Add(this.coeReactionPortEduct);
         this.layoutControl.Controls.Add(this.coeReactionNode);
         this.layoutControl.Controls.Add(this.coeReactionLinkProduct);
         this.layoutControl.Controls.Add(this.coeReactionPortProduct);
         this.layoutControl.Controls.Add(this.chkSnapGridVisible);
         this.layoutControl.Controls.Add(this.chkMoleculePropertiesVisible);
         this.layoutControl.Controls.Add(this.coeReactionLinkEduct);
         this.layoutControl.Controls.Add(this.cbeDefaultNodeSizeReaction);
         this.layoutControl.Controls.Add(this.cbeDefaultNodeSizeMolecule);
         this.layoutControl.Controls.Add(this.coeContainerLogical);
         this.layoutControl.Controls.Add(this.coeMoleculeNode);
         this.layoutControl.Controls.Add(this.coeContainerPhysical);
         this.layoutControl.Controls.Add(this.txtContainerOpacity);
         this.layoutControl.Controls.Add(this.coeNeighborhoodLink);
         this.layoutControl.Controls.Add(this.coeNeighborhoodNode);
         this.layoutControl.Controls.Add(this.coeNeighborhoodPort);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(587, 404);
         this.layoutControl.TabIndex = 114;
         this.layoutControl.Text = "layoutControl1";
         // 
         // coeReactionPortModifier
         // 
         this.coeReactionPortModifier.EditValue = System.Drawing.Color.Empty;
         this.coeReactionPortModifier.Location = new System.Drawing.Point(490, 292);
         this.coeReactionPortModifier.Name = "coeReactionPortModifier";
         this.coeReactionPortModifier.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeReactionPortModifier.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeReactionPortModifier.Size = new System.Drawing.Size(83, 20);
         this.coeReactionPortModifier.StyleController = this.layoutControl;
         this.coeReactionPortModifier.TabIndex = 214;
         // 
         // coeReactionLinkModifier
         // 
         this.coeReactionLinkModifier.EditValue = System.Drawing.Color.Empty;
         this.coeReactionLinkModifier.Location = new System.Drawing.Point(490, 316);
         this.coeReactionLinkModifier.Name = "coeReactionLinkModifier";
         this.coeReactionLinkModifier.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeReactionLinkModifier.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeReactionLinkModifier.Size = new System.Drawing.Size(83, 20);
         this.coeReactionLinkModifier.StyleController = this.layoutControl;
         this.coeReactionLinkModifier.TabIndex = 202;
         // 
         // coeReactionPortEduct
         // 
         this.coeReactionPortEduct.EditValue = System.Drawing.Color.Empty;
         this.coeReactionPortEduct.Location = new System.Drawing.Point(490, 196);
         this.coeReactionPortEduct.Name = "coeReactionPortEduct";
         this.coeReactionPortEduct.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeReactionPortEduct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeReactionPortEduct.Size = new System.Drawing.Size(83, 20);
         this.coeReactionPortEduct.StyleController = this.layoutControl;
         this.coeReactionPortEduct.TabIndex = 198;
         // 
         // coeReactionNode
         // 
         this.coeReactionNode.EditValue = System.Drawing.Color.Empty;
         this.coeReactionNode.Location = new System.Drawing.Point(490, 172);
         this.coeReactionNode.Name = "coeReactionNode";
         this.coeReactionNode.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeReactionNode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeReactionNode.Size = new System.Drawing.Size(83, 20);
         this.coeReactionNode.StyleController = this.layoutControl;
         this.coeReactionNode.TabIndex = 196;
         // 
         // coeReactionLinkProduct
         // 
         this.coeReactionLinkProduct.EditValue = System.Drawing.Color.Empty;
         this.coeReactionLinkProduct.Location = new System.Drawing.Point(490, 268);
         this.coeReactionLinkProduct.Name = "coeReactionLinkProduct";
         this.coeReactionLinkProduct.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeReactionLinkProduct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeReactionLinkProduct.Size = new System.Drawing.Size(83, 20);
         this.coeReactionLinkProduct.StyleController = this.layoutControl;
         this.coeReactionLinkProduct.TabIndex = 184;
         // 
         // coeReactionPortProduct
         // 
         this.coeReactionPortProduct.EditValue = System.Drawing.Color.Empty;
         this.coeReactionPortProduct.Location = new System.Drawing.Point(490, 244);
         this.coeReactionPortProduct.Name = "coeReactionPortProduct";
         this.coeReactionPortProduct.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeReactionPortProduct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeReactionPortProduct.Size = new System.Drawing.Size(83, 20);
         this.coeReactionPortProduct.StyleController = this.layoutControl;
         this.coeReactionPortProduct.TabIndex = 186;
         // 
         // chkSnapGridVisible
         // 
         this.chkSnapGridVisible.AllowClicksOutsideControlArea = false;
         this.chkSnapGridVisible.EditValue = true;
         this.chkSnapGridVisible.Location = new System.Drawing.Point(14, 14);
         this.chkSnapGridVisible.Name = "chkSnapGridVisible";
         this.chkSnapGridVisible.Properties.Caption = "Show Snap Grid";
         this.chkSnapGridVisible.Size = new System.Drawing.Size(248, 19);
         this.chkSnapGridVisible.StyleController = this.layoutControl;
         this.chkSnapGridVisible.TabIndex = 101;
         // 
         // coeReactionLinkEduct
         // 
         this.coeReactionLinkEduct.EditValue = System.Drawing.Color.Empty;
         this.coeReactionLinkEduct.Location = new System.Drawing.Point(490, 220);
         this.coeReactionLinkEduct.Name = "coeReactionLinkEduct";
         this.coeReactionLinkEduct.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeReactionLinkEduct.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeReactionLinkEduct.Size = new System.Drawing.Size(83, 20);
         this.coeReactionLinkEduct.StyleController = this.layoutControl;
         this.coeReactionLinkEduct.TabIndex = 178;
         // 
         // cbeDefaultNodeSizeReaction
         // 
         this.cbeDefaultNodeSizeReaction.Location = new System.Drawing.Point(457, 14);
         this.cbeDefaultNodeSizeReaction.Name = "cbeDefaultNodeSizeReaction";
         this.cbeDefaultNodeSizeReaction.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbeDefaultNodeSizeReaction.Size = new System.Drawing.Size(116, 20);
         this.cbeDefaultNodeSizeReaction.StyleController = this.layoutControl;
         this.cbeDefaultNodeSizeReaction.TabIndex = 103;
         // 
         // cbeDefaultNodeSizeMolecule
         // 
         this.cbeDefaultNodeSizeMolecule.Location = new System.Drawing.Point(457, 38);
         this.cbeDefaultNodeSizeMolecule.Name = "cbeDefaultNodeSizeMolecule";
         this.cbeDefaultNodeSizeMolecule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbeDefaultNodeSizeMolecule.Size = new System.Drawing.Size(116, 20);
         this.cbeDefaultNodeSizeMolecule.StyleController = this.layoutControl;
         this.cbeDefaultNodeSizeMolecule.TabIndex = 104;
         // 
         // coeContainerLogical
         // 
         this.coeContainerLogical.EditValue = System.Drawing.Color.Empty;
         this.coeContainerLogical.Location = new System.Drawing.Point(205, 148);
         this.coeContainerLogical.Name = "coeContainerLogical";
         this.coeContainerLogical.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeContainerLogical.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeContainerLogical.Size = new System.Drawing.Size(90, 20);
         this.coeContainerLogical.StyleController = this.layoutControl;
         this.coeContainerLogical.TabIndex = 166;
         // 
         // coeMoleculeNode
         // 
         this.coeMoleculeNode.EditValue = System.Drawing.Color.Empty;
         this.coeMoleculeNode.Location = new System.Drawing.Point(490, 148);
         this.coeMoleculeNode.Name = "coeMoleculeNode";
         this.coeMoleculeNode.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeMoleculeNode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeMoleculeNode.Size = new System.Drawing.Size(83, 20);
         this.coeMoleculeNode.StyleController = this.layoutControl;
         this.coeMoleculeNode.TabIndex = 190;
         // 
         // coeContainerPhysical
         // 
         this.coeContainerPhysical.EditValue = System.Drawing.Color.Empty;
         this.coeContainerPhysical.Location = new System.Drawing.Point(205, 172);
         this.coeContainerPhysical.Name = "coeContainerPhysical";
         this.coeContainerPhysical.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeContainerPhysical.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeContainerPhysical.Size = new System.Drawing.Size(90, 20);
         this.coeContainerPhysical.StyleController = this.layoutControl;
         this.coeContainerPhysical.TabIndex = 172;
         // 
         // txtContainerOpacity
         // 
         this.txtContainerOpacity.EditValue = "0.1";
         this.txtContainerOpacity.Location = new System.Drawing.Point(205, 196);
         this.txtContainerOpacity.Name = "txtContainerOpacity";
         this.txtContainerOpacity.Size = new System.Drawing.Size(90, 20);
         this.txtContainerOpacity.StyleController = this.layoutControl;
         this.txtContainerOpacity.TabIndex = 174;
         // 
         // coeNeighborhoodLink
         // 
         this.coeNeighborhoodLink.EditValue = System.Drawing.Color.Empty;
         this.coeNeighborhoodLink.Location = new System.Drawing.Point(205, 220);
         this.coeNeighborhoodLink.Name = "coeNeighborhoodLink";
         this.coeNeighborhoodLink.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeNeighborhoodLink.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeNeighborhoodLink.Size = new System.Drawing.Size(90, 20);
         this.coeNeighborhoodLink.StyleController = this.layoutControl;
         this.coeNeighborhoodLink.TabIndex = 154;
         // 
         // coeNeighborhoodNode
         // 
         this.coeNeighborhoodNode.EditValue = System.Drawing.Color.Empty;
         this.coeNeighborhoodNode.Location = new System.Drawing.Point(205, 244);
         this.coeNeighborhoodNode.Name = "coeNeighborhoodNode";
         this.coeNeighborhoodNode.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeNeighborhoodNode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeNeighborhoodNode.Size = new System.Drawing.Size(90, 20);
         this.coeNeighborhoodNode.StyleController = this.layoutControl;
         this.coeNeighborhoodNode.TabIndex = 162;
         // 
         // coeNeighborhoodPort
         // 
         this.coeNeighborhoodPort.EditValue = System.Drawing.Color.Empty;
         this.coeNeighborhoodPort.Location = new System.Drawing.Point(205, 268);
         this.coeNeighborhoodPort.Name = "coeNeighborhoodPort";
         this.coeNeighborhoodPort.Properties.AutomaticColor = System.Drawing.Color.Black;
         this.coeNeighborhoodPort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.coeNeighborhoodPort.Size = new System.Drawing.Size(90, 20);
         this.coeNeighborhoodPort.StyleController = this.layoutControl;
         this.coeNeighborhoodPort.TabIndex = 160;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.colorsLayoutControlGroup});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(587, 404);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.defaultSizeOfNewMoleculeControlItem,
            this.defaultSizeOfNewReactionControlItem,
            this.layoutControlItem2,
            this.showSnapGridControlItem});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Size = new System.Drawing.Size(587, 116);
         this.layoutControlGroup2.TextVisible = false;
         // 
         // defaultSizeOfNewMoleculeControlItem
         // 
         this.defaultSizeOfNewMoleculeControlItem.Control = this.cbeDefaultNodeSizeMolecule;
         this.defaultSizeOfNewMoleculeControlItem.CustomizationFormText = "defaultSizeOfNewMoleculeControlItem";
         this.defaultSizeOfNewMoleculeControlItem.Location = new System.Drawing.Point(252, 24);
         this.defaultSizeOfNewMoleculeControlItem.Name = "defaultSizeOfNewMoleculeControlItem";
         this.defaultSizeOfNewMoleculeControlItem.Size = new System.Drawing.Size(311, 24);
         this.defaultSizeOfNewMoleculeControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // defaultSizeOfNewReactionControlItem
         // 
         this.defaultSizeOfNewReactionControlItem.Control = this.cbeDefaultNodeSizeReaction;
         this.defaultSizeOfNewReactionControlItem.CustomizationFormText = "defaultSizeOfNewReactionControlItem";
         this.defaultSizeOfNewReactionControlItem.Location = new System.Drawing.Point(252, 0);
         this.defaultSizeOfNewReactionControlItem.Name = "defaultSizeOfNewReactionControlItem";
         this.defaultSizeOfNewReactionControlItem.Size = new System.Drawing.Size(311, 24);
         this.defaultSizeOfNewReactionControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chkMoleculePropertiesVisible;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(252, 25);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // showSnapGridControlItem
         // 
         this.showSnapGridControlItem.Control = this.chkSnapGridVisible;
         this.showSnapGridControlItem.CustomizationFormText = "layoutControlItem1";
         this.showSnapGridControlItem.Location = new System.Drawing.Point(0, 0);
         this.showSnapGridControlItem.Name = "showSnapGridControlItem";
         this.showSnapGridControlItem.Size = new System.Drawing.Size(252, 23);
         this.showSnapGridControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.showSnapGridControlItem.TextVisible = false;
         // 
         // colorsLayoutControlGroup
         // 
         this.colorsLayoutControlGroup.CustomizationFormText = "layoutControlGroup3";
         this.colorsLayoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.containerLogicalControlItem,
            this.moleculeNodeControlItem,
            this.containerPhysicalControlItem,
            this.containerOpacityControlItem,
            this.neighborhoodLinkControlItem,
            this.neighborhoodNodeControlItem,
            this.neighborhoodPortControlItem,
            this.reactionNodeControlItem,
            this.reactionPortEductControlItem,
            this.reactionLinkEductControlItem,
            this.reactionPortProductControlItem,
            this.reactionLinkProductControlItem,
            this.reactionPortModifierControlItem,
            this.reactionLinkModifierControlItem});
         this.colorsLayoutControlGroup.Location = new System.Drawing.Point(0, 116);
         this.colorsLayoutControlGroup.Name = "colorsLayoutControlGroup";
         this.colorsLayoutControlGroup.Size = new System.Drawing.Size(587, 288);
         // 
         // containerLogicalControlItem
         // 
         this.containerLogicalControlItem.Control = this.coeContainerLogical;
         this.containerLogicalControlItem.CustomizationFormText = "layoutControlItem6";
         this.containerLogicalControlItem.Location = new System.Drawing.Point(0, 0);
         this.containerLogicalControlItem.Name = "containerLogicalControlItem";
         this.containerLogicalControlItem.Size = new System.Drawing.Size(285, 24);
         this.containerLogicalControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // moleculeNodeControlItem
         // 
         this.moleculeNodeControlItem.Control = this.coeMoleculeNode;
         this.moleculeNodeControlItem.CustomizationFormText = "moleculeNodeControlItem";
         this.moleculeNodeControlItem.Location = new System.Drawing.Point(285, 0);
         this.moleculeNodeControlItem.Name = "moleculeNodeControlItem";
         this.moleculeNodeControlItem.Size = new System.Drawing.Size(278, 24);
         this.moleculeNodeControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // containerPhysicalControlItem
         // 
         this.containerPhysicalControlItem.Control = this.coeContainerPhysical;
         this.containerPhysicalControlItem.CustomizationFormText = "layoutControlItem6";
         this.containerPhysicalControlItem.Location = new System.Drawing.Point(0, 24);
         this.containerPhysicalControlItem.Name = "containerPhysicalControlItem";
         this.containerPhysicalControlItem.Size = new System.Drawing.Size(285, 24);
         this.containerPhysicalControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // containerOpacityControlItem
         // 
         this.containerOpacityControlItem.Control = this.txtContainerOpacity;
         this.containerOpacityControlItem.CustomizationFormText = "layoutControlItem7";
         this.containerOpacityControlItem.Location = new System.Drawing.Point(0, 48);
         this.containerOpacityControlItem.Name = "containerOpacityControlItem";
         this.containerOpacityControlItem.Size = new System.Drawing.Size(285, 24);
         this.containerOpacityControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // neighborhoodLinkControlItem
         // 
         this.neighborhoodLinkControlItem.Control = this.coeNeighborhoodLink;
         this.neighborhoodLinkControlItem.CustomizationFormText = "layoutControlItem8";
         this.neighborhoodLinkControlItem.Location = new System.Drawing.Point(0, 72);
         this.neighborhoodLinkControlItem.Name = "neighborhoodLinkControlItem";
         this.neighborhoodLinkControlItem.Size = new System.Drawing.Size(285, 24);
         this.neighborhoodLinkControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // neighborhoodNodeControlItem
         // 
         this.neighborhoodNodeControlItem.Control = this.coeNeighborhoodNode;
         this.neighborhoodNodeControlItem.CustomizationFormText = "layoutControlItem9";
         this.neighborhoodNodeControlItem.Location = new System.Drawing.Point(0, 96);
         this.neighborhoodNodeControlItem.Name = "neighborhoodNodeControlItem";
         this.neighborhoodNodeControlItem.Size = new System.Drawing.Size(285, 24);
         this.neighborhoodNodeControlItem.Text = "neighborhoodModeControlItem";
         this.neighborhoodNodeControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // neighborhoodPortControlItem
         // 
         this.neighborhoodPortControlItem.Control = this.coeNeighborhoodPort;
         this.neighborhoodPortControlItem.CustomizationFormText = "layoutControlItem10";
         this.neighborhoodPortControlItem.Location = new System.Drawing.Point(0, 120);
         this.neighborhoodPortControlItem.Name = "neighborhoodPortControlItem";
         this.neighborhoodPortControlItem.Size = new System.Drawing.Size(285, 126);
         this.neighborhoodPortControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // reactionNodeControlItem
         // 
         this.reactionNodeControlItem.Control = this.coeReactionNode;
         this.reactionNodeControlItem.CustomizationFormText = "reactionNodeControlItem";
         this.reactionNodeControlItem.Location = new System.Drawing.Point(285, 24);
         this.reactionNodeControlItem.Name = "reactionNodeControlItem";
         this.reactionNodeControlItem.Size = new System.Drawing.Size(278, 24);
         this.reactionNodeControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // reactionPortEductControlItem
         // 
         this.reactionPortEductControlItem.Control = this.coeReactionPortEduct;
         this.reactionPortEductControlItem.CustomizationFormText = "reactionPortEductControlItem";
         this.reactionPortEductControlItem.Location = new System.Drawing.Point(285, 48);
         this.reactionPortEductControlItem.Name = "reactionPortEductControlItem";
         this.reactionPortEductControlItem.Size = new System.Drawing.Size(278, 24);
         this.reactionPortEductControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // reactionLinkEductControlItem
         // 
         this.reactionLinkEductControlItem.Control = this.coeReactionLinkEduct;
         this.reactionLinkEductControlItem.CustomizationFormText = "reactionLinkEductControlItem";
         this.reactionLinkEductControlItem.Location = new System.Drawing.Point(285, 72);
         this.reactionLinkEductControlItem.Name = "reactionLinkEductControlItem";
         this.reactionLinkEductControlItem.Size = new System.Drawing.Size(278, 24);
         this.reactionLinkEductControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // reactionPortProductControlItem
         // 
         this.reactionPortProductControlItem.Control = this.coeReactionPortProduct;
         this.reactionPortProductControlItem.CustomizationFormText = "reactionPortProductControlItem";
         this.reactionPortProductControlItem.Location = new System.Drawing.Point(285, 96);
         this.reactionPortProductControlItem.Name = "reactionPortProductControlItem";
         this.reactionPortProductControlItem.Size = new System.Drawing.Size(278, 24);
         this.reactionPortProductControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // reactionLinkProductControlItem
         // 
         this.reactionLinkProductControlItem.Control = this.coeReactionLinkProduct;
         this.reactionLinkProductControlItem.CustomizationFormText = "reactionLinkProductControlItem";
         this.reactionLinkProductControlItem.Location = new System.Drawing.Point(285, 120);
         this.reactionLinkProductControlItem.Name = "reactionLinkProductControlItem";
         this.reactionLinkProductControlItem.Size = new System.Drawing.Size(278, 24);
         this.reactionLinkProductControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // reactionPortModifierControlItem
         // 
         this.reactionPortModifierControlItem.Control = this.coeReactionPortModifier;
         this.reactionPortModifierControlItem.CustomizationFormText = "reactionPortModifierControlItem";
         this.reactionPortModifierControlItem.Location = new System.Drawing.Point(285, 144);
         this.reactionPortModifierControlItem.Name = "reactionPortModifierControlItem";
         this.reactionPortModifierControlItem.Size = new System.Drawing.Size(278, 24);
         this.reactionPortModifierControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // reactionLinkModifierControlItem
         // 
         this.reactionLinkModifierControlItem.Control = this.coeReactionLinkModifier;
         this.reactionLinkModifierControlItem.CustomizationFormText = "reactionLinkModifierControlItem";
         this.reactionLinkModifierControlItem.Location = new System.Drawing.Point(285, 168);
         this.reactionLinkModifierControlItem.Name = "reactionLinkModifierControlItem";
         this.reactionLinkModifierControlItem.Size = new System.Drawing.Size(278, 78);
         this.reactionLinkModifierControlItem.TextSize = new System.Drawing.Size(188, 13);
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(50, 20);
         // 
         // DiagramOptionsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "DiagramOptionsView";
         this.Size = new System.Drawing.Size(587, 404);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkMoleculePropertiesVisible.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionPortModifier.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionLinkModifier.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionPortEduct.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionNode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionLinkProduct.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionPortProduct.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkSnapGridVisible.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeReactionLinkEduct.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbeDefaultNodeSizeReaction.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbeDefaultNodeSizeMolecule.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeContainerLogical.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeMoleculeNode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeContainerPhysical.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtContainerOpacity.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeNeighborhoodLink.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeNeighborhoodNode.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.coeNeighborhoodPort.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultSizeOfNewMoleculeControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.defaultSizeOfNewReactionControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.showSnapGridControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.colorsLayoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.containerLogicalControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.moleculeNodeControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.containerPhysicalControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.containerOpacityControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.neighborhoodLinkControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.neighborhoodNodeControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.neighborhoodPortControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionNodeControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionPortEductControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionLinkEductControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionPortProductControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionLinkProductControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionPortModifierControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reactionLinkModifierControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxComboBoxEdit cbeDefaultNodeSizeReaction;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbeDefaultNodeSizeMolecule;
      private System.Windows.Forms.Timer timer1;
      private DevExpress.XtraEditors.TextEdit txtContainerOpacity;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraLayout.LayoutControlItem defaultSizeOfNewMoleculeControlItem;
      private DevExpress.XtraLayout.LayoutControlItem defaultSizeOfNewReactionControlItem;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem containerLogicalControlItem;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
      private UxCheckEdit chkMoleculePropertiesVisible;
      private DevExpress.XtraLayout.LayoutControlGroup colorsLayoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem moleculeNodeControlItem;
      private DevExpress.XtraLayout.LayoutControlItem containerPhysicalControlItem;
      private DevExpress.XtraLayout.LayoutControlItem containerOpacityControlItem;
      private DevExpress.XtraLayout.LayoutControlItem neighborhoodLinkControlItem;
      private DevExpress.XtraLayout.LayoutControlItem neighborhoodNodeControlItem;
      private DevExpress.XtraLayout.LayoutControlItem neighborhoodPortControlItem;
      private DevExpress.XtraLayout.LayoutControlItem reactionNodeControlItem;
      private DevExpress.XtraLayout.LayoutControlItem reactionPortEductControlItem;
      private DevExpress.XtraLayout.LayoutControlItem reactionLinkEductControlItem;
      private DevExpress.XtraLayout.LayoutControlItem reactionPortProductControlItem;
      private DevExpress.XtraLayout.LayoutControlItem reactionLinkProductControlItem;
      private DevExpress.XtraLayout.LayoutControlItem reactionPortModifierControlItem;
      private DevExpress.XtraLayout.LayoutControlItem reactionLinkModifierControlItem;
      private UxCheckEdit chkSnapGridVisible;
      private DevExpress.XtraLayout.LayoutControlItem showSnapGridControlItem;
      private UxColorPickEditWithHistory coeReactionPortModifier;
      private UxColorPickEditWithHistory coeReactionLinkModifier;
      private UxColorPickEditWithHistory coeReactionPortEduct;
      private UxColorPickEditWithHistory coeReactionNode;
      private UxColorPickEditWithHistory coeMoleculeNode;
      private UxColorPickEditWithHistory coeReactionPortProduct;
      private UxColorPickEditWithHistory coeReactionLinkProduct;
      private UxColorPickEditWithHistory coeReactionLinkEduct;
      private UxColorPickEditWithHistory coeContainerPhysical;
      private UxColorPickEditWithHistory coeContainerLogical;
      private UxColorPickEditWithHistory coeNeighborhoodNode;
      private UxColorPickEditWithHistory coeNeighborhoodPort;
      private UxColorPickEditWithHistory coeNeighborhoodLink;
   }
}