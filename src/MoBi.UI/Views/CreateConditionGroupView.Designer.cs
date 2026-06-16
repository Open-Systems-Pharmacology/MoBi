namespace MoBi.UI.Views
{
   partial class CreateConditionGroupView
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.descriptionLabelControl = new DevExpress.XtraEditors.LabelControl();
         this.operatorComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.addConditionButton = new DevExpress.XtraEditors.SimpleButton();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.descriptionLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.operatorLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.addConditionButtonLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.gridLayoutItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.addConditionEmptySpaceItem = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.operatorComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.operatorLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.addConditionButtonLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLayoutItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.addConditionEmptySpaceItem)).BeginInit();
         this.SuspendLayout();
         //
         // tablePanel
         //
         this.tablePanel.Location = new System.Drawing.Point(0, 360);
         this.tablePanel.Size = new System.Drawing.Size(580, 43);
         //
         // layoutControl
         //
         this.layoutControl.Controls.Add(this.descriptionLabelControl);
         this.layoutControl.Controls.Add(this.operatorComboBoxEdit);
         this.layoutControl.Controls.Add(this.addConditionButton);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(580, 360);
         this.layoutControl.TabIndex = 0;
         //
         // descriptionLabelControl
         //
         this.descriptionLabelControl.Location = new System.Drawing.Point(11, 11);
         this.descriptionLabelControl.Name = "descriptionLabelControl";
         this.descriptionLabelControl.Size = new System.Drawing.Size(558, 13);
         this.descriptionLabelControl.StyleController = this.layoutControl;
         this.descriptionLabelControl.TabIndex = 0;
         //
         // operatorComboBoxEdit
         //
         this.operatorComboBoxEdit.Location = new System.Drawing.Point(150, 31);
         this.operatorComboBoxEdit.Name = "operatorComboBoxEdit";
         this.operatorComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.operatorComboBoxEdit.Size = new System.Drawing.Size(419, 20);
         this.operatorComboBoxEdit.StyleController = this.layoutControl;
         this.operatorComboBoxEdit.TabIndex = 1;
         //
         // addConditionButton
         //
         this.addConditionButton.Location = new System.Drawing.Point(11, 55);
         this.addConditionButton.Name = "addConditionButton";
         this.addConditionButton.Size = new System.Drawing.Size(140, 22);
         this.addConditionButton.StyleController = this.layoutControl;
         this.addConditionButton.TabIndex = 2;
         //
         // gridControl
         //
         this.gridControl.Cursor = System.Windows.Forms.Cursors.Default;
         this.gridControl.Location = new System.Drawing.Point(11, 81);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(558, 268);
         this.gridControl.TabIndex = 3;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         //
         // gridView
         //
         this.gridView.AllowsFiltering = false;
         this.gridView.EnableColumnContextMenu = false;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = false;
         this.gridView.Name = "gridView";
         //
         // Root
         //
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.descriptionLayoutItem,
            this.operatorLayoutItem,
            this.addConditionButtonLayoutItem,
            this.addConditionEmptySpaceItem,
            this.gridLayoutItem});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);
         this.Root.Size = new System.Drawing.Size(580, 360);
         this.Root.TextVisible = false;
         //
         // descriptionLayoutItem
         //
         this.descriptionLayoutItem.Control = this.descriptionLabelControl;
         this.descriptionLayoutItem.Location = new System.Drawing.Point(0, 0);
         this.descriptionLayoutItem.Name = "descriptionLayoutItem";
         this.descriptionLayoutItem.Size = new System.Drawing.Size(560, 17);
         this.descriptionLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.descriptionLayoutItem.TextVisible = false;
         //
         // operatorLayoutItem
         //
         this.operatorLayoutItem.Control = this.operatorComboBoxEdit;
         this.operatorLayoutItem.Location = new System.Drawing.Point(0, 17);
         this.operatorLayoutItem.Name = "operatorLayoutItem";
         this.operatorLayoutItem.Size = new System.Drawing.Size(560, 24);
         this.operatorLayoutItem.TextSize = new System.Drawing.Size(135, 13);
         //
         // addConditionButtonLayoutItem
         //
         this.addConditionButtonLayoutItem.Control = this.addConditionButton;
         this.addConditionButtonLayoutItem.Location = new System.Drawing.Point(0, 41);
         this.addConditionButtonLayoutItem.Name = "addConditionButtonLayoutItem";
         this.addConditionButtonLayoutItem.Size = new System.Drawing.Size(144, 26);
         this.addConditionButtonLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.addConditionButtonLayoutItem.TextVisible = false;
         //
         // addConditionEmptySpaceItem
         //
         this.addConditionEmptySpaceItem.AllowHotTrack = false;
         this.addConditionEmptySpaceItem.Location = new System.Drawing.Point(144, 41);
         this.addConditionEmptySpaceItem.Name = "addConditionEmptySpaceItem";
         this.addConditionEmptySpaceItem.Size = new System.Drawing.Size(416, 26);
         this.addConditionEmptySpaceItem.TextSize = new System.Drawing.Size(0, 0);
         //
         // gridLayoutItem
         //
         this.gridLayoutItem.Control = this.gridControl;
         this.gridLayoutItem.Location = new System.Drawing.Point(0, 67);
         this.gridLayoutItem.Name = "gridLayoutItem";
         this.gridLayoutItem.Size = new System.Drawing.Size(560, 273);
         this.gridLayoutItem.TextSize = new System.Drawing.Size(0, 0);
         this.gridLayoutItem.TextVisible = false;
         //
         // CreateConditionGroupView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(580, 403);
         this.Controls.Add(this.layoutControl);
         this.Name = "CreateConditionGroupView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.operatorComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.descriptionLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.operatorLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.addConditionButtonLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridLayoutItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.addConditionEmptySpaceItem)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();
      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.LabelControl descriptionLabelControl;
      private DevExpress.XtraEditors.ComboBoxEdit operatorComboBoxEdit;
      private DevExpress.XtraEditors.SimpleButton addConditionButton;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private MoBi.UI.Views.UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem descriptionLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem operatorLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem addConditionButtonLayoutItem;
      private DevExpress.XtraLayout.LayoutControlItem gridLayoutItem;
      private DevExpress.XtraLayout.EmptySpaceItem addConditionEmptySpaceItem;
   }
}
