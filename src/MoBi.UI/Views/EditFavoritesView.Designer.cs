namespace MoBi.UI.Views
{
   partial class EditFavoritesView
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
         this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
         this.panelParameters = new DevExpress.XtraEditors.PanelControl();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemParameters = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.buttonMoveUp = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemButtonMoveUp = new DevExpress.XtraLayout.LayoutControlItem();
         this.buttonMoveDown = new DevExpress.XtraEditors.SimpleButton();
         this.layoutItemButtonMoveDown = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveUp)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveDown)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.buttonMoveDown);
         this.layoutControl1.Controls.Add(this.buttonMoveUp);
         this.layoutControl1.Controls.Add(this.panelParameters);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(404, 399);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // panelParameters
         // 
         this.panelParameters.Location = new System.Drawing.Point(2, 2);
         this.panelParameters.Name = "panelParameters";
         this.panelParameters.Size = new System.Drawing.Size(302, 395);
         this.panelParameters.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemParameters,
            this.emptySpaceItem1,
            this.layoutItemButtonMoveUp,
            this.layoutItemButtonMoveDown});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(404, 399);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutItemParameters.Control = this.panelParameters;
         this.layoutItemParameters.Location = new System.Drawing.Point(0, 0);
         this.layoutItemParameters.Name = "layoutItemParameters";
         this.layoutItemParameters.Size = new System.Drawing.Size(306, 399);
         this.layoutItemParameters.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemParameters.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(306, 52);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(98, 347);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // buttonMoveUp
         // 
         this.buttonMoveUp.Location = new System.Drawing.Point(308, 2);
         this.buttonMoveUp.Name = "buttonMoveUp";
         this.buttonMoveUp.Size = new System.Drawing.Size(94, 22);
         this.buttonMoveUp.StyleController = this.layoutControl1;
         this.buttonMoveUp.TabIndex = 5;
         this.buttonMoveUp.Text = "buttonMoveUp";
         // 
         // layoutControlItem2
         // 
         this.layoutItemButtonMoveUp.Control = this.buttonMoveUp;
         this.layoutItemButtonMoveUp.Location = new System.Drawing.Point(306, 0);
         this.layoutItemButtonMoveUp.Name = "layoutItemButtonMoveUp";
         this.layoutItemButtonMoveUp.Size = new System.Drawing.Size(98, 26);
         this.layoutItemButtonMoveUp.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonMoveUp.TextVisible = false;
         // 
         // buttonMoveDown
         // 
         this.buttonMoveDown.Location = new System.Drawing.Point(308, 28);
         this.buttonMoveDown.Name = "buttonMoveDown";
         this.buttonMoveDown.Size = new System.Drawing.Size(94, 22);
         this.buttonMoveDown.StyleController = this.layoutControl1;
         this.buttonMoveDown.TabIndex = 6;
         this.buttonMoveDown.Text = "buttonMoveDown";
         // 
         // layoutControlItem3
         // 
         this.layoutItemButtonMoveDown.Control = this.buttonMoveDown;
         this.layoutItemButtonMoveDown.Location = new System.Drawing.Point(306, 26);
         this.layoutItemButtonMoveDown.Name = "layoutItemButtonMoveDown";
         this.layoutItemButtonMoveDown.Size = new System.Drawing.Size(98, 26);
         this.layoutItemButtonMoveDown.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonMoveDown.TextVisible = false;
         // 
         // EditFavoritesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "EditFavoritesView";
         this.Size = new System.Drawing.Size(404, 399);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemParameters)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveUp)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonMoveDown)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraEditors.PanelControl panelParameters;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemParameters;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.SimpleButton buttonMoveDown;
      private DevExpress.XtraEditors.SimpleButton buttonMoveUp;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonMoveUp;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonMoveDown;
   }
}
