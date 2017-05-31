using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class ValidationMessagesView
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
         _gridViewBinder.Dispose();
         _screenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btnSaveLog = new DevExpress.XtraEditors.SimpleButton();
         this.chkError = new UxCheckEdit();
         this.chkWarning = new UxCheckEdit();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new MoBi.UI.Views.UxGridView();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemWarning = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemError = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSaveLog = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkError.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkWarning.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemWarning)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemError)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveLog)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.btnSaveLog);
         this.layoutControl.Controls.Add(this.chkError);
         this.layoutControl.Controls.Add(this.chkWarning);
         this.layoutControl.Controls.Add(this.gridControl);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(538, 267, 250, 350);
         this.layoutControl.Root = this.layoutControlGroup1;
         this.layoutControl.Size = new System.Drawing.Size(487, 447);
         this.layoutControl.TabIndex = 1;
         this.layoutControl.Text = "layoutControl1";
         // 
         // btnExportLog
         // 
         this.btnSaveLog.Location = new System.Drawing.Point(12, 413);
         this.btnSaveLog.Name = "btnSaveLog";
         this.btnSaveLog.Size = new System.Drawing.Size(229, 22);
         this.btnSaveLog.StyleController = this.layoutControl;
         this.btnSaveLog.TabIndex = 7;
         this.btnSaveLog.Text = "btnExportLog";
         // 
         // chkError
         // 
         this.chkError.Location = new System.Drawing.Point(245, 12);
         this.chkError.Name = "chkError";
         this.chkError.Properties.Caption = "chkError";
         this.chkError.Size = new System.Drawing.Size(230, 19);
         this.chkError.StyleController = this.layoutControl;
         this.chkError.TabIndex = 6;
         // 
         // chkWarning
         // 
         this.chkWarning.Location = new System.Drawing.Point(12, 12);
         this.chkWarning.Name = "chkWarning";
         this.chkWarning.Properties.Caption = "chkWarning";
         this.chkWarning.Size = new System.Drawing.Size(229, 19);
         this.chkWarning.StyleController = this.layoutControl;
         this.chkWarning.TabIndex = 5;
         // 
         // gridControl
         // 
         this.gridControl.Location = new System.Drawing.Point(12, 35);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.gridControl.Size = new System.Drawing.Size(463, 374);
         this.gridControl.TabIndex = 4;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.GridControl = this.gridControl;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.RowsInsertable = false;
         this.gridView.ShouldUseColorForDisabledCell = true;
         this.gridView.ShowColumnChooser = true;
         this.gridView.ShowRowIndicator = true;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutItemWarning,
            this.layoutItemError,
            this.layoutItemSaveLog,
            this.emptySpaceItem1});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "Root";
         this.layoutControlGroup1.Size = new System.Drawing.Size(487, 447);
         this.layoutControlGroup1.Text = "Root";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.gridControl;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 23);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(467, 378);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutItemWarning
         // 
         this.layoutItemWarning.Control = this.chkWarning;
         this.layoutItemWarning.CustomizationFormText = "layoutControlItem2";
         this.layoutItemWarning.Location = new System.Drawing.Point(0, 0);
         this.layoutItemWarning.Name = "layoutItemWarning";
         this.layoutItemWarning.Size = new System.Drawing.Size(233, 23);
         this.layoutItemWarning.Text = "layoutItemWarning";
         this.layoutItemWarning.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemWarning.TextToControlDistance = 0;
         this.layoutItemWarning.TextVisible = false;
         // 
         // layoutItemError
         // 
         this.layoutItemError.Control = this.chkError;
         this.layoutItemError.CustomizationFormText = "layoutControlItem3";
         this.layoutItemError.Location = new System.Drawing.Point(233, 0);
         this.layoutItemError.Name = "layoutItemError";
         this.layoutItemError.Size = new System.Drawing.Size(234, 23);
         this.layoutItemError.Text = "layoutItemError";
         this.layoutItemError.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemError.TextToControlDistance = 0;
         this.layoutItemError.TextVisible = false;
         // 
         // layoutItemExportLog
         // 
         this.layoutItemSaveLog.Control = this.btnSaveLog;
         this.layoutItemSaveLog.CustomizationFormText = "layoutItemExportLog";
         this.layoutItemSaveLog.Location = new System.Drawing.Point(0, 401);
         this.layoutItemSaveLog.Name = "layoutItemExportLog";
         this.layoutItemSaveLog.Size = new System.Drawing.Size(233, 26);
         this.layoutItemSaveLog.Text = "layoutItemExportLog";
         this.layoutItemSaveLog.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSaveLog.TextToControlDistance = 0;
         this.layoutItemSaveLog.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(233, 401);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(234, 26);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // ValidationMessagesView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(487, 493);
         this.Controls.Add(this.layoutControl);
         this.Name = "ValidationMessagesView";
         this.Text = "ValidationMessageView";
         this.Controls.SetChildIndex(this.layoutControl, 0);
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkError.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkWarning.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemWarning)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemError)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveLog)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraGrid.GridControl gridControl;
      private MoBi.UI.Views.UxGridView gridView;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraEditors.SimpleButton btnSaveLog;
      private DevExpress.XtraEditors.CheckEdit chkError;
      private DevExpress.XtraEditors.CheckEdit chkWarning;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemWarning;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemError;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSaveLog;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
   }
}