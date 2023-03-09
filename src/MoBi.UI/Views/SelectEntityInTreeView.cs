using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
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
      private readonly IObjectBaseDTOToSpatialStructureNodeMapper _spatialStructureNodeMapper;
      private readonly UxTreeView _treeView;
      private ISelectEntityInTreePresenter _presenter;
      public event EventHandler<ITreeNode> OnNodeSelected = delegate { };

      public SelectEntityInTreeView(IObjectBaseDTOToSpatialStructureNodeMapper spatialStructureNodeMapper, IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _treeView = new UxTreeView();
         Controls.Add(_treeView);
         initTreeView(imageListRetriever);
         _treeView.NodeClick += onNodeClick;
         _spatialStructureNodeMapper = spatialStructureNodeMapper;
         _spatialStructureNodeMapper.Initialize(objectBase => _presenter.GetChildren(objectBase));
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

      public void BindTo(IEnumerable<ObjectBaseDTO> allDTOs)
      {
         _treeView.Clear();
         allDTOs.Each(x => _treeView.AddNode(mapToNode(x)));
      }

      private ITreeNode mapToNode(ObjectBaseDTO dto)
      {
         switch (dto)
         {
            case SpatialStructureDTO spatialStructureDTO:
               return getSpatialStructureNode(spatialStructureDTO);
            case BuildingBlockDTO buildingBlockDTO:
               return getBuildingBlockNode(buildingBlockDTO);
            default:
               return _spatialStructureNodeMapper.MapFrom(dto);
         }
      }

      public ObjectBaseDTO Selected => _treeView.SelectedNode?.TagAsObject as ObjectBaseDTO;

      public override bool HasError => !_presenter.IsValidSelection(Selected);

      private void onNodeClick(MouseEventArgs e, ITreeNode node) => OnNodeSelected(this, node);

      private void initTreeView(IImageListRetriever imageListRetriever)
      {
         _treeView.ShouldExpandAddedNode = false;
         _treeView.UseLazyLoading = true;
         _treeView.StateImageList = imageListRetriever.AllImages16x16;
         this.FillWith(_treeView);
      }

      private HierarchicalStructureNode getBuildingBlockNode(BuildingBlockDTO buildingBlockDTO)
      {
         var buildingBlockNode = _spatialStructureNodeMapper.MapFrom(buildingBlockDTO);
         buildingBlockDTO.Builder.MapAllUsing(_spatialStructureNodeMapper).Each(buildingBlockNode.AddChild);
         return buildingBlockNode;
      }

      private HierarchicalStructureNode getSpatialStructureNode(SpatialStructureDTO spatialStructureDTO)
      {
         var spatialStructureNode = _spatialStructureNodeMapper.MapFrom(spatialStructureDTO);

         if (spatialStructureDTO.MoleculeProperties != null)
            spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(spatialStructureDTO.MoleculeProperties));

         if (spatialStructureDTO.TopContainer != null && spatialStructureDTO.TopContainer.Any())
            spatialStructureDTO.TopContainer.Each(dto => spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(dto)));

         if (spatialStructureDTO.Neighborhoods != null)
            spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(spatialStructureDTO.Neighborhoods));

         return spatialStructureNode;
      }

      public ITreeNode GetNode(string id) => _treeView.NodeById(id);
   }
}