using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class EventGroupsListView : BaseUserControl, IEventGroupsListView, IViewWithPopup
   {
      private readonly UxTreeView _treeView;
      private readonly IDTOEventGroupBuilderToEventNodeMapper _dtoEventGroupToEventNodeMapper;
      private IEventGroupListPresenter _presenter;
      private readonly List<string> _fixedNodeIds = new List<string>();

      public EventGroupsListView(IDTOEventGroupBuilderToEventNodeMapper dtoEventGroupToEventNodeMapper,
         IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _treeView = new UxTreeView
         {
            StateImageList = imageListRetriever.AllImages16x16,
            UseLazyLoading = true,
            ShouldExpandAddedNode = false
         };
         Controls.Add(_treeView);
         _treeView.Dock = DockStyle.Fill;
         _treeView.NodeClick += onNodeClick;
         _treeView.MouseClick += onMouseClick;
         _dtoEventGroupToEventNodeMapper = dtoEventGroupToEventNodeMapper;
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      private void onMouseClick(object sender, MouseEventArgs e)
      {
         this.DoWithinExceptionHandler(() =>
         {
            if (e.Button.Equals(MouseButtons.Right))
            {
               var hitInfo = _treeView.CalcHitInfo(e.Location);
               if (hitInfo.Node == null)
               {
                  _presenter.CreatePopupMenuFor(null).At(e.Location);
               }
            }
         });
      }

      private void onNodeClick(MouseEventArgs e, ITreeNode node)
      {
         this.DoWithinExceptionHandler(() =>
         {
            if (e.Button.Equals(MouseButtons.Right))
            {
               _presenter.CreatePopupMenuFor((IViewItem) node.TagAsObject).At(e.Location);
            }
            _presenter.Select(objectBaseFromNode(node));
         });
      }

      private static ObjectBaseDTO objectBaseFromNode(ITreeNode node)
      {
         return node.TagAsObject.DowncastTo<ObjectBaseDTO>();
      }

      public void AttachPresenter(IEventGroupListPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IReadOnlyList<EventGroupBuilderDTO> dtoEventGroupBuilders)
      {
         var nodesToAdd = dtoEventGroupBuilders.MapAllUsing(_dtoEventGroupToEventNodeMapper);
         nodesToAdd.Each(_treeView.AddNode);

         removeUnusedNodes(nodesToAdd);

         _presenter.Select(objectBaseFromNode(_treeView.SelectedNode));
      }

      private void removeUnusedNodes(IReadOnlyList<ITreeNode> nodesAdded)
      {
         var allNodeIds = nodesAdded.SelectMany(x => x.AllNodes).Select(x => x.Id).Concat(_fixedNodeIds);
         _treeView.Nodes.SelectMany(x => _treeView.NodeFrom(x).AllNodes).Where(existingNode => !allNodeIds.Contains(existingNode.Id)).ToList().Each(_treeView.RemoveNode);
      }

      public void AddFixedNode(ITreeNode treeNode)
      {
         _fixedNodeIds.Add(treeNode.Id);
         _treeView.AddNode(treeNode);
      }

      public BarManager PopupBarManager => barManager;
   }
}