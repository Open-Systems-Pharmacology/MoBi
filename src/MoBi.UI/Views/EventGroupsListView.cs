using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.UI;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
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

      protected override int TopicId => HelpId.MoBi_ModelBuilding_Events;

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
            _presenter.Select(node.TagAsObject.DowncastTo<IObjectBaseDTO>());
         });
      }

      public void AttachPresenter(IEventGroupListPresenter presenter)
      {
         _presenter = presenter;
      }

      public void Show(IEnumerable<EventGroupBuilderDTO> dtoEventGroupBuilders)
      {
         var nodes = dtoEventGroupBuilders.MapAllUsing(_dtoEventGroupToEventNodeMapper);
         nodes.Each(node => _treeView.AddNode(node));
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