using OSPSuite.UI.Controls;

namespace MoBi.UI.Views
{
   partial class SearchView
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
         _gridResultBinder.Dispose();
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
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkCaseSensitive = new UxCheckEdit();
         this.chkRegExSearch = new UxCheckEdit();
         this.ckWholeName = new UxCheckEdit();
         this.cbScope = new DevExpress.XtraEditors.ComboBoxEdit();
         this.txtSearchExpression = new DevExpress.XtraEditors.ButtonEdit();
         this.grdResultControl = new OSPSuite.UI.Controls.UxGridControl();
         this.gridSearchResult = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlResult = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlSearchTerm = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlSearchScope = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItemCaseSensitive = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkCaseSensitive.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkRegExSearch.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ckWholeName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbScope.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSearchExpression.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdResultControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridSearchResult)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlResult)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlSearchTerm)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlSearchScope)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCaseSensitive)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.chkCaseSensitive);
         this.layoutControl1.Controls.Add(this.chkRegExSearch);
         this.layoutControl1.Controls.Add(this.ckWholeName);
         this.layoutControl1.Controls.Add(this.cbScope);
         this.layoutControl1.Controls.Add(this.txtSearchExpression);
         this.layoutControl1.Controls.Add(this.grdResultControl);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(329, 364);
         this.layoutControl1.TabIndex = 1;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // chkCaseSensitive
         // 
         this.chkCaseSensitive.Location = new System.Drawing.Point(12, 106);
         this.chkCaseSensitive.Name = "chkCaseSensitive";
         this.chkCaseSensitive.Properties.Caption = "checkEdit1";
         this.chkCaseSensitive.Size = new System.Drawing.Size(305, 19);
         this.chkCaseSensitive.StyleController = this.layoutControl1;
         this.chkCaseSensitive.TabIndex = 9;
         // 
         // chkRegExSearch
         // 
         this.chkRegExSearch.Location = new System.Drawing.Point(12, 83);
         this.chkRegExSearch.Name = "chkRegExSearch";
         this.chkRegExSearch.Properties.Caption = "checkEdit2";
         this.chkRegExSearch.Size = new System.Drawing.Size(305, 19);
         this.chkRegExSearch.StyleController = this.layoutControl1;
         this.chkRegExSearch.TabIndex = 8;
         // 
         // ckWholeName
         // 
         this.ckWholeName.Location = new System.Drawing.Point(12, 60);
         this.ckWholeName.Name = "ckWholeName";
         this.ckWholeName.Properties.Caption = "checkEdit1";
         this.ckWholeName.Size = new System.Drawing.Size(305, 19);
         this.ckWholeName.StyleController = this.layoutControl1;
         this.ckWholeName.TabIndex = 7;
         // 
         // cbScope
         // 
         this.cbScope.Location = new System.Drawing.Point(142, 36);
         this.cbScope.Name = "cbScope";
         this.cbScope.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbScope.Size = new System.Drawing.Size(175, 20);
         this.cbScope.StyleController = this.layoutControl1;
         this.cbScope.TabIndex = 6;
         // 
         // txtSearchExpression
         // 
         this.txtSearchExpression.Location = new System.Drawing.Point(142, 12);
         this.txtSearchExpression.Name = "txtSearchExpression";
         this.txtSearchExpression.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.txtSearchExpression.Size = new System.Drawing.Size(175, 20);
         this.txtSearchExpression.StyleController = this.layoutControl1;
         this.txtSearchExpression.TabIndex = 5;
         // 
         // grdResultControl
         // 
         this.grdResultControl.Location = new System.Drawing.Point(12, 145);
         this.grdResultControl.MainView = this.gridSearchResult;
         this.grdResultControl.Name = "grdResultControl";
         this.grdResultControl.Size = new System.Drawing.Size(305, 207);
         this.grdResultControl.TabIndex = 4;
         this.grdResultControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridSearchResult});
         // 
         // gridSearchResult
         // 
         this.gridSearchResult.GridControl = this.grdResultControl;
         this.gridSearchResult.Name = "gridSearchResult";
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlResult,
            this.layoutControlSearchTerm,
            this.layoutControlSearchScope,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.layoutControlItemCaseSensitive});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(329, 364);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlResult
         // 
         this.layoutControlResult.Control = this.grdResultControl;
         this.layoutControlResult.CustomizationFormText = "layoutControlResult";
         this.layoutControlResult.Location = new System.Drawing.Point(0, 117);
         this.layoutControlResult.Name = "layoutControlResult";
         this.layoutControlResult.Size = new System.Drawing.Size(309, 227);
         this.layoutControlResult.Text = "layoutControlResult";
         this.layoutControlResult.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutControlResult.TextSize = new System.Drawing.Size(127, 13);
         // 
         // layoutControlSearchTerm
         // 
         this.layoutControlSearchTerm.Control = this.txtSearchExpression;
         this.layoutControlSearchTerm.CustomizationFormText = "layoutControlSearchTerm";
         this.layoutControlSearchTerm.Location = new System.Drawing.Point(0, 0);
         this.layoutControlSearchTerm.Name = "layoutControlSearchTerm";
         this.layoutControlSearchTerm.Size = new System.Drawing.Size(309, 24);
         this.layoutControlSearchTerm.Text = "layoutControlSearchTerm";
         this.layoutControlSearchTerm.TextSize = new System.Drawing.Size(127, 13);
         // 
         // layoutControlSearchScope
         // 
         this.layoutControlSearchScope.Control = this.cbScope;
         this.layoutControlSearchScope.CustomizationFormText = "layoutControlSearchScope";
         this.layoutControlSearchScope.Location = new System.Drawing.Point(0, 24);
         this.layoutControlSearchScope.Name = "layoutControlSearchScope";
         this.layoutControlSearchScope.Size = new System.Drawing.Size(309, 24);
         this.layoutControlSearchScope.Text = "layoutControlSearchScope";
         this.layoutControlSearchScope.TextSize = new System.Drawing.Size(127, 13);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.ckWholeName;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 48);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(309, 23);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chkRegExSearch;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(0, 71);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(309, 23);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutControlItemCaseSensitive
         // 
         this.layoutControlItemCaseSensitive.Control = this.chkCaseSensitive;
         this.layoutControlItemCaseSensitive.CustomizationFormText = "layoutControlItemCaseSensitive";
         this.layoutControlItemCaseSensitive.Location = new System.Drawing.Point(0, 94);
         this.layoutControlItemCaseSensitive.Name = "layoutControlItemCaseSensitive";
         this.layoutControlItemCaseSensitive.Size = new System.Drawing.Size(309, 23);
         this.layoutControlItemCaseSensitive.Text = "layoutControlItemCaseSensitive";
         this.layoutControlItemCaseSensitive.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItemCaseSensitive.TextToControlDistance = 0;
         this.layoutControlItemCaseSensitive.TextVisible = false;
         // 
         // SearchView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl1);
         this.Name = "SearchView";
         this.Size = new System.Drawing.Size(329, 364);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkCaseSensitive.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkRegExSearch.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ckWholeName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbScope.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.txtSearchExpression.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdResultControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridSearchResult)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlResult)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlSearchTerm)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlSearchScope)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemCaseSensitive)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraEditors.ComboBoxEdit cbScope;
      private DevExpress.XtraEditors.ButtonEdit txtSearchExpression;
      private DevExpress.XtraGrid.GridControl grdResultControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridSearchResult;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlResult;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlSearchTerm;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlSearchScope;
      private DevExpress.XtraEditors.CheckEdit chkRegExSearch;
      private DevExpress.XtraEditors.CheckEdit ckWholeName;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.CheckEdit chkCaseSensitive;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItemCaseSensitive;
   }
}
