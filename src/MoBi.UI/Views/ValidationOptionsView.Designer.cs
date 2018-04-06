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
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkValidateRules = new DevExpress.XtraEditors.CheckEdit();
         this.chkShowPKSimWarnings = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkShowUnableToCalculateWarnings = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkValidateDimensions = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkValiadatePkSimStandardObserver = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutControlGroup = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemPKSIMWarnings = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.chkPerformCircularReferenceCheck = new DevExpress.XtraEditors.CheckEdit();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateRules.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowPKSimWarnings.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowUnableToCalculateWarnings.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateDimensions.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValiadatePkSimStandardObserver.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPKSIMWarnings)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkPerformCircularReferenceCheck.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.chkPerformCircularReferenceCheck);
         this.layoutControl.Controls.Add(this.chkValidateRules);
         this.layoutControl.Controls.Add(this.chkShowPKSimWarnings);
         this.layoutControl.Controls.Add(this.chkShowUnableToCalculateWarnings);
         this.layoutControl.Controls.Add(this.chkValidateDimensions);
         this.layoutControl.Controls.Add(this.chkValiadatePkSimStandardObserver);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.layoutControlGroup;
         this.layoutControl.Size = new System.Drawing.Size(331, 243);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // chkValidateRules
         // 
         this.chkValidateRules.Location = new System.Drawing.Point(2, 94);
         this.chkValidateRules.Name = "chkValidateRules";
         this.chkValidateRules.Properties.Caption = "chkValidateRules";
         this.chkValidateRules.Size = new System.Drawing.Size(327, 19);
         this.chkValidateRules.StyleController = this.layoutControl;
         this.chkValidateRules.TabIndex = 8;
         // 
         // chkShowPKSimWarnings
         // 
         this.chkShowPKSimWarnings.AllowClicksOutsideControlArea = false;
         this.chkShowPKSimWarnings.Location = new System.Drawing.Point(2, 71);
         this.chkShowPKSimWarnings.Name = "chkShowPKSimWarnings";
         this.chkShowPKSimWarnings.Properties.Caption = "chkShowPKSimWarnings";
         this.chkShowPKSimWarnings.Size = new System.Drawing.Size(327, 19);
         this.chkShowPKSimWarnings.StyleController = this.layoutControl;
         this.chkShowPKSimWarnings.TabIndex = 7;
         // 
         // chkShowUnableToCalculateWarnings
         // 
         this.chkShowUnableToCalculateWarnings.AllowClicksOutsideControlArea = false;
         this.chkShowUnableToCalculateWarnings.Location = new System.Drawing.Point(2, 48);
         this.chkShowUnableToCalculateWarnings.Name = "chkShowUnableToCalculateWarnings";
         this.chkShowUnableToCalculateWarnings.Properties.Caption = "chkShowUnableToCalculateWarnings";
         this.chkShowUnableToCalculateWarnings.Size = new System.Drawing.Size(327, 19);
         this.chkShowUnableToCalculateWarnings.StyleController = this.layoutControl;
         this.chkShowUnableToCalculateWarnings.TabIndex = 6;
         // 
         // chkValidateDimensions
         // 
         this.chkValidateDimensions.AllowClicksOutsideControlArea = false;
         this.chkValidateDimensions.Location = new System.Drawing.Point(2, 25);
         this.chkValidateDimensions.Name = "chkValidateDimensions";
         this.chkValidateDimensions.Properties.Caption = "chkValidateDimensions";
         this.chkValidateDimensions.Size = new System.Drawing.Size(327, 19);
         this.chkValidateDimensions.StyleController = this.layoutControl;
         this.chkValidateDimensions.TabIndex = 5;
         // 
         // chkValiadatePkSimStandardObserver
         // 
         this.chkValiadatePkSimStandardObserver.AllowClicksOutsideControlArea = false;
         this.chkValiadatePkSimStandardObserver.Location = new System.Drawing.Point(2, 2);
         this.chkValiadatePkSimStandardObserver.Name = "chkValiadatePkSimStandardObserver";
         this.chkValiadatePkSimStandardObserver.Properties.Caption = "chkValiadatePkSimStandardObserver";
         this.chkValiadatePkSimStandardObserver.Size = new System.Drawing.Size(327, 19);
         this.chkValiadatePkSimStandardObserver.StyleController = this.layoutControl;
         this.chkValiadatePkSimStandardObserver.TabIndex = 4;
         // 
         // layoutControlGroup
         // 
         this.layoutControlGroup.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup.GroupBordersVisible = false;
         this.layoutControlGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItem3,
            this.layoutItemPKSIMWarnings,
            this.layoutControlItem4,
            this.layoutControlItem5});
         this.layoutControlGroup.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup.Name = "layoutControlGroup";
         this.layoutControlGroup.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup.Size = new System.Drawing.Size(331, 243);
         this.layoutControlGroup.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkValiadatePkSimStandardObserver;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(331, 23);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chkValidateDimensions;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(331, 23);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.chkShowUnableToCalculateWarnings;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 46);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(331, 23);
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextVisible = false;
         // 
         // layoutItemPKSIMWarnings
         // 
         this.layoutItemPKSIMWarnings.Control = this.chkShowPKSimWarnings;
         this.layoutItemPKSIMWarnings.CustomizationFormText = "layoutItemPKSIMWarnings";
         this.layoutItemPKSIMWarnings.Location = new System.Drawing.Point(0, 69);
         this.layoutItemPKSIMWarnings.Name = "layoutItemPKSIMWarnings";
         this.layoutItemPKSIMWarnings.Size = new System.Drawing.Size(331, 23);
         this.layoutItemPKSIMWarnings.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemPKSIMWarnings.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.chkValidateRules;
         this.layoutControlItem4.Location = new System.Drawing.Point(0, 92);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(331, 23);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // chkPerformCircularReferenceCheck
         // 
         this.chkPerformCircularReferenceCheck.Location = new System.Drawing.Point(2, 117);
         this.chkPerformCircularReferenceCheck.Name = "chkPerformCircularReferenceCheck";
         this.chkPerformCircularReferenceCheck.Properties.Caption = "chkPerformCircularReferenceCheck";
         this.chkPerformCircularReferenceCheck.Size = new System.Drawing.Size(327, 19);
         this.chkPerformCircularReferenceCheck.StyleController = this.layoutControl;
         this.chkPerformCircularReferenceCheck.TabIndex = 9;
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.chkPerformCircularReferenceCheck;
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 115);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(331, 128);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // ValidationOptionsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "ValidationOptionsView";
         this.Size = new System.Drawing.Size(331, 243);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateRules.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowPKSimWarnings.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkShowUnableToCalculateWarnings.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValidateDimensions.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkValiadatePkSimStandardObserver.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPKSIMWarnings)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkPerformCircularReferenceCheck.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup;
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
      private DevExpress.XtraEditors.CheckEdit chkPerformCircularReferenceCheck;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
   }
}
