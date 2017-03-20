using OSPSuite.UI.Controls;
using DevExpress.XtraEditors;
using OSPSuite.Core.Domain;

namespace MoBi.UI.Views
{
   partial class MergeConflictResolverView
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
         this.layoutControl2 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
         this.btnMerge = new DevExpress.XtraEditors.SimpleButton();
         this._applyDefaultRenaming = new OSPSuite.UI.Controls.UxCheckEdit();
         this._chkApplyToRemaning = new OSPSuite.UI.Controls.UxCheckEdit();
         this.btnClone = new DevExpress.XtraEditors.SimpleButton();
         this.grpMerge = new DevExpress.XtraEditors.GroupControl();
         this._mergePanel = new System.Windows.Forms.Panel();
         this.btnKeepMerge = new DevExpress.XtraEditors.SimpleButton();
         this.grpTarget = new DevExpress.XtraEditors.GroupControl();
         this._targetPanel = new System.Windows.Forms.Panel();
         this.btnKeepTarget = new DevExpress.XtraEditors.SimpleButton();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCloneButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCloneCheckBox = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemMergeButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.checkEditLayoutControl = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
         this.layoutControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this._applyDefaultRenaming.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._chkApplyToRemaning.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grpMerge)).BeginInit();
         this.grpMerge.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.grpTarget)).BeginInit();
         this.grpTarget.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCloneButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCloneCheckBox)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMergeButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.checkEditLayoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl2
         // 
         this.layoutControl2.AllowCustomization = false;
         this.layoutControl2.Controls.Add(this.btnCancel);
         this.layoutControl2.Controls.Add(this.btnMerge);
         this.layoutControl2.Controls.Add(this._applyDefaultRenaming);
         this.layoutControl2.Controls.Add(this._chkApplyToRemaning);
         this.layoutControl2.Controls.Add(this.btnClone);
         this.layoutControl2.Controls.Add(this.grpMerge);
         this.layoutControl2.Controls.Add(this.grpTarget);
         this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl2.Location = new System.Drawing.Point(0, 0);
         this.layoutControl2.Name = "layoutControl2";
         this.layoutControl2.Root = this.Root;
         this.layoutControl2.Size = new System.Drawing.Size(1033, 691);
         this.layoutControl2.TabIndex = 42;
         this.layoutControl2.Text = "layoutControl2";
         // 
         // cancelBtn
         // 
         this.btnCancel.Location = new System.Drawing.Point(4, 665);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(1025, 22);
         this.btnCancel.StyleController = this.layoutControl2;
         this.btnCancel.TabIndex = 41;
         this.btnCancel.Text = "cancelBtn";
         // 
         // _btnMerge
         // 
         this.btnMerge.Location = new System.Drawing.Point(4, 639);
         this.btnMerge.Name = "btnMerge";
         this.btnMerge.Size = new System.Drawing.Size(1025, 22);
         this.btnMerge.StyleController = this.layoutControl2;
         this.btnMerge.TabIndex = 7;
         this.btnMerge.Text = "_mergeButton";
         this.btnMerge.Visible = false;
         // 
         // _applyDefaultRenaming
         // 
         this._applyDefaultRenaming.AllowClicksOutsideControlArea = false;
         this._applyDefaultRenaming.Location = new System.Drawing.Point(307, 593);
         this._applyDefaultRenaming.Name = "_applyDefaultRenaming";
         this._applyDefaultRenaming.Properties.Caption = "_applyDefaultRenaming";
         this._applyDefaultRenaming.Size = new System.Drawing.Size(722, 19);
         this._applyDefaultRenaming.StyleController = this.layoutControl2;
         this._applyDefaultRenaming.TabIndex = 6;
         this._applyDefaultRenaming.Visible = false;
         // 
         // _chkApplyToRemaning
         // 
         this._chkApplyToRemaning.AllowClicksOutsideControlArea = false;
         this._chkApplyToRemaning.Location = new System.Drawing.Point(4, 616);
         this._chkApplyToRemaning.Name = "_chkApplyToRemaning";
         this._chkApplyToRemaning.Properties.Caption = "_chkApplyToRemaning";
         this._chkApplyToRemaning.Size = new System.Drawing.Size(1025, 19);
         this._chkApplyToRemaning.StyleController = this.layoutControl2;
         this._chkApplyToRemaning.TabIndex = 3;
         // 
         // _btnClone
         // 
         this.btnClone.Location = new System.Drawing.Point(4, 590);
         this.btnClone.Name = "btnClone";
         this.btnClone.Size = new System.Drawing.Size(299, 22);
         this.btnClone.StyleController = this.layoutControl2;
         this.btnClone.TabIndex = 5;
         this.btnClone.Text = "_btnClone";
         this.btnClone.Visible = false;
         // 
         // grpMerge
         // 
         this.grpMerge.Controls.Add(this._mergePanel);
         this.grpMerge.Controls.Add(this.btnKeepMerge);
         this.grpMerge.Location = new System.Drawing.Point(512, 4);
         this.grpMerge.Name = "grpMerge";
         this.grpMerge.Size = new System.Drawing.Size(517, 582);
         this.grpMerge.TabIndex = 0;
         this.grpMerge.Text = "groupControl2";
         // 
         // _mergePanel
         // 
         this._mergePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this._mergePanel.Location = new System.Drawing.Point(2, 20);
         this._mergePanel.Name = "_mergePanel";
         this._mergePanel.Size = new System.Drawing.Size(513, 540);
         this._mergePanel.TabIndex = 5;
         // 
         // _keepMergeButton
         // 
         this.btnKeepMerge.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.btnKeepMerge.Location = new System.Drawing.Point(2, 560);
         this.btnKeepMerge.Name = "btnKeepMerge";
         this.btnKeepMerge.Size = new System.Drawing.Size(513, 20);
         this.btnKeepMerge.TabIndex = 1;
         // 
         // grpTarget
         // 
         this.grpTarget.Controls.Add(this._targetPanel);
         this.grpTarget.Controls.Add(this.btnKeepTarget);
         this.grpTarget.Location = new System.Drawing.Point(4, 4);
         this.grpTarget.Name = "grpTarget";
         this.grpTarget.Size = new System.Drawing.Size(504, 582);
         this.grpTarget.TabIndex = 4;
         this.grpTarget.Text = "groupControl1";
         // 
         // _targetPanel
         // 
         this._targetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this._targetPanel.Location = new System.Drawing.Point(2, 20);
         this._targetPanel.Name = "_targetPanel";
         this._targetPanel.Size = new System.Drawing.Size(500, 540);
         this._targetPanel.TabIndex = 4;
         // 
         // _keepTargetButton
         // 
         this.btnKeepTarget.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.btnKeepTarget.Location = new System.Drawing.Point(2, 560);
         this.btnKeepTarget.Name = "btnKeepTarget";
         this.btnKeepTarget.Size = new System.Drawing.Size(500, 20);
         this.btnKeepTarget.TabIndex = 0;
         // 
         // Root
         // 
         this.Root.CustomizationFormText = "Root";
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItemCloneButton,
            this.layoutControlItemCloneCheckBox,
            this.layoutControlItemMergeButton,
            this.checkEditLayoutControl,
            this.layoutControlItem1});
         this.Root.Location = new System.Drawing.Point(0, 0);
         this.Root.Name = "Root";
         this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);
         this.Root.Size = new System.Drawing.Size(1033, 691);
         this.Root.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.grpTarget;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(508, 586);
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItem4
         // 
         this.layoutControlItem4.Control = this.grpMerge;
         this.layoutControlItem4.CustomizationFormText = "layoutControlItem4";
         this.layoutControlItem4.Location = new System.Drawing.Point(508, 0);
         this.layoutControlItem4.Name = "layoutControlItem4";
         this.layoutControlItem4.Size = new System.Drawing.Size(521, 586);
         this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem4.TextVisible = false;
         // 
         // layoutControlItemCloneButton
         // 
         this.layoutControlItemCloneButton.Control = this.btnClone;
         this.layoutControlItemCloneButton.CustomizationFormText = "layoutControlItemCloneButton";
         this.layoutControlItemCloneButton.Location = new System.Drawing.Point(0, 586);
         this.layoutControlItemCloneButton.Name = "layoutControlItemCloneButton";
         this.layoutControlItemCloneButton.Size = new System.Drawing.Size(303, 26);
         this.layoutControlItemCloneButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCloneButton.TextVisible = false;
         // 
         // layoutControlItemCloneCheckBox
         // 
         this.layoutControlItemCloneCheckBox.Control = this._applyDefaultRenaming;
         this.layoutControlItemCloneCheckBox.ControlAlignment = System.Drawing.ContentAlignment.MiddleLeft;
         this.layoutControlItemCloneCheckBox.CustomizationFormText = "layoutControlItemCloneCheckBox";
         this.layoutControlItemCloneCheckBox.FillControlToClientArea = false;
         this.layoutControlItemCloneCheckBox.Location = new System.Drawing.Point(303, 586);
         this.layoutControlItemCloneCheckBox.Name = "layoutControlItemCloneCheckBox";
         this.layoutControlItemCloneCheckBox.Padding = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
         this.layoutControlItemCloneCheckBox.Size = new System.Drawing.Size(726, 26);
         this.layoutControlItemCloneCheckBox.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCloneCheckBox.TextVisible = false;
         // 
         // layoutControlItemMergeButton
         // 
         this.layoutControlItemMergeButton.Control = this.btnMerge;
         this.layoutControlItemMergeButton.CustomizationFormText = "layoutControlItemMergeButton";
         this.layoutControlItemMergeButton.Location = new System.Drawing.Point(0, 635);
         this.layoutControlItemMergeButton.Name = "layoutControlItemMergeButton";
         this.layoutControlItemMergeButton.Size = new System.Drawing.Size(1029, 26);
         this.layoutControlItemMergeButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemMergeButton.TextVisible = false;
         // 
         // checkEditLayoutControl
         // 
         this.checkEditLayoutControl.Control = this._chkApplyToRemaning;
         this.checkEditLayoutControl.CustomizationFormText = "layoutControlItem3";
         this.checkEditLayoutControl.Location = new System.Drawing.Point(0, 612);
         this.checkEditLayoutControl.Name = "checkEditLayoutControl";
         this.checkEditLayoutControl.Size = new System.Drawing.Size(1029, 23);
         this.checkEditLayoutControl.TextSize = new System.Drawing.Size(0, 0);
         this.checkEditLayoutControl.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.btnCancel;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 661);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1029, 26);
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // MergeConflictResolverView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(1033, 691);
         this.Controls.Add(this.layoutControl2);
         this.Name = "MergeConflictResolverView";
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
         this.layoutControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this._applyDefaultRenaming.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._chkApplyToRemaning.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grpMerge)).EndInit();
         this.grpMerge.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.grpTarget)).EndInit();
         this.grpTarget.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCloneButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCloneCheckBox)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemMergeButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.checkEditLayoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private SimpleButton btnCancel;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl2;
      private GroupControl grpMerge;
      private System.Windows.Forms.Panel _mergePanel;
      private SimpleButton btnKeepMerge;
      private GroupControl grpTarget;
      private System.Windows.Forms.Panel _targetPanel;
      private SimpleButton btnKeepTarget;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
      private SimpleButton btnClone;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCloneButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCloneCheckBox;
      private SimpleButton btnMerge;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemMergeButton;
      private DevExpress.XtraLayout.LayoutControlItem checkEditLayoutControl;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private UxCheckEdit _chkApplyToRemaning;
      private UxCheckEdit _applyDefaultRenaming;
   }
}
