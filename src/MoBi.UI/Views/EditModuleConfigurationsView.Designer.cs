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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.cbParameterValuesSelection = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.cbInitialConditionsSelection = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.selectedModuleTreeView = new OSPSuite.UI.Controls.UxTreeView();
         this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
         this.btnRemove = new DevExpress.XtraEditors.SimpleButton();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemBtnRemove = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemBtnAdd = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.startValuesSelectionGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemPSVSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMSVSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupModuleSelection = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemModuleSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupSelectedModules = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemSelectedModules = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonMoveUp = new OSPSuite.UI.Controls.UxSimpleButton();
         this.layoutItemButtonMoveUp = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.buttonMoveDown = new OSPSuite.UI.Controls.UxSimpleButton();
         this.layoutItemButtonMoveDown = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbParameterValuesSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbInitialConditionsSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.selectedModuleTreeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnRemove)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnAdd)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.startValuesSelectionGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPSVSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMSVSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupModuleSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemModuleSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSelectedModules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectedModules)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveUp)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveDown)).BeginInit();
         this.SuspendLayout();
         // 
         // moduleSelectionTreeView
         // 
         this.moduleSelectionTreeView.Location = new System.Drawing.Point(24, 45);
         this.moduleSelectionTreeView.Name = "moduleSelectionTreeView";
         this.moduleSelectionTreeView.ShowDescendantNode = true;
         this.moduleSelectionTreeView.Size = new System.Drawing.Size(463, 699);
         this.moduleSelectionTreeView.TabIndex = 0;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.cbParameterValuesSelection);
         this.layoutControl.Controls.Add(this.cbInitialConditionsSelection);
         this.layoutControl.Controls.Add(this.selectedModuleTreeView);
         this.layoutControl.Controls.Add(this.btnAdd);
         this.layoutControl.Controls.Add(this.btnRemove);
         this.layoutControl.Controls.Add(this.moduleSelectionTreeView);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "uxLayoutControl1";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1674, 938, 650, 864);
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(1100, 768);
         this.layoutControl.TabIndex = 2;
         this.layoutControl.Text = "uxLayoutControl1";
         // 
         // cbParameterValuesSelection
         // 
         this.cbParameterValuesSelection.Location = new System.Drawing.Point(649, 700);
         this.cbParameterValuesSelection.Name = "cbParameterValuesSelection";
         this.cbParameterValuesSelection.Properties.AllowMouseWheel = false;
         this.cbParameterValuesSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbParameterValuesSelection.Size = new System.Drawing.Size(427, 20);
         this.cbParameterValuesSelection.StyleController = this.layoutControl;
         this.cbParameterValuesSelection.TabIndex = 8;
         // 
         // cbInitialConditionsSelection
         // 
         this.cbInitialConditionsSelection.Location = new System.Drawing.Point(649, 724);
         this.cbInitialConditionsSelection.Name = "cbInitialConditionsSelection";
         this.cbInitialConditionsSelection.Properties.AllowMouseWheel = false;
         this.cbInitialConditionsSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbInitialConditionsSelection.Size = new System.Drawing.Size(427, 20);
         this.cbInitialConditionsSelection.StyleController = this.layoutControl;
         this.cbInitialConditionsSelection.TabIndex = 7;
         // 
         // selectedModuleTreeView
         // 
         this.selectedModuleTreeView.IsLatched = false;
         this.selectedModuleTreeView.Location = new System.Drawing.Point(617, 47);
         this.selectedModuleTreeView.Name = "selectedModuleTreeView";
         this.selectedModuleTreeView.OptionsBehavior.Editable = false;
         this.selectedModuleTreeView.OptionsMenu.ShowExpandCollapseItems = false;
         this.selectedModuleTreeView.OptionsView.ShowColumns = false;
         this.selectedModuleTreeView.OptionsView.ShowHorzLines = false;
         this.selectedModuleTreeView.OptionsView.ShowIndicator = false;
         this.selectedModuleTreeView.OptionsView.ShowVertLines = false;
         this.selectedModuleTreeView.Size = new System.Drawing.Size(457, 602);
         this.selectedModuleTreeView.TabIndex = 6;
         this.selectedModuleTreeView.ToolTipForNode = null;
         this.selectedModuleTreeView.UseLazyLoading = false;
         // 
         // btnAdd
         // 
         this.btnAdd.Location = new System.Drawing.Point(503, 176);
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Size = new System.Drawing.Size(96, 22);
         this.btnAdd.StyleController = this.layoutControl;
         this.btnAdd.TabIndex = 5;
         this.btnAdd.Text = "btnAdd";
         // 
         // btnRemove
         // 
         this.btnRemove.Location = new System.Drawing.Point(503, 202);
         this.btnRemove.Name = "btnRemove";
         this.btnRemove.Size = new System.Drawing.Size(96, 22);
         this.btnRemove.StyleController = this.layoutControl;
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
         this.Root.Size = new System.Drawing.Size(1100, 768);
         this.Root.TextVisible = false;
         // 
         // layoutItemBtnRemove
         // 
         this.layoutItemBtnRemove.Control = this.btnRemove;
         this.layoutItemBtnRemove.Location = new System.Drawing.Point(491, 190);
         this.layoutItemBtnRemove.Name = "layoutItemBtnRemove";
         this.layoutItemBtnRemove.Size = new System.Drawing.Size(100, 26);
         this.layoutItemBtnRemove.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemBtnRemove.TextVisible = false;
         // 
         // layoutItemBtnAdd
         // 
         this.layoutItemBtnAdd.Control = this.btnAdd;
         this.layoutItemBtnAdd.Location = new System.Drawing.Point(491, 164);
         this.layoutItemBtnAdd.Name = "layoutItemBtnAdd";
         this.layoutItemBtnAdd.Size = new System.Drawing.Size(100, 26);
         this.layoutItemBtnAdd.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemBtnAdd.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(491, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(100, 164);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(491, 216);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(100, 439);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // startValuesSelectionGroup
         // 
         this.startValuesSelectionGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemPSVSelection,
            this.layoutItemMSVSelection});
         this.startValuesSelectionGroup.Location = new System.Drawing.Point(491, 655);
         this.startValuesSelectionGroup.Name = "startValuesSelectionGroup";
         this.startValuesSelectionGroup.Size = new System.Drawing.Size(589, 93);
         this.startValuesSelectionGroup.Spacing = new DevExpress.XtraLayout.Utils.Padding(9, 2, 2, 2);
         // 
         // layoutItemPSVSelection
         // 
         this.layoutItemPSVSelection.Control = this.cbParameterValuesSelection;
         this.layoutItemPSVSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemPSVSelection.Name = "layoutItemPSVSelection";
         this.layoutItemPSVSelection.Size = new System.Drawing.Size(558, 24);
         this.layoutItemPSVSelection.TextSize = new System.Drawing.Size(115, 13);
         // 
         // layoutItemMSVSelection
         // 
         this.layoutItemMSVSelection.Control = this.cbInitialConditionsSelection;
         this.layoutItemMSVSelection.Location = new System.Drawing.Point(0, 24);
         this.layoutItemMSVSelection.Name = "layoutItemMSVSelection";
         this.layoutItemMSVSelection.Size = new System.Drawing.Size(558, 24);
         this.layoutItemMSVSelection.TextSize = new System.Drawing.Size(115, 13);
         // 
         // layoutGroupModuleSelection
         // 
         this.layoutGroupModuleSelection.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemModuleSelection});
         this.layoutGroupModuleSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupModuleSelection.Name = "layoutGroupModuleSelection";
         this.layoutGroupModuleSelection.Size = new System.Drawing.Size(491, 748);
         // 
         // layoutItemModuleSelection
         // 
         this.layoutItemModuleSelection.Control = this.moduleSelectionTreeView;
         this.layoutItemModuleSelection.Location = new System.Drawing.Point(0, 0);
         this.layoutItemModuleSelection.Name = "layoutItemModuleSelection";
         this.layoutItemModuleSelection.Size = new System.Drawing.Size(467, 703);
         this.layoutItemModuleSelection.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemModuleSelection.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemModuleSelection.TextVisible = false;
         // 
         // layoutGroupSelectedModules
         // 
         this.layoutGroupSelectedModules.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemSelectedModules,
            this.layoutItemButtonMoveUp,
            this.emptySpaceItem3,
            this.layoutItemButtonMoveDown});
         this.layoutGroupSelectedModules.Location = new System.Drawing.Point(591, 0);
         this.layoutGroupSelectedModules.Name = "layoutGroupSelectedModules";
         this.layoutGroupSelectedModules.Size = new System.Drawing.Size(489, 655);
         // 
         // layoutItemSelectedModules
         // 
         this.layoutItemSelectedModules.Control = this.selectedModuleTreeView;
         this.layoutItemSelectedModules.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSelectedModules.Name = "layoutItemSelectedModules";
         this.layoutItemSelectedModules.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
         this.layoutItemSelectedModules.Size = new System.Drawing.Size(465, 610);
         this.layoutItemSelectedModules.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemSelectedModules.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSelectedModules.TextVisible = false;
         // 
         // buttonMoveUp
         // 
         this.buttonMoveUp.Location = new System.Drawing.Point(1050, 45);
         this.buttonMoveUp.Manager = null;
         this.buttonMoveUp.Name = "buttonMoveUp";
         this.buttonMoveUp.Shortcut = System.Windows.Forms.Keys.None;
         this.buttonMoveUp.Size = new System.Drawing.Size(92, 22);
         this.buttonMoveUp.StyleController = this.layoutControl;
         this.buttonMoveUp.TabIndex = 9;
         this.buttonMoveUp.Text = "buttonMoveUp";
         // 
         // layoutItemButtonMoveUp
         // 
         this.layoutItemButtonMoveUp.Control = this.buttonMoveUp;
         this.layoutItemButtonMoveUp.Location = new System.Drawing.Point(396, 0);
         this.layoutItemButtonMoveUp.Name = "layoutItemButtonMoveUp";
         this.layoutItemButtonMoveUp.Size = new System.Drawing.Size(96, 26);
         this.layoutItemButtonMoveUp.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonMoveUp.TextVisible = false;
         // 
         // emptySpaceItem3
         // 
         this.emptySpaceItem3.AllowHotTrack = false;
         this.emptySpaceItem3.Location = new System.Drawing.Point(396, 52);
         this.emptySpaceItem3.Name = "emptySpaceItem3";
         this.emptySpaceItem3.Size = new System.Drawing.Size(96, 558);
         this.emptySpaceItem3.TextSize = new System.Drawing.Size(0, 0);
         // 
         // buttonMoveDown
         // 
         this.buttonMoveDown.Location = new System.Drawing.Point(1050, 71);
         this.buttonMoveDown.Manager = null;
         this.buttonMoveDown.Name = "buttonMoveDown";
         this.buttonMoveDown.Shortcut = System.Windows.Forms.Keys.None;
         this.buttonMoveDown.Size = new System.Drawing.Size(92, 22);
         this.buttonMoveDown.StyleController = this.layoutControl;
         this.buttonMoveDown.TabIndex = 10;
         this.buttonMoveDown.Text = "buttonMoveDown";
         // 
         // layoutItemButtonMoveDown
         // 
         this.layoutItemButtonMoveDown.Control = this.buttonMoveDown;
         this.layoutItemButtonMoveDown.Location = new System.Drawing.Point(396, 26);
         this.layoutItemButtonMoveDown.Name = "layoutItemButtonMoveDown";
         this.layoutItemButtonMoveDown.Size = new System.Drawing.Size(96, 26);
         this.layoutItemButtonMoveDown.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonMoveDown.TextVisible = false;
         // 
         // EditModuleConfigurationsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditModuleConfigurationsView";
         this.Size = new System.Drawing.Size(1100, 768);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbParameterValuesSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbInitialConditionsSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.selectedModuleTreeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnRemove)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemBtnAdd)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.startValuesSelectionGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPSVSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMSVSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupModuleSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemModuleSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSelectedModules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectedModules)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveUp)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveDown)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.FilterTreeView moduleSelectionTreeView;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbParameterValuesSelection;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbInitialConditionsSelection;
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
      private OSPSuite.UI.Controls.UxSimpleButton buttonMoveDown;
      private OSPSuite.UI.Controls.UxSimpleButton buttonMoveUp;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonMoveUp;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonMoveDown;
   }
}
