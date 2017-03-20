namespace MoBi.UI.Views
{
   partial class EditFormulaView
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
         this.cbExplicitFormulaName = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnCloneFormula = new DevExpress.XtraEditors.SimpleButton();
         this.btnAddFormula = new DevExpress.XtraEditors.SimpleButton();
         this.cbFormulaType = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemFormulaView = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemExplicitFormulaName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFormulaType = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAddFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCloneFormula = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.pnlEdit)).BeginInit();
         this.pnlEdit.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula)).BeginInit();
         this.splitFormula.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbExplicitFormulaName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExplicitFormulaName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaType)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCloneFormula)).BeginInit();
         this.SuspendLayout();
         // 
         // pnlEdit
         // 
         this.pnlEdit.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.pnlEdit.Controls.Add(this.splitFormula);
         this.pnlEdit.Location = new System.Drawing.Point(2, 52);
         this.pnlEdit.Name = "pnlEdit";
         this.pnlEdit.Size = new System.Drawing.Size(876, 403);
         this.pnlEdit.TabIndex = 1;
         // 
         // splitFormula
         // 
         this.splitFormula.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitFormula.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
         this.splitFormula.Location = new System.Drawing.Point(2, 2);
         this.splitFormula.Name = "splitFormula";
         this.splitFormula.Panel1.Text = "Panel1";
         this.splitFormula.Panel2.Text = "Panel2";
         this.splitFormula.Size = new System.Drawing.Size(872, 399);
         this.splitFormula.SplitterPosition = 436;
         this.splitFormula.TabIndex = 0;
         this.splitFormula.Text = "splitContainerControl1";
         // 
         // cbExplicitFormulaName
         // 
         this.cbExplicitFormulaName.Location = new System.Drawing.Point(156, 26);
         this.cbExplicitFormulaName.Name = "cbExplicitFormulaName";
         this.cbExplicitFormulaName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbExplicitFormulaName.Size = new System.Drawing.Size(282, 20);
         this.cbExplicitFormulaName.StyleController = this.layoutControl1;
         this.cbExplicitFormulaName.TabIndex = 3;
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.btnCloneFormula);
         this.layoutControl1.Controls.Add(this.btnAddFormula);
         this.layoutControl1.Controls.Add(this.cbFormulaType);
         this.layoutControl1.Controls.Add(this.pnlEdit);
         this.layoutControl1.Controls.Add(this.cbExplicitFormulaName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(880, 457);
         this.layoutControl1.TabIndex = 5;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // btnCloneFormula
         // 
         this.btnCloneFormula.Location = new System.Drawing.Point(662, 26);
         this.btnCloneFormula.Name = "btnCloneFormula";
         this.btnCloneFormula.Size = new System.Drawing.Size(216, 22);
         this.btnCloneFormula.StyleController = this.layoutControl1;
         this.btnCloneFormula.TabIndex = 6;
         this.btnCloneFormula.Text = "btnCloneFormula";
         // 
         // btnAddFormula
         // 
         this.btnAddFormula.Location = new System.Drawing.Point(442, 26);
         this.btnAddFormula.Name = "btnAddFormula";
         this.btnAddFormula.Size = new System.Drawing.Size(216, 22);
         this.btnAddFormula.StyleController = this.layoutControl1;
         this.btnAddFormula.TabIndex = 5;
         this.btnAddFormula.Text = "btnAddFormula";
         // 
         // cbFormulaType
         // 
         this.cbFormulaType.Location = new System.Drawing.Point(156, 2);
         this.cbFormulaType.Name = "cbFormulaType";
         this.cbFormulaType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFormulaType.Size = new System.Drawing.Size(722, 20);
         this.cbFormulaType.StyleController = this.layoutControl1;
         this.cbFormulaType.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFormulaView,
            this.layoutItemExplicitFormulaName,
            this.layoutItemFormulaType,
            this.layoutItemAddFormula,
            this.layoutItemCloneFormula});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(880, 457);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemFormulaView
         // 
         this.layoutItemFormulaView.Control = this.pnlEdit;
         this.layoutItemFormulaView.CustomizationFormText = "layoutControlItem1";
         this.layoutItemFormulaView.Location = new System.Drawing.Point(0, 50);
         this.layoutItemFormulaView.Name = "layoutItemFormulaView";
         this.layoutItemFormulaView.Size = new System.Drawing.Size(880, 407);
         this.layoutItemFormulaView.Text = "layoutItemFormulaView";
         this.layoutItemFormulaView.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemFormulaView.TextToControlDistance = 0;
         this.layoutItemFormulaView.TextVisible = false;
         // 
         // layoutItemExplicitFormulaName
         // 
         this.layoutItemExplicitFormulaName.Control = this.cbExplicitFormulaName;
         this.layoutItemExplicitFormulaName.CustomizationFormText = "layoutItemExplicitFormulaName";
         this.layoutItemExplicitFormulaName.Location = new System.Drawing.Point(0, 24);
         this.layoutItemExplicitFormulaName.Name = "layoutItemExplicitFormulaName";
         this.layoutItemExplicitFormulaName.Size = new System.Drawing.Size(440, 26);
         this.layoutItemExplicitFormulaName.Text = "layoutItemExplicitFormulaName";
         this.layoutItemExplicitFormulaName.TextSize = new System.Drawing.Size(150, 13);
         // 
         // layoutItemFormulaType
         // 
         this.layoutItemFormulaType.Control = this.cbFormulaType;
         this.layoutItemFormulaType.CustomizationFormText = "layoutItemFormulaType";
         this.layoutItemFormulaType.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFormulaType.Name = "layoutItemFormulaType";
         this.layoutItemFormulaType.Size = new System.Drawing.Size(880, 24);
         this.layoutItemFormulaType.Text = "layoutItemFormulaType";
         this.layoutItemFormulaType.TextSize = new System.Drawing.Size(150, 13);
         // 
         // layoutItemAddFormula
         // 
         this.layoutItemAddFormula.Control = this.btnAddFormula;
         this.layoutItemAddFormula.CustomizationFormText = "layoutControlItem2";
         this.layoutItemAddFormula.Location = new System.Drawing.Point(440, 24);
         this.layoutItemAddFormula.Name = "layoutItemAddFormula";
         this.layoutItemAddFormula.Size = new System.Drawing.Size(220, 26);
         this.layoutItemAddFormula.Text = "layoutItemAddFormula";
         this.layoutItemAddFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddFormula.TextToControlDistance = 0;
         this.layoutItemAddFormula.TextVisible = false;
         // 
         // layoutItemCloneFormula
         // 
         this.layoutItemCloneFormula.Control = this.btnCloneFormula;
         this.layoutItemCloneFormula.CustomizationFormText = "layoutControlItem3";
         this.layoutItemCloneFormula.Location = new System.Drawing.Point(660, 24);
         this.layoutItemCloneFormula.Name = "layoutItemCloneFormula";
         this.layoutItemCloneFormula.Size = new System.Drawing.Size(220, 26);
         this.layoutItemCloneFormula.Text = "layoutItemCloneFormula";
         this.layoutItemCloneFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCloneFormula.TextToControlDistance = 0;
         this.layoutItemCloneFormula.TextVisible = false;
         // 
         // EditFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "EditFormulaView";
         this.Size = new System.Drawing.Size(880, 457);
         ((System.ComponentModel.ISupportInitialize)(this.pnlEdit)).EndInit();
         this.pnlEdit.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitFormula)).EndInit();
         this.splitFormula.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbExplicitFormulaName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExplicitFormulaName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaType)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCloneFormula)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl pnlEdit;
      private DevExpress.XtraEditors.ComboBoxEdit cbExplicitFormulaName;
      private DevExpress.XtraEditors.ComboBoxEdit cbFormulaType;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaView;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemExplicitFormulaName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaType;
      private DevExpress.XtraEditors.SplitContainerControl splitFormula;
      private DevExpress.XtraEditors.SimpleButton btnCloneFormula;
      private DevExpress.XtraEditors.SimpleButton btnAddFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCloneFormula;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
   }
}
