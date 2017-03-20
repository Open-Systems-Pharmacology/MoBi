namespace MoBi.UI.Views
{
   partial class EditSumFormulaView
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
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.lblFormula = new DevExpress.XtraEditors.LabelControl();
         this.txtVariableName = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemVariableName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemFormulaString = new DevExpress.XtraLayout.LayoutControlItem();
         this._barManager = new DevExpress.XtraBars.BarManager();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.panelCriteria = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlItemCriteria = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtVariableName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVariableName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormulaString)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCriteria)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCriteria)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.panelCriteria);
         this.layoutControl1.Controls.Add(this.lblFormula);
         this.layoutControl1.Controls.Add(this.txtVariableName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(645, 479);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // lblFormula
         // 
         this.lblFormula.Location = new System.Drawing.Point(12, 454);
         this.lblFormula.Name = "lblFormula";
         this.lblFormula.Size = new System.Drawing.Size(48, 13);
         this.lblFormula.StyleController = this.layoutControl1;
         this.lblFormula.TabIndex = 6;
         this.lblFormula.Text = "lblFormula";
         // 
         // txtVariableName
         // 
         this.txtVariableName.Location = new System.Drawing.Point(167, 12);
         this.txtVariableName.Name = "txtVariableName";
         this.txtVariableName.Size = new System.Drawing.Size(466, 20);
         this.txtVariableName.StyleController = this.layoutControl1;
         this.txtVariableName.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemVariableName,
            this.layoutControlItemFormulaString,
            this.layoutControlItemCriteria});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(645, 479);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemVariableName
         // 
         this.layoutControlItemVariableName.Control = this.txtVariableName;
         this.layoutControlItemVariableName.CustomizationFormText = "layoutControlItemVariableName";
         this.layoutControlItemVariableName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemVariableName.Name = "layoutControlItemVariableName";
         this.layoutControlItemVariableName.Size = new System.Drawing.Size(625, 24);
         this.layoutControlItemVariableName.Text = "layoutControlItemVariableName";
         this.layoutControlItemVariableName.TextSize = new System.Drawing.Size(152, 13);
         // 
         // layoutControlItemFormulaString
         // 
         this.layoutControlItemFormulaString.Control = this.lblFormula;
         this.layoutControlItemFormulaString.CustomizationFormText = "layoutControlItemFormulaString";
         this.layoutControlItemFormulaString.Location = new System.Drawing.Point(0, 442);
         this.layoutControlItemFormulaString.Name = "layoutControlItemFormulaString";
         this.layoutControlItemFormulaString.Size = new System.Drawing.Size(625, 17);
         this.layoutControlItemFormulaString.Text = "layoutControlItemFormulaString";
         this.layoutControlItemFormulaString.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemFormulaString.TextToControlDistance = 0;
         this.layoutControlItemFormulaString.TextVisible = false;
         // 
         // _barManager
         // 
         this._barManager.DockControls.Add(this.barDockControlTop);
         this._barManager.DockControls.Add(this.barDockControlBottom);
         this._barManager.DockControls.Add(this.barDockControlLeft);
         this._barManager.DockControls.Add(this.barDockControlRight);
         this._barManager.Form = this;
         this._barManager.MaxItemId = 0;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(645, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 479);
         this.barDockControlBottom.Size = new System.Drawing.Size(645, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 479);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(645, 0);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 479);
         // 
         // panelControl1
         // 
         this.panelCriteria.Location = new System.Drawing.Point(12, 52);
         this.panelCriteria.Name = "panelCriteria";
         this.panelCriteria.Size = new System.Drawing.Size(621, 398);
         this.panelCriteria.TabIndex = 7;
         // 
         // layoutControlItemCriteria
         // 
         this.layoutControlItemCriteria.Control = this.panelCriteria;
         this.layoutControlItemCriteria.CustomizationFormText = "layoutControlItemCriteria";
         this.layoutControlItemCriteria.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemCriteria.Name = "layoutControlItemCriteria";
         this.layoutControlItemCriteria.Size = new System.Drawing.Size(625, 418);
         this.layoutControlItemCriteria.Text = "layoutControlItemCriteria";
         this.layoutControlItemCriteria.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemCriteria.TextSize = new System.Drawing.Size(152, 13);
         // 
         // EditSumFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "EditSumFormulaView";
         this.Size = new System.Drawing.Size(645, 479);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.txtVariableName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVariableName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFormulaString)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCriteria)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCriteria)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.TextEdit txtVariableName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemVariableName;
      private DevExpress.XtraBars.BarManager _barManager;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraEditors.LabelControl lblFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFormulaString;
      private DevExpress.XtraEditors.PanelControl panelCriteria;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCriteria;
   }
}
