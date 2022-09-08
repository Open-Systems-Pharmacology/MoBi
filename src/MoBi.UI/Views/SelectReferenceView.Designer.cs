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
         this.groupControl = new DevExpress.XtraEditors.GroupControl();
         this.lblLocalisation = new DevExpress.XtraEditors.LabelControl();
         this.grpEntityTreeView = new DevExpress.XtraEditors.GroupControl();
         this.radioGroupReferenceType = new DevExpress.XtraEditors.RadioGroup();
         this.btEditSelectLocalisation = new DevExpress.XtraEditors.ButtonEdit();
         ((System.ComponentModel.ISupportInitialize)(this.groupControl)).BeginInit();
         this.groupControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.grpEntityTreeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroupReferenceType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditSelectLocalisation.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // groupControl
         // 
         this.groupControl.Controls.Add(this.lblLocalisation);
         this.groupControl.Controls.Add(this.grpEntityTreeView);
         this.groupControl.Controls.Add(this.radioGroupReferenceType);
         this.groupControl.Controls.Add(this.btEditSelectLocalisation);
         this.groupControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupControl.Location = new System.Drawing.Point(0, 0);
         this.groupControl.Name = "groupControl";
         this.groupControl.Size = new System.Drawing.Size(239, 451);
         this.groupControl.TabIndex = 0;
         this.groupControl.Text = "groupControl";
         // 
         // lblLocalisation
         // 
         this.lblLocalisation.Location = new System.Drawing.Point(3, 26);
         this.lblLocalisation.Name = "lblLocalisation";
         this.lblLocalisation.Size = new System.Drawing.Size(104, 13);
         this.lblLocalisation.TabIndex = 6;
         // 
         // grpEntityTreeView
         // 
         this.grpEntityTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                     | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.grpEntityTreeView.Location = new System.Drawing.Point(4, 98);
         this.grpEntityTreeView.Name = "grpEntityTreeView";
         this.grpEntityTreeView.Size = new System.Drawing.Size(232, 348);
         this.grpEntityTreeView.TabIndex = 5;
         this.grpEntityTreeView.Text = "Possible Referenced Objects";
         // 
         // rgReferenceType
         // 
         this.radioGroupReferenceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.radioGroupReferenceType.Location = new System.Drawing.Point(3, 71);
         this.radioGroupReferenceType.Name = "radioGroupReferenceType";
         this.radioGroupReferenceType.Size = new System.Drawing.Size(232, 21);
         this.radioGroupReferenceType.TabIndex = 4;
         this.radioGroupReferenceType.SelectedIndexChanged += new System.EventHandler(this.rgReferenceType_SelectedIndexChanged);
         // 
         // btEditSelectLocalisation
         // 
         this.btEditSelectLocalisation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.btEditSelectLocalisation.Location = new System.Drawing.Point(3, 45);
         this.btEditSelectLocalisation.Name = "btEditSelectLocalisation";
         this.btEditSelectLocalisation.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btEditSelectLocalisation.Size = new System.Drawing.Size(231, 20);
         this.btEditSelectLocalisation.TabIndex = 3;
         this.btEditSelectLocalisation.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.btEditSelectLocalisation_ButtonClick);
         // 
         // SelectReferenceView
         // 
         this.Controls.Add(this.groupControl);
         this.Name = "SelectReferenceView";
         this.Size = new System.Drawing.Size(239, 451);
         ((System.ComponentModel.ISupportInitialize)(this.groupControl)).EndInit();
         this.groupControl.ResumeLayout(false);
         this.groupControl.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.grpEntityTreeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.radioGroupReferenceType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditSelectLocalisation.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.GroupControl groupControl;
      private DevExpress.XtraEditors.GroupControl grpEntityTreeView;
      private DevExpress.XtraEditors.RadioGroup radioGroupReferenceType;
      private DevExpress.XtraEditors.ButtonEdit btEditSelectLocalisation;
      private DevExpress.XtraEditors.LabelControl lblLocalisation;

   }
}
