﻿using MoBi.Presentation.Views;

namespace MoBi.UI.Views
{
   partial class HierarchicalStructureView 
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
         _presenter = null;
         Clear();
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
         this.barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.treeView = new OSPSuite.UI.Controls.UxTreeView();
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).BeginInit();
         this.SuspendLayout();
         // 
         // errorProvider
         // 
         // 
         // barManager1
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
         this.barDockControlTop.Size = new System.Drawing.Size(150, 0);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 682);
         this.barDockControlBottom.Size = new System.Drawing.Size(150, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 682);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(150, 0);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 682);
         // 
         // treeView
         // 
         this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView.IsLatched = false;
         this.treeView.Location = new System.Drawing.Point(0, 0);
         this.treeView.Name = "treeView";
         this.treeView.OptionsBehavior.Editable = false;
         this.treeView.OptionsView.ShowColumns = false;
         this.treeView.OptionsView.ShowHorzLines = false;
         this.treeView.OptionsView.ShowIndicator = false;
         this.treeView.OptionsView.ShowVertLines = false;
         this.treeView.Size = new System.Drawing.Size(150, 682);
         this.treeView.TabIndex = 4;
         // 
         // HirarchicalStructureView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.treeView);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Name = "HierarchicalStructureView";
         this.Size = new System.Drawing.Size(150, 682);
         ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.treeView)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.BarDockControl barDockControlLeft;
      private DevExpress.XtraBars.BarDockControl barDockControlRight;
      private DevExpress.XtraBars.BarDockControl barDockControlBottom;
      private DevExpress.XtraBars.BarDockControl barDockControlTop;
      private DevExpress.XtraBars.BarManager barManager;
      private OSPSuite.UI.Controls.UxTreeView treeView;
   }
}
