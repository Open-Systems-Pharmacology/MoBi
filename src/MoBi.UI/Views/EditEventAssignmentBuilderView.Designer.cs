using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class EditEventAssignmentBuilderView
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
         this.btnName = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.pnlFormula = new DevExpress.XtraEditors.PanelControl();
         this.btnTargetPath = new DevExpress.XtraEditors.ButtonEdit();
         this.chkUseAsValue = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemChangedEntity = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupAssignment = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlFormula)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnTargetPath.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUseAsValue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChangedEntity)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupAssignment)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         this.SuspendLayout();
         // 
         // btName
         // 
         this.btnName.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.btnName.Location = new System.Drawing.Point(128, 2);
         this.btnName.Name = "btnName";
         this.btnName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btnName.Size = new System.Drawing.Size(701, 20);
         this.btnName.StyleController = this.layoutControl;
         this.btnName.TabIndex = 0;
         // 
         // layoutControl1
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.htmlEditor);
         this.layoutControl.Controls.Add(this.pnlFormula);
         this.layoutControl.Controls.Add(this.btnTargetPath);
         this.layoutControl.Controls.Add(this.chkUseAsValue);
         this.layoutControl.Controls.Add(this.btnName);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(807, 326, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(831, 555);
         this.layoutControl.TabIndex = 19;
         this.layoutControl.Text = "layoutControl1";
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(128, 533);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(701, 20);
         this.htmlEditor.StyleController = this.layoutControl;
         this.htmlEditor.TabIndex = 18;
         // 
         // pnlFormula
         // 
         this.pnlFormula.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.pnlFormula.Location = new System.Drawing.Point(14, 103);
         this.pnlFormula.Name = "pnlFormula";
         this.pnlFormula.Size = new System.Drawing.Size(803, 414);
         this.pnlFormula.TabIndex = 1;
         // 
         // btbTargetPath
         // 
         this.btnTargetPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btnTargetPath.Location = new System.Drawing.Point(128, 26);
         this.btnTargetPath.Name = "btnTargetPath";
         this.btnTargetPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btnTargetPath.Size = new System.Drawing.Size(701, 20);
         this.btnTargetPath.StyleController = this.layoutControl;
         this.btnTargetPath.TabIndex = 16;
         // 
         // chkUseAsValue
         // 
         this.chkUseAsValue.AllowClicksOutsideControlArea = false;
         this.chkUseAsValue.Location = new System.Drawing.Point(14, 80);
         this.chkUseAsValue.Name = "chkUseAsValue";
         this.chkUseAsValue.Properties.Caption = "Use assingment as value";
         this.chkUseAsValue.Size = new System.Drawing.Size(803, 19);
         this.chkUseAsValue.StyleController = this.layoutControl;
         this.chkUseAsValue.TabIndex = 0;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutItemChangedEntity,
            this.layoutItemDescription,
            this.layoutGroupAssignment});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(831, 555);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btnName;
         this.layoutItemName.CustomizationFormText = "layoutItemName";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(831, 24);
         this.layoutItemName.TextSize = new System.Drawing.Size(123, 13);
         // 
         // layoutItemChangedEntity
         // 
         this.layoutItemChangedEntity.Control = this.btnTargetPath;
         this.layoutItemChangedEntity.CustomizationFormText = "layoutItemChangedEntity";
         this.layoutItemChangedEntity.Location = new System.Drawing.Point(0, 24);
         this.layoutItemChangedEntity.Name = "layoutItemChangedEntity";
         this.layoutItemChangedEntity.Size = new System.Drawing.Size(831, 24);
         this.layoutItemChangedEntity.TextSize = new System.Drawing.Size(123, 13);
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "layoutItemDescription";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 531);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(831, 24);
         this.layoutItemDescription.TextSize = new System.Drawing.Size(123, 13);
         // 
         // layoutGroupAssignment
         // 
         this.layoutGroupAssignment.CustomizationFormText = "layoutGroupAssignment";
         this.layoutGroupAssignment.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem4});
         this.layoutGroupAssignment.Location = new System.Drawing.Point(0, 48);
         this.layoutGroupAssignment.Name = "layoutGroupAssignment";
         this.layoutGroupAssignment.Size = new System.Drawing.Size(831, 483);
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.chkUseAsValue;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(807, 23);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.pnlFormula;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(807, 418);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // EditEventAssignmentBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditEventAssignmentBuilderView";
         this.Size = new System.Drawing.Size(831, 555);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlFormula)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btnTargetPath.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkUseAsValue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemChangedEntity)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupAssignment)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.ButtonEdit btnName;
      private DevExpress.XtraEditors.ButtonEdit btnTargetPath;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraEditors.PanelControl pnlFormula;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemChangedEntity;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupAssignment;
      private UxCheckEdit chkUseAsValue;
   }
}
