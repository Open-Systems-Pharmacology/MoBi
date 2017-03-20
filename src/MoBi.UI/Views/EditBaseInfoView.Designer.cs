namespace MoBi.UI.Views
{
   partial class EditBaseInfoView
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
         this.components = new System.ComponentModel.Container();
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.lblName = new DevExpress.XtraEditors.LabelControl();
         this.htmlEditor = new DevExpress.XtraEditors.MemoEdit();
         this.bttxtName = new DevExpress.XtraEditors.ButtonEdit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.bttxtName.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // lblDescription
         // 
         this.lblDescription.Location = new System.Drawing.Point(3, 28);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(53, 13);
         this.lblDescription.TabIndex = 7;
         this.lblDescription.Text = "Description:";
         // 
         // lblName
         // 
         this.lblName.Location = new System.Drawing.Point(3, 6);
         this.lblName.Name = "lblName";
         this.lblName.Size = new System.Drawing.Size(27, 13);
         this.lblName.TabIndex = 6;
         this.lblName.Text = "Name:";
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(3, 51);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Size = new System.Drawing.Size(285, 107);
         this.htmlEditor.TabIndex = 4;
         // 
         // errorProvider
         // 
         // 
         // bttxtName
         // 
         this.bttxtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.bttxtName.Location = new System.Drawing.Point(70, 3);
         this.bttxtName.Name = "bttxtName";
         this.bttxtName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.bttxtName.Size = new System.Drawing.Size(218, 20);
         this.bttxtName.TabIndex = 0;
         this.bttxtName.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.bttxtName_ButtonClick);
         // 
         // EditBaseInfoView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.bttxtName);
         this.Controls.Add(this.lblDescription);
         this.Controls.Add(this.lblName);
         this.Controls.Add(this.htmlEditor);
         this.Name = "EditBaseInfoView";
         this.Size = new System.Drawing.Size(298, 165);
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.bttxtName.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraEditors.LabelControl lblName;
      private DevExpress.XtraEditors.MemoEdit htmlEditor;
      private DevExpress.XtraEditors.ButtonEdit bttxtName;
   }
}
