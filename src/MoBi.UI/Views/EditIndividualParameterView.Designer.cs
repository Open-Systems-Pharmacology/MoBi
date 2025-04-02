namespace MoBi.UI.Views
{
   partial class EditIndividualParameterView
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
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnCreateFormula = new OSPSuite.UI.Controls.UxSimpleButton();
         this.cbDisplayUnit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.textEditValue = new DevExpress.XtraEditors.TextEdit();
         this.panelFormula = new OSPSuite.UI.Controls.UxPanelControl();
         this.textEditDimension = new DevExpress.XtraEditors.TextEdit();
         this.panelOriginView = new OSPSuite.UI.Controls.UxPanelControl();
         this.textEditName = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroupProperties = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemValueOrigin = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemDimension = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemValue = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemUnits = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroupFormula = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCreateFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         this.lblWarning = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItemWarning = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroupWarning = new DevExpress.XtraLayout.LayoutControlGroup();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbDisplayUnit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEditValue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEditDimension.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOriginView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOrigin)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDimension)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValue)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUnits)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCreateFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemWarning)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupWarning)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.lblWarning);
         this.uxLayoutControl.Controls.Add(this.btnCreateFormula);
         this.uxLayoutControl.Controls.Add(this.cbDisplayUnit);
         this.uxLayoutControl.Controls.Add(this.textEditValue);
         this.uxLayoutControl.Controls.Add(this.panelFormula);
         this.uxLayoutControl.Controls.Add(this.textEditDimension);
         this.uxLayoutControl.Controls.Add(this.panelOriginView);
         this.uxLayoutControl.Controls.Add(this.textEditName);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1020, 24, 650, 400);
         this.uxLayoutControl.Root = this.Root;
         this.uxLayoutControl.Size = new System.Drawing.Size(546, 316);
         this.uxLayoutControl.TabIndex = 0;
         this.uxLayoutControl.Text = "uxLayoutControl";
         // 
         // btnCreateFormula
         // 
         this.btnCreateFormula.Location = new System.Drawing.Point(12, 232);
         this.btnCreateFormula.Manager = null;
         this.btnCreateFormula.Name = "btnCreateFormula";
         this.btnCreateFormula.Shortcut = System.Windows.Forms.Keys.None;
         this.btnCreateFormula.Size = new System.Drawing.Size(259, 22);
         this.btnCreateFormula.StyleController = this.uxLayoutControl;
         this.btnCreateFormula.TabIndex = 9;
         this.btnCreateFormula.Text = "btnCreateFormula";
         // 
         // cbDisplayUnit
         // 
         this.cbDisplayUnit.Location = new System.Drawing.Point(472, 131);
         this.cbDisplayUnit.Name = "cbDisplayUnit";
         this.cbDisplayUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbDisplayUnit.Size = new System.Drawing.Size(50, 20);
         this.cbDisplayUnit.StyleController = this.uxLayoutControl;
         this.cbDisplayUnit.TabIndex = 8;
         // 
         // textEditValue
         // 
         this.textEditValue.Location = new System.Drawing.Point(171, 131);
         this.textEditValue.Name = "textEditValue";
         this.textEditValue.Size = new System.Drawing.Size(150, 20);
         this.textEditValue.StyleController = this.uxLayoutControl;
         this.textEditValue.TabIndex = 7;
         // 
         // panelFormula
         // 
         this.panelFormula.Location = new System.Drawing.Point(24, 291);
         this.panelFormula.Name = "panelFormula";
         this.panelFormula.Size = new System.Drawing.Size(498, 1);
         this.panelFormula.TabIndex = 6;
         // 
         // textEditDimension
         // 
         this.textEditDimension.Location = new System.Drawing.Point(171, 155);
         this.textEditDimension.Name = "textEditDimension";
         this.textEditDimension.Size = new System.Drawing.Size(351, 20);
         this.textEditDimension.StyleController = this.uxLayoutControl;
         this.textEditDimension.TabIndex = 5;
         // 
         // panelOriginView
         // 
         this.panelOriginView.Location = new System.Drawing.Point(24, 179);
         this.panelOriginView.Name = "panelOriginView";
         this.panelOriginView.Size = new System.Drawing.Size(498, 37);
         this.panelOriginView.TabIndex = 4;
         // 
         // textEditName
         // 
         this.textEditName.Location = new System.Drawing.Point(159, 12);
         this.textEditName.Name = "textEditName";
         this.textEditName.Size = new System.Drawing.Size(375, 20);
         this.textEditName.StyleController = this.uxLayoutControl;
         this.textEditName.TabIndex = 0;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemName,
            this.layoutControlGroupProperties,
            this.layoutControlGroupFormula,
            this.layoutControlItemCreateFormula,
            this.emptySpaceItem,
            this.layoutControlGroupWarning});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(546, 316);
         this.Root.TextVisible = false;
         // 
         // layoutControlItemName
         // 
         this.layoutControlItemName.Control = this.textEditName;
         this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemName.Name = "layoutControlItemName";
         this.layoutControlItemName.Size = new System.Drawing.Size(526, 24);
         this.layoutControlItemName.TextSize = new System.Drawing.Size(135, 13);
         // 
         // layoutControlGroupProperties
         // 
         this.layoutControlGroupProperties.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValueOrigin,
            this.layoutControlItemDimension,
            this.layoutControlItemValue,
            this.layoutControlItemUnits});
         this.layoutControlGroupProperties.Location = new System.Drawing.Point(0, 86);
         this.layoutControlGroupProperties.Name = "layoutControlGroupProperties";
         this.layoutControlGroupProperties.Size = new System.Drawing.Size(526, 134);
         // 
         // layoutItemValueOrigin
         // 
         this.layoutItemValueOrigin.Control = this.panelOriginView;
         this.layoutItemValueOrigin.Location = new System.Drawing.Point(0, 48);
         this.layoutItemValueOrigin.Name = "layoutItemValueOrigin";
         this.layoutItemValueOrigin.Size = new System.Drawing.Size(502, 41);
         this.layoutItemValueOrigin.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemValueOrigin.TextVisible = false;
         // 
         // layoutControlItemDimension
         // 
         this.layoutControlItemDimension.Control = this.textEditDimension;
         this.layoutControlItemDimension.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemDimension.Name = "layoutControlItemDimension";
         this.layoutControlItemDimension.Size = new System.Drawing.Size(502, 24);
         this.layoutControlItemDimension.TextSize = new System.Drawing.Size(135, 13);
         // 
         // layoutControlItemValue
         // 
         this.layoutControlItemValue.Control = this.textEditValue;
         this.layoutControlItemValue.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemValue.Name = "layoutControlItemValue";
         this.layoutControlItemValue.Size = new System.Drawing.Size(301, 24);
         this.layoutControlItemValue.TextSize = new System.Drawing.Size(135, 13);
         // 
         // layoutControlItemUnits
         // 
         this.layoutControlItemUnits.Control = this.cbDisplayUnit;
         this.layoutControlItemUnits.Location = new System.Drawing.Point(301, 0);
         this.layoutControlItemUnits.Name = "layoutControlItemUnits";
         this.layoutControlItemUnits.Size = new System.Drawing.Size(201, 24);
         this.layoutControlItemUnits.TextSize = new System.Drawing.Size(135, 13);
         // 
         // layoutControlGroupFormula
         // 
         this.layoutControlGroupFormula.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemFormula});
         this.layoutControlGroupFormula.Location = new System.Drawing.Point(0, 246);
         this.layoutControlGroupFormula.Name = "layoutControlGroupFormula";
         this.layoutControlGroupFormula.Size = new System.Drawing.Size(526, 50);
         // 
         // layoutControlItemFormula
         // 
         this.layoutControlItemFormula.Control = this.panelFormula;
         this.layoutControlItemFormula.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemFormula.Name = "layoutControlItemFormula";
         this.layoutControlItemFormula.Size = new System.Drawing.Size(502, 5);
         this.layoutControlItemFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemFormula.TextVisible = false;
         // 
         // layoutControlItemCreateFormula
         // 
         this.layoutControlItemCreateFormula.Control = this.btnCreateFormula;
         this.layoutControlItemCreateFormula.Location = new System.Drawing.Point(0, 220);
         this.layoutControlItemCreateFormula.Name = "layoutControlItemCreateFormula";
         this.layoutControlItemCreateFormula.Size = new System.Drawing.Size(263, 26);
         this.layoutControlItemCreateFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCreateFormula.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(263, 220);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(263, 26);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // lblWarning
         // 
         this.lblWarning.Location = new System.Drawing.Point(24, 69);
         this.lblWarning.Name = "lblWarning";
         this.lblWarning.Size = new System.Drawing.Size(50, 13);
         this.lblWarning.StyleController = this.uxLayoutControl;
         this.lblWarning.TabIndex = 10;
         this.lblWarning.Text = "lblWarning";
         // 
         // layoutControlItemWarning
         // 
         this.layoutControlItemWarning.Control = this.lblWarning;
         this.layoutControlItemWarning.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemWarning.Name = "layoutControlItemWarning";
         this.layoutControlItemWarning.Size = new System.Drawing.Size(502, 17);
         this.layoutControlItemWarning.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemWarning.TextVisible = false;
         // 
         // layoutControlGroupWarning
         // 
         this.layoutControlGroupWarning.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemWarning});
         this.layoutControlGroupWarning.Location = new System.Drawing.Point(0, 24);
         this.layoutControlGroupWarning.Name = "layoutControlGroupWarning";
         this.layoutControlGroupWarning.Size = new System.Drawing.Size(526, 62);
         // 
         // EditIndividualParameterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Name = "EditIndividualParameterView";
         this.Size = new System.Drawing.Size(546, 316);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbDisplayUnit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEditValue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEditDimension.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelOriginView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValueOrigin)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDimension)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValue)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUnits)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCreateFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemWarning)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupWarning)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private OSPSuite.UI.Controls.UxPanelControl panelOriginView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValueOrigin;
      private DevExpress.XtraEditors.TextEdit textEditName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
      private DevExpress.XtraEditors.TextEdit textEditDimension;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDimension;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupProperties;
      private OSPSuite.UI.Controls.UxPanelControl panelFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFormula;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupFormula;
      private DevExpress.XtraEditors.TextEdit textEditValue;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemValue;
      private DevExpress.XtraEditors.ComboBoxEdit cbDisplayUnit;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemUnits;
      private OSPSuite.UI.Controls.UxSimpleButton btnCreateFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCreateFormula;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
      private DevExpress.XtraEditors.LabelControl lblWarning;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemWarning;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupWarning;
   }
}
