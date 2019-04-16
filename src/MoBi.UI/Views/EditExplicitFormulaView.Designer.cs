namespace MoBi.UI.Views
{
   partial class EditExplicitFormulaView
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
         this.grdObjectPaths = new OSPSuite.UI.Controls.UxGridControl();
         this.gridViewReferencePaths = new MoBi.UI.Views.UxGridView();
         this.txtFormulaString = new DevExpress.XtraEditors.TextEdit();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemFormulaPathGrid = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFormulaString = new DevExpress.XtraLayout.LayoutControlItem();
         this.panelReferencePaths = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdObjectPaths)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewReferencePaths)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFormulaString.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaPathGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaString)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelReferencePaths)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // grdObjectPaths
         // 
         this.grdObjectPaths.Location = new System.Drawing.Point(2, 2);
         this.grdObjectPaths.MainView = this.gridViewReferencePaths;
         this.grdObjectPaths.Name = "grdObjectPaths";
         this.grdObjectPaths.Size = new System.Drawing.Size(599, 179);
         this.grdObjectPaths.TabIndex = 7;
         this.grdObjectPaths.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewReferencePaths});
         // 
         // gridViewReferencePaths
         // 
         this.gridViewReferencePaths.AllowsFiltering = true;
         this.gridViewReferencePaths.EnableColumnContextMenu = true;
         this.gridViewReferencePaths.GridControl = this.grdObjectPaths;
         this.gridViewReferencePaths.MultiSelect = false;
         this.gridViewReferencePaths.Name = "gridViewReferencePaths";
         this.gridViewReferencePaths.OptionsView.ShowGroupPanel = false;
         // 
         // txtFormulaString
         // 
         this.txtFormulaString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtFormulaString.Location = new System.Drawing.Point(123, 400);
         this.txtFormulaString.Name = "txtFormulaString";
         this.txtFormulaString.Size = new System.Drawing.Size(478, 20);
         this.txtFormulaString.StyleController = this.layoutControl1;
         this.txtFormulaString.TabIndex = 6;
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.panelReferencePaths);
         this.layoutControl1.Controls.Add(this.txtFormulaString);
         this.layoutControl1.Controls.Add(this.grdObjectPaths);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup;
         this.layoutControl1.Size = new System.Drawing.Size(603, 422);
         this.layoutControl1.TabIndex = 8;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemFormulaPathGrid,
            this.layoutItemFormulaString,
            this.layoutControlItem1});
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(603, 422);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutItemFormulaPathGrid
         // 
         this.layoutItemFormulaPathGrid.Control = this.grdObjectPaths;
         this.layoutItemFormulaPathGrid.CustomizationFormText = "layoutItemFormulaPathGrid";
         this.layoutItemFormulaPathGrid.Location = new System.Drawing.Point(0, 0);
         this.layoutItemFormulaPathGrid.Name = "layoutItemFormulaPathGrid";
         this.layoutItemFormulaPathGrid.Size = new System.Drawing.Size(603, 183);
         this.layoutItemFormulaPathGrid.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemFormulaPathGrid.TextVisible = false;
         // 
         // layoutItemFormulaString
         // 
         this.layoutItemFormulaString.Control = this.txtFormulaString;
         this.layoutItemFormulaString.CustomizationFormText = "layoutItemFormulaString";
         this.layoutItemFormulaString.Location = new System.Drawing.Point(0, 398);
         this.layoutItemFormulaString.Name = "layoutItemFormulaString";
         this.layoutItemFormulaString.Size = new System.Drawing.Size(603, 24);
         this.layoutItemFormulaString.TextSize = new System.Drawing.Size(118, 13);
         // 
         // panelReferencePaths
         // 
         this.panelReferencePaths.Location = new System.Drawing.Point(2, 185);
         this.panelReferencePaths.Name = "panelReferencePaths";
         this.panelReferencePaths.Size = new System.Drawing.Size(599, 211);
         this.panelReferencePaths.TabIndex = 8;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelReferencePaths;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 183);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(603, 215);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // EditExplicitFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "EditExplicitFormulaView";
         this.Size = new System.Drawing.Size(603, 422);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdObjectPaths)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridViewReferencePaths)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtFormulaString.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaPathGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFormulaString)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelReferencePaths)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private MoBi.UI.Views.UxGridView gridViewReferencePaths;
      private DevExpress.XtraEditors.TextEdit txtFormulaString;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaPathGrid;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFormulaString;
      private DevExpress.XtraEditors.PanelControl panelReferencePaths;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private OSPSuite.UI.Controls.UxGridControl grdObjectPaths;
   }
}
