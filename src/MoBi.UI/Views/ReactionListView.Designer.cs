namespace MoBi.UI.Views
{
   partial class ReactionListView
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
         _gridViewBinder.Dispose();
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
         this.grdReactionList = new OSPSuite.UI.Controls.UxGridControl();
         this.grdViewReactionList = new MoBi.UI.Views.UxGridView();
         this.barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         ((System.ComponentModel.ISupportInitialize)(this.grdReactionList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdViewReactionList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         this.SuspendLayout();
         // 
         // grdReactionList
         // 
         this.grdReactionList.Dock = System.Windows.Forms.DockStyle.Fill;
         this.grdReactionList.Location = new System.Drawing.Point(0, 0);
         this.grdReactionList.MainView = this.grdViewReactionList;
         this.grdReactionList.Name = "grdReactionList";
         this.grdReactionList.Size = new System.Drawing.Size(436, 357);
         this.grdReactionList.TabIndex = 0;
         this.grdReactionList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewReactionList});
         this.grdReactionList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.reactionListMouseClick);
         // 
         // grdViewReactionList
         // 
         this.grdViewReactionList.GridControl = this.grdReactionList;
         this.grdViewReactionList.Name = "grdViewReactionList";
         // 
         // barManager
         // 
         this.barManager.Form = this;
         this.barManager.MaxItemId = 0;
         // 
         // ReactionListView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.grdReactionList);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "ReactionListView";
         this.Size = new System.Drawing.Size(436, 357);
         ((System.ComponentModel.ISupportInitialize)(this.grdReactionList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.grdViewReactionList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraGrid.GridControl grdReactionList;
      private MoBi.UI.Views.UxGridView grdViewReactionList;
      private DevExpress.XtraBars.BarManager barManager;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
   }
}
