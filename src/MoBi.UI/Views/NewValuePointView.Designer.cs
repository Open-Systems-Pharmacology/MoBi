namespace MoBi.UI.Views
{
   partial class NewValuePointView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemX = new DevExpress.XtraLayout.LayoutControlItem();

         this.layoutControlItemY = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemX)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemY)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(554, 75);
         this.layoutControl1.TabIndex = 14;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemX,
            this.layoutControlItemY});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(554, 75);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // veX
         // 
         // 
         this.layoutControlItemX.CustomizationFormText = "layoutControlItemX";
         this.layoutControlItemX.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemX.Name = "layoutControlItemX";
         this.layoutControlItemX.Size = new System.Drawing.Size(534, 24);
         this.layoutControlItemX.Text = "layoutControlItemX";
         this.layoutControlItemX.TextSize = new System.Drawing.Size(93, 13);
         // 
         // veY

         // 
         // layoutControlItemY

         this.layoutControlItemY.CustomizationFormText = "layoutControlItemY";
         this.layoutControlItemY.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemY.Name = "layoutControlItemY";
         this.layoutControlItemY.Size = new System.Drawing.Size(534, 31);
         this.layoutControlItemY.Text = "layoutControlItemY";
         this.layoutControlItemY.TextSize = new System.Drawing.Size(93, 13);
         // 
         // NewValuePointView
         // 
         this.AcceptButton = null;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(554, 121);
         this.Controls.Add(this.layoutControl1);
         this.Name = "NewValuePointView";
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemX)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemY)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemX;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemY;


   }
}
