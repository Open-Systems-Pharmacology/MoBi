namespace MoBi.UI.Views
{
   partial class EditEventGroupView
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
         this.components = new System.ComponentModel.Container();
         this.tabControl = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.grpContainerDescriptor = new DevExpress.XtraEditors.GroupControl();
         this.panelDescriptorCriteria = new DevExpress.XtraEditors.PanelControl();
         this.lblDescription = new DevExpress.XtraEditors.LabelControl();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.lbl = new DevExpress.XtraEditors.LabelControl();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         this.tabTags = new DevExpress.XtraTab.XtraTabPage();
         this.barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).BeginInit();
         this.tabControl.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.grpContainerDescriptor)).BeginInit();
         this.grpContainerDescriptor.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelDescriptorCriteria)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         this.SuspendLayout();
         // 
         // tabControl
         // 
         this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabControl.Location = new System.Drawing.Point(0, 0);
         this.tabControl.Name = "tabControl";
         this.tabControl.SelectedTabPage = this.tabProperties;
         this.tabControl.Size = new System.Drawing.Size(977, 583);
         this.tabControl.TabIndex = 0;
         this.tabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabParameters,
            this.tabTags});
         // 
         // tabProperties
         // 
         this.tabProperties.Controls.Add(this.grpContainerDescriptor);
         this.tabProperties.Controls.Add(this.lblDescription);
         this.tabProperties.Controls.Add(this.htmlEditor);
         this.tabProperties.Controls.Add(this.lbl);
         this.tabProperties.Controls.Add(this.btName);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(975, 558);
         this.tabProperties.Text = "tabInfo";
         // 
         // grpContainerDescriptor
         // 
         this.grpContainerDescriptor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.grpContainerDescriptor.Controls.Add(this.panelDescriptorCriteria);
         this.grpContainerDescriptor.Location = new System.Drawing.Point(2, 36);
         this.grpContainerDescriptor.Name = "grpContainerDescriptor";
         this.grpContainerDescriptor.Size = new System.Drawing.Size(966, 490);
         this.grpContainerDescriptor.TabIndex = 20;
         this.grpContainerDescriptor.Text = "In Container with";
         // 
         // panelDescriptorCriteria
         // 
         this.panelDescriptorCriteria.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panelDescriptorCriteria.Location = new System.Drawing.Point(2, 23);
         this.panelDescriptorCriteria.Name = "panelDescriptorCriteria";
         this.panelDescriptorCriteria.Size = new System.Drawing.Size(962, 465);
         this.panelDescriptorCriteria.TabIndex = 0;
         // 
         // lblDescription
         // 
         this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.lblDescription.Location = new System.Drawing.Point(2, 532);
         this.lblDescription.Name = "lblDescription";
         this.lblDescription.Size = new System.Drawing.Size(57, 13);
         this.lblDescription.TabIndex = 19;
         this.lblDescription.Text = "Description:";
         // 
         // htmlEditor
         // 
         this.htmlEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.htmlEditor.Location = new System.Drawing.Point(61, 532);
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Properties.ShowIcon = false;
         this.htmlEditor.Size = new System.Drawing.Size(907, 20);
         this.htmlEditor.TabIndex = 18;
         // 
         // lbl
         // 
         this.lbl.Location = new System.Drawing.Point(2, 13);
         this.lbl.Name = "lbl";
         this.lbl.Size = new System.Drawing.Size(31, 13);
         this.lbl.TabIndex = 17;
         this.lbl.Text = "Name:";
         // 
         // btName
         // 
         this.btName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btName.Location = new System.Drawing.Point(66, 10);
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(900, 20);
         this.btName.TabIndex = 16;
         this.btName.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btName_ButtonClick);
         // 
         // tabParameters
         // 
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.Size = new System.Drawing.Size(975, 558);
         this.tabParameters.Text = "tabParameters";
         // 
         // tabTags
         // 
         this.tabTags.Name = "tabTags";
         this.tabTags.Padding = new System.Windows.Forms.Padding(10);
         this.tabTags.Size = new System.Drawing.Size(975, 558);
         this.tabTags.Text = "tabTags";
         // 
         // barManager
         // 
         this.barManager.DockControls.Add(this.barDockControlTop);
         this.barManager.DockControls.Add(this.barDockControlBottom);
         this.barManager.DockControls.Add(this.barDockControlLeft);
         this.barManager.DockControls.Add(this.barDockControlRight);
         this.barManager.Form = this;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = this.barManager;
         this.barDockControlTop.Size = new System.Drawing.Size(977, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 583);
         this.barDockControlBottom.Manager = this.barManager;
         this.barDockControlBottom.Size = new System.Drawing.Size(977, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Manager = this.barManager;
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 583);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(977, 0);
         this.barDockControlRight.Manager = this.barManager;
         this.barDockControlRight.Size = new System.Drawing.Size(0, 583);
         // 
         // EditEventGroupView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabControl);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "EditEventGroupView";
         this.Size = new System.Drawing.Size(977, 583);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
         this.tabControl.ResumeLayout(false);
         this.tabProperties.ResumeLayout(false);
         this.tabProperties.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.grpContainerDescriptor)).EndInit();
         this.grpContainerDescriptor.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelDescriptorCriteria)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraTab.XtraTabControl tabControl;
      private DevExpress.XtraTab.XtraTabPage tabProperties;
      private DevExpress.XtraTab.XtraTabPage tabParameters;
      private DevExpress.XtraEditors.LabelControl lblDescription;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraEditors.LabelControl lbl;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private DevExpress.XtraEditors.GroupControl grpContainerDescriptor;
      private DevExpress.XtraBars.BarManager barManager;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraEditors.PanelControl panelDescriptorCriteria;
      private DevExpress.XtraTab.XtraTabPage tabTags;
   }
}
