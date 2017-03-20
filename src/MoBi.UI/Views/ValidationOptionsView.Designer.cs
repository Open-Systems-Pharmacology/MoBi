using OSPSuite.UI.Controls;
using MoBi.Presentation.Presenter;

namespace MoBi.UI.Views
{
   
   partial class ValidationOptionsView
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkShowPKSimWarnings = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkShowUnableToCalculateWarnings = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkValidateDimensions = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkValiadatePkSimStandardObserver = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemPKSIMWarnings = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkValidateRules = new DevExpress.XtraEditors.CheckEdit();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowPKSimWarnings.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowUnableToCalculateWarnings.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateDimensions.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValiadatePkSimStandardObserver.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPKSIMWarnings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateRules.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.AllowCustomization = false;
         this.layoutControl1.Controls.Add(this.chkValidateRules);
         this.layoutControl1.Controls.Add(this.chkShowPKSimWarnings);
         this.layoutControl1.Controls.Add(this.chkShowUnableToCalculateWarnings);
         this.layoutControl1.Controls.Add(this.chkValidateDimensions);
         this.layoutControl1.Controls.Add(this.chkValiadatePkSimStandardObserver);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(150, 150);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // chkShowPKSimWarnings
         // 
         this.chkShowPKSimWarnings.AllowClicksOutsideControlArea = false;
         this.chkShowPKSimWarnings.Location = new System.Drawing.Point(12, 81);
         this.chkShowPKSimWarnings.Name = "chkShowPKSimWarnings";
         this.chkShowPKSimWarnings.Properties.Caption = "checkEdit4";
         this.chkShowPKSimWarnings.Size = new System.Drawing.Size(126, 19);
         this.chkShowPKSimWarnings.StyleController = this.layoutControl1;
         this.chkShowPKSimWarnings.TabIndex = 7;
         // 
         // chkShowUnableToCalculateWarnings
         // 
         this.chkShowUnableToCalculateWarnings.AllowClicksOutsideControlArea = false;
         this.chkShowUnableToCalculateWarnings.Location = new System.Drawing.Point(12, 58);
         this.chkShowUnableToCalculateWarnings.Name = "chkShowUnableToCalculateWarnings";
         this.chkShowUnableToCalculateWarnings.Properties.Caption = "checkEdit3";
         this.chkShowUnableToCalculateWarnings.Size = new System.Drawing.Size(126, 19);
         this.chkShowUnableToCalculateWarnings.StyleController = this.layoutControl1;
         this.chkShowUnableToCalculateWarnings.TabIndex = 6;
         // 
         // chkValidateDimensions
         // 
         this.chkValidateDimensions.AllowClicksOutsideControlArea = false;
         this.chkValidateDimensions.Location = new System.Drawing.Point(12, 35);
         this.chkValidateDimensions.Name = "chkValidateDimensions";
         this.chkValidateDimensions.Properties.Caption = "checkEdit2";
         this.chkValidateDimensions.Size = new System.Drawing.Size(126, 19);
         this.chkValidateDimensions.StyleController = this.layoutControl1;
         this.chkValidateDimensions.TabIndex = 5;
         // 
         // chkValiadatePkSimStandardObserver
         // 
         this.chkValiadatePkSimStandardObserver.AllowClicksOutsideControlArea = false;
         this.chkValiadatePkSimStandardObserver.Location = new System.Drawing.Point(12, 12);
         this.chkValiadatePkSimStandardObserver.Name = "chkValiadatePkSimStandardObserver";
         this.chkValiadatePkSimStandardObserver.Properties.Caption = "checkEdit1";
         this.chkValiadatePkSimStandardObserver.Size = new System.Drawing.Size(126, 19);
         this.chkValiadatePkSimStandardObserver.StyleController = this.layoutControl1;
         this.chkValiadatePkSimStandardObserver.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutItemPKSIMWarnings,
            this.layoutControlItem4});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(150, 150);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkValiadatePkSimStandardObserver;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(130, 23);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chkValidateDimensions;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(130, 23);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.chkShowUnableToCalculateWarnings;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 46);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(130, 23);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutItemPKSIMWarnings
         // 
         this.layoutItemPKSIMWarnings.Control = this.chkShowPKSimWarnings;
         this.layoutItemPKSIMWarnings.CustomizationFormText = "layoutItemPKSIMWarnings";
         this.layoutItemPKSIMWarnings.Location = new System.Drawing.Point(0, 69);
         this.layoutItemPKSIMWarnings.Name = "layoutItemPKSIMWarnings";
         this.layoutItemPKSIMWarnings.Size = new System.Drawing.Size(130, 23);
         this.layoutItemPKSIMWarnings.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemPKSIMWarnings.TextVisible = false;
         // 
         // chkValidatRules
         // 
         this.chkValidateRules.Location = new System.Drawing.Point(12, 104);
         this.chkValidateRules.Name = "chkValidateRules";
         this.chkValidateRules.Properties.Caption = "checkEdit1";
         this.chkValidateRules.Size = new System.Drawing.Size(126, 19);
         this.chkValidateRules.StyleController = this.layoutControl1;
         this.chkValidateRules.TabIndex = 8;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.chkValidateRules;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 92);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(130, 38);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // ValidationOptionsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "ValidationOptionsView";
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkShowPKSimWarnings.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowUnableToCalculateWarnings.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateDimensions.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValiadatePkSimStandardObserver.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPKSIMWarnings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateRules.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPKSIMWarnings;
      private UxCheckEdit chkShowPKSimWarnings;
      private UxCheckEdit chkShowUnableToCalculateWarnings;
      private UxCheckEdit chkValidateDimensions;
      private UxCheckEdit chkValiadatePkSimStandardObserver;
      private DevExpress.XtraEditors.CheckEdit chkValidateRules;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
   }
}
