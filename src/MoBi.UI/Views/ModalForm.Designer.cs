using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class ModalForm
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
         this.pnlControl = new DevExpress.XtraEditors.PanelControl();
         this.pnlButtons = new DevExpress.XtraEditors.PanelControl();
         this.btClose = new DevExpress.XtraEditors.SimpleButton();
         this.btOK = new DevExpress.XtraEditors.SimpleButton();
         this.btCanncel = new DevExpress.XtraEditors.SimpleButton();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).BeginInit();
         this.pnlButtons.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(574, 12);
         this.btnCancel.Size = new System.Drawing.Size(119, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(430, 12);
         this.btnOk.Size = new System.Drawing.Size(140, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 581);
         this.layoutControlBase.Size = new System.Drawing.Size(705, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(205, 22);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(705, 46);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(418, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(144, 26);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(562, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(123, 26);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(209, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(209, 26);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(209, 26);
         // 
         // pnlControl
         // 
         this.pnlControl.Location = new System.Drawing.Point(2, 2);
         this.pnlControl.Name = "pnlControl";
         this.pnlControl.Size = new System.Drawing.Size(701, 577);
         this.pnlControl.TabIndex = 0;
         // 
         // pnlButtons
         // 
         this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.pnlButtons.Controls.Add(this.btClose);
         this.pnlButtons.Controls.Add(this.btOK);
         this.pnlButtons.Controls.Add(this.btCanncel);
         this.pnlButtons.Location = new System.Drawing.Point(0, 591);
         this.pnlButtons.Name = "pnlButtons";
         this.pnlButtons.Size = new System.Drawing.Size(705, 36);
         this.pnlButtons.TabIndex = 1;
         // 
         // btClose
         // 
         this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btClose.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btClose.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.btClose.Location = new System.Drawing.Point(625, 5);
         this.btClose.Name = "btClose";
         this.btClose.Size = new System.Drawing.Size(75, 23);
         this.btClose.TabIndex = 3;
         this.btClose.Text = "Close";
         // 
         // btOK
         // 
         this.btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btOK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.btOK.Location = new System.Drawing.Point(544, 5);
         this.btOK.Name = "btOK";
         this.btOK.Size = new System.Drawing.Size(75, 23);
         this.btOK.TabIndex = 2;
         this.btOK.Text = "OK";
         // 
         // btCanncel
         // 
         this.btCanncel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btCanncel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btCanncel.Location = new System.Drawing.Point(625, 5);
         this.btCanncel.Name = "btCanncel";
         this.btCanncel.Size = new System.Drawing.Size(75, 23);
         this.btCanncel.TabIndex = 1;
         this.btCanncel.Text = "Cancel";
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.pnlControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(705, 581);
         this.layoutControl1.TabIndex = 34;
         this.layoutControl1.Text = "layoutControl";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(705, 581);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.pnlControl;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(705, 581);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // ModalForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btCanncel;
         this.Caption = "ModalForm";
         this.ClientSize = new System.Drawing.Size(705, 627);
         this.Controls.Add(this.layoutControl1);
         this.Controls.Add(this.pnlButtons);
         this.Name = "ModalForm";
         this.Text = "ModalForm";
         this.Controls.SetChildIndex(this.pnlButtons, 0);
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pnlButtons)).EndInit();
         this.pnlButtons.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      protected DevExpress.XtraEditors.PanelControl pnlControl;
      private DevExpress.XtraEditors.PanelControl pnlButtons;
      private DevExpress.XtraEditors.SimpleButton btClose;
      protected DevExpress.XtraEditors.SimpleButton btOK;
      private DevExpress.XtraEditors.SimpleButton btCanncel;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
   }
}