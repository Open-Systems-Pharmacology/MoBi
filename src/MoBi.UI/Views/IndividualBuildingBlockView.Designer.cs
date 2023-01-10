namespace MoBi.UI.Views
{
    partial class IndividualBuildingBlockView
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.tablePanel = new DevExpress.Utils.Layout.TablePanel();
         this.gridControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridView = new OSPSuite.UI.Controls.UxGridView();
         this.originGridControl = new DevExpress.XtraGrid.GridControl();
         this.cardView1 = new DevExpress.XtraGrid.Views.Card.CardView();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).BeginInit();
         this.tablePanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.originGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cardView1)).BeginInit();
         this.SuspendLayout();
         // 
         // tablePanel
         // 
         this.tablePanel.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
         this.tablePanel.Controls.Add(this.originGridControl);
         this.tablePanel.Controls.Add(this.gridControl);
         this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tablePanel.Location = new System.Drawing.Point(0, 0);
         this.tablePanel.Name = "tablePanel";
         this.tablePanel.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.AutoSize, 26F)});
         this.tablePanel.Size = new System.Drawing.Size(731, 387);
         this.tablePanel.TabIndex = 0;
         // 
         // gridControl
         // 
         this.tablePanel.SetColumn(this.gridControl, 0);
         this.tablePanel.SetColumnSpan(this.gridControl, 2);
         this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl.Location = new System.Drawing.Point(3, 128);
         this.gridControl.MainView = this.gridView;
         this.gridControl.Name = "gridControl";
         this.tablePanel.SetRow(this.gridControl, 1);
         this.gridControl.Size = new System.Drawing.Size(725, 256);
         this.gridControl.TabIndex = 0;
         this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
         // 
         // gridView
         // 
         this.gridView.AllowsFiltering = true;
         this.gridView.EnableColumnContextMenu = true;
         this.gridView.GridControl = this.gridControl;
         this.gridView.MultiSelect = true;
         this.gridView.Name = "gridView";
         this.gridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
         this.gridView.OptionsNavigation.AutoFocusNewRow = true;
         this.gridView.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView.OptionsSelection.EnableAppearanceFocusedRow = false;
         this.gridView.OptionsSelection.MultiSelect = true;
         this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
         // 
         // originGridControl
         // 
         this.tablePanel.SetColumn(this.originGridControl, 0);
         this.tablePanel.SetColumnSpan(this.originGridControl, 2);
         this.originGridControl.Location = new System.Drawing.Point(3, 3);
         this.originGridControl.MainView = this.cardView1;
         this.originGridControl.Name = "originGridControl";
         this.tablePanel.SetRow(this.originGridControl, 0);
         this.originGridControl.Size = new System.Drawing.Size(725, 119);
         this.originGridControl.TabIndex = 1;
         this.originGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.cardView1});
         // 
         // cardView1
         // 
         this.cardView1.GridControl = this.originGridControl;
         this.cardView1.Name = "cardView1";
         this.cardView1.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
         // 
         // IndividualBuildingBlockView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tablePanel);
         this.Name = "IndividualBuildingBlockView";
         this.Size = new System.Drawing.Size(731, 387);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tablePanel)).EndInit();
         this.tablePanel.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.originGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cardView1)).EndInit();
         this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel;
        private OSPSuite.UI.Controls.UxGridControl gridControl;
        private OSPSuite.UI.Controls.UxGridView gridView;
        private DevExpress.XtraGrid.GridControl originGridControl;
        private DevExpress.XtraGrid.Views.Card.CardView cardView1;
    }
}
