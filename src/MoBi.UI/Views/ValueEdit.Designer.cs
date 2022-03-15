using DevExpress.XtraEditors;

namespace MoBi.UI.Views
{
   partial class  ValueEdit
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         this.fProperties = new DevExpress.XtraEditors.Repository.RepositoryItem();
         this.tbValue = new DevExpress.XtraEditors.TextEdit();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControl();
         this.cbUnit = new OSPSuite.UI.Controls.UxComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemValue = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemUnit = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.warningProvider = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.fProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.cbUnit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemUnit)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.warningProvider)).BeginInit();
         this.SuspendLayout();
         // 
         // fProperties
         // 
         this.fProperties.Name = "fProperties";
         // 
         // tbValue
         // 
         this.tbValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tbValue.Location = new System.Drawing.Point(99, 10);
         this.tbValue.Name = "tbValue";
         this.tbValue.Size = new System.Drawing.Size(152, 20);
         this.tbValue.StyleController = this.layoutControl;
         this.tbValue.TabIndex = 1;
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.cbUnit);
         this.layoutControl.Controls.Add(this.tbValue);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(507, 41);
         this.layoutControl.TabIndex = 3;
         this.layoutControl.Text = "layoutControl1";
         // 
         // cbUnit
         // 
         this.cbUnit.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.cbUnit.Location = new System.Drawing.Point(353, 10);
         this.cbUnit.Name = "cbUnit";
         this.cbUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbUnit.Size = new System.Drawing.Size(143, 20);
         this.cbUnit.StyleController = this.layoutControl;
         this.cbUnit.TabIndex = 2;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemValue,
            this.layoutItemUnit,
            this.emptySpaceItem2});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(507, 41);
         this.Root.TextVisible = false;
         // 
         // layoutItemValue
         // 
         this.layoutItemValue.Control = this.tbValue;
         this.layoutItemValue.Location = new System.Drawing.Point(0, 0);
         this.layoutItemValue.Name = "layoutItemValue";
         this.layoutItemValue.Size = new System.Drawing.Size(244, 25);
         this.layoutItemValue.TextSize = new System.Drawing.Size(78, 13);
         // 
         // layoutItemUnit
         // 
         this.layoutItemUnit.Control = this.cbUnit;
         this.layoutItemUnit.Location = new System.Drawing.Point(254, 0);
         this.layoutItemUnit.Name = "layoutItemUnit";
         this.layoutItemUnit.Size = new System.Drawing.Size(235, 25);
         this.layoutItemUnit.TextSize = new System.Drawing.Size(78, 13);
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(244, 0);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(10, 25);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // warningProvider
         // 
         this.warningProvider.ContainerControl = this;
         // 
         // ValueEdit
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.Controls.Add(this.layoutControl);
         this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.Name = "ValueEdit";
         this.Size = new System.Drawing.Size(507, 41);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.fProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbValue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.cbUnit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemValue)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemUnit)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.warningProvider)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private TextEdit tbValue;
      private DevExpress.XtraEditors.Repository.RepositoryItem fProperties;
      private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider warningProvider;
      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemValue;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemUnit;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private OSPSuite.UI.Controls.UxComboBoxEdit cbUnit;
   }
}
