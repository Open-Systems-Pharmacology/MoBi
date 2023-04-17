namespace MoBi.UI.Views
{
   partial class EditModuleConfigurationsView
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

         disposeBinders();
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
         this.moduleSelectionTreeView = new OSPSuite.UI.Controls.FilterTreeView();
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.cbParameterStartValuesSelection = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.cbMoleculeStartValuesSelection = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.selectedModuleTreeView = new OSPSuite.UI.Controls.UxTreeView();
         this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
         this.btnRemove = new DevExpress.XtraEditors.SimpleButton();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemModuleSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemBtnRemove = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemBtnAdd = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemSelectedModules = new DevExpress.XtraLayout.LayoutControlItem();
         this.startValuesSelectionGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemPSVSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMSVSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupModuleSelection = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupSelectedModules = new DevExpress.XtraLayout.LayoutControlGroup();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbParameterStartValuesSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbMoleculeStartValuesSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.selectedModuleTreeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemModuleSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnRemove)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnAdd)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectedModules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.startValuesSelectionGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPSVSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMSVSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupModuleSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSelectedModules)).BeginInit();
         this.SuspendLayout();
         // 
         // moduleSelectionTreeView
         // 
         this.moduleSelectionTreeView.Location = new System.Drawing.Point(24, 45);
         this.moduleSelectionTreeView.Name = "moduleSelectionTreeView";
         this.moduleSelectionTreeView.ShowDescendantNode = true;
         this.moduleSelectionTreeView.Size = new System.Drawing.Size(474, 699);
         this.moduleSelectionTreeView.TabIndex = 0;
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.cbParameterStartValuesSelection);
         this.uxLayoutControl1.Controls.Add(this.cbMoleculeStartValuesSelection);
         this.uxLayoutControl1.Controls.Add(this.selectedModuleTreeView);
         this.uxLayoutControl1.Controls.Add(this.btnAdd);
         this.uxLayoutControl1.Controls.Add(this.btnRemove);
         this.uxLayoutControl1.Controls.Add(this.moduleSelectionTreeView);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1631, 476, 650, 400);
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(1166, 768);
         this.uxLayoutControl1.TabIndex = 2;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // cbParameterStartValuesSelection
         // 
         this.cbParameterStartValuesSelection.Location = new System.Drawing.Point(653, 700);
         this.cbParameterStartValuesSelection.Name = "cbParameterStartValuesSelection";
         this.cbParameterStartValuesSelection.Properties.AllowMouseWheel = false;
         this.cbParameterStartValuesSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbParameterStartValuesSelection.Size = new System.Drawing.Size(489, 20);
         this.cbParameterStartValuesSelection.StyleController = this.uxLayoutControl1;
         this.cbParameterStartValuesSelection.TabIndex = 8;
         // 
         // cbMoleculeStartValuesSelection
         // 
         this.cbMoleculeStartValuesSelection.Location = new System.Drawing.Point(653, 724);
         this.cbMoleculeStartValuesSelection.Name = "cbMoleculeStartValuesSelection";
         this.cbMoleculeStartValuesSelection.Properties.AllowMouseWheel = false;
         this.cbMoleculeStartValuesSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbMoleculeStartValuesSelection.Size = new System.Drawing.Size(489, 20);
         this.cbMoleculeStartValuesSelection.StyleController = this.uxLayoutControl1;
         this.cbMoleculeStartValuesSelection.TabIndex = 7;
         // 
         // selectedModuleTreeView
         // 
         this.selectedModuleTreeView.IsLatched = false;
         this.selectedModuleTreeView.Location = new System.Drawing.Point(654, 45);
         this.selectedModuleTreeView.Name = "selectedModuleTreeView";
         this.selectedModuleTreeView.OptionsBehavior.Editable = false;
         this.selectedModuleTreeView.OptionsMenu.ShowExpandCollapseItems = false;
         this.selectedModuleTreeView.OptionsView.ShowColumns = false;
         this.selectedModuleTreeView.OptionsView.ShowHorzLines = false;
         this.selectedModuleTreeView.OptionsView.ShowIndicator = false;
         this.selectedModuleTreeView.OptionsView.ShowVertLines = false;
         this.selectedModuleTreeView.Size = new System.Drawing.Size(488, 606);
         this.selectedModuleTreeView.TabIndex = 6;
         this.selectedModuleTreeView.ToolTipForNode = null;
         this.selectedModuleTreeView.UseLazyLoading = false;
         // 
         // btnAdd
         // 
         this.btnAdd.Location = new System.Drawing.Point(514, 176);
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Size = new System.Drawing.Size(124, 22);
         this.btnAdd.StyleController = this.uxLayoutControl1;
         this.btnAdd.TabIndex = 5;
         this.btnAdd.Text = "btnAdd";
         // 
         // btnRemove
         // 
         this.btnRemove.Location = new System.Drawing.Point(514, 202);
         this.btnRemove.Name = "btnRemove";
         this.btnRemove.Size = new System.Drawing.Size(124, 22);
         this.btnRemove.StyleController = this.uxLayoutControl1;
         this.btnRemove.TabIndex = 4;
         this.btnRemove.Text = "btnRemove";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemBtnRemove,
            this.layoutItemBtnAdd,
            this.emptySpaceItem1,
            this.emptySpaceItem2,
            this.startValuesSelectionGroup,
            this.layoutGroupModuleSelection,
            this.layoutGroupSelectedModules});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1166, 768);
         this.Root.TextVisible = false;
         // 
         // layoutItemModuleSelection
         // 
         this.layoutItemModuleSelection.Control = this.moduleSelectionTreeView;
         this.layoutItemModuleSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemModuleSelection.Name = "layoutItemModuleSelection";
         this.layoutItemModuleSelection.Size = new System.Drawing.Size(478, 703);
         this.layoutItemModuleSelection.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemModuleSelection.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemModuleSelection.TextVisible = false;
         // 
         // layoutItemBtnRemove
         // 
         this.layoutItemBtnRemove.Control = this.btnRemove;
         this.layoutItemBtnRemove.Location = new System.Drawing.Point(502, 190);
         this.layoutItemBtnRemove.Name = "layoutItemBtnRemove";
         this.layoutItemBtnRemove.Size = new System.Drawing.Size(128, 26);
         this.layoutItemBtnRemove.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemBtnRemove.TextVisible = false;
         // 
         // layoutItemBtnAdd
         // 
         this.layoutItemBtnAdd.Control = this.btnAdd;
         this.layoutItemBtnAdd.Location = new System.Drawing.Point(502, 164);
         this.layoutItemBtnAdd.Name = "layoutItemBtnAdd";
         this.layoutItemBtnAdd.Size = new System.Drawing.Size(128, 26);
         this.layoutItemBtnAdd.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemBtnAdd.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(502, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(128, 164);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(502, 216);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(128, 439);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemSelectedModules
         // 
         this.layoutItemSelectedModules.Control = this.selectedModuleTreeView;
         this.layoutItemSelectedModules.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSelectedModules.Name = "layoutItemSelectedModules";
         this.layoutItemSelectedModules.Size = new System.Drawing.Size(492, 610);
         this.layoutItemSelectedModules.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemSelectedModules.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSelectedModules.TextVisible = false;
         // 
         // startValuesSelectionGroup
         // 
         this.startValuesSelectionGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemPSVSelection,
            this.layoutItemMSVSelection});
         this.startValuesSelectionGroup.Location = new System.Drawing.Point(502, 655);
         this.startValuesSelectionGroup.Name = "startValuesSelectionGroup";
         this.startValuesSelectionGroup.Size = new System.Drawing.Size(644, 93);
         // 
         // layoutItemPSVSelection
         // 
         this.layoutItemPSVSelection.Control = this.cbParameterStartValuesSelection;
         this.layoutItemPSVSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemPSVSelection.Name = "layoutItemPSVSelection";
         this.layoutItemPSVSelection.Size = new System.Drawing.Size(620, 24);
         this.layoutItemPSVSelection.TextSize = new System.Drawing.Size(115, 13);
         // 
         // layoutItemMSVSelection
         // 
         this.layoutItemMSVSelection.Control = this.cbMoleculeStartValuesSelection;
         this.layoutItemMSVSelection.Location = new System.Drawing.Point(0, 24);
         this.layoutItemMSVSelection.Name = "layoutItemMSVSelection";
         this.layoutItemMSVSelection.Size = new System.Drawing.Size(620, 24);
         this.layoutItemMSVSelection.TextSize = new System.Drawing.Size(115, 13);
         // 
         // layoutGroupModuleSelection
         // 
         this.layoutGroupModuleSelection.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemModuleSelection});
         this.layoutGroupModuleSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupModuleSelection.Name = "layoutGroupModuleSelection";
         this.layoutGroupModuleSelection.Size = new System.Drawing.Size(502, 748);
         // 
         // layoutGroupSelectedModules
         // 
         this.layoutGroupSelectedModules.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemSelectedModules});
         this.layoutGroupSelectedModules.Location = new System.Drawing.Point(630, 0);
         this.layoutGroupSelectedModules.Name = "layoutGroupSelectedModules";
         this.layoutGroupSelectedModules.Size = new System.Drawing.Size(516, 655);
         // 
         // EditModuleConfigurationsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "EditModuleConfigurationsView";
         this.Size = new System.Drawing.Size(1166, 768);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbParameterStartValuesSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbMoleculeStartValuesSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.selectedModuleTreeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemModuleSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnRemove)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnAdd)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectedModules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.startValuesSelectionGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPSVSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMSVSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupModuleSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSelectedModules)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.FilterTreeView moduleSelectionTreeView;
      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbParameterStartValuesSelection;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbMoleculeStartValuesSelection;
      private OSPSuite.UI.Controls.UxTreeView selectedModuleTreeView;
      private DevExpress.XtraEditors.SimpleButton btnAdd;
      private DevExpress.XtraEditors.SimpleButton btnRemove;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemModuleSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemBtnRemove;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemBtnAdd;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSelectedModules;
      private DevExpress.XtraLayout.LayoutControlGroup startValuesSelectionGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPSVSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMSVSelection;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupModuleSelection;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupSelectedModules;
   }
}
