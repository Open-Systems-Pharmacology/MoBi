using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class EditTableFormulaView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.gridControl = new DevExpress.XtraGrid.GridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutControlItemValueGrid = new DevExpress.XtraLayout.LayoutControlItem();
         this.btnAddValuePoint = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemAddValuePoint = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkUseDerivedValues = new UxCheckEdit();
         this.layoutControlItemUseDerivedValues = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValueGrid)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddValuePoint)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUseDerivedValues.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUseDerivedValues)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.chkUseDerivedValues);
         this.layoutControl1.Controls.Add(this.btnAddValuePoint);
         this.layoutControl1.Controls.Add(this.gridControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(332, 293);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemValueGrid,
            this.layoutItemAddValuePoint,
            this.layoutControlItemUseDerivedValues});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(332, 293);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // grdValues
         // 
         this.gridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.gridControl.Location = new System.Drawing.Point(12, 38);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(308, 243);
         this.gridControl.TabIndex = 5;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // grdValuesView
         // 
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsView.ShowGroupPanel = false;
         // 
         // layoutControlItemValueGrid
         // 
         this.layoutControlItemValueGrid.Control = this.gridControl;
         this.layoutControlItemValueGrid.CustomizationFormText = "layoutControlItemValueGrid";
         this.layoutControlItemValueGrid.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItemValueGrid.Name = "layoutControlItemValueGrid";
         this.layoutControlItemValueGrid.Size = new System.Drawing.Size(312, 247);
         this.layoutControlItemValueGrid.Text = "layoutControlItemValueGrid";
         this.layoutControlItemValueGrid.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemValueGrid.TextToControlDistance = 0;
         this.layoutControlItemValueGrid.TextVisible = false;
         // 
         // btAddValuePoint
         // 
         this.btnAddValuePoint.Location = new System.Drawing.Point(168, 12);
         this.btnAddValuePoint.Name = "btnAddValuePoint";
         this.btnAddValuePoint.Size = new System.Drawing.Size(152, 22);
         this.btnAddValuePoint.StyleController = this.layoutControl1;
         this.btnAddValuePoint.TabIndex = 6;
         this.btnAddValuePoint.Text = "simpleButton1";
         // 
         // layoutControlItemAddValuePoint
         // 
         this.layoutItemAddValuePoint.Control = this.btnAddValuePoint;
         this.layoutItemAddValuePoint.CustomizationFormText = "layoutControlItemAddValuePoint";
         this.layoutItemAddValuePoint.Location = new System.Drawing.Point(156, 0);
         this.layoutItemAddValuePoint.Name = "layoutItemAddValuePoint";
         this.layoutItemAddValuePoint.Size = new System.Drawing.Size(156, 26);
         this.layoutItemAddValuePoint.Text = "layoutControlItemAddValuePoint";
         this.layoutItemAddValuePoint.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddValuePoint.TextToControlDistance = 0;
         this.layoutItemAddValuePoint.TextVisible = false;
         // 
         // ckUseDerivedValues
         // 
         this.chkUseDerivedValues.Location = new System.Drawing.Point(12, 12);
         this.chkUseDerivedValues.Name = "chkUseDerivedValues";
         this.chkUseDerivedValues.Properties.Caption = "checkEdit1";
         this.chkUseDerivedValues.Size = new System.Drawing.Size(152, 19);
         this.chkUseDerivedValues.StyleController = this.layoutControl1;
         this.chkUseDerivedValues.TabIndex = 7;
         // 
         // layoutControlItemUseDerivedValues
         // 
         this.layoutControlItemUseDerivedValues.Control = this.chkUseDerivedValues;
         this.layoutControlItemUseDerivedValues.CustomizationFormText = "layoutControlItemUseDerivedValues";
         this.layoutControlItemUseDerivedValues.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemUseDerivedValues.Name = "layoutControlItemUseDerivedValues";
         this.layoutControlItemUseDerivedValues.Size = new System.Drawing.Size(156, 26);
         this.layoutControlItemUseDerivedValues.Text = "layoutControlItemUseDerivedValues";
         this.layoutControlItemUseDerivedValues.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemUseDerivedValues.TextToControlDistance = 0;
         this.layoutControlItemUseDerivedValues.TextVisible = false;
         // 
         // EditTableFormulaView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "EditTableFormulaView";
         this.Size = new System.Drawing.Size(332, 293);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemValueGrid)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddValuePoint)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUseDerivedValues.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUseDerivedValues)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.CheckEdit chkUseDerivedValues;
      private DevExpress.XtraEditors.SimpleButton btnAddValuePoint;
      private DevExpress.XtraGrid.GridControl gridControl;
      private UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemValueGrid;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddValuePoint;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemUseDerivedValues;

   }
}
