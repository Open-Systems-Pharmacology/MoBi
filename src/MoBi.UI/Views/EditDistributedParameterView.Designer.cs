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
         this.tableProperties = new DevExpress.Utils.Layout.TablePanel();
         this.veValue = new MoBi.UI.Views.ValueEdit();
         this.labelPercentile = new DevExpress.XtraEditors.LabelControl();
         this.labelValue = new DevExpress.XtraEditors.LabelControl();
         this.labelDimension = new DevExpress.XtraEditors.LabelControl();
         this.labelName = new DevExpress.XtraEditors.LabelControl();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.cbDimension = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.tbPercentile = new DevExpress.XtraEditors.TextEdit();
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         this.veDeviation = new MoBi.UI.Views.ValueEdit();
         this.veGeoStd = new MoBi.UI.Views.ValueEdit();
         this.veMaximum = new MoBi.UI.Views.ValueEdit();
         this.veMinimum = new MoBi.UI.Views.ValueEdit();
         this.veMean = new MoBi.UI.Views.ValueEdit();
         this.labellDistribution = new DevExpress.XtraEditors.LabelControl();
         this.labelDeviation = new DevExpress.XtraEditors.LabelControl();
         this.labelGeoStd = new DevExpress.XtraEditors.LabelControl();
         this.labelMaximum = new DevExpress.XtraEditors.LabelControl();
         this.labelMinimum = new DevExpress.XtraEditors.LabelControl();
         this.labelMean = new DevExpress.XtraEditors.LabelControl();
         this.cbFormulaType = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupDistribution = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tableProperties)).BeginInit();
         this.tableProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbPercentile.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupDistribution)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
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
         this.layoutControl.Controls.Add(this.tableProperties);
         this.layoutControl.Controls.Add(this.tablePanel);
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(568, 509);
         this.layoutControl.TabIndex = 37;
         this.layoutControl.Text = "layoutControl1";
         // 
         // tableProperties
         // 
         this.tableProperties.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
         this.tableProperties.Controls.Add(this.veValue);
         this.tableProperties.Controls.Add(this.labelPercentile);
         this.tableProperties.Controls.Add(this.labelValue);
         this.tableProperties.Controls.Add(this.labelDimension);
         this.tableProperties.Controls.Add(this.labelName);
         this.tableProperties.Controls.Add(this.btName);
         this.tableProperties.Controls.Add(this.cbDimension);
         this.tableProperties.Controls.Add(this.tbPercentile);
         this.tableProperties.Location = new System.Drawing.Point(12, 12);
         this.tableProperties.Name = "tableProperties";
         this.tableProperties.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F)});
         this.tableProperties.Size = new System.Drawing.Size(544, 115);
         this.tableProperties.TabIndex = 41;
         // 
         // veValue
         // 
         this.veValue.Caption = "";
         this.tableProperties.SetColumn(this.veValue, 1);
         this.veValue.Location = new System.Drawing.Point(79, 55);
         this.veValue.Name = "veValue";
         this.tableProperties.SetRow(this.veValue, 2);
         this.veValue.Size = new System.Drawing.Size(462, 20);
         this.veValue.TabIndex = 44;
         this.veValue.ToolTip = "";
         // 
         // labelPercentile
         // 
         this.tableProperties.SetColumn(this.labelPercentile, 0);
         this.labelPercentile.Location = new System.Drawing.Point(3, 84);
         this.labelPercentile.Name = "labelPercentile";
         this.tableProperties.SetRow(this.labelPercentile, 3);
         this.labelPercentile.Size = new System.Drawing.Size(69, 13);
         this.labelPercentile.TabIndex = 43;
         this.labelPercentile.Text = "labelPercentile";
         // 
         // labelValue
         // 
         this.tableProperties.SetColumn(this.labelValue, 0);
         this.labelValue.Location = new System.Drawing.Point(3, 58);
         this.labelValue.Name = "labelValue";
         this.tableProperties.SetRow(this.labelValue, 2);
         this.labelValue.Size = new System.Drawing.Size(48, 13);
         this.labelValue.TabIndex = 42;
         this.labelValue.Text = "labelValue";
         // 
         // labelDimension
         // 
         this.tableProperties.SetColumn(this.labelDimension, 0);
         this.labelDimension.Location = new System.Drawing.Point(3, 32);
         this.labelDimension.Name = "labelDimension";
         this.tableProperties.SetRow(this.labelDimension, 1);
         this.labelDimension.Size = new System.Drawing.Size(70, 13);
         this.labelDimension.TabIndex = 41;
         this.labelDimension.Text = "labelDimension";
         // 
         // labelName
         // 
         this.tableProperties.SetColumn(this.labelName, 0);
         this.labelName.Location = new System.Drawing.Point(3, 6);
         this.labelName.Name = "labelName";
         this.tableProperties.SetRow(this.labelName, 0);
         this.labelName.Size = new System.Drawing.Size(49, 13);
         this.labelName.TabIndex = 40;
         this.labelName.Text = "labelName";
         // 
         // btName
         // 
         this.btName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tableProperties.SetColumn(this.btName, 1);
         this.btName.Location = new System.Drawing.Point(79, 3);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.tableProperties.SetRow(this.btName, 0);
         this.btName.Size = new System.Drawing.Size(462, 20);
         this.btName.TabIndex = 0;
         // 
         // cbDimension
         // 
         this.cbDimension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tableProperties.SetColumn(this.cbDimension, 1);
         this.cbDimension.Location = new System.Drawing.Point(79, 29);
         this.cbDimension.Name = "cbDimension";
         this.cbDimension.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.tableProperties.SetRow(this.cbDimension, 1);
         this.cbDimension.Size = new System.Drawing.Size(462, 20);
         this.cbDimension.TabIndex = 19;
         // 
         // tbPercentile
         // 
         this.tableProperties.SetColumn(this.tbPercentile, 1);
         this.tbPercentile.Location = new System.Drawing.Point(79, 81);
         this.tbPercentile.Name = "tbPercentile";
         this.tableProperties.SetRow(this.tbPercentile, 3);
         this.tbPercentile.Size = new System.Drawing.Size(462, 20);
         this.tbPercentile.TabIndex = 39;
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
         this.tablePanel.Controls.Add(this.veDeviation);
         this.tablePanel.Controls.Add(this.veGeoStd);
         this.tablePanel.Controls.Add(this.veMaximum);
         this.tablePanel.Controls.Add(this.veMinimum);
         this.tablePanel.Controls.Add(this.veMean);
         this.tablePanel.Controls.Add(this.labellDistribution);
         this.tablePanel.Controls.Add(this.labelDeviation);
         this.tablePanel.Controls.Add(this.labelGeoStd);
         this.tablePanel.Controls.Add(this.labelMaximum);
         this.tablePanel.Controls.Add(this.labelMinimum);
         this.tablePanel.Controls.Add(this.labelMean);
         this.tablePanel.Controls.Add(this.cbFormulaType);
         this.tablePanel.Location = new System.Drawing.Point(24, 164);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(520, 297);
         this.tablePanel.TabIndex = 40;
         // 
         // veDeviation
         // 
         this.veDeviation.Caption = "";
         this.tablePanel.SetColumn(this.veDeviation, 1);
         this.veDeviation.Location = new System.Drawing.Point(87, 133);
         this.veDeviation.Name = "veDeviation";
         this.tablePanel.SetRow(this.veDeviation, 5);
         this.veDeviation.Size = new System.Drawing.Size(430, 20);
         this.veDeviation.TabIndex = 49;
         this.veDeviation.ToolTip = "";
         // 
         // veGeoStd
         // 
         this.veGeoStd.Caption = "";
         this.tablePanel.SetColumn(this.veGeoStd, 1);
         this.veGeoStd.Location = new System.Drawing.Point(87, 107);
         this.veGeoStd.Name = "veGeoStd";
         this.tablePanel.SetRow(this.veGeoStd, 4);
         this.veGeoStd.Size = new System.Drawing.Size(430, 20);
         this.veGeoStd.TabIndex = 48;
         this.veGeoStd.ToolTip = "";
         // 
         // veMaximum
         // 
         this.veMaximum.Caption = "";
         this.tablePanel.SetColumn(this.veMaximum, 1);
         this.veMaximum.Location = new System.Drawing.Point(87, 81);
         this.veMaximum.Name = "veMaximum";
         this.tablePanel.SetRow(this.veMaximum, 3);
         this.veMaximum.Size = new System.Drawing.Size(430, 20);
         this.veMaximum.TabIndex = 47;
         this.veMaximum.ToolTip = "";
         // 
         // veMinimum
         // 
         this.veMinimum.Caption = "";
         this.tablePanel.SetColumn(this.veMinimum, 1);
         this.veMinimum.Location = new System.Drawing.Point(87, 55);
         this.veMinimum.Name = "veMinimum";
         this.tablePanel.SetRow(this.veMinimum, 2);
         this.veMinimum.Size = new System.Drawing.Size(430, 20);
         this.veMinimum.TabIndex = 46;
         this.veMinimum.ToolTip = "";
         // 
         // veMean
         // 
         this.veMean.Caption = "";
         this.tablePanel.SetColumn(this.veMean, 1);
         this.veMean.Location = new System.Drawing.Point(87, 29);
         this.veMean.Name = "veMean";
         this.tablePanel.SetRow(this.veMean, 1);
         this.veMean.Size = new System.Drawing.Size(430, 20);
         this.veMean.TabIndex = 45;
         this.veMean.ToolTip = "";
         // 
         // labellDistribution
         // 
         this.tablePanel.SetColumn(this.labellDistribution, 0);
         this.labellDistribution.Location = new System.Drawing.Point(3, 6);
         this.labellDistribution.Name = "labellDistribution";
         this.tablePanel.SetRow(this.labellDistribution, 0);
         this.labellDistribution.Size = new System.Drawing.Size(78, 13);
         this.labellDistribution.TabIndex = 44;
         this.labellDistribution.Text = "labellDistribution";
         // 
         // labelDeviation
         // 
         this.tablePanel.SetColumn(this.labelDeviation, 0);
         this.labelDeviation.Location = new System.Drawing.Point(3, 136);
         this.labelDeviation.Name = "labelDeviation";
         this.tablePanel.SetRow(this.labelDeviation, 5);
         this.labelDeviation.Size = new System.Drawing.Size(67, 13);
         this.labelDeviation.TabIndex = 43;
         this.labelDeviation.Text = "labelDeviation";
         // 
         // labelGeoStd
         // 
         this.tablePanel.SetColumn(this.labelGeoStd, 0);
         this.labelGeoStd.Location = new System.Drawing.Point(3, 110);
         this.labelGeoStd.Name = "labelGeoStd";
         this.tablePanel.SetRow(this.labelGeoStd, 4);
         this.labelGeoStd.Size = new System.Drawing.Size(57, 13);
         this.labelGeoStd.TabIndex = 42;
         this.labelGeoStd.Text = "labelGeoStd";
         // 
         // labelMaximum
         // 
         this.tablePanel.SetColumn(this.labelMaximum, 0);
         this.labelMaximum.Location = new System.Drawing.Point(3, 84);
         this.labelMaximum.Name = "labelMaximum";
         this.tablePanel.SetRow(this.labelMaximum, 3);
         this.labelMaximum.Size = new System.Drawing.Size(66, 13);
         this.labelMaximum.TabIndex = 41;
         this.labelMaximum.Text = "labelMaximum";
         // 
         // labelMinimum
         // 
         this.tablePanel.SetColumn(this.labelMinimum, 0);
         this.labelMinimum.Location = new System.Drawing.Point(3, 58);
         this.labelMinimum.Name = "labelMinimum";
         this.tablePanel.SetRow(this.labelMinimum, 2);
         this.labelMinimum.Size = new System.Drawing.Size(62, 13);
         this.labelMinimum.TabIndex = 40;
         this.labelMinimum.Text = "labelMinimum";
         // 
         // labelMean
         // 
         this.tablePanel.SetColumn(this.labelMean, 0);
         this.labelMean.Location = new System.Drawing.Point(3, 32);
         this.labelMean.Name = "labelMean";
         this.tablePanel.SetRow(this.labelMean, 1);
         this.labelMean.Size = new System.Drawing.Size(48, 13);
         this.labelMean.TabIndex = 39;
         this.labelMean.Text = "labelMean";
         // 
         // cbFormulaType
         // 
         this.tablePanel.SetColumn(this.cbFormulaType, 1);
         this.tablePanel.SetColumnSpan(this.cbFormulaType, 2);
         this.cbFormulaType.Location = new System.Drawing.Point(87, 3);
         this.cbFormulaType.Name = "cbFormulaType";
         this.cbFormulaType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.tablePanel.SetRow(this.cbFormulaType, 0);
         this.cbFormulaType.Size = new System.Drawing.Size(430, 20);
         this.cbFormulaType.TabIndex = 37;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(81, 477);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(475, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 20;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDescription,
            this.layoutGroupDistribution,
            this.layoutControlItem2});
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Size = new System.Drawing.Size(568, 509);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "Description";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 465);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(548, 24);
         this.layoutItemDescription.Text = "Description:";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(57, 13);
         // 
         // layoutGroupDistribution
         // 
         this.layoutGroupDistribution.CustomizationFormText = "Distribution";
         this.layoutGroupDistribution.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
         this.layoutGroupDistribution.Location = new System.Drawing.Point(0, 119);
         this.layoutGroupDistribution.Name = "layoutGroupDistribution";
         this.layoutGroupDistribution.Size = new System.Drawing.Size(548, 346);
         this.layoutGroupDistribution.Text = "Distribution";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.tablePanel;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(524, 301);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.tableProperties;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(548, 119);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // EditDistributedParameterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditDistributedParameterView";
         this.Size = new System.Drawing.Size(568, 509);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tableProperties)).EndInit();
         this.tableProperties.ResumeLayout(false);
         this.tableProperties.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbDimension.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbPercentile.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         this.tablePanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbFormulaType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupDistribution)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbDimension;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbFormulaType;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupDistribution;
      private DevExpress.XtraEditors.TextEdit tbPercentile;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.Utils.Layout.TablePanel tablePanel;
      private DevExpress.XtraEditors.LabelControl labelDeviation;
      private DevExpress.XtraEditors.LabelControl labelGeoStd;
      private DevExpress.XtraEditors.LabelControl labelMaximum;
      private DevExpress.XtraEditors.LabelControl labelMinimum;
      private DevExpress.XtraEditors.LabelControl labelMean;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.Utils.Layout.TablePanel tableProperties;
      private DevExpress.XtraEditors.LabelControl labelPercentile;
      private DevExpress.XtraEditors.LabelControl labelValue;
      private DevExpress.XtraEditors.LabelControl labelDimension;
      private DevExpress.XtraEditors.LabelControl labelName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.LabelControl labellDistribution;
      private ValueEdit veValue;
      private ValueEdit veDeviation;
      private ValueEdit veGeoStd;
      private ValueEdit veMaximum;
      private ValueEdit veMinimum;
      private ValueEdit veMean;
   }
}
