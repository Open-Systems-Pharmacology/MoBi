namespace MoBi.UI.Views
{
   partial class EditDimensionView
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
         this.sePrefix = new DevExpress.XtraEditors.SpinEdit();
         this.txtName = new DevExpress.XtraEditors.TextEdit();
         this.lblName = new DevExpress.XtraEditors.LabelControl();
         this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
         this.txtBaseUnit = new DevExpress.XtraEditors.TextEdit();
         this.seLength = new DevExpress.XtraEditors.SpinEdit();
         this.seTime = new DevExpress.XtraEditors.SpinEdit();
         this.seMass = new DevExpress.XtraEditors.SpinEdit();
         this.seLuminousIntensity = new DevExpress.XtraEditors.SpinEdit();
         this.seAmount = new DevExpress.XtraEditors.SpinEdit();
         this.seTemperature = new DevExpress.XtraEditors.SpinEdit();
         this.seElectricCurrent = new DevExpress.XtraEditors.SpinEdit();
         this.lblLength = new DevExpress.XtraEditors.LabelControl();
         this.lblAmount = new DevExpress.XtraEditors.LabelControl();
         this.lblTemperature = new DevExpress.XtraEditors.LabelControl();
         this.lblElectricCurrent = new DevExpress.XtraEditors.LabelControl();
         this.lblTime = new DevExpress.XtraEditors.LabelControl();
         this.lblMass = new DevExpress.XtraEditors.LabelControl();
         this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
         this.lblPrefix = new DevExpress.XtraEditors.LabelControl();
         this.grdUnits = new DevExpress.XtraGrid.GridControl();
         this.grdViewUnits = new MoBi.UI.Views.UxGridView();
         this.btAddUnit = new DevExpress.XtraEditors.SimpleButton();
         this.btRemoveUnit = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.sePrefix.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtBaseUnit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.seLength.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.seTime.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.seMass.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.seLuminousIntensity.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.seAmount.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.seTemperature.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.seElectricCurrent.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdUnits)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdViewUnits)).BeginInit();
         this.SuspendLayout();
         // 
         // sePrefix
         // 
         this.sePrefix.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.sePrefix.Location = new System.Drawing.Point(129, 60);
         this.sePrefix.Name = "sePrefix";
         this.sePrefix.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.sePrefix.Size = new System.Drawing.Size(44, 20);
         this.sePrefix.TabIndex = 0;
         // 
         // txtName
         // 
         this.txtName.Location = new System.Drawing.Point(88, 4);
         this.txtName.Name = "txtName";
         this.txtName.Size = new System.Drawing.Size(85, 20);
         this.txtName.TabIndex = 1;
         // 
         // lblName
         // 
         this.lblName.Location = new System.Drawing.Point(4, 7);
         this.lblName.Name = "lblName";
         this.lblName.Size = new System.Drawing.Size(78, 13);
         this.lblName.TabIndex = 2;
         this.lblName.Text = "Dimension Name";
         // 
         // labelControl2
         // 
         this.labelControl2.Location = new System.Drawing.Point(4, 34);
         this.labelControl2.Name = "labelControl2";
         this.labelControl2.Size = new System.Drawing.Size(45, 13);
         this.labelControl2.TabIndex = 4;
         this.labelControl2.Text = "Base Unit";
         // 
         // txtBaseUnit
         // 
         this.txtBaseUnit.Location = new System.Drawing.Point(88, 31);
         this.txtBaseUnit.Name = "txtBaseUnit";
         this.txtBaseUnit.Size = new System.Drawing.Size(85, 20);
         this.txtBaseUnit.TabIndex = 3;
         // 
         // seLength
         // 
         this.seLength.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.seLength.Location = new System.Drawing.Point(129, 86);
         this.seLength.Name = "seLength";
         this.seLength.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.seLength.Size = new System.Drawing.Size(44, 20);
         this.seLength.TabIndex = 5;
         // 
         // seTime
         // 
         this.seTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.seTime.Location = new System.Drawing.Point(129, 136);
         this.seTime.Name = "seTime";
         this.seTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.seTime.Size = new System.Drawing.Size(44, 20);
         this.seTime.TabIndex = 7;
         // 
         // seMass
         // 
         this.seMass.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.seMass.Location = new System.Drawing.Point(129, 110);
         this.seMass.Name = "seMass";
         this.seMass.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.seMass.Size = new System.Drawing.Size(44, 20);
         this.seMass.TabIndex = 6;
         // 
         // seLuminousIntensity
         // 
         this.seLuminousIntensity.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.seLuminousIntensity.Location = new System.Drawing.Point(129, 240);
         this.seLuminousIntensity.Name = "seLuminousIntensity";
         this.seLuminousIntensity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.seLuminousIntensity.Size = new System.Drawing.Size(44, 20);
         this.seLuminousIntensity.TabIndex = 11;
         // 
         // seAmount
         // 
         this.seAmount.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.seAmount.Location = new System.Drawing.Point(129, 214);
         this.seAmount.Name = "seAmount";
         this.seAmount.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.seAmount.Size = new System.Drawing.Size(44, 20);
         this.seAmount.TabIndex = 10;
         // 
         // seTemperature
         // 
         this.seTemperature.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.seTemperature.Location = new System.Drawing.Point(129, 188);
         this.seTemperature.Name = "seTemperature";
         this.seTemperature.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.seTemperature.Size = new System.Drawing.Size(44, 20);
         this.seTemperature.TabIndex = 9;
         // 
         // seElectricCurrent
         // 
         this.seElectricCurrent.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
         this.seElectricCurrent.Location = new System.Drawing.Point(129, 162);
         this.seElectricCurrent.Name = "seElectricCurrent";
         this.seElectricCurrent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.seElectricCurrent.Size = new System.Drawing.Size(44, 20);
         this.seElectricCurrent.TabIndex = 8;
         // 
         // lblLength
         // 
         this.lblLength.Location = new System.Drawing.Point(0, 90);
         this.lblLength.Name = "lblLength";
         this.lblLength.Size = new System.Drawing.Size(33, 13);
         this.lblLength.TabIndex = 12;
         this.lblLength.Text = "Length";
         // 
         // lblAmount
         // 
         this.lblAmount.Location = new System.Drawing.Point(0, 221);
         this.lblAmount.Name = "lblAmount";
         this.lblAmount.Size = new System.Drawing.Size(37, 13);
         this.lblAmount.TabIndex = 13;
         this.lblAmount.Text = "Amount";
         // 
         // lblTemperature
         // 
         this.lblTemperature.Location = new System.Drawing.Point(0, 195);
         this.lblTemperature.Name = "lblTemperature";
         this.lblTemperature.Size = new System.Drawing.Size(62, 13);
         this.lblTemperature.TabIndex = 14;
         this.lblTemperature.Text = "Temperature";
         // 
         // lblElectricCurrent
         // 
         this.lblElectricCurrent.Location = new System.Drawing.Point(0, 169);
         this.lblElectricCurrent.Name = "lblElectricCurrent";
         this.lblElectricCurrent.Size = new System.Drawing.Size(71, 13);
         this.lblElectricCurrent.TabIndex = 15;
         this.lblElectricCurrent.Text = "ElectricCurrent";
         // 
         // lblTime
         // 
         this.lblTime.Location = new System.Drawing.Point(0, 140);
         this.lblTime.Name = "lblTime";
         this.lblTime.Size = new System.Drawing.Size(22, 13);
         this.lblTime.TabIndex = 16;
         this.lblTime.Text = "Time";
         // 
         // lblMass
         // 
         this.lblMass.Location = new System.Drawing.Point(0, 114);
         this.lblMass.Name = "lblMass";
         this.lblMass.Size = new System.Drawing.Size(24, 13);
         this.lblMass.TabIndex = 17;
         this.lblMass.Text = "Mass";
         // 
         // labelControl8
         // 
         this.labelControl8.Location = new System.Drawing.Point(0, 247);
         this.labelControl8.Name = "labelControl8";
         this.labelControl8.Size = new System.Drawing.Size(90, 13);
         this.labelControl8.TabIndex = 18;
         this.labelControl8.Text = "Luminous Intensity";
         // 
         // lblPrefix
         // 
         this.lblPrefix.Location = new System.Drawing.Point(0, 64);
         this.lblPrefix.Name = "lblPrefix";
         this.lblPrefix.Size = new System.Drawing.Size(28, 13);
         this.lblPrefix.TabIndex = 19;
         this.lblPrefix.Text = "Prefix";
         // 
         // grdUnits
         // 
         this.grdUnits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grdUnits.Location = new System.Drawing.Point(200, 18);
         this.grdUnits.MainView = this.grdViewUnits;
         this.grdUnits.Name = "grdUnits";
         this.grdUnits.Size = new System.Drawing.Size(438, 241);
         this.grdUnits.TabIndex = 22;
         this.grdUnits.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewUnits});
         // 
         // grdViewUnits
         // 
         this.grdViewUnits.GridControl = this.grdUnits;
         this.grdViewUnits.Name = "grdViewUnits";
         // 
         // btAddUnit
         // 
         this.btAddUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btAddUnit.Location = new System.Drawing.Point(645, 18);
         this.btAddUnit.Name = "btAddUnit";
         this.btAddUnit.Size = new System.Drawing.Size(75, 23);
         this.btAddUnit.TabIndex = 23;
         this.btAddUnit.Text = "Add Unit";
         this.btAddUnit.Click += new System.EventHandler(this.btAddUnit_Click);
         // 
         // btRemoveUnit
         // 
         this.btRemoveUnit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btRemoveUnit.Location = new System.Drawing.Point(645, 48);
         this.btRemoveUnit.Name = "btRemoveUnit";
         this.btRemoveUnit.Size = new System.Drawing.Size(75, 23);
         this.btRemoveUnit.TabIndex = 24;
         this.btRemoveUnit.Text = "Remove Unit";
         this.btRemoveUnit.Click += new System.EventHandler(this.btRemoveUnit_Click);
         // 
         // EditDimensionView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.btRemoveUnit);
         this.Controls.Add(this.btAddUnit);
         this.Controls.Add(this.grdUnits);
         this.Controls.Add(this.lblPrefix);
         this.Controls.Add(this.labelControl8);
         this.Controls.Add(this.lblMass);
         this.Controls.Add(this.lblTime);
         this.Controls.Add(this.lblElectricCurrent);
         this.Controls.Add(this.lblTemperature);
         this.Controls.Add(this.lblAmount);
         this.Controls.Add(this.lblLength);
         this.Controls.Add(this.seLuminousIntensity);
         this.Controls.Add(this.seAmount);
         this.Controls.Add(this.seTemperature);
         this.Controls.Add(this.seElectricCurrent);
         this.Controls.Add(this.seTime);
         this.Controls.Add(this.seMass);
         this.Controls.Add(this.seLength);
         this.Controls.Add(this.labelControl2);
         this.Controls.Add(this.txtBaseUnit);
         this.Controls.Add(this.lblName);
         this.Controls.Add(this.txtName);
         this.Controls.Add(this.sePrefix);
         this.Name = "EditDimensionView";
         this.Size = new System.Drawing.Size(726, 317);
         ((System.ComponentModel.ISupportInitialize)(this.sePrefix.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtBaseUnit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.seLength.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.seTime.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.seMass.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.seLuminousIntensity.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.seAmount.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.seTemperature.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.seElectricCurrent.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdUnits)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdViewUnits)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.SpinEdit sePrefix;
      private DevExpress.XtraEditors.TextEdit txtName;
      private DevExpress.XtraEditors.LabelControl lblName;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private DevExpress.XtraEditors.TextEdit txtBaseUnit;
      private DevExpress.XtraEditors.SpinEdit seLength;
      private DevExpress.XtraEditors.SpinEdit seTime;
      private DevExpress.XtraEditors.SpinEdit seMass;
      private DevExpress.XtraEditors.SpinEdit seLuminousIntensity;
      private DevExpress.XtraEditors.SpinEdit seAmount;
      private DevExpress.XtraEditors.SpinEdit seTemperature;
      private DevExpress.XtraEditors.SpinEdit seElectricCurrent;
      private DevExpress.XtraEditors.LabelControl lblLength;
      private DevExpress.XtraEditors.LabelControl lblAmount;
      private DevExpress.XtraEditors.LabelControl lblTemperature;
      private DevExpress.XtraEditors.LabelControl lblElectricCurrent;
      private DevExpress.XtraEditors.LabelControl lblTime;
      private DevExpress.XtraEditors.LabelControl lblMass;
      private DevExpress.XtraEditors.LabelControl labelControl8;
      private DevExpress.XtraEditors.LabelControl lblPrefix;
      private DevExpress.XtraGrid.GridControl grdUnits;
      private MoBi.UI.Views.UxGridView grdViewUnits;
      private DevExpress.XtraEditors.SimpleButton btAddUnit;
      private DevExpress.XtraEditors.SimpleButton btRemoveUnit;
   }
}
