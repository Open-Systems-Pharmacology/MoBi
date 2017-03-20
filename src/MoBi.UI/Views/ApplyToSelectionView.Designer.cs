namespace MoBi.UI.Views
{
   partial class ApplyToSelectionView
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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.lblCaption = new DevExpress.XtraEditors.LabelControl();
         this.cbSelection = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.btnSelection = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutButtonSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSelectionChoice = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCaption = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutButtonSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectionChoice)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCaption)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.lblCaption);
         this.layoutControl.Controls.Add(this.cbSelection);
         this.layoutControl.Controls.Add(this.btnSelection);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(469, 20);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // lblCaption
         // 
         this.lblCaption.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.lblCaption.Location = new System.Drawing.Point(109, 2);
         this.lblCaption.Name = "lblCaption";
         this.lblCaption.Size = new System.Drawing.Size(89, 16);
         this.lblCaption.StyleController = this.layoutControl;
         this.lblCaption.TabIndex = 10;
         this.lblCaption.Text = "lblCaption";
         // 
         // cbSelection
         // 
         this.cbSelection.Location = new System.Drawing.Point(202, 0);
         this.cbSelection.Name = "cbSelection";
         this.cbSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbSelection.Size = new System.Drawing.Size(180, 20);
         this.cbSelection.StyleController = this.layoutControl;
         this.cbSelection.TabIndex = 9;
         // 
         // btnSelection
         // 
         this.btnSelection.Location = new System.Drawing.Point(386, 0);
         this.btnSelection.Name = "btnSelection";
         this.btnSelection.Size = new System.Drawing.Size(81, 20);
         this.btnSelection.StyleController = this.layoutControl;
         this.btnSelection.TabIndex = 8;
         this.btnSelection.Text = "btnSelection";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutButtonSelection,
            this.layoutItemSelectionChoice,
            this.layoutItemCaption,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(469, 20);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutButtonSelection
         // 
         this.layoutButtonSelection.Control = this.btnSelection;
         this.layoutButtonSelection.CustomizationFormText = "layoutButtonSelection";
         this.layoutButtonSelection.Location = new System.Drawing.Point(384, 0);
         this.layoutButtonSelection.MaxSize = new System.Drawing.Size(85, 20);
         this.layoutButtonSelection.MinSize = new System.Drawing.Size(85, 20);
         this.layoutButtonSelection.Name = "layoutButtonSelection";
         this.layoutButtonSelection.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 0, 0);
         this.layoutButtonSelection.Size = new System.Drawing.Size(85, 20);
         this.layoutButtonSelection.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutButtonSelection.TextSize = new System.Drawing.Size(0, 0);
         this.layoutButtonSelection.TextVisible = false;
         // 
         // layoutItemSelectionChoice
         // 
         this.layoutItemSelectionChoice.Control = this.cbSelection;
         this.layoutItemSelectionChoice.CustomizationFormText = "layoutItemSelectionChoice";
         this.layoutItemSelectionChoice.Location = new System.Drawing.Point(200, 0);
         this.layoutItemSelectionChoice.MaxSize = new System.Drawing.Size(184, 20);
         this.layoutItemSelectionChoice.MinSize = new System.Drawing.Size(184, 20);
         this.layoutItemSelectionChoice.Name = "layoutItemSelectionChoice";
         this.layoutItemSelectionChoice.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 0, 0);
         this.layoutItemSelectionChoice.Size = new System.Drawing.Size(184, 20);
         this.layoutItemSelectionChoice.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemSelectionChoice.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSelectionChoice.TextVisible = false;
         // 
         // layoutItemCaption
         // 
         this.layoutItemCaption.Control = this.lblCaption;
         this.layoutItemCaption.CustomizationFormText = "layoutItemCaption";
         this.layoutItemCaption.Location = new System.Drawing.Point(107, 0);
         this.layoutItemCaption.MaxSize = new System.Drawing.Size(93, 20);
         this.layoutItemCaption.MinSize = new System.Drawing.Size(93, 20);
         this.layoutItemCaption.Name = "layoutItemCaption";
         this.layoutItemCaption.Size = new System.Drawing.Size(93, 20);
         this.layoutItemCaption.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemCaption.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCaption.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(107, 20);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ApplyToSelectionView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ApplyToSelectionView";
         this.Size = new System.Drawing.Size(469, 20);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutButtonSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSelectionChoice)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCaption)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.ImageComboBoxEdit cbSelection;
      private DevExpress.XtraEditors.SimpleButton btnSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutButtonSelection;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSelectionChoice;
      private DevExpress.XtraEditors.LabelControl lblCaption;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCaption;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
