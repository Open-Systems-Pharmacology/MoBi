using System;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using MoBi.Presentation.Views.BaseDiagram;
using MoBi.UI.Extensions;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class EditSpatialStructureView : EditBuildingBlockBaseView, IEditSpatialStructureView
   {
      private IEditSpatialStructurePresenter spatialStructurePresenter => _presenter.DowncastTo<IEditSpatialStructurePresenter>();

      public EditSpatialStructureView(IMainView mainView) : base(mainView)
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         tabsNavigation.SelectedPageChanging += (o, e) => OnEvent(tabSelectionChanging, e);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = AppConstants.Captions.SpatialStructure;
         ApplicationIcon = ApplicationIcons.SpatialStructure;
         tabDiagram.InitWith(AppConstants.Captions.ModelDiagram, ApplicationIcons.Diagram);
         tabTree.InitWith(AppConstants.Captions.Tree, ApplicationIcons.Tree);

         spliterDiagram.CollapsePanel = SplitCollapsePanel.Panel1;
         spliterDiagram.Horizontal = true;

         splitHierarchyEdit.CollapsePanel = SplitCollapsePanel.Panel1;
         splitHierarchyEdit.Horizontal = true;
         spliterDiagram.SplitterPosition = Convert.ToInt32(Height * AppConstants.Diagram.SplitterDiagramRatio);

         EditCaption = AppConstants.Captions.Parameters;
         EditIcon = ApplicationIcons.Parameter;
      }

      public void AttachPresenter(IEditSpatialStructurePresenter presenter)
      {
         _presenter = presenter;
      }

      private void tabSelectionChanging(TabPageChangingEventArgs e)
      {
         if (e.Page.Equals(tabDiagram))
         {
            spatialStructurePresenter.LoadDiagram();
         }
      }

      public void SetEditView(IView view)
      {
         if (view == null && splitHierarchyEdit.Panel2.Controls.Count > 0)
            splitHierarchyEdit.Panel2.Controls.Clear();
         
         splitHierarchyEdit.Panel2.FillWith(view);
      }

      public void SetHierarchicalStructureView(IView view)
      {
         tabTree.FillWith(view);
      }

      public void SetSpaceDiagramView(ISpatialStructureDiagramView view)
      {
         spliterDiagram.Panel2.FillWith(view);
         view.Overview = _diagramOverview;
      }
   }
}