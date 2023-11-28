using System.Collections.Generic;
using System.Windows.Forms;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class SelectEntityInTreeView : BaseUserControl, ISelectEntityInTreeView
   {
      private readonly UxTreeView _treeView;
      private ISelectEntityInTreePresenter _presenter;

      public SelectEntityInTreeView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _treeView = new UxTreeView();
         Controls.Add(_treeView);
         initTreeView(imageListRetriever);
         _treeView.NodeClick += onNodeClick;
      }

      public void AttachPresenter(ISelectEntityInTreePresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.SelectChangedEntity;
      }

      public void Display(IReadOnlyList<ITreeNode> treeNodes)
      {
         _treeView.Clear();
         treeNodes.Each(x => _treeView.AddNode(x));
      }

      public ObjectBaseDTO Selected => _treeView.SelectedNode?.TagAsObject as ObjectBaseDTO;

      public override bool HasError => !_presenter.IsValidSelection(Selected);

      private void onNodeClick(MouseEventArgs e, ITreeNode node) => _presenter.SelectObjectBaseDTO(Selected);

      private void initTreeView(IImageListRetriever imageListRetriever)
      {
         _treeView.ShouldExpandAddedNode = false;
         _treeView.UseLazyLoading = true;
         _treeView.StateImageList = imageListRetriever.AllImages16x16;
         this.FillWith(_treeView);
      }

      public ITreeNode GetNode(string id) => _treeView.NodeById(id);
   }
}