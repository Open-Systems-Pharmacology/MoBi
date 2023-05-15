namespace MoBi.UI.Views
{
   partial class EditIndividualAndExpressionConfigurationsView
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
         this.components = new System.ComponentModel.Container();
         this.uxLayoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.simulationExpressionsTree = new OSPSuite.UI.Controls.UxTreeView();
         this.btnAdd = new OSPSuite.UI.Controls.UxSimpleButton();
         this.btnRemove = new OSPSuite.UI.Controls.UxSimpleButton();
         this.projectExpressionsTree = new OSPSuite.UI.Controls.FilterTreeView();
         this.cbIndividualSelection = new DevExpress.XtraEditors.ImageComboBoxEdit();
         this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupIndividual = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupExpression = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemProjectExpressions = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemAddButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemRemoveButton = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutItemSimulationExpressions = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).BeginInit();
         this.uxLayoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.simulationExpressionsTree)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbIndividualSelection.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupIndividual)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupExpression)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemProjectExpressions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveButton)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSimulationExpressions)).BeginInit();
         this.SuspendLayout();
         // 
         // uxLayoutControl1
         // 
         this.uxLayoutControl1.AllowCustomization = false;
         this.uxLayoutControl1.Controls.Add(this.simulationExpressionsTree);
         this.uxLayoutControl1.Controls.Add(this.btnAdd);
         this.uxLayoutControl1.Controls.Add(this.btnRemove);
         this.uxLayoutControl1.Controls.Add(this.projectExpressionsTree);
         this.uxLayoutControl1.Controls.Add(this.cbIndividualSelection);
         this.uxLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.uxLayoutControl1.Location = new System.Drawing.Point(0, 0);
         this.uxLayoutControl1.Name = "uxLayoutControl1";
         this.uxLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1087, 142, 650, 400);
         this.uxLayoutControl1.Root = this.Root;
         this.uxLayoutControl1.Size = new System.Drawing.Size(1100, 650);
         this.uxLayoutControl1.TabIndex = 0;
         this.uxLayoutControl1.Text = "uxLayoutControl1";
         // 
         // simulationExpressionsTree
         // 
         this.simulationExpressionsTree.IsLatched = false;
         this.simulationExpressionsTree.Location = new System.Drawing.Point(603, 116);
         this.simulationExpressionsTree.Name = "simulationExpressionsTree";
         this.simulationExpressionsTree.OptionsBehavior.Editable = false;
         this.simulationExpressionsTree.OptionsMenu.ShowExpandCollapseItems = false;
         this.simulationExpressionsTree.OptionsView.ShowColumns = false;
         this.simulationExpressionsTree.OptionsView.ShowHorzLines = false;
         this.simulationExpressionsTree.OptionsView.ShowIndicator = false;
         this.simulationExpressionsTree.OptionsView.ShowVertLines = false;
         this.simulationExpressionsTree.Size = new System.Drawing.Size(471, 508);
         this.simulationExpressionsTree.TabIndex = 8;
         this.simulationExpressionsTree.ToolTipForNode = null;
         this.simulationExpressionsTree.UseLazyLoading = false;
         // 
         // btnAdd
         // 
         this.btnAdd.Location = new System.Drawing.Point(501, 350);
         this.btnAdd.Manager = null;
         this.btnAdd.Name = "btnAdd";
         this.btnAdd.Shortcut = System.Windows.Forms.Keys.None;
         this.btnAdd.Size = new System.Drawing.Size(96, 22);
         this.btnAdd.StyleController = this.uxLayoutControl1;
         this.btnAdd.TabIndex = 7;
         this.btnAdd.Text = "btnAdd";
         // 
         // btnRemove
         // 
         this.btnRemove.Location = new System.Drawing.Point(501, 376);
         this.btnRemove.Manager = null;
         this.btnRemove.Name = "btnRemove";
         this.btnRemove.Shortcut = System.Windows.Forms.Keys.None;
         this.btnRemove.Size = new System.Drawing.Size(96, 22);
         this.btnRemove.StyleController = this.uxLayoutControl1;
         this.btnRemove.TabIndex = 6;
         this.btnRemove.Text = "btnRemove";
         // 
         // projectExpressionsTree
         // 
         this.projectExpressionsTree.Location = new System.Drawing.Point(24, 114);
         this.projectExpressionsTree.Name = "projectExpressionsTree";
         this.projectExpressionsTree.ShowDescendantNode = true;
         this.projectExpressionsTree.Size = new System.Drawing.Size(473, 512);
         this.projectExpressionsTree.TabIndex = 5;
         // 
         // cbIndividualSelection
         // 
         this.cbIndividualSelection.Location = new System.Drawing.Point(24, 45);
         this.cbIndividualSelection.Name = "cbIndividualSelection";
         this.cbIndividualSelection.Properties.AllowMouseWheel = false;
         this.cbIndividualSelection.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbIndividualSelection.Size = new System.Drawing.Size(1052, 20);
         this.cbIndividualSelection.StyleController = this.uxLayoutControl1;
         this.cbIndividualSelection.TabIndex = 4;
         // 
         // Root
         // 
         this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.Root.GroupBordersVisible = false;
         this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupIndividual,
            this.layoutGroupExpression});
         this.Root.Name = "Root";
         this.Root.Size = new System.Drawing.Size(1100, 650);
         this.Root.TextVisible = false;
         // 
         // layoutGroupIndividual
         // 
         this.layoutGroupIndividual.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
         this.layoutGroupIndividual.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupIndividual.Name = "layoutGroupIndividual";
         this.layoutGroupIndividual.Size = new System.Drawing.Size(1080, 69);
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.cbIndividualSelection;
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(1056, 24);
         this.layoutControlItem1.Text = "layoutItemIndividualSelect";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutGroupExpression
         // 
         this.layoutGroupExpression.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemProjectExpressions,
            this.emptySpaceItem1,
            this.layoutItemAddButton,
            this.layoutItemRemoveButton,
            this.emptySpaceItem2,
            this.layoutItemSimulationExpressions});
         this.layoutGroupExpression.Location = new System.Drawing.Point(0, 69);
         this.layoutGroupExpression.Name = "layoutGroupExpression";
         this.layoutGroupExpression.Size = new System.Drawing.Size(1080, 561);
         // 
         // layoutItemProjectExpressions
         // 
         this.layoutItemProjectExpressions.Control = this.projectExpressionsTree;
         this.layoutItemProjectExpressions.Location = new System.Drawing.Point(0, 0);
         this.layoutItemProjectExpressions.Name = "layoutItemProjectExpressions";
         this.layoutItemProjectExpressions.Size = new System.Drawing.Size(477, 516);
         this.layoutItemProjectExpressions.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemProjectExpressions.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.Location = new System.Drawing.Point(477, 0);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(100, 236);
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemAddButton
         // 
         this.layoutItemAddButton.Control = this.btnAdd;
         this.layoutItemAddButton.Location = new System.Drawing.Point(477, 236);
         this.layoutItemAddButton.Name = "layoutItemAddButton";
         this.layoutItemAddButton.Size = new System.Drawing.Size(100, 26);
         this.layoutItemAddButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemAddButton.TextVisible = false;
         // 
         // layoutItemRemoveButton
         // 
         this.layoutItemRemoveButton.Control = this.btnRemove;
         this.layoutItemRemoveButton.Location = new System.Drawing.Point(477, 262);
         this.layoutItemRemoveButton.Name = "layoutItemRemoveButton";
         this.layoutItemRemoveButton.Size = new System.Drawing.Size(100, 26);
         this.layoutItemRemoveButton.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemRemoveButton.TextVisible = false;
         // 
         // emptySpaceItem2
         // 
         this.emptySpaceItem2.AllowHotTrack = false;
         this.emptySpaceItem2.Location = new System.Drawing.Point(477, 288);
         this.emptySpaceItem2.Name = "emptySpaceItem2";
         this.emptySpaceItem2.Size = new System.Drawing.Size(100, 228);
         this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutItemSimulationExpressions
         // 
         this.layoutItemSimulationExpressions.Control = this.simulationExpressionsTree;
         this.layoutItemSimulationExpressions.Location = new System.Drawing.Point(577, 0);
         this.layoutItemSimulationExpressions.Name = "layoutItemSimulationExpressions";
         this.layoutItemSimulationExpressions.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
         this.layoutItemSimulationExpressions.Size = new System.Drawing.Size(479, 516);
         this.layoutItemSimulationExpressions.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSimulationExpressions.TextVisible = false;
         // 
         // EditIndividualAndExpressionConfigurationsView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.uxLayoutControl1);
         this.Name = "EditIndividualAndExpressionConfigurationsView";
         this.Size = new System.Drawing.Size(1100, 650);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.uxLayoutControl1)).EndInit();
         this.uxLayoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.simulationExpressionsTree)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbIndividualSelection.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupIndividual)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupExpression)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemProjectExpressions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAddButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemRemoveButton)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSimulationExpressions)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private OSPSuite.UI.Controls.UxLayoutControl uxLayoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup Root;
      private OSPSuite.UI.Controls.UxTreeView simulationExpressionsTree;
      private OSPSuite.UI.Controls.UxSimpleButton btnAdd;
      private OSPSuite.UI.Controls.UxSimpleButton btnRemove;
      private OSPSuite.UI.Controls.FilterTreeView projectExpressionsTree;
      private DevExpress.XtraEditors.ImageComboBoxEdit cbIndividualSelection;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupIndividual;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupExpression;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemProjectExpressions;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAddButton;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemRemoveButton;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSimulationExpressions;
   }
}
