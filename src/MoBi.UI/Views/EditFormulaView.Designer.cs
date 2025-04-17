namespace MoBi.UI.Views
{
   abstract partial class EditFormulaView<TPresenter>
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
         this.pnlEdit = new DevExpress.XtraEditors.PanelControl();
         this.splitFormula = new DevExpress.XtraEditors.SplitContainerControl();
         this.cbFormulaName = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.tbFormulaName = new DevExpress.XtraEditors.TextEdit();
         this.btnCloneFormula = new DevExpress.XtraEditors.SimpleButton();
         this.btnAddFormula = new DevExpress.XtraEditors.SimpleButton();
         this.cbFormulaType = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemFormulaView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFormulaSelect = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFormulaTypeComboBox = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAddFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCloneFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFormulaName = new DevExpress.XtraLayout.LayoutControlItem();
         this.tbFormulaType = new DevExpress.XtraEditors.TextEdit();
         this.layoutItemFormulaTypeTextEdit = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlEdit)).BeginInit();
         this.pnlEdit.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula.Panel1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula.Panel2)).BeginInit();
         this.splitFormula.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbFormulaName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaSelect)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaTypeComboBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCloneFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbFormulaType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaTypeTextEdit)).BeginInit();
         this.SuspendLayout();
         // 
         // pnlEdit
         // 
         this.pnlEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.pnlEdit.Controls.Add(this.splitFormula);
         this.pnlEdit.Location = new System.Drawing.Point(2, 98);
         this.pnlEdit.Name = "pnlEdit";
         this.pnlEdit.Size = new System.Drawing.Size(876, 357);
         this.pnlEdit.TabIndex = 1;
         // 
         // splitFormula
         // 
         this.splitFormula.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitFormula.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitFormula.Location = new System.Drawing.Point(2, 2);
         this.splitFormula.Name = "splitFormula";
         // 
         // splitFormula.Panel1
         // 
         this.splitFormula.Panel1.Text = "Panel1";
         // 
         // splitFormula.Panel2
         // 
         this.splitFormula.Panel2.Text = "Panel2";
         this.splitFormula.Size = new System.Drawing.Size(872, 353);
         this.splitFormula.SplitterPosition = 436;
         this.splitFormula.TabIndex = 0;
         this.splitFormula.Text = "splitContainerControl1";
         // 
         // cbFormulaName
         // 
         this.cbFormulaName.Location = new System.Drawing.Point(179, 50);
         this.cbFormulaName.Name = "cbFormulaName";
         this.cbFormulaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFormulaName.Size = new System.Drawing.Size(259, 20);
         this.cbFormulaName.StyleController = this.layoutControl;
         this.cbFormulaName.TabIndex = 3;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.tbFormulaType);
         this.layoutControl.Controls.Add(this.tbFormulaName);
         this.layoutControl.Controls.Add(this.btnCloneFormula);
         this.layoutControl.Controls.Add(this.btnAddFormula);
         this.layoutControl.Controls.Add(this.cbFormulaType);
         this.layoutControl.Controls.Add(this.pnlEdit);
         this.layoutControl.Controls.Add(this.cbFormulaName);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(880, 457);
         this.layoutControl.TabIndex = 5;
         this.layoutControl.Text = "layoutControl1";
         // 
         // tbFormulaName
         // 
         this.tbFormulaName.Location = new System.Drawing.Point(179, 74);
         this.tbFormulaName.Name = "tbFormulaName";
         this.tbFormulaName.Size = new System.Drawing.Size(259, 20);
         this.tbFormulaName.StyleController = this.layoutControl;
         this.tbFormulaName.TabIndex = 7;
         // 
         // btnCloneFormula
         // 
         this.btnCloneFormula.Location = new System.Drawing.Point(662, 50);
         this.btnCloneFormula.Name = "btnCloneFormula";
         this.btnCloneFormula.Size = new System.Drawing.Size(216, 22);
         this.btnCloneFormula.StyleController = this.layoutControl;
         this.btnCloneFormula.TabIndex = 6;
         this.btnCloneFormula.Text = "btnCloneFormula";
         // 
         // btnAddFormula
         // 
         this.btnAddFormula.Location = new System.Drawing.Point(442, 50);
         this.btnAddFormula.Name = "btnAddFormula";
         this.btnAddFormula.Size = new System.Drawing.Size(216, 22);
         this.btnAddFormula.StyleController = this.layoutControl;
         this.btnAddFormula.TabIndex = 5;
         this.btnAddFormula.Text = "btnAddFormula";
         // 
         // cbFormulaType
         // 
         this.cbFormulaType.Location = new System.Drawing.Point(179, 2);
         this.cbFormulaType.Name = "cbFormulaType";
         this.cbFormulaType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFormulaType.Size = new System.Drawing.Size(699, 20);
         this.cbFormulaType.StyleController = this.layoutControl;
         this.cbFormulaType.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFormulaView,
            this.layoutItemFormulaSelect,
            this.layoutItemFormulaTypeComboBox,
            this.layoutItemAddFormula,
            this.layoutItemCloneFormula,
            this.layoutItemFormulaName,
            this.layoutItemFormulaTypeTextEdit});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(880, 457);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemFormulaView
         // 
         this.layoutItemFormulaView.Control = this.pnlEdit;
         this.layoutItemFormulaView.CustomizationFormText = "layoutControlItem1";
         this.layoutItemFormulaView.Location = new System.Drawing.Point(0, 96);
         this.layoutItemFormulaView.Name = "layoutItemFormulaView";
         this.layoutItemFormulaView.Size = new System.Drawing.Size(880, 361);
         this.layoutItemFormulaView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemFormulaView.TextVisible = false;
         // 
         // layoutItemFormulaSelect
         // 
         this.layoutItemFormulaSelect.Control = this.cbFormulaName;
         this.layoutItemFormulaSelect.CustomizationFormText = "layoutItemExplicitFormulaName";
         this.layoutItemFormulaSelect.Location = new System.Drawing.Point(0, 48);
         this.layoutItemFormulaSelect.Name = "layoutItemFormulaSelect";
         this.layoutItemFormulaSelect.Size = new System.Drawing.Size(440, 24);
         this.layoutItemFormulaSelect.TextSize = new System.Drawing.Size(165, 13);
         // 
         // layoutItemFormulaTypeComboBox
         // 
         this.layoutItemFormulaTypeComboBox.Control = this.cbFormulaType;
         this.layoutItemFormulaTypeComboBox.CustomizationFormText = "layoutItemFormulaType";
         this.layoutItemFormulaTypeComboBox.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFormulaTypeComboBox.Name = "layoutItemFormulaTypeComboBox";
         this.layoutItemFormulaTypeComboBox.Size = new System.Drawing.Size(880, 24);
         this.layoutItemFormulaTypeComboBox.TextSize = new System.Drawing.Size(165, 13);
         // 
         // layoutItemAddFormula
         // 
         this.layoutItemAddFormula.Control = this.btnAddFormula;
         this.layoutItemAddFormula.CustomizationFormText = "layoutControlItem2";
         this.layoutItemAddFormula.Location = new System.Drawing.Point(440, 48);
         this.layoutItemAddFormula.Name = "layoutItemAddFormula";
         this.layoutItemAddFormula.Size = new System.Drawing.Size(220, 48);
         this.layoutItemAddFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddFormula.TextVisible = false;
         // 
         // layoutItemCloneFormula
         // 
         this.layoutItemCloneFormula.Control = this.btnCloneFormula;
         this.layoutItemCloneFormula.CustomizationFormText = "layoutControlItem3";
         this.layoutItemCloneFormula.Location = new System.Drawing.Point(660, 48);
         this.layoutItemCloneFormula.Name = "layoutItemCloneFormula";
         this.layoutItemCloneFormula.Size = new System.Drawing.Size(220, 48);
         this.layoutItemCloneFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCloneFormula.TextVisible = false;
         // 
         // layoutItemFormulaName
         // 
         this.layoutItemFormulaName.Control = this.tbFormulaName;
         this.layoutItemFormulaName.Location = new System.Drawing.Point(0, 72);
         this.layoutItemFormulaName.Name = "layoutItemFormulaName";
         this.layoutItemFormulaName.Size = new System.Drawing.Size(440, 24);
         this.layoutItemFormulaName.TextSize = new System.Drawing.Size(165, 13);
         // 
         // tbFormulaType
         // 
         this.tbFormulaType.Location = new System.Drawing.Point(179, 26);
         this.tbFormulaType.Name = "tbFormulaType";
         this.tbFormulaType.Size = new System.Drawing.Size(699, 20);
         this.tbFormulaType.StyleController = this.layoutControl;
         this.tbFormulaType.TabIndex = 8;
         // 
         // layoutItemFormulaTypeTextEdit
         // 
         this.layoutItemFormulaTypeTextEdit.Control = this.tbFormulaType;
         this.layoutItemFormulaTypeTextEdit.Location = new System.Drawing.Point(0, 24);
         this.layoutItemFormulaTypeTextEdit.Name = "layoutItemFormulaTypeTextEdit";
         this.layoutItemFormulaTypeTextEdit.Size = new System.Drawing.Size(880, 24);
         this.layoutItemFormulaTypeTextEdit.TextSize = new System.Drawing.Size(165, 13);
         // 
         // EditFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditFormulaView";
         this.Size = new System.Drawing.Size(880, 457);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlEdit)).EndInit();
         this.pnlEdit.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula.Panel1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula.Panel2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula)).EndInit();
         this.splitFormula.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbFormulaName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaSelect)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaTypeComboBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCloneFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbFormulaType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaTypeTextEdit)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl pnlEdit;
      protected OSPSuite.UI.Controls.UxComboBoxEdit cbFormulaName;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbFormulaType;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaTypeComboBox;
      private DevExpress.XtraEditors.SplitContainerControl splitFormula;
      private DevExpress.XtraEditors.SimpleButton btnCloneFormula;
      protected DevExpress.XtraEditors.SimpleButton btnAddFormula;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      protected DevExpress.XtraEditors.TextEdit tbFormulaName;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaName;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaSelect;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemAddFormula;
      protected DevExpress.XtraLayout.LayoutControlItem layoutItemCloneFormula;
      private DevExpress.XtraEditors.TextEdit tbFormulaType;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaTypeTextEdit;
   }
}
