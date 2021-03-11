
namespace MoBi.UI.Views
{
   partial class TagsView
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
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.btnAddTag = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlItemAddTag = new DevExpress.XtraLayout.LayoutControlItem();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutControlItemTags = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddTag)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTags)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.gridControl);
         this.uxLayoutControl.Controls.Add(this.btnAddTag);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.Root = this.Root;
         this.uxLayoutControl.Size = new System.Drawing.Size(538, 491);
         this.uxLayoutControl.TabIndex = 0;
         this.uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemAddTag,
            this.layoutControlItemTags});
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.Root.Size = new System.Drawing.Size(538, 491);
         this.Root.TextVisible = false;
         // 
         // btnAddTag
         // 
         this.btnAddTag.Location = new System.Drawing.Point(2, 2);
         this.btnAddTag.Name = "btnAddTag";
         this.btnAddTag.Size = new System.Drawing.Size(534, 22);
         this.btnAddTag.StyleController = this.uxLayoutControl;
         this.btnAddTag.TabIndex = 4;
         this.btnAddTag.Text = "btnAddTag";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItemAddTag.Control = this.btnAddTag;
         this.layoutControlItemAddTag.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemAddTag.Name = "layoutControlItemAddTag";
         this.layoutControlItemAddTag.Size = new System.Drawing.Size(538, 26);
         this.layoutControlItemAddTag.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddTag.TextVisible = false;
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(2, 28);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(534, 461);
         this.gridControl.TabIndex = 5;
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
         // layoutControlItem2
         // 
         this.layoutControlItemTags.Control = this.gridControl;
         this.layoutControlItemTags.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemTags.Name = "layoutControlItemTags";
         this.layoutControlItemTags.Size = new System.Drawing.Size(538, 465);
         this.layoutControlItemTags.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemTags.TextVisible = false;
         // 
         // TagsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Name = "TagsView";
         this.Size = new System.Drawing.Size(538, 491);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddTag)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTags)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl;
      private OSPSuite.UI.Controls.UxGridControl gridControl;
      private MoBi.UI.Views.UxGridView gridView;
      private DevExpress.XtraEditors.SimpleButton btnAddTag;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddTag;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTags;
   }
}
