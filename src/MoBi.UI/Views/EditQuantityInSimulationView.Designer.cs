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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnGoToSource = new OSPSuite.UI.Controls.UxSimpleButton();
         this.sourceTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.btnResetToFormulaValue = new DevExpress.XtraEditors.SimpleButton();
         this.pnlFormula = new DevExpress.XtraEditors.PanelControl();
         this.valueEdit = new MoBi.UI.Views.ValueEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemValue = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemReset = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemGoToSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
         this.tabControl.SuspendLayout();
         this.tabValue.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.sourceTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValue)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemReset)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGoToSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).BeginInit();
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
         // tabProperties
         // 
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(1248, 378);
         this.tabProperties.Text = "tabProperties";
         // 
         // tabValue
         // 
         this.tabValue.Controls.Add(this.layoutControl);
         this.tabValue.Name = "tabValue";
         this.tabValue.Size = new System.Drawing.Size(1248, 378);
         this.tabValue.Text = "tabValue";
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.btnGoToSource);
         this.layoutControl.Controls.Add(this.sourceTextEdit);
         this.layoutControl.Controls.Add(this.btnResetToFormulaValue);
         this.layoutControl.Controls.Add(this.pnlFormula);
         this.layoutControl.Controls.Add(this.valueEdit);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(1248, 378);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnGoToSource
         // 
         this.btnGoToSource.Location = new System.Drawing.Point(1141, 38);
         this.btnGoToSource.Manager = null;
         this.btnGoToSource.Name = "btnGoToSource";
         this.btnGoToSource.Shortcut = System.Windows.Forms.Keys.None;
         this.btnGoToSource.Size = new System.Drawing.Size(85, 22);
         this.btnGoToSource.StyleController = this.layoutControl;
         this.btnGoToSource.TabIndex = 7;
         this.btnGoToSource.Text = "btnGoToSource";
         // 
         // sourceTextEdit
         // 
         this.sourceTextEdit.Location = new System.Drawing.Point(149, 38);
         this.sourceTextEdit.Name = "sourceTextEdit";
         this.sourceTextEdit.Size = new System.Drawing.Size(988, 20);
         this.sourceTextEdit.StyleController = this.layoutControl;
         this.sourceTextEdit.TabIndex = 6;
         // 
         // btnResetToFormulaValue
         // 
         this.btnResetToFormulaValue.Location = new System.Drawing.Point(621, 12);
         this.btnResetToFormulaValue.Name = "btnResetToFormulaValue";
         this.btnResetToFormulaValue.Size = new System.Drawing.Size(605, 22);
         this.btnResetToFormulaValue.StyleController = this.layoutControl;
         this.btnResetToFormulaValue.TabIndex = 5;
         this.btnResetToFormulaValue.Text = "btnResetToFormulaValue";
         // 
         // pnlFormula
         // 
         this.pnlFormula.Location = new System.Drawing.Point(12, 80);
         this.pnlFormula.Name = "pnlFormula";
         this.pnlFormula.Size = new System.Drawing.Size(1224, 286);
         this.pnlFormula.TabIndex = 4;
         // 
         // valueEdit
         // 
         this.valueEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.valueEdit.Caption = "";
         this.valueEdit.Location = new System.Drawing.Point(149, 12);
         this.valueEdit.MaximumSize = new System.Drawing.Size(200000, 20);
         this.valueEdit.Name = "valueEdit";
         this.valueEdit.Size = new System.Drawing.Size(468, 20);
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
            this.layoutControlItemReset,
            this.layoutControlItemSource,
            this.layoutControlItemGoToSource,
            this.emptySpaceItem});
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(1248, 378);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemFormula
         // 
         this.layoutControlItemFormula.Control = this.pnlFormula;
         this.layoutControlItemFormula.CustomizationFormText = "layoutControlItemFormula";
         this.layoutControlItemFormula.Location = new System.Drawing.Point(0, 52);
         this.layoutControlItemFormula.Name = "layoutControlItemSource";
         this.layoutControlItemFormula.Size = new System.Drawing.Size(1228, 306);
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
         this.layoutControlItemValue.Size = new System.Drawing.Size(609, 26);
         this.layoutControlItemValue.Text = "layoutControlItemValue";
         this.layoutControlItemValue.TextSize = new System.Drawing.Size(125, 13);
         // 
         // layoutControlItemReset
         // 
         this.layoutControlItemReset.Control = this.btnResetToFormulaValue;
         this.layoutControlItemReset.CustomizationFormText = "layoutControlItemReset";
         this.layoutControlItemReset.Location = new System.Drawing.Point(609, 0);
         this.layoutControlItemReset.Name = "layoutControlItemReset";
         this.layoutControlItemReset.Size = new System.Drawing.Size(609, 26);
         this.layoutControlItemReset.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemReset.TextVisible = false;
         // 
         // layoutControlItemSource
         // 
         this.layoutControlItemSource.Control = this.sourceTextEdit;
         this.layoutControlItemSource.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemSource.Name = "item0";
         this.layoutControlItemSource.Size = new System.Drawing.Size(1129, 26);
         this.layoutControlItemSource.Text = "layoutControlItemSource";
         this.layoutControlItemSource.TextSize = new System.Drawing.Size(125, 13);
         // 
         // layoutControlItemGoToSource
         // 
         this.layoutControlItemGoToSource.Control = this.btnGoToSource;
         this.layoutControlItemGoToSource.Location = new System.Drawing.Point(1129, 26);
         this.layoutControlItemGoToSource.Name = "layoutControlItemGoToSource";
         this.layoutControlItemGoToSource.Size = new System.Drawing.Size(89, 26);
         this.layoutControlItemGoToSource.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemGoToSource.TextVisible = false;
         // 
         // emptySpaceItem
         // 
         this.emptySpaceItem.AllowHotTrack = false;
         this.emptySpaceItem.Location = new System.Drawing.Point(1218, 0);
         this.emptySpaceItem.Name = "emptySpaceItem";
         this.emptySpaceItem.Size = new System.Drawing.Size(10, 52);
         this.emptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         // 
         // tabParameters
         // 
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.Size = new System.Drawing.Size(1248, 378);
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
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.sourceTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValue)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemReset)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemGoToSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem)).EndInit();
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
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private OSPSuite.UI.Controls.UxSimpleButton btnGoToSource;
      private DevExpress.XtraEditors.TextEdit sourceTextEdit;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGoToSource;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem;
   }
}
