using System.Windows.Forms;

namespace MoBi.UI.Views
{
    partial class BuildingBlockSelectionView
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
         this.cbBuildingBlocks = new DevExpress.XtraEditors.ComboBoxEdit();
         this.btnNew = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemNew = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemComboBox = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbBuildingBlocks.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNew)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemComboBox)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.cbBuildingBlocks);
         this.layoutControl.Controls.Add(this.btnNew);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(790, 280, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(418, 26);
         this.layoutControl.TabIndex = 2;
         this.layoutControl.Text = "layoutControl1";
         // 
         // cbBuildingBlocks
         // 
         this.cbBuildingBlocks.Location = new System.Drawing.Point(2, 3);
         this.cbBuildingBlocks.Name = "cbBuildingBlocks";
         this.cbBuildingBlocks.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbBuildingBlocks.Size = new System.Drawing.Size(344, 20);
         this.cbBuildingBlocks.StyleController = this.layoutControl;
         this.cbBuildingBlocks.TabIndex = 4;
         // 
         // btnNew
         // 
         this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnNew.Location = new System.Drawing.Point(350, 2);
         this.btnNew.Name = "btnNew";
         this.btnNew.Size = new System.Drawing.Size(66, 22);
         this.btnNew.StyleController = this.layoutControl;
         this.btnNew.TabIndex = 1;
         this.btnNew.Text = "btnNew";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemNew,
            this.layoutItemComboBox});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(418, 26);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemNew
         // 
         this.layoutItemNew.Control = this.btnNew;
         this.layoutItemNew.CustomizationFormText = "layoutControlItem2";
         this.layoutItemNew.Location = new System.Drawing.Point(348, 0);
         this.layoutItemNew.MaxSize = new System.Drawing.Size(0, 26);
         this.layoutItemNew.MinSize = new System.Drawing.Size(52, 26);
         this.layoutItemNew.Name = "layoutControlItem2";
         this.layoutItemNew.Size = new System.Drawing.Size(70, 26);
         this.layoutItemNew.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemNew.Text = "layoutControlItem2";
         this.layoutItemNew.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemNew.TextToControlDistance = 0;
         this.layoutItemNew.TextVisible = false;
         // 
         // layoutItemComboBox
         // 
         this.layoutItemComboBox.Control = this.cbBuildingBlocks;
         this.layoutItemComboBox.CustomizationFormText = "layoutItemComboBox";
         this.layoutItemComboBox.Location = new System.Drawing.Point(0, 0);
         this.layoutItemComboBox.MaxSize = new System.Drawing.Size(0, 26);
         this.layoutItemComboBox.MinSize = new System.Drawing.Size(54, 26);
         this.layoutItemComboBox.Name = "layoutItemComboBox";
         this.layoutItemComboBox.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 3, 1);
         this.layoutItemComboBox.Size = new System.Drawing.Size(348, 26);
         this.layoutItemComboBox.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemComboBox.Text = "layoutItemComboBox";
         this.layoutItemComboBox.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemComboBox.TextToControlDistance = 0;
         this.layoutItemComboBox.TextVisible = false;
         // 
         // BuildingBlockSelectionView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "BuildingBlockSelectionView";
         this.Size = new System.Drawing.Size(418, 26);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbBuildingBlocks.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemNew)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemComboBox)).EndInit();
         this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnNew;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutItemNew;
        private DevExpress.XtraEditors.ComboBoxEdit cbBuildingBlocks;
        private DevExpress.XtraLayout.LayoutControlItem layoutItemComboBox;
        private OSPSuite.UI.Controls.UxLayoutControl layoutControl;

    }
}
