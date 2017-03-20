namespace MoBi.UI.Views
{
   partial class EditUnitView
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
         this.edName = new DevExpress.XtraEditors.TextEdit();
         this.lblName = new DevExpress.XtraEditors.LabelControl();
         this.lblfacrtor = new DevExpress.XtraEditors.LabelControl();
         this.lblOffset = new DevExpress.XtraEditors.LabelControl();
         this.edFactor = new DevExpress.XtraEditors.CalcEdit();
         this.edOffset = new DevExpress.XtraEditors.CalcEdit();
         ((System.ComponentModel.ISupportInitialize)(this.edName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.edFactor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.edOffset.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // edName
         // 
         this.edName.Location = new System.Drawing.Point(106, 18);
         this.edName.Name = "edName";
         this.edName.Size = new System.Drawing.Size(98, 20);
         this.edName.TabIndex = 24;
         // 
         // lblName
         // 
         this.lblName.Location = new System.Drawing.Point(23, 25);
         this.lblName.Name = "lblName";
         this.lblName.Size = new System.Drawing.Size(27, 13);
         this.lblName.TabIndex = 27;
         this.lblName.Text = "Name:";
         // 
         // lblfacrtor
         // 
         this.lblfacrtor.Location = new System.Drawing.Point(23, 61);
         this.lblfacrtor.Name = "lblfacrtor";
         this.lblfacrtor.Size = new System.Drawing.Size(31, 13);
         this.lblfacrtor.TabIndex = 28;
         this.lblfacrtor.Text = "Factor:";
         // 
         // lblOffset
         // 
         this.lblOffset.Location = new System.Drawing.Point(23, 97);
         this.lblOffset.Name = "lblOffset";
         this.lblOffset.Size = new System.Drawing.Size(31, 13);
         this.lblOffset.TabIndex = 29;
         this.lblOffset.Text = "Offset:";
         // 
         // edFactor
         // 
         this.edFactor.Location = new System.Drawing.Point(104, 58);
         this.edFactor.Name = "edFactor";
         this.edFactor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.edFactor.Size = new System.Drawing.Size(100, 20);
         this.edFactor.TabIndex = 30;
         // 
         // edOffset
         // 
         this.edOffset.Location = new System.Drawing.Point(104, 89);
         this.edOffset.Name = "edOffset";
         this.edOffset.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.edOffset.Size = new System.Drawing.Size(100, 20);
         this.edOffset.TabIndex = 31;
         // 
         // EditUnitView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.edOffset);
         this.Controls.Add(this.edFactor);
         this.Controls.Add(this.lblOffset);
         this.Controls.Add(this.lblfacrtor);
         this.Controls.Add(this.lblName);
         this.Controls.Add(this.edName);
         this.Name = "EditUnitView";
         this.Size = new System.Drawing.Size(207, 117);
         ((System.ComponentModel.ISupportInitialize)(this.edName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.edFactor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.edOffset.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.TextEdit edName;
      private DevExpress.XtraEditors.LabelControl lblName;
      private DevExpress.XtraEditors.LabelControl lblfacrtor;
      private DevExpress.XtraEditors.LabelControl lblOffset;
      private DevExpress.XtraEditors.CalcEdit edFactor;
      private DevExpress.XtraEditors.CalcEdit edOffset;
   }
}
