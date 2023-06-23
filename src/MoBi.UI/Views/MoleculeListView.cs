using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Nodes;
using DevExpress.XtraBars;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Core;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class MoleculeListView : BaseUserControl, IMoleculeListView, IViewWithPopup
   {
      private IMoleculeListPresenter _presenter;
      private readonly UxTreeView _treeView;
      private readonly IMoleculeBuilderDTOToTreeNodeMapper _moleculeBuilderToTreeNodeMapper;


      public MoleculeListView(IMoleculeBuilderDTOToTreeNodeMapper moleculeBuilderToTreeNodeMapper,
         IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _treeView = new UxTreeView {StateImageList = imageListRetriever.AllImages16x16};
         barManager.Images = imageListRetriever.AllImages16x16;
         Controls.Add(_treeView);
         _treeView.Dock = DockStyle.Fill;
         _treeView.NodeClick += onNodeClick;
         _treeView.MouseClick += onClick;
         _treeView.SelectedNodeChanged += selectedNodeChanged;
         _moleculeBuilderToTreeNodeMapper = moleculeBuilderToTreeNodeMapper;
      }

      private void selectedNodeChanged(ITreeNode selectedNode)
      {
         if (selectedNode == null) return;
         OnEvent(() => _presenter.Select(selectedNode.TagAsObject.DowncastTo<ObjectBaseDTO>()));
      }

      private void onClick(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;

         //Show popup only if click was performed on empty space
         var hitInfo = _treeView.CalcHitInfo(e.Location);
         if (hitInfo.Node != null) return;

         _presenter.CreatePopupMenuFor(null).At(e.Location);
      }

      private void onNodeClick(MouseEventArgs e, ITreeNode node)
      {
         if (e.Button != MouseButtons.Right) return;
         _presenter.CreatePopupMenuFor(node.TagAsObject as IViewItem).At(e.Location);
      }

      public void AttachPresenter(IMoleculeListPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IEnumerable<MoleculeBuilderDTO> dtos)
      {
         var nodes = dtos.MapAllUsing(_moleculeBuilderToTreeNodeMapper);

         nodes.Each(node => _treeView.AddNode(node));
      }

      public void SelectItem(IObjectBase objectBase)
      {
         var node = _treeView.NodeById(objectBase.Id);
         if (node == null) return;
         _treeView.SelectNode(node);
      }

      public void AddNode(ITreeNode treeNode)
      {
         _treeView.AddNode(treeNode);
      }

      public void Clear()
      {
         _treeView.Clear();
      }

      public BarManager PopupBarManager
      {
         get { return barManager; }
      }
   }
}