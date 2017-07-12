namespace MoBi.UI.Views
{
   partial class EditActiveTransportBuilderContainerView
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
         this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
         this.tabInfo = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControlProperties = new OSPSuite.UI.Controls.UxLayoutControl();
         this.btTransportName = new DevExpress.XtraEditors.ButtonEdit();
         this.btEditName = new DevExpress.XtraEditors.ButtonEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemTranportName = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabParameter = new DevExpress.XtraTab.XtraTabPage();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
         this.xtraTabControl1.SuspendLayout();
         this.tabInfo.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlProperties)).BeginInit();
         this.layoutControlProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.btTransportName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTranportName)).BeginInit();
         this.SuspendLayout();
         // 
         // xtraTabControl1
         // 
         this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
         this.xtraTabControl1.Name = "xtraTabControl1";
         this.xtraTabControl1.SelectedTabPage = this.tabInfo;
         this.xtraTabControl1.Size = new System.Drawing.Size(767, 344);
         this.xtraTabControl1.TabIndex = 0;
         this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabInfo,
            this.tabParameter});
         // 
         // tabInfo
         // 
         this.tabInfo.Controls.Add(this.layoutControlProperties);
         this.tabInfo.Name = "tabInfo";
         this.tabInfo.Size = new System.Drawing.Size(761, 316);
         this.tabInfo.Text = "Info";
         // 
         // layoutControlProperties
         // 
         this.layoutControlProperties.AllowCustomization = false;
         this.layoutControlProperties.Controls.Add(this.btTransportName);
         this.layoutControlProperties.Controls.Add(this.btEditName);
         this.layoutControlProperties.Controls.Add(this.htmlEditor);
         this.layoutControlProperties.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControlProperties.Location = new System.Drawing.Point(0, 0);
         this.layoutControlProperties.Name = "layoutControlProperties";
         this.layoutControlProperties.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(899, 160, 250, 350);
         this.layoutControlProperties.Root = this.layoutControlGroup1;
         this.layoutControlProperties.Size = new System.Drawing.Size(761, 316);
         this.layoutControlProperties.TabIndex = 1;
         this.layoutControlProperties.Text = "layoutControl1";
         // 
         // btTransportName
         // 
         this.btTransportName.Location = new System.Drawing.Point(171, 36);
         this.btTransportName.Name = "btTransportName";
         this.btTransportName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btTransportName.Size = new System.Drawing.Size(578, 20);
         this.btTransportName.StyleController = this.layoutControlProperties;
         this.btTransportName.TabIndex = 8;
         // 
         // btEditName
         // 
         this.btEditName.Location = new System.Drawing.Point(171, 12);
         this.btEditName.Name = "btEditName";
         this.btEditName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btEditName.Size = new System.Drawing.Size(578, 20);
         this.btEditName.StyleController = this.layoutControlProperties;
         this.btEditName.TabIndex = 7;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Location = new System.Drawing.Point(12, 76);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Size = new System.Drawing.Size(737, 228);
         this.htmlEditor.StyleController = this.layoutControlProperties;
         this.htmlEditor.TabIndex = 6;
         this.htmlEditor.UseOptimizedRendering = true;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemDescription,
            this.layoutControlItemName,
            this.layoutControlItemTranportName});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(761, 316);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItemDescription
         // 
         this.layoutControlItemDescription.Control = this.htmlEditor;
         this.layoutControlItemDescription.CustomizationFormText = "layoutControlItemDescription";
         this.layoutControlItemDescription.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItemDescription.Name = "layoutControlItemDescription";
         this.layoutControlItemDescription.Size = new System.Drawing.Size(741, 248);
         this.layoutControlItemDescription.Text = "layoutControlItemDescription";
         this.layoutControlItemDescription.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlItemDescription.TextSize = new System.Drawing.Size(156, 13);
         // 
         // layoutControlItemName
         // 
         this.layoutControlItemName.Control = this.btEditName;
         this.layoutControlItemName.CustomizationFormText = "layoutControlItemName";
         this.layoutControlItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItemName.Name = "layoutControlItemName";
         this.layoutControlItemName.Size = new System.Drawing.Size(741, 24);
         this.layoutControlItemName.Text = "layoutControlItemName";
         this.layoutControlItemName.TextSize = new System.Drawing.Size(156, 13);
         // 
         // layoutControlItemTranportName
         // 
         this.layoutControlItemTranportName.Control = this.btTransportName;
         this.layoutControlItemTranportName.CustomizationFormText = "layoutControlItemTranportName";
         this.layoutControlItemTranportName.Location = new System.Drawing.Point(0, 24);
         this.layoutControlItemTranportName.Name = "layoutControlItemTranportName";
         this.layoutControlItemTranportName.Size = new System.Drawing.Size(741, 24);
         this.layoutControlItemTranportName.Text = "layoutControlItemTranportName";
         this.layoutControlItemTranportName.TextSize = new System.Drawing.Size(156, 13);
         // 
         // tabParameter
         // 
         this.tabParameter.Name = "tabParameter";
         this.tabParameter.Size = new System.Drawing.Size(761, 316);
         this.tabParameter.Text = "tabParameter";
         // 
         // EditActiveTransportBuilderContainerView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.xtraTabControl1);
         this.Name = "EditActiveTransportBuilderContainerView";
         this.Size = new System.Drawing.Size(767, 344);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
         this.xtraTabControl1.ResumeLayout(false);
         this.tabInfo.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlProperties)).EndInit();
         this.layoutControlProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.btTransportName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemTranportName)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
      private DevExpress.XtraTab.XtraTabPage tabInfo;
      private DevExpress.XtraTab.XtraTabPage tabParameter;
      private DevExpress.XtraEditors.MemoEdit htmlEditor;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemDescription;
      private DevExpress.XtraEditors.ButtonEdit btEditName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemName;
      private DevExpress.XtraEditors.ButtonEdit btTransportName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemTranportName;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControlProperties;
   }
}