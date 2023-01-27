namespace MoBi.UI.Views
{
    partial class NewNameExpressionProfileBuildingBlockView
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

            disposeBinders();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.renameTablePanel = new DevExpress.Utils.Layout.TablePanel();
         this.tbCategory = new DevExpress.XtraEditors.TextEdit();
         this.tbMoleculeName = new DevExpress.XtraEditors.TextEdit();
         this.tbSpecies = new DevExpress.XtraEditors.TextEdit();
         this.lblCategory = new DevExpress.XtraEditors.LabelControl();
         this.lblNameType = new DevExpress.XtraEditors.LabelControl();
         this.lblSpecies = new DevExpress.XtraEditors.LabelControl();
         this.lblName = new DevExpress.XtraEditors.LabelControl();
         this.tbName = new DevExpress.XtraEditors.TextEdit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.renameTablePanel)).BeginInit();
         this.renameTablePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Location = new System.Drawing.Point(0, 106);
         // 
         // renameTablePanel
         // 
         this.renameTablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 50F)});
         this.renameTablePanel.Controls.Add(this.tbName);
         this.renameTablePanel.Controls.Add(this.lblName);
         this.renameTablePanel.Controls.Add(this.tbCategory);
         this.renameTablePanel.Controls.Add(this.tbMoleculeName);
         this.renameTablePanel.Controls.Add(this.tbSpecies);
         this.renameTablePanel.Controls.Add(this.lblCategory);
         this.renameTablePanel.Controls.Add(this.lblNameType);
         this.renameTablePanel.Controls.Add(this.lblSpecies);
         this.renameTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.renameTablePanel.Location = new System.Drawing.Point(0, 0);
         this.renameTablePanel.Name = "renameTablePanel";
         this.renameTablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F)});
         this.renameTablePanel.Size = new System.Drawing.Size(580, 106);
         this.renameTablePanel.TabIndex = 39;
         // 
         // tbCategory
         // 
         this.renameTablePanel.SetColumn(this.tbCategory, 1);
         this.tbCategory.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbCategory.Location = new System.Drawing.Point(70, 55);
         this.tbCategory.Name = "tbCategory";
         this.renameTablePanel.SetRow(this.tbCategory, 2);
         this.tbCategory.Size = new System.Drawing.Size(507, 20);
         this.tbCategory.TabIndex = 5;
         // 
         // tbMoleculeName
         // 
         this.renameTablePanel.SetColumn(this.tbMoleculeName, 1);
         this.tbMoleculeName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbMoleculeName.Location = new System.Drawing.Point(70, 29);
         this.tbMoleculeName.Name = "tbMoleculeName";
         this.renameTablePanel.SetRow(this.tbMoleculeName, 1);
         this.tbMoleculeName.Size = new System.Drawing.Size(507, 20);
         this.tbMoleculeName.TabIndex = 4;
         // 
         // tbSpecies
         // 
         this.renameTablePanel.SetColumn(this.tbSpecies, 1);
         this.tbSpecies.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbSpecies.Location = new System.Drawing.Point(70, 3);
         this.tbSpecies.Name = "tbSpecies";
         this.renameTablePanel.SetRow(this.tbSpecies, 0);
         this.tbSpecies.Size = new System.Drawing.Size(507, 20);
         this.tbSpecies.TabIndex = 3;
         // 
         // lblCategory
         // 
         this.renameTablePanel.SetColumn(this.lblCategory, 0);
         this.lblCategory.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblCategory.Location = new System.Drawing.Point(3, 55);
         this.lblCategory.Name = "lblCategory";
         this.renameTablePanel.SetRow(this.lblCategory, 2);
         this.lblCategory.Size = new System.Drawing.Size(61, 20);
         this.lblCategory.TabIndex = 2;
         this.lblCategory.Text = "lblCategory";
         // 
         // lblNameType
         // 
         this.renameTablePanel.SetColumn(this.lblNameType, 0);
         this.lblNameType.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblNameType.Location = new System.Drawing.Point(3, 29);
         this.lblNameType.Name = "lblNameType";
         this.renameTablePanel.SetRow(this.lblNameType, 1);
         this.lblNameType.Size = new System.Drawing.Size(61, 20);
         this.lblNameType.TabIndex = 1;
         this.lblNameType.Text = "lblNameType";
         // 
         // lblSpecies
         // 
         this.renameTablePanel.SetColumn(this.lblSpecies, 0);
         this.lblSpecies.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblSpecies.Location = new System.Drawing.Point(3, 3);
         this.lblSpecies.Name = "lblSpecies";
         this.renameTablePanel.SetRow(this.lblSpecies, 0);
         this.lblSpecies.Size = new System.Drawing.Size(61, 20);
         this.lblSpecies.TabIndex = 0;
         this.lblSpecies.Text = "lblSpecies";
         // 
         // lblName
         // 
         this.renameTablePanel.SetColumn(this.lblName, 0);
         this.lblName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.lblName.Location = new System.Drawing.Point(3, 81);
         this.lblName.Name = "lblName";
         this.renameTablePanel.SetRow(this.lblName, 3);
         this.lblName.Size = new System.Drawing.Size(61, 22);
         this.lblName.TabIndex = 6;
         this.lblName.Text = "lblName";
         // 
         // tbName
         // 
         this.renameTablePanel.SetColumn(this.tbName, 1);
         this.tbName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tbName.Location = new System.Drawing.Point(70, 81);
         this.tbName.Name = "tbName";
         this.renameTablePanel.SetRow(this.tbName, 3);
         this.tbName.Size = new System.Drawing.Size(507, 20);
         this.tbName.TabIndex = 7;
         // 
         // RenameExpressionProfileBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(580, 149);
         this.Controls.Add(this.renameTablePanel);
         this.Name = "NewNameExpressionProfileBuildingBlockView";
         this.Controls.SetChildIndex(this.tablePanel, 0);
         this.Controls.SetChildIndex(this.renameTablePanel, 0);
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.renameTablePanel)).EndInit();
         this.renameTablePanel.ResumeLayout(false);
         this.renameTablePanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbCategory.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbMoleculeName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSpecies.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbName.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel renameTablePanel;
        private DevExpress.XtraEditors.TextEdit tbCategory;
        private DevExpress.XtraEditors.TextEdit tbMoleculeName;
        private DevExpress.XtraEditors.TextEdit tbSpecies;
        private DevExpress.XtraEditors.LabelControl lblCategory;
        private DevExpress.XtraEditors.LabelControl lblNameType;
        private DevExpress.XtraEditors.LabelControl lblSpecies;
        private DevExpress.XtraEditors.LabelControl lblName;
        private DevExpress.XtraEditors.TextEdit tbName;
    }
}
