namespace DXApplication1
{
   partial class Form1
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
         this.userControl12 = new DXApplication1.UserControl1();
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
         this.splitContainerControl1.Panel2.SuspendLayout();
         this.splitContainerControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // userControl12
         // 
         this.userControl12.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.userControl12.Appearance.Options.UseBackColor = true;
         this.userControl12.Dock = System.Windows.Forms.DockStyle.Fill;
         this.userControl12.Location = new System.Drawing.Point(0, 0);
         this.userControl12.Name = "userControl12";
         this.userControl12.Size = new System.Drawing.Size(166, 284);
         this.userControl12.TabIndex = 1;
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
         this.splitContainerControl1.Name = "splitContainerControl1";
         // 
         // splitContainerControl1.Panel1
         // 
         this.splitContainerControl1.Panel1.Text = "Panel1";
         // 
         // splitContainerControl1.Panel2
         // 
         this.splitContainerControl1.Panel2.Controls.Add(this.userControl12);
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(640, 284);
         this.splitContainerControl1.SplitterPosition = 464;
         this.splitContainerControl1.TabIndex = 2;
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(640, 284);
         this.Controls.Add(this.splitContainerControl1);
         this.Name = "Form1";
         this.Text = "Form1";
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
         this.splitContainerControl1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

        #endregion

        private UserControl1 userControl12;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    }
}

