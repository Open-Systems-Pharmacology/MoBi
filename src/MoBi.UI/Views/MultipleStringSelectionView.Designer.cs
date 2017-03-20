using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class MultipleStringSelectionView 
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.selectionList = new DevExpress.XtraEditors.ListBoxControl();
         this.txtNewName = new DevExpress.XtraEditors.TextEdit();
         this.btAddNew = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemAddButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemNewName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemSelection = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.selectionList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtNewName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNewName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelection)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.lblDescription);
         this.layoutControl1.Controls.Add(this.selectionList);
         this.layoutControl1.Controls.Add(this.txtNewName);
         this.layoutControl1.Controls.Add(this.btAddNew);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(500, 216);
         this.layoutControl1.TabIndex = 2;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // lblDescription
         // 
         this.lblDescription.Location = new System.Drawing.Point(12, 12);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(63, 13);
         this.lblDescription.StyleController = this.layoutControl1;
         this.lblDescription.TabIndex = 8;
         this.lblDescription.Text = "lblDescription";
         // 
         // selectionList
         // 
         this.selectionList.Location = new System.Drawing.Point(12, 55);
         this.selectionList.Name = "selectionList";
         this.selectionList.Size = new System.Drawing.Size(476, 149);
         this.selectionList.StyleController = this.layoutControl1;
         this.selectionList.TabIndex = 7;
         // 
         // txtNewName
         // 
         this.txtNewName.Location = new System.Drawing.Point(12, 29);
         this.txtNewName.Name = "txtNewName";
         this.txtNewName.Size = new System.Drawing.Size(189, 20);
         this.txtNewName.StyleController = this.layoutControl1;
         this.txtNewName.TabIndex = 6;
         // 
         // btAddNew
         // 
         this.btAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btAddNew.Location = new System.Drawing.Point(205, 29);
         this.btAddNew.Name = "btAddNew";
         this.btAddNew.Size = new System.Drawing.Size(283, 22);
         this.btAddNew.StyleController = this.layoutControl1;
         this.btAddNew.TabIndex = 5;
         this.btAddNew.Text = "Add To List";
         this.btAddNew.Click += new System.EventHandler(this.addNewClick);
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemAddButton,
            this.layoutControlItemNewName,
            this.layoutControlItemSelection,
            this.layoutControlItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(500, 216);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemAddButton
         // 
         this.layoutControlItemAddButton.Control = this.btAddNew;
         this.layoutControlItemAddButton.ControlAlignment = System.Drawing.ContentAlignment.MiddleCenter;
         this.layoutControlItemAddButton.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItemAddButton.FillControlToClientArea = false;
         this.layoutControlItemAddButton.Location = new System.Drawing.Point(193, 17);
         this.layoutControlItemAddButton.Name = "layoutControlItemAddButton";
         this.layoutControlItemAddButton.Size = new System.Drawing.Size(287, 26);
         this.layoutControlItemAddButton.Text = "layoutControlItemAddButton";
         this.layoutControlItemAddButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemAddButton.TextToControlDistance = 0;
         this.layoutControlItemAddButton.TextVisible = false;
         // 
         // layoutControlItemNewName
         // 
         this.layoutControlItemNewName.Control = this.txtNewName;
         this.layoutControlItemNewName.CustomizationFormText = "layoutControlItemNewName";
         this.layoutControlItemNewName.Location = new System.Drawing.Point(0, 17);
         this.layoutControlItemNewName.Name = "layoutControlItem2";
         this.layoutControlItemNewName.Size = new System.Drawing.Size(193, 26);
         this.layoutControlItemNewName.Text = "layoutControlItemNewName";
         this.layoutControlItemNewName.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemNewName.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemNewName.TextToControlDistance = 0;
         this.layoutControlItemNewName.TextVisible = false;
         // 
         // layoutControlItemSelection
         // 
         this.layoutControlItemSelection.Control = this.selectionList;
         this.layoutControlItemSelection.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItemSelection.Location = new System.Drawing.Point(0, 43);
         this.layoutControlItemSelection.Name = "layoutControlItemSelection";
         this.layoutControlItemSelection.Size = new System.Drawing.Size(480, 153);
         this.layoutControlItemSelection.Text = "layoutControlItemSelection";
         this.layoutControlItemSelection.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemSelection.TextToControlDistance = 0;
         this.layoutControlItemSelection.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.lblDescription;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(480, 17);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // MultipleStringSelectionView
         // 
         this.AcceptButton = null;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(500, 262);
         this.Controls.Add(this.layoutControl1);
         this.Name = "MultipleStringSelectionView";
         this.Text = "MultipleStringSelectionView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.selectionList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtNewName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAddButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemNewName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSelection)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraEditors.TextEdit txtNewName;
      private DevExpress.XtraEditors.SimpleButton btAddNew;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemNewName;
      private DevExpress.XtraEditors.ListBoxControl selectionList;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSelection;
      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}