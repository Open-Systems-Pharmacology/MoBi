namespace MoBi.UI.Views
{
   partial class SelectFolderAndIndividualAndExpressionFromProjectView
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

         disposeBinders();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.btnSelectFilePath = new DevExpress.XtraEditors.ButtonEdit();
         this.cmbSelectIndividual = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.descriptionLabel = new DevExpress.XtraEditors.LabelControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSelectIndividual = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSelectFilePath = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemExpressionSelect = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnSelectFilePath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSelectIndividual.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectFilePath)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpressionSelect)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 646);
         this.tablePanel.Size = new System.Drawing.Size(581, 43);
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.gridControl);
         this.uxLayoutControl1.Controls.Add(this.btnSelectFilePath);
         this.uxLayoutControl1.Controls.Add(this.cmbSelectIndividual);
         this.uxLayoutControl1.Controls.Add(this.descriptionLabel);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(581, 646);
         this.uxLayoutControl1.TabIndex = 39;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(12, 60);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(557, 557);
         this.gridControl.TabIndex = 11;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = true;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsSelection.MultiSelect = true;
         this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // btnSelectFilePath
         // 
         this.btnSelectFilePath.Location = new System.Drawing.Point(186, 12);
         this.btnSelectFilePath.Name = "btnSelectFilePath";
         this.btnSelectFilePath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btnSelectFilePath.Size = new System.Drawing.Size(383, 20);
         this.btnSelectFilePath.StyleController = this.uxLayoutControl1;
         this.btnSelectFilePath.TabIndex = 10;
         this.btnSelectFilePath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btnSelectFilePathClick);
         // 
         // cmbSelectIndividual
         // 
         this.cmbSelectIndividual.Location = new System.Drawing.Point(186, 36);
         this.cmbSelectIndividual.Name = "cmbSelectIndividual";
         this.cmbSelectIndividual.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cmbSelectIndividual.Size = new System.Drawing.Size(383, 20);
         this.cmbSelectIndividual.StyleController = this.uxLayoutControl1;
         this.cmbSelectIndividual.TabIndex = 8;
         // 
         // descriptionLabel
         // 
         this.descriptionLabel.AllowHtmlString = true;
         this.descriptionLabel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this.descriptionLabel.Location = new System.Drawing.Point(12, 621);
         this.descriptionLabel.Name = "descriptionLabel";
         this.descriptionLabel.Size = new System.Drawing.Size(557, 13);
         this.descriptionLabel.StyleController = this.uxLayoutControl1;
         this.descriptionLabel.TabIndex = 9;
         this.descriptionLabel.Text = "containerExportDescription";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemDescription,
            this.layoutControlItemSelectIndividual,
            this.layoutControlItemSelectFilePath,
            this.layoutControlItemExpressionSelect});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(581, 646);
         this.Root.TextVisible = false;
         // 
         // layoutControlItemDescription
         // 
         this.layoutControlItemDescription.Control = this.descriptionLabel;
         this.layoutControlItemDescription.Location = new System.Drawing.Point(0, 609);
         this.layoutControlItemDescription.Name = "layoutControlItemDescription";
         this.layoutControlItemDescription.Size = new System.Drawing.Size(561, 17);
         this.layoutControlItemDescription.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemDescription.TextVisible = false;
         // 
         // layoutControlItemSelectIndividual
         // 
         this.layoutControlItemSelectIndividual.Control = this.cmbSelectIndividual;
         this.layoutControlItemSelectIndividual.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemSelectIndividual.Name = "layoutControlItemSelectIndividual";
         this.layoutControlItemSelectIndividual.Size = new System.Drawing.Size(561, 24);
         this.layoutControlItemSelectIndividual.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutControlItemSelectFilePath
         // 
         this.layoutControlItemSelectFilePath.Control = this.btnSelectFilePath;
         this.layoutControlItemSelectFilePath.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemSelectFilePath.Name = "layoutControlItemSelectFilePath";
         this.layoutControlItemSelectFilePath.Size = new System.Drawing.Size(561, 24);
         this.layoutControlItemSelectFilePath.TextSize = new System.Drawing.Size(162, 13);
         // 
         // layoutControlItemExpressionSelect
         // 
         this.layoutControlItemExpressionSelect.Control = this.gridControl;
         this.layoutControlItemExpressionSelect.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItemExpressionSelect.Name = "layoutControlItemExpressionSelect";
         this.layoutControlItemExpressionSelect.Size = new System.Drawing.Size(561, 561);
         this.layoutControlItemExpressionSelect.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemExpressionSelect.TextVisible = false;
         // 
         // SelectFolderAndIndividualAndExpressionFromProjectView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "SelectExportSettings";
         this.ClientSize = new System.Drawing.Size(581, 689);
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "SelectFolderAndIndividualAndExpressionFromProjectView";
         this.Text = "SelectExportSettings";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.uxLayoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnSelectFilePath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cmbSelectIndividual.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectIndividual)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelectFilePath)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemExpressionSelect)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.LabelControl descriptionLabel;
      private OSPSuite.UI.Controls.UxComboBoxEdit cmbSelectIndividual;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelectIndividual;
      private DevExpress.XtraEditors.ButtonEdit btnSelectFilePath;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelectFilePath;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private OSPSuite.UI.Controls.UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemExpressionSelect;
   }
}