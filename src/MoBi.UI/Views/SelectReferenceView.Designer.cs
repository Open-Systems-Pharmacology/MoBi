namespace MoBi.UI.Views
{
   partial class SelectReferenceView
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
       _treeView.Dispose();
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
         this.panelReferenceTreeView = new DevExpress.XtraEditors.PanelControl();
         this.btEditSelectLocalisation = new DevExpress.XtraEditors.ButtonEdit();
         this.radioGroupReferenceType = new DevExpress.XtraEditors.RadioGroup();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemRadioGroup = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemLocalisation = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemPanelTreeView = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.panelReferenceTreeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditSelectLocalisation.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroupReferenceType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRadioGroup)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLocalisation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanelTreeView)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.AllowCustomization = false;
         this.layoutControl.Controls.Add(this.panelReferenceTreeView);
         this.layoutControl.Controls.Add(this.btEditSelectLocalisation);
         this.layoutControl.Controls.Add(this.radioGroupReferenceType);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1138, 340, 650, 400);
         this.layoutControl.Root = this.Root;
         this.layoutControl.Size = new System.Drawing.Size(580, 632);
         this.layoutControl.TabIndex = 7;
         this.layoutControl.Text = "uxLayoutControl1";
         // 
         // panelReferenceTreeView
         // 
         this.panelReferenceTreeView.Location = new System.Drawing.Point(12, 129);
         this.panelReferenceTreeView.Name = "panelReferenceTreeView";
         this.panelReferenceTreeView.Size = new System.Drawing.Size(556, 491);
         this.panelReferenceTreeView.TabIndex = 4;
         // 
         // btEditSelectLocalisation
         // 
         this.btEditSelectLocalisation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.btEditSelectLocalisation.Location = new System.Drawing.Point(12, 49);
         this.btEditSelectLocalisation.Name = "btEditSelectLocalisation";
         this.btEditSelectLocalisation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btEditSelectLocalisation.Size = new System.Drawing.Size(556, 20);
         this.btEditSelectLocalisation.StyleController = this.layoutControl;
         this.btEditSelectLocalisation.TabIndex = 3;
         this.btEditSelectLocalisation.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btEditSelectLocalisation_ButtonClick);
         // 
         // radioGroupReferenceType
         // 
         this.radioGroupReferenceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.radioGroupReferenceType.Location = new System.Drawing.Point(146, 73);
         this.radioGroupReferenceType.Name = "radioGroupReferenceType";
         this.radioGroupReferenceType.Size = new System.Drawing.Size(422, 27);
         this.radioGroupReferenceType.StyleController = this.layoutControl;
         this.radioGroupReferenceType.TabIndex = 4;
         this.radioGroupReferenceType.SelectedIndexChanged += new System.EventHandler(this.rgReferenceType_SelectedIndexChanged);
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemRadioGroup,
            this.layoutItemLocalisation,
            this.layoutItemPanelTreeView});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(580, 632);
         // 
         // layoutItemRadioGroup
         // 
         this.layoutItemRadioGroup.Control = this.radioGroupReferenceType;
         this.layoutItemRadioGroup.Location = new System.Drawing.Point(0, 40);
         this.layoutItemRadioGroup.Name = "layoutItemRadioGroup";
         this.layoutItemRadioGroup.Size = new System.Drawing.Size(560, 31);
         this.layoutItemRadioGroup.TextSize = new System.Drawing.Size(122, 13);
         // 
         // layoutItemLocalisation
         // 
         this.layoutItemLocalisation.Control = this.btEditSelectLocalisation;
         this.layoutItemLocalisation.Location = new System.Drawing.Point(0, 0);
         this.layoutItemLocalisation.Name = "layoutItemLocalisation";
         this.layoutItemLocalisation.Size = new System.Drawing.Size(560, 40);
         this.layoutItemLocalisation.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemLocalisation.TextSize = new System.Drawing.Size(122, 13);
         // 
         // layoutItemPanelTreeView
         // 
         this.layoutItemPanelTreeView.Control = this.panelReferenceTreeView;
         this.layoutItemPanelTreeView.Location = new System.Drawing.Point(0, 71);
         this.layoutItemPanelTreeView.Name = "layoutItemPanelTreeView";
         this.layoutItemPanelTreeView.Size = new System.Drawing.Size(560, 520);
         this.layoutItemPanelTreeView.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemPanelTreeView.TextSize = new System.Drawing.Size(122, 13);
         // 
         // SelectReferenceView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.Controls.Add(this.layoutControl);
         this.Name = "SelectReferenceView";
         this.Size = new System.Drawing.Size(580, 632);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.panelReferenceTreeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditSelectLocalisation.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroupReferenceType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRadioGroup)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemLocalisation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanelTreeView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl;
      private DevExpress.XtraEditors.PanelControl panelReferenceTreeView;
      private DevExpress.XtraEditors.ButtonEdit btEditSelectLocalisation;
      private DevExpress.XtraEditors.RadioGroup radioGroupReferenceType;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRadioGroup;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemLocalisation;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPanelTreeView;
   }
}
