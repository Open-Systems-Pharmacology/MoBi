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
         this.components = new System.ComponentModel.Container();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.txtFormulaString = new DevExpress.XtraEditors.TextEdit();
         this._barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.panelFormulaUsablePath = new DevExpress.XtraEditors.PanelControl();
         this.panelCriteria = new DevExpress.XtraEditors.PanelControl();
         this.txtVariableName = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemVariableName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCriteria = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFormulaString = new DevExpress.XtraLayout.LayoutControlItem();
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.txtFormulaString.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormulaUsablePath)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCriteria)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtVariableName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVariableName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCriteria)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaString)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.lblDescription);
         this.layoutControl1.Controls.Add(this.txtFormulaString);
         this.layoutControl1.Controls.Add(this.panelFormulaUsablePath);
         this.layoutControl1.Controls.Add(this.panelCriteria);
         this.layoutControl1.Controls.Add(this.txtVariableName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(645, 479);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // txtFormulaString
         // 
         this.txtFormulaString.Location = new System.Drawing.Point(135, 447);
         this.txtFormulaString.MenuManager = this._barManager;
         this.txtFormulaString.Name = "txtFormulaString";
         this.txtFormulaString.Size = new System.Drawing.Size(498, 20);
         this.txtFormulaString.StyleController = this.layoutControl1;
         this.txtFormulaString.TabIndex = 9;
         // 
         // _barManager
         // 
         this._barManager.DockControls.Add(this.barDockControlTop);
         this._barManager.DockControls.Add(this.barDockControlBottom);
         this._barManager.DockControls.Add(this.barDockControlLeft);
         this._barManager.DockControls.Add(this.barDockControlRight);
         this._barManager.Form = this;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = this._barManager;
         this.barDockControlTop.Size = new System.Drawing.Size(645, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 479);
         this.barDockControlBottom.Manager = this._barManager;
         this.barDockControlBottom.Size = new System.Drawing.Size(645, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Manager = this._barManager;
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 479);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(645, 0);
         this.barDockControlRight.Manager = this._barManager;
         this.barDockControlRight.Size = new System.Drawing.Size(0, 479);
         // 
         // panelFormulaUsablePath
         // 
         this.panelFormulaUsablePath.Location = new System.Drawing.Point(12, 242);
         this.panelFormulaUsablePath.Name = "panelFormulaUsablePath";
         this.panelFormulaUsablePath.Size = new System.Drawing.Size(621, 201);
         this.panelFormulaUsablePath.TabIndex = 8;
         // 
         // panelCriteria
         // 
         this.panelCriteria.Location = new System.Drawing.Point(12, 69);
         this.panelCriteria.Name = "panelCriteria";
         this.panelCriteria.Size = new System.Drawing.Size(621, 164);
         this.panelCriteria.TabIndex = 7;
         // 
         // txtVariableName
         // 
         this.txtVariableName.Location = new System.Drawing.Point(167, 29);
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
            this.layoutControlItemCriteria,
            this.layoutControlItem1,
            this.layoutItemFormulaString,
            this.layoutItemDescription,
            this.splitterItem1});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(645, 479);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemVariableName
         // 
         this.layoutControlItemVariableName.Control = this.txtVariableName;
         this.layoutControlItemVariableName.CustomizationFormText = "layoutControlItemVariableName";
         this.layoutControlItemVariableName.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItemVariableName.Name = "layoutControlItemVariableName";
         this.layoutControlItemVariableName.Size = new System.Drawing.Size(625, 24);
         this.layoutControlItemVariableName.TextSize = new System.Drawing.Size(152, 13);
         // 
         // layoutControlItemCriteria
         // 
         this.layoutControlItemCriteria.Control = this.panelCriteria;
         this.layoutControlItemCriteria.CustomizationFormText = "layoutControlItemCriteria";
         this.layoutControlItemCriteria.Location = new System.Drawing.Point(0, 41);
         this.layoutControlItemCriteria.Name = "layoutControlItemCriteria";
         this.layoutControlItemCriteria.Size = new System.Drawing.Size(625, 184);
         this.layoutControlItemCriteria.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemCriteria.TextSize = new System.Drawing.Size(152, 13);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelFormulaUsablePath;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 230);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(625, 205);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutItemFormulaString
         // 
         this.layoutItemFormulaString.Control = this.txtFormulaString;
         this.layoutItemFormulaString.Location = new System.Drawing.Point(0, 435);
         this.layoutItemFormulaString.Name = "layoutItemFormulaString";
         this.layoutItemFormulaString.Size = new System.Drawing.Size(625, 24);
         this.layoutItemFormulaString.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
         this.layoutItemFormulaString.TextSize = new System.Drawing.Size(118, 13);
         this.layoutItemFormulaString.TextToControlDistance = 5;
         // 
         // lblDescription
         // 
         this.lblDescription.Location = new System.Drawing.Point(12, 12);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(63, 13);
         this.lblDescription.StyleController = this.layoutControl1;
         this.lblDescription.TabIndex = 10;
         this.lblDescription.Text = "lblDescription";
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.lblDescription;
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 0);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(625, 17);
         this.layoutItemDescription.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDescription.TextVisible = false;
         // 
         // splitterItem1
         // 
         this.splitterItem1.AllowHotTrack = true;
         this.splitterItem1.Location = new System.Drawing.Point(0, 225);
         this.splitterItem1.Name = "splitterItem1";
         this.splitterItem1.Size = new System.Drawing.Size(625, 5);
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
         ((System.ComponentModel.ISupportInitialize)(this.txtFormulaString.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormulaUsablePath)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelCriteria)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtVariableName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemVariableName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCriteria)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaString)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

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
      private DevExpress.XtraEditors.PanelControl panelCriteria;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCriteria;
      private DevExpress.XtraEditors.PanelControl panelFormulaUsablePath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.TextEdit txtFormulaString;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaString;
      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.SplitterItem splitterItem1;
   }
}
