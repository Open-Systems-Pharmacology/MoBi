namespace MoBi.UI.Views
{
   partial class EditQuantityInSimulationView
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
         this.tabControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.tabValue = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnResetToFormulaValue = new DevExpress.XtraEditors.SimpleButton();
         this.pnlFormula = new DevExpress.XtraEditors.PanelControl();
         this.valueEdit = new MoBi.UI.Views.ValueEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemValue = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemReset = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
         this.tabControl.SuspendLayout();
         this.tabValue.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pnlFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValue)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemReset)).BeginInit();
         this.SuspendLayout();
         // 
         // tabControl
         // 
         this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabControl.Location = new System.Drawing.Point(0, 0);
         this.tabControl.Name = "tabControl";
         this.tabControl.SelectedTabPage = this.tabProperties;
         this.tabControl.Size = new System.Drawing.Size(1250, 403);
         this.tabControl.TabIndex = 0;
         this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabValue,
            this.tabParameters});
         // 
         // tabInfo
         // 
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(1244, 375);
         this.tabProperties.Text = "tabProperties";
         // 
         // tabValue
         // 
         this.tabValue.Controls.Add(this.layoutControl1);
         this.tabValue.Name = "tabValue";
         this.tabValue.Size = new System.Drawing.Size(1244, 375);
         this.tabValue.Text = "tabValue";
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.btnResetToFormulaValue);
         this.layoutControl1.Controls.Add(this.pnlFormula);
         this.layoutControl1.Controls.Add(this.valueEdit);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(1244, 375);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // btnResetToFormulaValue
         // 
         this.btnResetToFormulaValue.Location = new System.Drawing.Point(624, 12);
         this.btnResetToFormulaValue.Name = "btnResetToFormulaValue";
         this.btnResetToFormulaValue.Size = new System.Drawing.Size(608, 22);
         this.btnResetToFormulaValue.StyleController = this.layoutControl1;
         this.btnResetToFormulaValue.TabIndex = 5;
         this.btnResetToFormulaValue.Text = "btnResetToFormulaValue";
         // 
         // pnlFormula
         // 
         this.pnlFormula.Location = new System.Drawing.Point(12, 54);
         this.pnlFormula.Name = "pnlFormula";
         this.pnlFormula.Size = new System.Drawing.Size(1220, 309);
         this.pnlFormula.TabIndex = 4;
         // 
         // valueEdit
         // 
         this.valueEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.valueEdit.Caption = "";
         this.valueEdit.Location = new System.Drawing.Point(140, 12);
         this.valueEdit.MaximumSize = new System.Drawing.Size(200000, 20);
         this.valueEdit.Name = "valueEdit";
         this.valueEdit.Size = new System.Drawing.Size(480, 20);
         this.valueEdit.TabIndex = 2;
         this.valueEdit.ToolTip = "";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemFormula,
            this.layoutControlItemValue,
            this.layoutControlItemReset});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(1244, 375);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemFormula
         // 
         this.layoutControlItemFormula.Control = this.pnlFormula;
         this.layoutControlItemFormula.CustomizationFormText = "layoutControlItemFormula";
         this.layoutControlItemFormula.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemFormula.Name = "layoutControlItem1";
         this.layoutControlItemFormula.Size = new System.Drawing.Size(1224, 329);
         this.layoutControlItemFormula.Text = "layoutControlItemFormula";
         this.layoutControlItemFormula.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemFormula.TextSize = new System.Drawing.Size(125, 13);
         // 
         // layoutControlItemValue
         // 
         this.layoutControlItemValue.Control = this.valueEdit;
         this.layoutControlItemValue.CustomizationFormText = "layoutControlItemValue";
         this.layoutControlItemValue.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemValue.Name = "layoutControlItemVlaue";
         this.layoutControlItemValue.Size = new System.Drawing.Size(612, 26);
         this.layoutControlItemValue.Text = "layoutControlItemValue";
         this.layoutControlItemValue.TextSize = new System.Drawing.Size(125, 13);
         // 
         // layoutControlItemReset
         // 
         this.layoutControlItemReset.Control = this.btnResetToFormulaValue;
         this.layoutControlItemReset.CustomizationFormText = "layoutControlItemReset";
         this.layoutControlItemReset.Location = new System.Drawing.Point(612, 0);
         this.layoutControlItemReset.Name = "layoutControlItemReset";
         this.layoutControlItemReset.Size = new System.Drawing.Size(612, 26);
         this.layoutControlItemReset.Text = "layoutControlItemReset";
         this.layoutControlItemReset.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemReset.TextToControlDistance = 0;
         this.layoutControlItemReset.TextVisible = false;
         // 
         // tabParameters
         // 
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.Size = new System.Drawing.Size(1244, 375);
         this.tabParameters.Text = "Parameters";
         // 
         // EditQuantityInSimulationView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabControl);
         this.Name = "EditQuantityInSimulationView";
         this.Size = new System.Drawing.Size(1250, 403);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
         this.tabControl.ResumeLayout(false);
         this.tabValue.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pnlFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValue)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemReset)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl tabControl;
      private DevExpress.XtraTab.XtraTabPage tabValue;
      private DevExpress.XtraTab.XtraTabPage tabProperties;
      private DevExpress.XtraTab.XtraTabPage tabParameters;
      private DevExpress.XtraEditors.PanelControl pnlFormula;
      private ValueEdit valueEdit;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemValue;
      private DevExpress.XtraEditors.SimpleButton btnResetToFormulaValue;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemReset;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
   }
}
