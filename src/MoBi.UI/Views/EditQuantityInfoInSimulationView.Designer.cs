namespace MoBi.UI.Views
{
   partial class EditQuantityInfoInSimulationView
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
         htmlEditor = new DevExpress.XtraEditors.MemoEdit();
         uxLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         btnGoToSource = new OSPSuite.UI.Controls.UxSimpleButton();
         tbSource = new DevExpress.XtraEditors.TextEdit();
         tbName = new DevExpress.XtraEditors.TextEdit();
         Root = new DevExpress.XtraLayout.LayoutControlGroup();
         nameLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         descriptionLayoutControlItem = new DevExpress.XtraLayout.LayoutControlItem();
         layoutControlItemSource = new DevExpress.XtraLayout.LayoutControlItem();
         layoutControlItemGoToSource = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
         ((System.ComponentModel.ISupportInitialize)htmlEditor.Properties).BeginInit();
         ((System.ComponentModel.ISupportInitialize)uxLayoutControl).BeginInit();
         uxLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)tbSource.Properties).BeginInit();
         ((System.ComponentModel.ISupportInitialize)tbName.Properties).BeginInit();
         ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
         ((System.ComponentModel.ISupportInitialize)nameLayoutControlItem).BeginInit();
         ((System.ComponentModel.ISupportInitialize)descriptionLayoutControlItem).BeginInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItemSource).BeginInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItemGoToSource).BeginInit();
         SuspendLayout();
         // 
         // htmlEditor
         // 
         htmlEditor.Location = new System.Drawing.Point(12, 78);
         htmlEditor.Name = "htmlEditor";
         htmlEditor.Size = new System.Drawing.Size(374, 253);
         htmlEditor.StyleController = uxLayoutControl;
         htmlEditor.TabIndex = 4;
         // 
         // uxLayoutControl
         // 
         uxLayoutControl.AllowCustomization = false;
         uxLayoutControl.Controls.Add(btnGoToSource);
         uxLayoutControl.Controls.Add(tbSource);
         uxLayoutControl.Controls.Add(htmlEditor);
         uxLayoutControl.Controls.Add(tbName);
         uxLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         uxLayoutControl.Location = new System.Drawing.Point(0, 0);
         uxLayoutControl.Name = "uxLayoutControl";
         uxLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1038, 283, 1317, 613);
         uxLayoutControl.Root = Root;
         uxLayoutControl.Size = new System.Drawing.Size(398, 343);
         uxLayoutControl.TabIndex = 8;
         uxLayoutControl.Text = "uxLayoutControl1";
         // 
         // btnGoToSource
         // 
         btnGoToSource.Location = new System.Drawing.Point(299, 36);
         btnGoToSource.Manager = null;
         btnGoToSource.Name = "btnGoToSource";
         btnGoToSource.Shortcut = System.Windows.Forms.Keys.None;
         btnGoToSource.Size = new System.Drawing.Size(87, 22);
         btnGoToSource.StyleController = uxLayoutControl;
         btnGoToSource.TabIndex = 9;
         btnGoToSource.Text = "btnGoToSource";
         // 
         // tbSource
         // 
         tbSource.Location = new System.Drawing.Point(166, 36);
         tbSource.Name = "tbSource";
         tbSource.Size = new System.Drawing.Size(129, 20);
         tbSource.StyleController = uxLayoutControl;
         tbSource.TabIndex = 5;
         // 
         // tbName
         // 
         tbName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
         tbName.Location = new System.Drawing.Point(166, 12);
         tbName.Name = "tbName";
         tbName.Size = new System.Drawing.Size(220, 20);
         tbName.StyleController = uxLayoutControl;
         tbName.TabIndex = 0;
         // 
         // Root
         // 
         Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         Root.GroupBordersVisible = false;
         Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { nameLayoutControlItem, descriptionLayoutControlItem, layoutControlItemSource, layoutControlItemGoToSource });
         Root.Name = "Root";
         Root.Size = new System.Drawing.Size(398, 343);
         Root.TextVisible = false;
         // 
         // nameLayoutControlItem
         // 
         nameLayoutControlItem.Control = tbName;
         nameLayoutControlItem.Location = new System.Drawing.Point(0, 0);
         nameLayoutControlItem.Name = "nameLayoutControlItem";
         nameLayoutControlItem.Size = new System.Drawing.Size(378, 24);
         nameLayoutControlItem.TextSize = new System.Drawing.Size(142, 13);
         // 
         // descriptionLayoutControlItem
         // 
         descriptionLayoutControlItem.Control = htmlEditor;
         descriptionLayoutControlItem.Location = new System.Drawing.Point(0, 50);
         descriptionLayoutControlItem.Name = "descriptionLayoutControlItem";
         descriptionLayoutControlItem.Size = new System.Drawing.Size(378, 273);
         descriptionLayoutControlItem.TextLocation = DevExpress.Utils.Locations.Top;
         descriptionLayoutControlItem.TextSize = new System.Drawing.Size(142, 13);
         // 
         // layoutControlItemSource
         // 
         layoutControlItemSource.Control = tbSource;
         layoutControlItemSource.Location = new System.Drawing.Point(0, 24);
         layoutControlItemSource.Name = "layoutControlItemSource";
         layoutControlItemSource.Size = new System.Drawing.Size(287, 26);
         layoutControlItemSource.TextSize = new System.Drawing.Size(142, 13);
         // 
         // layoutControlItemGoToSource
         // 
         layoutControlItemGoToSource.Control = btnGoToSource;
         layoutControlItemGoToSource.Location = new System.Drawing.Point(287, 24);
         layoutControlItemGoToSource.Name = "layoutControlItemGoToSource";
         layoutControlItemGoToSource.Size = new System.Drawing.Size(91, 26);
         layoutControlItemGoToSource.TextSize = new System.Drawing.Size(0, 0);
         layoutControlItemGoToSource.TextVisible = false;
         // 
         // EditQuantityInfoInSimulationView
         // 
         AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         Controls.Add(uxLayoutControl);
         Name = "EditQuantityInfoInSimulationView";
         Size = new System.Drawing.Size(398, 343);
         ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
         ((System.ComponentModel.ISupportInitialize)htmlEditor.Properties).EndInit();
         ((System.ComponentModel.ISupportInitialize)uxLayoutControl).EndInit();
         uxLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)tbSource.Properties).EndInit();
         ((System.ComponentModel.ISupportInitialize)tbName.Properties).EndInit();
         ((System.ComponentModel.ISupportInitialize)Root).EndInit();
         ((System.ComponentModel.ISupportInitialize)nameLayoutControlItem).EndInit();
         ((System.ComponentModel.ISupportInitialize)descriptionLayoutControlItem).EndInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItemSource).EndInit();
         ((System.ComponentModel.ISupportInitialize)layoutControlItemGoToSource).EndInit();
         ResumeLayout(false);

      }

      #endregion
      private DevExpress.XtraEditors.MemoEdit htmlEditor;
      private DevExpress.XtraEditors.TextEdit tbName;
      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem nameLayoutControlItem;
      private DevExpress.XtraLayout.LayoutControlItem descriptionLayoutControlItem;
      private DevExpress.XtraEditors.TextEdit tbSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemSource;
      private OSPSuite.UI.Controls.UxSimpleButton btnGoToSource;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemGoToSource;
   }
}
