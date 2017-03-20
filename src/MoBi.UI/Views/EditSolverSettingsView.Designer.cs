namespace MoBi.UI.Views
{
   partial class EditSolverSettingsView
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
         this.components = new System.ComponentModel.Container();
         this.cbSolver = new DevExpress.XtraEditors.ComboBoxEdit();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.gridControl = new DevExpress.XtraGrid.GridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemSolver = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.toolTipController = new DevExpress.Utils.DefaultToolTipController(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.cbSolver.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSolver)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).BeginInit();
         this.SuspendLayout();
         // 
         // cbSolver
         // 
         this.cbSolver.Location = new System.Drawing.Point(88, 2);
         this.cbSolver.Name = "cbSolver";
         this.cbSolver.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbSolver.Size = new System.Drawing.Size(762, 20);
         this.cbSolver.StyleController = this.layoutControl;
         this.cbSolver.TabIndex = 0;
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.cbSolver);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(645, 301, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(852, 511);
         this.layoutControl.TabIndex = 3;
         this.layoutControl.Text = "layoutControl1";
         // 
         // gridControl
         // 
         this.gridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.gridControl.Location = new System.Drawing.Point(5, 48);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(842, 458);
         this.gridControl.TabIndex = 2;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsView.ShowIndicator = false;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemSolver,
            this.layoutGroup});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(852, 511);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemSolver
         // 
         this.layoutItemSolver.Control = this.cbSolver;
         this.layoutItemSolver.CustomizationFormText = "layoutItemSolver";
         this.layoutItemSolver.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSolver.Name = "layoutItemSolver";
         this.layoutItemSolver.Size = new System.Drawing.Size(852, 24);
         this.layoutItemSolver.Text = "layoutItemSolver";
         this.layoutItemSolver.TextSize = new System.Drawing.Size(82, 13);
         // 
         // layoutGroup
         // 
         this.layoutGroup.CustomizationFormText = "layoutGroup";
         this.layoutGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemParameters});
         this.layoutGroup.Location = new System.Drawing.Point(0, 24);
         this.layoutGroup.Name = "layoutGroup";
         this.layoutGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutGroup.Size = new System.Drawing.Size(852, 487);
         this.layoutGroup.Text = "layoutGroup";
         // 
         // layoutItemParameters
         // 
         this.layoutItemParameters.Control = this.gridControl;
         this.layoutItemParameters.CustomizationFormText = "layoutItemParameters";
         this.layoutItemParameters.Location = new System.Drawing.Point(0, 0);
         this.layoutItemParameters.Name = "layoutItemParameters";
         this.layoutItemParameters.Size = new System.Drawing.Size(846, 462);
         this.layoutItemParameters.Text = "layoutItemParameters";
         this.layoutItemParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemParameters.TextToControlDistance = 0;
         this.layoutItemParameters.TextVisible = false;
         // 
         // EditSolverSettingsView
         // 
         this.toolTipController.SetAllowHtmlText(this, DevExpress.Utils.DefaultBoolean.Default);
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditSolverSettingsView";
         this.Size = new System.Drawing.Size(852, 511);
         ((System.ComponentModel.ISupportInitialize)(this.cbSolver.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSolver)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.ComboBoxEdit cbSolver;
      private DevExpress.XtraGrid.GridControl gridControl;
      private MoBi.UI.Views.UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSolver;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameters;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroup;
      private DevExpress.Utils.DefaultToolTipController toolTipController;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
   }
}
