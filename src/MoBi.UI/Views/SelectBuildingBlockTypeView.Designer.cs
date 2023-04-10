using System.Windows.Forms;

namespace MoBi.UI.Views
{
   partial class SelectBuildingBlockTypeView
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
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.buildingBlockSelectionComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.buildingBlockSelectionComboBoxEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 501);
         this.tablePanel.Size = new System.Drawing.Size(1431, 109);
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.buildingBlockSelectionComboBoxEdit);
         this.layoutControl1.Controls.Add(this.labelControl1);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.Root;
         this.layoutControl1.Size = new System.Drawing.Size(1431, 501);
         this.layoutControl1.TabIndex = 39;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.emptySpaceItem1,
            this.layoutControlItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1431, 501);
         this.Root.TextVisible = false;
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(12, 12);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(156, 33);
         this.labelControl1.StyleController = this.layoutControl1;
         this.labelControl1.TabIndex = 4;
         this.labelControl1.Text = "labelControl1";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.labelControl1;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1411, 37);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(912, 37);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(499, 444);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // buildingBlockSelectionComboBoxEdit
         // 
         this.buildingBlockSelectionComboBoxEdit.Location = new System.Drawing.Point(253, 49);
         this.buildingBlockSelectionComboBoxEdit.Name = "buildingBlockSelectionComboBoxEdit";
         this.buildingBlockSelectionComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.buildingBlockSelectionComboBoxEdit.Size = new System.Drawing.Size(667, 48);
         this.buildingBlockSelectionComboBoxEdit.StyleController = this.layoutControl1;
         this.buildingBlockSelectionComboBoxEdit.TabIndex = 5;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.buildingBlockSelectionComboBoxEdit;
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 37);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(912, 444);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(229, 33);
         // 
         // SelectBuildingBlockTypeView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "XtraForm1";
         this.ClientSize = new System.Drawing.Size(1431, 610);
         this.Controls.Add(this.layoutControl1);
         this.Name = "SelectBuildingBlockTypeView";
         this.Text = "XtraForm1";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.buildingBlockSelectionComboBoxEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.ComboBoxEdit buildingBlockSelectionComboBoxEdit;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
   }
}