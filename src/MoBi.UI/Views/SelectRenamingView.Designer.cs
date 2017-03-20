using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class SelectRenamingView
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
         _gridViewBinder.Dispose();
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
         this.gridControl1 = new DevExpress.XtraGrid.GridControl();
         this.grdRenamings = new MoBi.UI.Views.UxGridView();
         this.chkShouldRename = new UxCheckEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemDependentRenames = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdRenamings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShouldRename.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDependentRenames)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.gridControl1);
         this.layoutControl1.Controls.Add(this.chkShouldRename);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(684, 528);
         this.layoutControl1.TabIndex = 1;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // gridControl1
         // 
         this.gridControl1.Location = new System.Drawing.Point(12, 35);
         this.gridControl1.MainView = this.grdRenamings;
         this.gridControl1.Name = "gridControl1";
         this.gridControl1.Size = new System.Drawing.Size(660, 481);
         this.gridControl1.TabIndex = 5;
         this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdRenamings});
         // 
         // grdRenamings
         // 
         this.grdRenamings.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.grdRenamings.GridControl = this.gridControl1;
         this.grdRenamings.Name = "grdRenamings";
         this.grdRenamings.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.grdRenamings.OptionsNavigation.AutoFocusNewRow = true;
         this.grdRenamings.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.grdRenamings.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.grdRenamings.RowsInsertable = false;
         this.grdRenamings.ShouldUseColorForDisabledCell = true;
         this.grdRenamings.ShowColumnChooser = false;
         this.grdRenamings.ShowRowIndicator = false;
         // 
         // chkShouldRename
         // 
         this.chkShouldRename.Location = new System.Drawing.Point(12, 12);
         this.chkShouldRename.Name = "chkShouldRename";
         this.chkShouldRename.Properties.Caption = "chkShouldRename";
         this.chkShouldRename.Size = new System.Drawing.Size(660, 19);
         this.chkShouldRename.StyleController = this.layoutControl1;
         this.chkShouldRename.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItemDependentRenames});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(684, 528);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkShouldRename;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(664, 23);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItemDependentRenames
         // 
         this.layoutControlItemDependentRenames.Control = this.gridControl1;
         this.layoutControlItemDependentRenames.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItemDependentRenames.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItemDependentRenames.Name = "layoutControlItemDependentRenames";
         this.layoutControlItemDependentRenames.Size = new System.Drawing.Size(664, 485);
         this.layoutControlItemDependentRenames.Text = "layoutControlItemDependentRenames";
         this.layoutControlItemDependentRenames.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemDependentRenames.TextToControlDistance = 0;
         this.layoutControlItemDependentRenames.TextVisible = false;
         // 
         // SelectRenamingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(684, 574);
         this.Controls.Add(this.layoutControl1);
         this.Name = "SelectRenamingView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdRenamings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShouldRename.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDependentRenames)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.CheckEdit chkShouldRename;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraGrid.GridControl gridControl1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDependentRenames;
      private UxGridView grdRenamings;

   }
}
