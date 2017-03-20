namespace MoBi.UI.Views
{
   partial class EditChartTemplateManagerView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.panelControl = new DevExpress.XtraEditors.PanelControl();
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
         this.saveButton = new DevExpress.XtraEditors.SimpleButton();
         this.editButton = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.editButtonControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.saveButtonControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.cancelButtonControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.editButtonControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveButtonControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cancelButtonControlItem)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl
         // 
         this.panelControl.Location = new System.Drawing.Point(12, 38);
         this.panelControl.Name = "panelControl";
         this.panelControl.Size = new System.Drawing.Size(407, 237);
         this.panelControl.TabIndex = 0;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.cancelButton);
         this.layoutControl.Controls.Add(this.saveButton);
         this.layoutControl.Controls.Add(this.editButton);
         this.layoutControl.Controls.Add(this.panelControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(431, 287);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // cancelButton
         // 
         this.cancelButton.Location = new System.Drawing.Point(170, 12);
         this.cancelButton.Name = "cancelButton";
         this.cancelButton.Size = new System.Drawing.Size(73, 22);
         this.cancelButton.StyleController = this.layoutControl;
         this.cancelButton.TabIndex = 6;
         this.cancelButton.Text = "cancelButton";
         // 
         // saveButton
         // 
         this.saveButton.Location = new System.Drawing.Point(100, 12);
         this.saveButton.Name = "saveButton";
         this.saveButton.Size = new System.Drawing.Size(66, 22);
         this.saveButton.StyleController = this.layoutControl;
         this.saveButton.TabIndex = 5;
         this.saveButton.Text = "saveButton";
         // 
         // editButton
         // 
         this.editButton.Location = new System.Drawing.Point(12, 12);
         this.editButton.Name = "editButton";
         this.editButton.Size = new System.Drawing.Size(84, 22);
         this.editButton.StyleController = this.layoutControl;
         this.editButton.TabIndex = 4;
         this.editButton.Text = "editButton";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.editButtonControlItem,
            this.saveButtonControlItem,
            this.cancelButtonControlItem,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(431, 287);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.panelControl;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 26);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(411, 241);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // editButtonControlItem
         // 
         this.editButtonControlItem.Control = this.editButton;
         this.editButtonControlItem.CustomizationFormText = "layoutControlItem2";
         this.editButtonControlItem.Location = new System.Drawing.Point(0, 0);
         this.editButtonControlItem.Name = "editButtonControlItem";
         this.editButtonControlItem.Size = new System.Drawing.Size(88, 26);
         this.editButtonControlItem.Text = "editButtonControlItem";
         this.editButtonControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.editButtonControlItem.TextToControlDistance = 0;
         this.editButtonControlItem.TextVisible = false;
         // 
         // saveButtonControlItem
         // 
         this.saveButtonControlItem.Control = this.saveButton;
         this.saveButtonControlItem.CustomizationFormText = "layoutControlItem3";
         this.saveButtonControlItem.Location = new System.Drawing.Point(88, 0);
         this.saveButtonControlItem.Name = "saveButtonControlItem";
         this.saveButtonControlItem.Size = new System.Drawing.Size(70, 26);
         this.saveButtonControlItem.Text = "saveButtonControlItem";
         this.saveButtonControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.saveButtonControlItem.TextToControlDistance = 0;
         this.saveButtonControlItem.TextVisible = false;
         // 
         // cancelButtonControlItem
         // 
         this.cancelButtonControlItem.Control = this.cancelButton;
         this.cancelButtonControlItem.CustomizationFormText = "layoutControlItem4";
         this.cancelButtonControlItem.Location = new System.Drawing.Point(158, 0);
         this.cancelButtonControlItem.Name = "cancelButtonControlItem";
         this.cancelButtonControlItem.Size = new System.Drawing.Size(77, 26);
         this.cancelButtonControlItem.Text = "cancelButtonControlItem";
         this.cancelButtonControlItem.TextSize = new System.Drawing.Size(0, 0);
         this.cancelButtonControlItem.TextToControlDistance = 0;
         this.cancelButtonControlItem.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(235, 0);
         this.emptySpaceItem1.MinSize = new System.Drawing.Size(104, 24);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(176, 26);
         this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // MoBiChartTemplateManagerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "EditChartTemplateManagerView";
         this.Size = new System.Drawing.Size(431, 287);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.editButtonControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.saveButtonControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cancelButtonControlItem)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.SimpleButton saveButton;
      private DevExpress.XtraEditors.SimpleButton editButton;
      private DevExpress.XtraLayout.LayoutControlItem editButtonControlItem;
      private DevExpress.XtraLayout.LayoutControlItem saveButtonControlItem;
      private DevExpress.XtraEditors.SimpleButton cancelButton;
      private DevExpress.XtraLayout.LayoutControlItem cancelButtonControlItem;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
