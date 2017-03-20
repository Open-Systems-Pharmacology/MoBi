using OSPSuite.UI.Controls;
using DevExpress.XtraTab;

namespace MoBi.UI.Views
{
   partial class EditTransportBuilderView
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
         this.tab = new DevExpress.XtraTab.XtraTabControl();
         this.tabProperties = new DevExpress.XtraTab.XtraTabPage();
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkPlotParameter = new UxCheckEdit();
         this.barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.chkCreateParameter = new UxCheckEdit();
         this.htmlEditor = new DevExpress.XtraEditors.MemoExEdit();
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         this.btName = new DevExpress.XtraEditors.ButtonEdit();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemName = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemTagKinetic = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.tabKinetic = new DevExpress.XtraTab.XtraTabPage();
         this.tabParameters = new DevExpress.XtraTab.XtraTabPage();
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.layoutControl2 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.panelSource = new DevExpress.XtraEditors.PanelControl();
         this.layoutItemPanelSource = new DevExpress.XtraLayout.LayoutControlItem();
         this.panelTarget = new DevExpress.XtraEditors.PanelControl();
         this.layouytItemPanelTarget = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tab)).BeginInit();
         this.tab.SuspendLayout();
         this.tabProperties.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkPlotParameter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkCreateParameter.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTagKinetic)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
         this.layoutControl2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanelSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTarget)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouytItemPanelTarget)).BeginInit();
         this.SuspendLayout();
         // 
         // tab
         // 
         this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tab.HeaderButtonsShowMode = DevExpress.XtraTab.TabButtonShowMode.Never;
         this.tab.Location = new System.Drawing.Point(0, 0);
         this.tab.Name = "tab";
         this.tab.SelectedTabPage = this.tabProperties;
         this.tab.Size = new System.Drawing.Size(826, 609);
         this.tab.TabIndex = 0;
         this.tab.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabProperties,
            this.tabKinetic,
            this.tabParameters});
         // 
         // tabProperties
         // 
         this.tabProperties.Controls.Add(this.layoutControl1);
         this.tabProperties.Name = "tabProperties";
         this.tabProperties.Size = new System.Drawing.Size(820, 581);
         this.tabProperties.Text = "Properties";
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.chkPlotParameter);
         this.layoutControl1.Controls.Add(this.chkCreateParameter);
         this.layoutControl1.Controls.Add(this.htmlEditor);
         this.layoutControl1.Controls.Add(this.splitContainerControl1);
         this.layoutControl1.Controls.Add(this.btName);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControlGroup1;
         this.layoutControl1.Size = new System.Drawing.Size(820, 581);
         this.layoutControl1.TabIndex = 0;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // chkPlotParameter
         // 
         this.chkPlotParameter.Location = new System.Drawing.Point(412, 526);
         this.chkPlotParameter.MenuManager = this.barManager;
         this.chkPlotParameter.Name = "chkPlotParameter";
         this.chkPlotParameter.Properties.Caption = "chkPlotParameter";
         this.chkPlotParameter.Size = new System.Drawing.Size(396, 19);
         this.chkPlotParameter.StyleController = this.layoutControl1;
         this.chkPlotParameter.TabIndex = 8;
         // 
         // barManager
         // 
         this.barManager.DockControls.Add(this.barDockControlTop);
         this.barManager.DockControls.Add(this.barDockControlBottom);
         this.barManager.DockControls.Add(this.barDockControlLeft);
         this.barManager.DockControls.Add(this.barDockControlRight);
         this.barManager.Form = this;
         this.barManager.MaxItemId = 0;
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Size = new System.Drawing.Size(826, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 609);
         this.barDockControlBottom.Size = new System.Drawing.Size(826, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 609);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(826, 0);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 609);
         // 
         // chkCreateParameter
         // 
         this.chkCreateParameter.Location = new System.Drawing.Point(12, 526);
         this.chkCreateParameter.MenuManager = this.barManager;
         this.chkCreateParameter.Name = "chkCreateParameter";
         this.chkCreateParameter.Properties.Caption = "chkCreateParameter";
         this.chkCreateParameter.Size = new System.Drawing.Size(396, 19);
         this.chkCreateParameter.StyleController = this.layoutControl1;
         this.chkCreateParameter.TabIndex = 7;
         // 
         // htmlEditor
         // 
         this.htmlEditor.Location = new System.Drawing.Point(120, 549);
         this.htmlEditor.MenuManager = this.barManager;
         this.htmlEditor.Name = "htmlEditor";
         this.htmlEditor.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.htmlEditor.Size = new System.Drawing.Size(688, 20);
         this.htmlEditor.StyleController = this.layoutControl1;
         this.htmlEditor.TabIndex = 6;
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Horizontal = false;
         this.splitContainerControl1.Location = new System.Drawing.Point(120, 36);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Controls.Add(this.layoutControl2);
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(688, 486);
         this.splitContainerControl1.SplitterPosition = 205;
         this.splitContainerControl1.TabIndex = 5;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // btName
         // 
         this.btName.Location = new System.Drawing.Point(120, 12);
         this.btName.MenuManager = this.barManager;
         this.btName.Name = "btName";
         this.btName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.btName.Size = new System.Drawing.Size(688, 20);
         this.btName.StyleController = this.layoutControl1;
         this.btName.TabIndex = 4;
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup1.GroupBordersVisible = false;
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemName,
            this.layoutItemTagKinetic,
            this.layoutItemDescription,
            this.layoutControlItem1,
            this.layoutControlItem2});
         this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Size = new System.Drawing.Size(820, 581);
         this.layoutControlGroup1.Text = "layoutControlGroup1";
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutItemName
         // 
         this.layoutItemName.Control = this.btName;
         this.layoutItemName.CustomizationFormText = "layoutItemName";
         this.layoutItemName.Location = new System.Drawing.Point(0, 0);
         this.layoutItemName.Name = "layoutItemName";
         this.layoutItemName.Size = new System.Drawing.Size(800, 24);
         this.layoutItemName.Text = "layoutItemName";
         this.layoutItemName.TextSize = new System.Drawing.Size(105, 13);
         // 
         // layoutItemTagKinetic
         // 
         this.layoutItemTagKinetic.Control = this.splitContainerControl1;
         this.layoutItemTagKinetic.CustomizationFormText = "layoutItemTagKinetic";
         this.layoutItemTagKinetic.Location = new System.Drawing.Point(0, 24);
         this.layoutItemTagKinetic.Name = "layoutItemTagKinetic";
         this.layoutItemTagKinetic.Size = new System.Drawing.Size(800, 490);
         this.layoutItemTagKinetic.Text = "layoutItemTagKinetic";
         this.layoutItemTagKinetic.TextSize = new System.Drawing.Size(105, 13);
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.htmlEditor;
         this.layoutItemDescription.CustomizationFormText = "layoutItemDescription";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 537);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(800, 24);
         this.layoutItemDescription.Text = "layoutItemDescription";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(105, 13);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkCreateParameter;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 514);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(400, 23);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chkPlotParameter;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(400, 514);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(400, 23);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // tabKinetic
         // 
         this.tabKinetic.Name = "tabKinetic";
         this.tabKinetic.Padding = new System.Windows.Forms.Padding(10);
         this.tabKinetic.Size = new System.Drawing.Size(935, 622);
         this.tabKinetic.Text = "Kinetic";
         // 
         // tabParameters
         // 
         this.tabParameters.Name = "tabParameters";
         this.tabParameters.Size = new System.Drawing.Size(935, 622);
         this.tabParameters.Text = "Parameters";
         // 
         // gridView1
         // 
         this.gridView1.Name = "gridView1";
         // 
         // layoutControl2
         // 
         this.layoutControl2.Controls.Add(this.panelTarget);
         this.layoutControl2.Controls.Add(this.panelSource);
         this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl2.Location = new System.Drawing.Point(0, 0);
         this.layoutControl2.Name = "layoutControl2";
         this.layoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(697, 35, 250, 350);
         this.layoutControl2.Root = this.layoutControlGroup2;
         this.layoutControl2.Size = new System.Drawing.Size(688, 205);
         this.layoutControl2.TabIndex = 0;
         this.layoutControl2.Text = "layoutControl2";
         // 
         // layoutControlGroup2
         // 
         this.layoutControlGroup2.CustomizationFormText = "layoutControlGroup2";
         this.layoutControlGroup2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControlGroup2.GroupBordersVisible = false;
         this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemPanelSource,
            this.layouytItemPanelTarget});
         this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
         this.layoutControlGroup2.Name = "layoutControlGroup2";
         this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup2.Size = new System.Drawing.Size(688, 205);
         this.layoutControlGroup2.Text = "layoutControlGroup2";
         this.layoutControlGroup2.TextVisible = false;
         // 
         // panelSource
         // 
         this.panelSource.Location = new System.Drawing.Point(2, 18);
         this.panelSource.Name = "panelSource";
         this.panelSource.Size = new System.Drawing.Size(342, 185);
         this.panelSource.TabIndex = 4;
         // 
         // layoutItemPanelSource
         // 
         this.layoutItemPanelSource.Control = this.panelSource;
         this.layoutItemPanelSource.CustomizationFormText = "layoutItemPanelSource";
         this.layoutItemPanelSource.Location = new System.Drawing.Point(0, 0);
         this.layoutItemPanelSource.Name = "layoutItemPanelSource";
         this.layoutItemPanelSource.Size = new System.Drawing.Size(346, 205);
         this.layoutItemPanelSource.Text = "layoutItemPanelSource";
         this.layoutItemPanelSource.TextLocation = DevExpress.Utils.Locations.Top;
         this.layoutItemPanelSource.TextSize = new System.Drawing.Size(116, 13);
         // 
         // panelTarget
         // 
         this.panelTarget.Location = new System.Drawing.Point(348, 18);
         this.panelTarget.Name = "panelTarget";
         this.panelTarget.Size = new System.Drawing.Size(338, 185);
         this.panelTarget.TabIndex = 5;
         // 
         // layouytItemPanelTarget
         // 
         this.layouytItemPanelTarget.Control = this.panelTarget;
         this.layouytItemPanelTarget.CustomizationFormText = "layouytItemPanelTarget";
         this.layouytItemPanelTarget.Location = new System.Drawing.Point(346, 0);
         this.layouytItemPanelTarget.Name = "layouytItemPanelTarget";
         this.layouytItemPanelTarget.Size = new System.Drawing.Size(342, 205);
         this.layouytItemPanelTarget.Text = "layouytItemPanelTarget";
         this.layouytItemPanelTarget.TextLocation = DevExpress.Utils.Locations.Top;
         this.layouytItemPanelTarget.TextSize = new System.Drawing.Size(116, 13);
         // 
         // EditTransportBuilderView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.Controls.Add(this.tab);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "EditTransportBuilderView";
         this.Size = new System.Drawing.Size(826, 609);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tab)).EndInit();
         this.tab.ResumeLayout(false);
         this.tabProperties.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkPlotParameter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkCreateParameter.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.htmlEditor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.btName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemName)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTagKinetic)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
         this.layoutControl2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemPanelSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelTarget)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layouytItemPanelTarget)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private XtraTabControl tab;
      private XtraTabPage tabParameters;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarManager barManager;
      private XtraTabPage tabProperties;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl1;
      private DevExpress.XtraEditors.MemoExEdit htmlEditor;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
      private DevExpress.XtraEditors.ButtonEdit btName;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemName;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTagKinetic;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraEditors.CheckEdit chkCreateParameter;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private XtraTabPage tabKinetic;
      private DevExpress.XtraEditors.CheckEdit chkPlotParameter;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private OSPSuite.UI.Controls.UxLayoutControl layoutControl2;
      private DevExpress.XtraEditors.PanelControl panelTarget;
      private DevExpress.XtraEditors.PanelControl panelSource;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemPanelSource;
      private DevExpress.XtraLayout.LayoutControlItem layouytItemPanelTarget;
   }
}
