namespace MoBi.UI
{
   partial class AboutView
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutView));
         this._labelInfo = new DevExpress.XtraEditors.LabelControl();
         this._websiteLink = new DevExpress.XtraEditors.HyperLinkEdit();
         this.licenseAgreementLink = new DevExpress.XtraEditors.HyperLinkEdit();
         this.peImage = new DevExpress.XtraEditors.PictureEdit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._websiteLink.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.licenseAgreementLink.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.peImage.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 237);
         this.tablePanel.Size = new System.Drawing.Size(579, 43);
         // 
         // _labelInfo
         // 
         this._labelInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this._labelInfo.Location = new System.Drawing.Point(29, 170);
         this._labelInfo.Name = "_labelInfo";
         this._labelInfo.Size = new System.Drawing.Size(200, 13);
         this._labelInfo.TabIndex = 1;
         this._labelInfo.Text = "_labelInfo";
         // 
         // _websiteLink
         // 
         this._websiteLink.EditValue = "www.open-systems-pharmacology.org";
         this._websiteLink.Location = new System.Drawing.Point(377, 165);
         this._websiteLink.Name = "_websiteLink";
         this._websiteLink.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this._websiteLink.Properties.Appearance.Options.UseBackColor = true;
         this._websiteLink.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this._websiteLink.Size = new System.Drawing.Size(205, 18);
         this._websiteLink.TabIndex = 100;
         // 
         // licenseAgreementLink
         // 
         this.licenseAgreementLink.EditValue = "licenseAgreementLink";
         this.licenseAgreementLink.Location = new System.Drawing.Point(377, 186);
         this.licenseAgreementLink.Name = "licenseAgreementLink";
         this.licenseAgreementLink.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.licenseAgreementLink.Size = new System.Drawing.Size(229, 18);
         this.licenseAgreementLink.TabIndex = 38;
         // 
         // peImage
         // 
         this.peImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.peImage.EditValue = ((object)(resources.GetObject("peImage.EditValue")));
         this.peImage.Location = new System.Drawing.Point(9, 9);
         this.peImage.Margin = new System.Windows.Forms.Padding(0);
         this.peImage.Name = "peImage";
         this.peImage.Properties.AllowFocused = false;
         this.peImage.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.peImage.Properties.Appearance.Options.UseBackColor = true;
         this.peImage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.peImage.Properties.ShowMenu = false;
         this.peImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
         this.peImage.Properties.SvgImageColorizationMode = DevExpress.Utils.SvgImageColorizationMode.None;
         this.peImage.Size = new System.Drawing.Size(561, 153);
         this.peImage.TabIndex = 101;
         // 
         // AboutView
         // 
         this.Appearance.BackColor = System.Drawing.Color.White;
         this.Appearance.Options.UseBackColor = true;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "AboutView";
         this.ClientSize = new System.Drawing.Size(579, 280);
         this.Controls.Add(this.peImage);
         this.Controls.Add(this.licenseAgreementLink);
         this.Controls.Add(this._websiteLink);
         this.Controls.Add(this._labelInfo);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "AboutView";
         this.Text = "AboutView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this._labelInfo, 0);
         this.Controls.SetChildIndex(this._websiteLink, 0);
         this.Controls.SetChildIndex(this.licenseAgreementLink, 0);
         this.Controls.SetChildIndex(this.peImage, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._websiteLink.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.licenseAgreementLink.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.peImage.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl _labelInfo;
      private DevExpress.XtraEditors.HyperLinkEdit _websiteLink;
      private DevExpress.XtraEditors.HyperLinkEdit licenseAgreementLink;
      private DevExpress.XtraEditors.PictureEdit peImage;
   }
}