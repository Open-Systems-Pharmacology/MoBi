namespace MoBi.UI.Views
{
   partial class EditTransportInSimulationView
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
         this.tabPagesControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.pnlKinetic = new DevExpress.XtraEditors.PanelControl();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.txtName = new DevExpress.XtraEditors.TextEdit();
         this.txtSource = new DevExpress.XtraEditors.TextEdit();
         this.txtTarget = new DevExpress.XtraEditors.TextEdit();
         this.txtMolecule = new DevExpress.XtraEditors.TextEdit();
         this.txtDimension = new DevExpress.XtraEditors.TextEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemTarget = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMolecule = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDimension = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlKinetic = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).BeginInit();
         this.tabPagesControl.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pnlKinetic)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSource.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtTarget.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtMolecule.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDimension.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTarget)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMolecule)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlKinetic)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlDescription)).BeginInit();
         this.SuspendLayout();
         //
         // tabPagesControl
         //
         this.tabPagesControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabPagesControl.Location = new System.Drawing.Point(0, 0);
         this.tabPagesControl.Name = "tabPagesControl";
         this.tabPagesControl.SelectedTabPage = this.tabProperties;
         this.tabPagesControl.Size = new System.Drawing.Size(924, 563);
         this.tabPagesControl.TabIndex = 0;
         this.tabPagesControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabParameters});
         //
         // tabProperties
         //
         this.tabProperties.Controls.Add(this.layoutControl1);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(918, 535);
         this.tabProperties.Text = "Properties";
         //
         // layoutControl1
         //
         this.layoutControl1.Controls.Add(this.pnlKinetic);
         this.layoutControl1.Controls.Add(this.htmlEditor);
         this.layoutControl1.Controls.Add(this.txtName);
         this.layoutControl1.Controls.Add(this.txtSource);
         this.layoutControl1.Controls.Add(this.txtTarget);
         this.layoutControl1.Controls.Add(this.txtMolecule);
         this.layoutControl1.Controls.Add(this.txtDimension);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(918, 535);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         //
         // pnlKinetic
         //
         this.pnlKinetic.Location = new System.Drawing.Point(147, 132);
         this.pnlKinetic.Name = "pnlKinetic";
         this.pnlKinetic.Size = new System.Drawing.Size(759, 367);
         this.pnlKinetic.TabIndex = 7;
         //
         // htmlEditor
         //
         this.htmlEditor.Location = new System.Drawing.Point(147, 503);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(759, 20);
         this.htmlEditor.StyleController = this.layoutControl1;
         this.htmlEditor.TabIndex = 5;
         //
         // txtName
         //
         this.txtName.Location = new System.Drawing.Point(147, 12);
         this.txtName.Name = "txtName";
         this.txtName.Size = new System.Drawing.Size(759, 20);
         this.txtName.StyleController = this.layoutControl1;
         this.txtName.TabIndex = 4;
         //
         // txtSource
         //
         this.txtSource.Location = new System.Drawing.Point(147, 36);
         this.txtSource.Name = "txtSource";
         this.txtSource.Size = new System.Drawing.Size(759, 20);
         this.txtSource.StyleController = this.layoutControl1;
         this.txtSource.TabIndex = 8;
         //
         // txtTarget
         //
         this.txtTarget.Location = new System.Drawing.Point(147, 60);
         this.txtTarget.Name = "txtTarget";
         this.txtTarget.Size = new System.Drawing.Size(759, 20);
         this.txtTarget.StyleController = this.layoutControl1;
         this.txtTarget.TabIndex = 9;
         //
         // txtMolecule
         //
         this.txtMolecule.Location = new System.Drawing.Point(147, 84);
         this.txtMolecule.Name = "txtMolecule";
         this.txtMolecule.Size = new System.Drawing.Size(759, 20);
         this.txtMolecule.StyleController = this.layoutControl1;
         this.txtMolecule.TabIndex = 10;
         //
         // txtDimension
         //
         this.txtDimension.Location = new System.Drawing.Point(147, 108);
         this.txtDimension.Name = "txtDimension";
         this.txtDimension.Size = new System.Drawing.Size(759, 20);
         this.txtDimension.StyleController = this.layoutControl1;
         this.txtDimension.TabIndex = 11;
         //
         // layoutControlGroup1
         //
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutItemSource,
            this.layoutItemTarget,
            this.layoutItemMolecule,
            this.layoutItemDimension,
            this.layoutControlKinetic,
            this.layoutControlDescription});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(918, 535);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         //
         // layoutItemName
         //
         this.layoutItemName.Control = this.txtName;
         this.layoutItemName.CustomizationFormText = "layoutItemName";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(898, 24);
         this.layoutItemName.Text = "layoutItemName";
         this.layoutItemName.TextSize = new System.Drawing.Size(132, 13);
         //
         // layoutItemSource
         //
         this.layoutItemSource.Control = this.txtSource;
         this.layoutItemSource.CustomizationFormText = "layoutItemSource";
         this.layoutItemSource.Location = new System.Drawing.Point(0, 24);
         this.layoutItemSource.Name = "layoutItemSource";
         this.layoutItemSource.Size = new System.Drawing.Size(898, 24);
         this.layoutItemSource.Text = "layoutItemSource";
         this.layoutItemSource.TextSize = new System.Drawing.Size(132, 13);
         //
         // layoutItemTarget
         //
         this.layoutItemTarget.Control = this.txtTarget;
         this.layoutItemTarget.CustomizationFormText = "layoutItemTarget";
         this.layoutItemTarget.Location = new System.Drawing.Point(0, 48);
         this.layoutItemTarget.Name = "layoutItemTarget";
         this.layoutItemTarget.Size = new System.Drawing.Size(898, 24);
         this.layoutItemTarget.Text = "layoutItemTarget";
         this.layoutItemTarget.TextSize = new System.Drawing.Size(132, 13);
         //
         // layoutItemMolecule
         //
         this.layoutItemMolecule.Control = this.txtMolecule;
         this.layoutItemMolecule.CustomizationFormText = "layoutItemMolecule";
         this.layoutItemMolecule.Location = new System.Drawing.Point(0, 72);
         this.layoutItemMolecule.Name = "layoutItemMolecule";
         this.layoutItemMolecule.Size = new System.Drawing.Size(898, 24);
         this.layoutItemMolecule.Text = "layoutItemMolecule";
         this.layoutItemMolecule.TextSize = new System.Drawing.Size(132, 13);
         //
         // layoutItemDimension
         //
         this.layoutItemDimension.Control = this.txtDimension;
         this.layoutItemDimension.CustomizationFormText = "layoutItemDimension";
         this.layoutItemDimension.Location = new System.Drawing.Point(0, 96);
         this.layoutItemDimension.Name = "layoutItemDimension";
         this.layoutItemDimension.Size = new System.Drawing.Size(898, 24);
         this.layoutItemDimension.Text = "layoutItemDimension";
         this.layoutItemDimension.TextSize = new System.Drawing.Size(132, 13);
         //
         // layoutControlKinetic
         //
         this.layoutControlKinetic.Control = this.pnlKinetic;
         this.layoutControlKinetic.CustomizationFormText = "layoutControlKinetic";
         this.layoutControlKinetic.Location = new System.Drawing.Point(0, 120);
         this.layoutControlKinetic.Name = "layoutControlKinetic";
         this.layoutControlKinetic.Size = new System.Drawing.Size(898, 371);
         this.layoutControlKinetic.Text = "layoutControlKinetic";
         this.layoutControlKinetic.TextSize = new System.Drawing.Size(132, 13);
         //
         // layoutControlDescription
         //
         this.layoutControlDescription.Control = this.htmlEditor;
         this.layoutControlDescription.CustomizationFormText = "layoutControlDescription";
         this.layoutControlDescription.Location = new System.Drawing.Point(0, 491);
         this.layoutControlDescription.Name = "layoutControlDescription";
         this.layoutControlDescription.Size = new System.Drawing.Size(898, 24);
         this.layoutControlDescription.Text = "layoutControlDescription";
         this.layoutControlDescription.TextSize = new System.Drawing.Size(132, 13);
         //
         // tabParameters
         //
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.Size = new System.Drawing.Size(918, 535);
         this.tabParameters.Text = "Parameter";
         //
         // EditTransportInSimulationView
         //
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabPagesControl);
         this.Name = "EditTransportInSimulationView";
         this.Size = new System.Drawing.Size(924, 563);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabPagesControl)).EndInit();
         this.tabPagesControl.ResumeLayout(false);
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.pnlKinetic)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSource.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtTarget.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtMolecule.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtDimension.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTarget)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMolecule)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDimension)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlKinetic)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlDescription)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl tabPagesControl;
      private DevExpress.XtraTab.XtraTabPage tabProperties;
      private DevExpress.XtraTab.XtraTabPage tabParameters;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraEditors.PanelControl pnlKinetic;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraEditors.TextEdit txtName;
      private DevExpress.XtraEditors.TextEdit txtSource;
      private DevExpress.XtraEditors.TextEdit txtTarget;
      private DevExpress.XtraEditors.TextEdit txtMolecule;
      private DevExpress.XtraEditors.TextEdit txtDimension;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTarget;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMolecule;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDimension;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlKinetic;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlDescription;
   }
}
