namespace MoBi.UI.Views
{
   partial class EditIndividualParameterView
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

         disposeBinders();
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnFindParameter = new DevExpress.XtraEditors.SimpleButton();
         this.lblWarning = new DevExpress.XtraEditors.LabelControl();
         this.textEditName = new DevExpress.XtraEditors.TextEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlGroupWarning = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemWarning = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemFindParameter = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).BeginInit();
         this.uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupWarning)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemWarning)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFindParameter)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl
         // 
         this.uxLayoutControl.AllowCustomization = false;
         this.uxLayoutControl.Controls.Add(this.btnFindParameter);
         this.uxLayoutControl.Controls.Add(this.lblWarning);
         this.uxLayoutControl.Controls.Add(this.textEditName);
         this.uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl.Name = "uxLayoutControl";
         this.uxLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1020, 24, 650, 400);
         this.uxLayoutControl.Root = this.Root;
         this.uxLayoutControl.Size = new System.Drawing.Size(546, 316);
         this.uxLayoutControl.TabIndex = 0;
         this.uxLayoutControl.Text = "uxLayoutControl";
         // 
         // btnFindParameter
         // 
         this.btnFindParameter.Location = new System.Drawing.Point(24, 86);
         this.btnFindParameter.Name = "btnFindParameter";
         this.btnFindParameter.Size = new System.Drawing.Size(246, 22);
         this.btnFindParameter.StyleController = this.uxLayoutControl;
         this.btnFindParameter.TabIndex = 11;
         this.btnFindParameter.Text = "simpleButton1";
         // 
         // lblWarning
         // 
         this.lblWarning.Location = new System.Drawing.Point(24, 69);
         this.lblWarning.Name = "lblWarning";
         this.lblWarning.Size = new System.Drawing.Size(50, 13);
         this.lblWarning.StyleController = this.uxLayoutControl;
         this.lblWarning.TabIndex = 10;
         this.lblWarning.Text = "lblWarning";
         // 
         // textEditName
         // 
         this.textEditName.Location = new System.Drawing.Point(138, 12);
         this.textEditName.Name = "textEditName";
         this.textEditName.Size = new System.Drawing.Size(396, 20);
         this.textEditName.StyleController = this.uxLayoutControl;
         this.textEditName.TabIndex = 0;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemName,
            this.layoutControlGroupWarning});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(546, 316);
         this.Root.TextVisible = false;
         // 
         // layoutControlItemName
         // 
         this.layoutControlItemName.Control = this.textEditName;
         this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemName.Name = "layoutControlItemName";
         this.layoutControlItemName.Size = new System.Drawing.Size(526, 24);
         this.layoutControlItemName.TextSize = new System.Drawing.Size(114, 13);
         // 
         // layoutControlGroupWarning
         // 
         this.layoutControlGroupWarning.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemWarning,
            this.layoutControlItemFindParameter,
            this.emptySpaceItem1});
         this.layoutControlGroupWarning.Location = new System.Drawing.Point(0, 24);
         this.layoutControlGroupWarning.Name = "layoutControlGroupWarning";
         this.layoutControlGroupWarning.Size = new System.Drawing.Size(526, 272);
         // 
         // layoutControlItemWarning
         // 
         this.layoutControlItemWarning.Control = this.lblWarning;
         this.layoutControlItemWarning.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemWarning.Name = "layoutControlItemWarning";
         this.layoutControlItemWarning.Size = new System.Drawing.Size(502, 17);
         this.layoutControlItemWarning.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemWarning.TextVisible = false;
         // 
         // layoutControlItemFindParameter
         // 
         this.layoutControlItemFindParameter.Control = this.btnFindParameter;
         this.layoutControlItemFindParameter.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItemFindParameter.Name = "layoutControlItemFindParameter";
         this.layoutControlItemFindParameter.Size = new System.Drawing.Size(250, 210);
         this.layoutControlItemFindParameter.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemFindParameter.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(250, 17);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(252, 210);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // EditIndividualParameterView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl);
         this.Name = "EditIndividualParameterView";
         this.Size = new System.Drawing.Size(546, 316);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl)).EndInit();
         this.uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.textEditName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupWarning)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemWarning)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemFindParameter)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraEditors.TextEdit textEditName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
      private DevExpress.XtraEditors.LabelControl lblWarning;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemWarning;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroupWarning;
      private DevExpress.XtraEditors.SimpleButton btnFindParameter;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemFindParameter;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}
