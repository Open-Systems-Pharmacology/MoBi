namespace MoBi.UI.Views
{
   partial class EditApplicationMoleculeBuilderView
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
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.lblName = new DevExpress.XtraEditors.LabelControl();
         this.btContainerPath = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.panelFormula = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemContainerName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupFormula = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemPanelFormula = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btContainerPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanelFormula)).BeginInit();
         this.SuspendLayout();
         // 
         // lblDescription
         // 
         this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.lblDescription.Location = new System.Drawing.Point(-138, 511);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(57, 13);
         this.lblDescription.TabIndex = 13;
         this.lblDescription.Text = "Description:";
         // 
         // lblName
         // 
         this.lblName.Location = new System.Drawing.Point(-138, -81);
         this.lblName.Name = "lblName";
         this.lblName.Size = new System.Drawing.Size(31, 13);
         this.lblName.TabIndex = 12;
         this.lblName.Text = "Name:";
         // 
         // btContainerPath
         // 
         this.btContainerPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btContainerPath.Location = new System.Drawing.Point(131, 2);
         this.btContainerPath.Name = "btContainerPath";
         this.btContainerPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btContainerPath.Properties.ReadOnly = true;
         this.btContainerPath.Size = new System.Drawing.Size(525, 20);
         this.btContainerPath.StyleController = this.layoutControl;
         this.btContainerPath.TabIndex = 19;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Controls.Add(this.panelFormula);
         this.layoutControl.Controls.Add(this.btContainerPath);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(658, 538);
         this.layoutControl.TabIndex = 23;
         this.layoutControl.Text = "layoutControl1";
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(131, 516);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(525, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 22;
         // 
         // panelFormula
         // 
         this.panelFormula.Location = new System.Drawing.Point(14, 56);
         this.panelFormula.Name = "panelFormula";
         this.panelFormula.Size = new System.Drawing.Size(630, 444);
         this.panelFormula.TabIndex = 20;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemContainerName,
            this.layoutItemDescription,
            this.layoutGroupFormula});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(658, 538);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemContainerName
         // 
         this.layoutItemContainerName.Control = this.btContainerPath;
         this.layoutItemContainerName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemContainerName.Name = "layoutItemContainerName";
         this.layoutItemContainerName.Size = new System.Drawing.Size(658, 24);
         this.layoutItemContainerName.TextSize = new System.Drawing.Size(126, 13);
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 514);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(658, 24);
         this.layoutItemDescription.TextSize = new System.Drawing.Size(126, 13);
         // 
         // layoutGroupFormula
         // 
         this.layoutGroupFormula.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemPanelFormula});
         this.layoutGroupFormula.Location = new System.Drawing.Point(0, 24);
         this.layoutGroupFormula.Name = "layoutGroupFormula";
         this.layoutGroupFormula.Size = new System.Drawing.Size(658, 490);
         // 
         // layoutItemPanelFormula
         // 
         this.layoutItemPanelFormula.Control = this.panelFormula;
         this.layoutItemPanelFormula.Location = new System.Drawing.Point(0, 0);
         this.layoutItemPanelFormula.Name = "layoutItemPanelFormula";
         this.layoutItemPanelFormula.Size = new System.Drawing.Size(634, 448);
         this.layoutItemPanelFormula.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemPanelFormula.TextVisible = false;
         // 
         // EditApplicationMoleculeBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Controls.Add(this.lblDescription);
         this.Controls.Add(this.lblName);
         this.Name = "EditApplicationMoleculeBuilderView";
         this.Size = new System.Drawing.Size(658, 538);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btContainerPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemContainerName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanelFormula)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraEditors.LabelControl lblName;
      private DevExpress.XtraEditors.ButtonEdit btContainerPath;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraEditors.PanelControl panelFormula;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemContainerName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPanelFormula;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupFormula;

   }
}
