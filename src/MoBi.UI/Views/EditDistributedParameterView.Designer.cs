namespace MoBi.UI.Views
{
   partial class EditDistributedParameterView
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
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.tbPercentile = new DevExpress.XtraEditors.TextEdit();
         this.cbFormulaType = new DevExpress.XtraEditors.ComboBoxEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.cbDimension = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDimension = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupDistribution = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDistributionType = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemPercentile = new DevExpress.XtraLayout.LayoutControlItem();
         this.veGeoStd = new MoBi.UI.Views.ValueEdit();
         this.veDeviation = new MoBi.UI.Views.ValueEdit();
         this.veMaximum = new MoBi.UI.Views.ValueEdit();
         this.veMean = new MoBi.UI.Views.ValueEdit();
         this.veMinimum = new MoBi.UI.Views.ValueEdit();
         this.veValue = new MoBi.UI.Views.ValueEdit();
         this.layoutItemValue = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMean = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMinimum = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMaximum = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDeviation = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemGeoDeviation = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbPercentile.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupDistribution)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDistributionType)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPercentile)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMean)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMinimum)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMaximum)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDeviation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGeoDeviation)).BeginInit();
         this.SuspendLayout();
         // 
         // lblDescription
         // 
         this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.lblDescription.Location = new System.Drawing.Point(4, 376);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(57, 13);
         this.lblDescription.TabIndex = 7;
         this.lblDescription.Text = "Description:";
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.veGeoStd);
         this.layoutControl.Controls.Add(this.tbPercentile);
         this.layoutControl.Controls.Add(this.veDeviation);
         this.layoutControl.Controls.Add(this.cbFormulaType);
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Controls.Add(this.veMaximum);
         this.layoutControl.Controls.Add(this.btName);
         this.layoutControl.Controls.Add(this.cbDimension);
         this.layoutControl.Controls.Add(this.veMean);
         this.layoutControl.Controls.Add(this.veMinimum);
         this.layoutControl.Controls.Add(this.veValue);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(697, 322);
         this.layoutControl.TabIndex = 37;
         this.layoutControl.Text = "layoutControl1";
         // 
         // tbPercentile
         // 
         this.tbPercentile.Location = new System.Drawing.Point(111, 82);
         this.tbPercentile.Name = "tbPercentile";
         this.tbPercentile.Size = new System.Drawing.Size(574, 20);
         this.tbPercentile.StyleController = this.layoutControl;
         this.tbPercentile.TabIndex = 39;
         // 
         // cbFormulaType
         // 
         this.cbFormulaType.Location = new System.Drawing.Point(24, 137);
         this.cbFormulaType.Name = "cbFormulaType";
         this.cbFormulaType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFormulaType.Size = new System.Drawing.Size(649, 20);
         this.cbFormulaType.StyleController = this.layoutControl;
         this.cbFormulaType.TabIndex = 37;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(111, 290);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(574, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 20;
         // 
         // btName
         // 
         this.btName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btName.Location = new System.Drawing.Point(111, 12);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(574, 20);
         this.btName.StyleController = this.layoutControl;
         this.btName.TabIndex = 0;
         // 
         // cbDimension
         // 
         this.cbDimension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cbDimension.Location = new System.Drawing.Point(111, 36);
         this.cbDimension.Name = "cbDimension";
         this.cbDimension.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbDimension.Size = new System.Drawing.Size(574, 20);
         this.cbDimension.StyleController = this.layoutControl;
         this.cbDimension.TabIndex = 19;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutItemDimension,
            this.layoutItemValue,
            this.layoutItemDescription,
            this.layoutGroupDistribution,
            this.layoutItemPercentile});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(697, 322);
         this.layoutControlGroup.Text = "layoutControlGroup";
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "Name";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(677, 24);
         this.layoutItemName.Text = "Name:";
         this.layoutItemName.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemDimension
         // 
         this.layoutItemDimension.Control = this.cbDimension;
         this.layoutItemDimension.CustomizationFormText = "Dimension";
         this.layoutItemDimension.Location = new System.Drawing.Point(0, 24);
         this.layoutItemDimension.Name = "layoutItemDimension";
         this.layoutItemDimension.Size = new System.Drawing.Size(677, 24);
         this.layoutItemDimension.Text = "Dimension:";
         this.layoutItemDimension.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "Description";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 278);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(677, 24);
         this.layoutItemDescription.Text = "Description:";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutGroupDistribution
         // 
         this.layoutGroupDistribution.CustomizationFormText = "Distribution";
         this.layoutGroupDistribution.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDistributionType,
            this.layoutItemMean,
            this.layoutItemMinimum,
            this.layoutItemMaximum,
            this.layoutItemDeviation,
            this.layoutItemGeoDeviation});
         this.layoutGroupDistribution.Location = new System.Drawing.Point(0, 94);
         this.layoutGroupDistribution.Name = "layoutGroupDistribution";
         this.layoutGroupDistribution.Size = new System.Drawing.Size(677, 184);
         this.layoutGroupDistribution.Text = "Distribution";
         // 
         // layoutItemDistributionType
         // 
         this.layoutItemDistributionType.Control = this.cbFormulaType;
         this.layoutItemDistributionType.CustomizationFormText = "layoutItemDistributionType";
         this.layoutItemDistributionType.Location = new System.Drawing.Point(0, 0);
         this.layoutItemDistributionType.Name = "layoutItemDistributionType";
         this.layoutItemDistributionType.Size = new System.Drawing.Size(653, 24);
         this.layoutItemDistributionType.Text = "layoutItemDistributionType";
         this.layoutItemDistributionType.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDistributionType.TextToControlDistance = 0;
         this.layoutItemDistributionType.TextVisible = false;
         // 
         // layoutItemPercentile
         // 
         this.layoutItemPercentile.Control = this.tbPercentile;
         this.layoutItemPercentile.CustomizationFormText = "Percentile";
         this.layoutItemPercentile.Location = new System.Drawing.Point(0, 70);
         this.layoutItemPercentile.Name = "layoutItemPercentile";
         this.layoutItemPercentile.Size = new System.Drawing.Size(677, 24);
         this.layoutItemPercentile.Text = "Percentile";
         this.layoutItemPercentile.TextSize = new System.Drawing.Size(96, 13);
         // 
         // veGeoStd
         // 
         this.veGeoStd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.veGeoStd.Caption = "";
         this.veGeoStd.Location = new System.Drawing.Point(123, 231);
         this.veGeoStd.MaximumSize = new System.Drawing.Size(200000, 20);
         this.veGeoStd.Name = "veGeoStd";
         this.veGeoStd.Size = new System.Drawing.Size(550, 20);
         this.veGeoStd.TabIndex = 38;
         this.veGeoStd.ToolTip = "";
         // 
         // veDeviation
         // 
         this.veDeviation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.veDeviation.Caption = "";
         this.veDeviation.Location = new System.Drawing.Point(123, 255);
         this.veDeviation.MaximumSize = new System.Drawing.Size(200000, 20);
         this.veDeviation.Name = "veDeviation";
         this.veDeviation.Size = new System.Drawing.Size(550, 19);
         this.veDeviation.TabIndex = 38;
         this.veDeviation.ToolTip = "";
         // 
         // veMaximum
         // 
         this.veMaximum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.veMaximum.Caption = "";
         this.veMaximum.Location = new System.Drawing.Point(123, 208);
         this.veMaximum.MaximumSize = new System.Drawing.Size(200000, 20);
         this.veMaximum.Name = "veMaximum";
         this.veMaximum.Size = new System.Drawing.Size(550, 19);
         this.veMaximum.TabIndex = 26;
         this.veMaximum.ToolTip = "";
         // 
         // veMean
         // 
         this.veMean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.veMean.Caption = "";
         this.veMean.Location = new System.Drawing.Point(123, 161);
         this.veMean.MaximumSize = new System.Drawing.Size(200000, 20);
         this.veMean.Name = "veMean";
         this.veMean.Size = new System.Drawing.Size(550, 19);
         this.veMean.TabIndex = 27;
         this.veMean.ToolTip = "";
         // 
         // veMinimum
         // 
         this.veMinimum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.veMinimum.Caption = "";
         this.veMinimum.Location = new System.Drawing.Point(123, 184);
         this.veMinimum.MaximumSize = new System.Drawing.Size(200000, 20);
         this.veMinimum.Name = "veMinimum";
         this.veMinimum.Size = new System.Drawing.Size(550, 20);
         this.veMinimum.TabIndex = 25;
         this.veMinimum.ToolTip = "";
         // 
         // veValue
         // 
         this.veValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.veValue.Caption = "";
         this.veValue.Location = new System.Drawing.Point(111, 60);
         this.veValue.MaximumSize = new System.Drawing.Size(200000, 20);
         this.veValue.Name = "veValue";
         this.veValue.Size = new System.Drawing.Size(574, 18);
         this.veValue.TabIndex = 35;
         this.veValue.ToolTip = "";
         // 
         // layoutItemValue
         // 
         this.layoutItemValue.Control = this.veValue;
         this.layoutItemValue.CustomizationFormText = "Value";
         this.layoutItemValue.Location = new System.Drawing.Point(0, 48);
         this.layoutItemValue.Name = "layoutItemValue";
         this.layoutItemValue.Size = new System.Drawing.Size(677, 22);
         this.layoutItemValue.Text = "Value:";
         this.layoutItemValue.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemMean
         // 
         this.layoutItemMean.Control = this.veMean;
         this.layoutItemMean.CustomizationFormText = "Mean";
         this.layoutItemMean.Location = new System.Drawing.Point(0, 24);
         this.layoutItemMean.Name = "layoutItemMean";
         this.layoutItemMean.Size = new System.Drawing.Size(653, 23);
         this.layoutItemMean.Text = "Mean";
         this.layoutItemMean.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemMinimum
         // 
         this.layoutItemMinimum.Control = this.veMinimum;
         this.layoutItemMinimum.CustomizationFormText = "Minimum";
         this.layoutItemMinimum.Location = new System.Drawing.Point(0, 47);
         this.layoutItemMinimum.Name = "layoutItemMinimum";
         this.layoutItemMinimum.Size = new System.Drawing.Size(653, 24);
         this.layoutItemMinimum.Text = "Minimum";
         this.layoutItemMinimum.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemMaximum
         // 
         this.layoutItemMaximum.Control = this.veMaximum;
         this.layoutItemMaximum.CustomizationFormText = "Maximum";
         this.layoutItemMaximum.Location = new System.Drawing.Point(0, 71);
         this.layoutItemMaximum.Name = "layoutItemMaximum";
         this.layoutItemMaximum.Size = new System.Drawing.Size(653, 23);
         this.layoutItemMaximum.Text = "Maximum";
         this.layoutItemMaximum.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemDeviation
         // 
         this.layoutItemDeviation.Control = this.veDeviation;
         this.layoutItemDeviation.CustomizationFormText = "layoutControlItem1";
         this.layoutItemDeviation.Location = new System.Drawing.Point(0, 118);
         this.layoutItemDeviation.Name = "layoutItemDeviation";
         this.layoutItemDeviation.Size = new System.Drawing.Size(653, 23);
         this.layoutItemDeviation.Text = "Standard Deviation";
         this.layoutItemDeviation.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemGeoDeviation
         // 
         this.layoutItemGeoDeviation.Control = this.veGeoStd;
         this.layoutItemGeoDeviation.CustomizationFormText = "Geometric Deviation";
         this.layoutItemGeoDeviation.Location = new System.Drawing.Point(0, 94);
         this.layoutItemGeoDeviation.Name = "layoutItemGeoDeviation";
         this.layoutItemGeoDeviation.Size = new System.Drawing.Size(653, 24);
         this.layoutItemGeoDeviation.Text = "Geometric Deviation";
         this.layoutItemGeoDeviation.TextSize = new System.Drawing.Size(96, 13);
         // 
         // EditDistributedParameterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditDistributedParameterView";
         this.Size = new System.Drawing.Size(697, 322);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbPercentile.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupDistribution)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDistributionType)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPercentile)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMean)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMinimum)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMaximum)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDeviation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemGeoDeviation)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl lblDescription;
      private ValueEdit veMaximum;
      private ValueEdit veMinimum;
      private ValueEdit veMean;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private DevExpress.XtraEditors.ComboBoxEdit cbDimension;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private ValueEdit veValue;
      private DevExpress.XtraEditors.ComboBoxEdit cbFormulaType;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDimension;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValue;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupDistribution;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDistributionType;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMean;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMinimum;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMaximum;
      private ValueEdit veDeviation;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDeviation;
      private DevExpress.XtraEditors.TextEdit tbPercentile;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPercentile;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private ValueEdit veGeoStd;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemGeoDeviation;

   }
}
