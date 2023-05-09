namespace MoBi.UI.Views
{
   partial class LegendView
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
         this.legendItemsLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendItemsLayoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         this.SuspendLayout();
         // 
         // legendItemsLayoutControl
         // 
         this.legendItemsLayoutControl.AllowCustomization = false;
         this.legendItemsLayoutControl.AutoScroll = false;
         this.legendItemsLayoutControl.Dock = System.Windows.Forms.DockStyle.Left;
         this.legendItemsLayoutControl.Location = new System.Drawing.Point(0, 0);
         this.legendItemsLayoutControl.Name = "legendItemsLayoutControl";
         this.legendItemsLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(994, 221, 250, 350);
         this.legendItemsLayoutControl.Root = this.layoutControlGroup;
         this.legendItemsLayoutControl.Size = new System.Drawing.Size(444, 69);
         this.legendItemsLayoutControl.TabIndex = 0;
         this.legendItemsLayoutControl.Text = "layoutControl1";
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(444, 69);
         this.layoutControlGroup.Text = "layoutControlGroup1";
         this.layoutControlGroup.TextVisible = false;
         // 
         // LegendView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.Controls.Add(this.legendItemsLayoutControl);
         this.Name = "LegendView";
         this.Size = new System.Drawing.Size(454, 69);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.legendItemsLayoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl legendItemsLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;

   }
}
