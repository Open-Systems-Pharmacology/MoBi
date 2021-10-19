using OSPSuite.UI.Controls;
using DevExpress.XtraGrid;

namespace MoBi.UI.Views
{
   partial class EditMoleculeBuilderView
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
         this.components = new System.ComponentModel.Container();
         this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControlProperties = new OSPSuite.UI.Controls.UxLayoutControl();
         this.cbMoleculeType = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.grpFormula = new DevExpress.XtraEditors.GroupControl();
         this.gridControlCalcualtionMethod = new OSPSuite.UI.Controls.UxGridControl();
         this.grdCalculationMethodsView = new MoBi.UI.Views.UxGridView();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.chkIsFloating = new UxCheckEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemIsFloating = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCalculationMethod = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemFormula = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemMoleculeType = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabParameter = new DevExpress.XtraTab.XtraTabPage();
         this._toolTipController = new DevExpress.Utils.ToolTipController(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlProperties)).BeginInit();
         this.layoutControlProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbMoleculeType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grpFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlCalcualtionMethod)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdCalculationMethodsView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkIsFloating.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemIsFloating)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCalculationMethod)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMoleculeType)).BeginInit();
         this.SuspendLayout();
         // 
         // xtraTabControl1
         // 
         this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
         this.xtraTabControl1.Name = "xtraTabControl1";
         this.xtraTabControl1.SelectedTabPage = this.tabProperties;
         this.xtraTabControl1.Size = new System.Drawing.Size(706, 587);
         this.xtraTabControl1.TabIndex = 0;
         this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabParameter});
         // 
         // tabProperties
         // 
         this.tabProperties.Controls.Add(this.layoutControlProperties);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(700, 559);
         this.tabProperties.Text = "Properties";
         // 
         // layoutControlProperties
         // 
         this.layoutControlProperties.Controls.Add(this.cbMoleculeType);
         this.layoutControlProperties.Controls.Add(this.grpFormula);
         this.layoutControlProperties.Controls.Add(this.gridControlCalcualtionMethod);
         this.layoutControlProperties.Controls.Add(this.btName);
         this.layoutControlProperties.Controls.Add(this.chkIsFloating);
         this.layoutControlProperties.Controls.Add(this.htmlEditor);
         this.layoutControlProperties.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControlProperties.Location = new System.Drawing.Point(0, 0);
         this.layoutControlProperties.Name = "layoutControlProperties";
         this.layoutControlProperties.Root = this.layoutControlGroup1;
         this.layoutControlProperties.Size = new System.Drawing.Size(700, 559);
         this.layoutControlProperties.TabIndex = 0;
         this.layoutControlProperties.Text = "layoutControlProperties";
         // 
         // cbMoleculeType
         // 
         this.cbMoleculeType.Location = new System.Drawing.Point(534, 36);
         this.cbMoleculeType.Name = "cbMoleculeType";
         this.cbMoleculeType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbMoleculeType.Size = new System.Drawing.Size(154, 20);
         this.cbMoleculeType.StyleController = this.layoutControlProperties;
         this.cbMoleculeType.TabIndex = 10;
         // 
         // grpFormula
         // 
         this.grpFormula.Location = new System.Drawing.Point(12, 60);
         this.grpFormula.Name = "grpFormula";
         this.grpFormula.Size = new System.Drawing.Size(676, 353);
         this.grpFormula.TabIndex = 9;
         this.grpFormula.Text = "grpFormula";
         // 
         // gridControlCalcualtionMethod
         // 
         this.gridControlCalcualtionMethod.Location = new System.Drawing.Point(12, 433);
         this.gridControlCalcualtionMethod.MainView = this.grdCalculationMethodsView;
         this.gridControlCalcualtionMethod.Name = "gridControlCalcualtionMethod";
         this.gridControlCalcualtionMethod.Size = new System.Drawing.Size(676, 90);
         this.gridControlCalcualtionMethod.TabIndex = 7;
         this.gridControlCalcualtionMethod.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdCalculationMethodsView});
         // 
         // grdCalculationMethodsView
         // 
         this.grdCalculationMethodsView.GridControl = this.gridControlCalcualtionMethod;
         this.grdCalculationMethodsView.Name = "grdCalculationMethodsView";
         // 
         // btName
         // 
         this.btName.Location = new System.Drawing.Point(131, 12);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(557, 20);
         this.btName.StyleController = this.layoutControlProperties;
         this.btName.TabIndex = 4;
         // 
         // chkIsFloating
         // 
         this.chkIsFloating.Location = new System.Drawing.Point(12, 36);
         this.chkIsFloating.Name = "chkIsFloating";
         this.chkIsFloating.Properties.Caption = "checkEdit1";
         this.chkIsFloating.Size = new System.Drawing.Size(361, 19);
         this.chkIsFloating.StyleController = this.layoutControlProperties;
         this.chkIsFloating.TabIndex = 5;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Location = new System.Drawing.Point(157, 527);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Size = new System.Drawing.Size(531, 20);
         this.htmlEditor.StyleController = this.layoutControlProperties;
         this.htmlEditor.TabIndex = 8;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemIsFloating,
            this.layoutControlItemName,
            this.layoutControlItemCalculationMethod,
            this.layoutControlItemDescription,
            this.layoutControlItemFormula,
            this.layoutControlItemMoleculeType});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(700, 559);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemIsFloating
         // 
         this.layoutControlItemIsFloating.Control = this.chkIsFloating;
         this.layoutControlItemIsFloating.CustomizationFormText = "layoutControlItemIsFloating";
         this.layoutControlItemIsFloating.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemIsFloating.Name = "layoutControlItem1";
         this.layoutControlItemIsFloating.Size = new System.Drawing.Size(365, 24);
         this.layoutControlItemIsFloating.Text = "layoutControlItemIsFloating";
         this.layoutControlItemIsFloating.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemIsFloating.TextToControlDistance = 0;
         this.layoutControlItemIsFloating.TextVisible = false;
         // 
         // layoutControlItemName
         // 
         this.layoutControlItemName.Control = this.btName;
         this.layoutControlItemName.CustomizationFormText = "layoutControlItemName";
         this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemName.Name = "layoutControlItemName";
         this.layoutControlItemName.Size = new System.Drawing.Size(680, 24);
         this.layoutControlItemName.Text = "layoutControlItemName";
         this.layoutControlItemName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutControlItemName.TextSize = new System.Drawing.Size(114, 13);
         this.layoutControlItemName.TextToControlDistance = 5;
         // 
         // layoutControlItemCalculationMethod
         // 
         this.layoutControlItemCalculationMethod.Control = this.gridControlCalcualtionMethod;
         this.layoutControlItemCalculationMethod.CustomizationFormText = "layoutControlItemCalculationMethod";
         this.layoutControlItemCalculationMethod.Location = new System.Drawing.Point(0, 405);
         this.layoutControlItemCalculationMethod.Name = "layoutControlItemCalculationMethod";
         this.layoutControlItemCalculationMethod.Size = new System.Drawing.Size(680, 110);
         this.layoutControlItemCalculationMethod.Text = "layoutControlItemCalculationMethod";
         this.layoutControlItemCalculationMethod.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemCalculationMethod.TextSize = new System.Drawing.Size(175, 13);
         // 
         // layoutControlItemDescription
         // 
         this.layoutControlItemDescription.Control = this.htmlEditor;
         this.layoutControlItemDescription.CustomizationFormText = "layoutControlItemDescription";
         this.layoutControlItemDescription.Location = new System.Drawing.Point(0, 515);
         this.layoutControlItemDescription.Name = "layoutControlItemDescription";
         this.layoutControlItemDescription.Size = new System.Drawing.Size(680, 24);
         this.layoutControlItemDescription.Text = "layoutControlItemDescription";
         this.layoutControlItemDescription.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutControlItemDescription.TextSize = new System.Drawing.Size(140, 13);
         this.layoutControlItemDescription.TextToControlDistance = 5;
         // 
         // layoutControlItemFormula
         // 
         this.layoutControlItemFormula.Control = this.grpFormula;
         this.layoutControlItemFormula.CustomizationFormText = "layoutControlItemFormula";
         this.layoutControlItemFormula.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItemFormula.Name = "layoutControlItem1";
         this.layoutControlItemFormula.Size = new System.Drawing.Size(680, 357);
         this.layoutControlItemFormula.Text = "layoutControlItemFormula";
         this.layoutControlItemFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemFormula.TextToControlDistance = 0;
         this.layoutControlItemFormula.TextVisible = false;
         // 
         // layoutControlItemMoleculeType
         // 
         this.layoutControlItemMoleculeType.Control = this.cbMoleculeType;
         this.layoutControlItemMoleculeType.CustomizationFormText = "layoutControlItemMoleculeType";
         this.layoutControlItemMoleculeType.Location = new System.Drawing.Point(365, 24);
         this.layoutControlItemMoleculeType.Name = "layoutControlItemMoleculeType";
         this.layoutControlItemMoleculeType.Size = new System.Drawing.Size(315, 24);
         this.layoutControlItemMoleculeType.Text = "layoutControlItemMoleculeType";
         this.layoutControlItemMoleculeType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutControlItemMoleculeType.TextSize = new System.Drawing.Size(152, 13);
         this.layoutControlItemMoleculeType.TextToControlDistance = 5;
         // 
         // tabParameter
         // 
         this.tabParameter.Name = "tabParameter";
         this.tabParameter.Size = new System.Drawing.Size(700, 559);
         this.tabParameter.Text = "Parameters";
         // 
         // EditMoleculeBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.xtraTabControl1);
         this.Name = "EditMoleculeBuilderView";
         this.Size = new System.Drawing.Size(706, 587);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
         this.xtraTabControl1.ResumeLayout(false);
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlProperties)).EndInit();
         this.layoutControlProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbMoleculeType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grpFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControlCalcualtionMethod)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdCalculationMethodsView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkIsFloating.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemIsFloating)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCalculationMethod)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMoleculeType)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
      private DevExpress.XtraTab.XtraTabPage tabParameter;
      private DevExpress.XtraTab.XtraTabPage tabProperties;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControlProperties;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
      private DevExpress.XtraEditors.CheckEdit chkIsFloating;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemIsFloating;
      private DevExpress.XtraGrid.GridControl gridControlCalcualtionMethod;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCalculationMethod;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDescription;
      private DevExpress.XtraEditors.GroupControl grpFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFormula;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbMoleculeType;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMoleculeType;
      private UxGridView grdCalculationMethodsView;
      private DevExpress.Utils.ToolTipController _toolTipController;
   }
}
