using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraBars;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Extensions;

namespace MoBi.UI.Views
{
   public partial class HierarchicalStructureView : BaseUserControl, IHierarchicalStructureView, IViewWithPopup
   {
      private IHierarchicalStructurePresenter _presenter;
      private readonly IObjectBaseDTOToSpatialStructureNodeMapper _spatialStructureNodeMapper;

      public HierarchicalStructureView(IObjectBaseDTOToSpatialStructureNodeMapper spatialStructureNodeMapper, IImageListRetriever imageListRetriever)
      {
         _spatialStructureNodeMapper = spatialStructureNodeMapper;
         InitializeComponent();
         treeView.ShouldExpandAddedNode = false;
         treeView.UseLazyLoading = false;
         treeView.MouseClick += onMouseClicked;
         treeView.StateImageList = imageListRetriever.AllImages16x16;
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      private void onMouseClicked(object sender, MouseEventArgs mouseEventArgs)
      {
         var hitInfo = treeView.CalcHitInfo(mouseEventArgs.Location);
         if (mouseEventArgs.Button.Equals(MouseButtons.Right))
         {
            if (hitInfo.Node != null)
            {
               var treeNode = treeView.NodeFrom(hitInfo.Node);
               _presenter.CreatePopupMenuFor(treeNode.TagAsObject as ObjectBaseDTO).At(mouseEventArgs.Location);
            }
            else
            {
               _presenter.CreatePopupMenuFor(null).At(mouseEventArgs.Location);
            }
         }

         if (hitInfo.Node != null)
         {
            var treeNode = treeView.NodeFrom(hitInfo.Node);
            OnEvent(() => _presenter.Select(treeNode.TagAsObject as ObjectBaseDTO));
         }
      }

      public void Show(IEnumerable<ObjectBaseDTO> roots)
      {
         _spatialStructureNodeMapper.Initialize(dto => _presenter.GetChildObjects(dto, child => !child.IsAnImplementationOf<IParameter>()));
         roots.Each(AddRoot);
      }

      public void AttachPresenter(IHierarchicalStructurePresenter presenter)
      {
         _presenter = presenter;
      }

      public BarManager PopupBarManager => barManager;

      public void Add(ObjectBaseDTO newChild, ObjectBaseDTO parent)
      {
         var newNode = _spatialStructureNodeMapper.MapFrom(newChild);
         var parentNode = treeView.NodeById(parent.Id);
         //Check if parentNode is already displayed? else it's not necessary to add new Node
         if (parentNode == null) 
            return;

         parentNode.AddChild(newNode);
         treeView.AddNode(newNode);
      }

      public void Remove(IWithId withId)
      {
         var nodeById = treeView.NodeById(withId.Id);
         if (nodeById == null) return;
         treeView.DestroyNode(nodeById);
      }

      public void AddNode(ITreeNode newNode)
      {
         treeView.AddNode(newNode);
      }

      public void AddRoot(ObjectBaseDTO dto)
      {
         treeView.AddNode(_spatialStructureNodeMapper.MapFrom(dto));
      }

      public void Select(string id)
      {
         var node = treeView.NodeById(id);
         if (node == null) return;
         treeView.SelectNode(node);
      }

      public void Clear()
      {
         treeView.DestroyAllNodes();
      }
   }
}