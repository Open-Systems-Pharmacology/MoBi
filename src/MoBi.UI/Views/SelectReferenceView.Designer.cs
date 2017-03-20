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
         this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
         this.lblLocalisation = new DevExpress.XtraEditors.LabelControl();
         this.grpEntityTreeView = new DevExpress.XtraEditors.GroupControl();
         this.rgReferenceType = new DevExpress.XtraEditors.RadioGroup();
         this.btEditSelectLocalisation = new DevExpress.XtraEditors.ButtonEdit();
         ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
         this.groupControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.grpEntityTreeView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgReferenceType.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditSelectLocalisation.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // groupControl1
         // 
         this.groupControl1.Controls.Add(this.lblLocalisation);
         this.groupControl1.Controls.Add(this.grpEntityTreeView);
         this.groupControl1.Controls.Add(this.rgReferenceType);
         this.groupControl1.Controls.Add(this.btEditSelectLocalisation);
         this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupControl1.Location = new System.Drawing.Point(0, 0);
         this.groupControl1.Name = "groupControl1";
         this.groupControl1.Size = new System.Drawing.Size(239, 451);
         this.groupControl1.TabIndex = 0;
         this.groupControl1.Text = "groupControl1";
         // 
         // lblLocalisation
         // 
         this.lblLocalisation.Location = new System.Drawing.Point(3, 26);
         this.lblLocalisation.Name = "lblLocalisation";
         this.lblLocalisation.Size = new System.Drawing.Size(104, 13);
         this.lblLocalisation.TabIndex = 6;
         this.lblLocalisation.Text = "Local Reference Point";
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
         this.rgReferenceType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                     | System.Windows.Forms.AnchorStyles.Right)));
         this.rgReferenceType.Location = new System.Drawing.Point(3, 71);
         this.rgReferenceType.Name = "rgReferenceType";
         this.rgReferenceType.Size = new System.Drawing.Size(232, 21);
         this.rgReferenceType.TabIndex = 4;
         this.rgReferenceType.SelectedIndexChanged += new System.EventHandler(this.rgReferenceType_SelectedIndexChanged);
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
         this.Controls.Add(this.groupControl1);
         this.Name = "SelectReferenceView";
         this.Size = new System.Drawing.Size(239, 451);
         ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
         this.groupControl1.ResumeLayout(false);
         this.groupControl1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.grpEntityTreeView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgReferenceType.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.btEditSelectLocalisation.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.GroupControl groupControl1;
      private DevExpress.XtraEditors.GroupControl grpEntityTreeView;
      private DevExpress.XtraEditors.RadioGroup rgReferenceType;
      private DevExpress.XtraEditors.ButtonEdit btEditSelectLocalisation;
      private DevExpress.XtraEditors.LabelControl lblLocalisation;

   }
}
