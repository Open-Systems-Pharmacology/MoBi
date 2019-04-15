using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MoBi.Assets;
using OSPSuite.UI;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace MoBi.UI.Views
{
   public partial class SelectEventAssignmentTargetView : BaseModalView, ISelectEventAssignmentTargetView
   {
      private readonly IObjectBaseDTOToSpatialStructureNodeMapper _spatialStructureNodeMapper;
      private readonly UxTreeView _treeView;
      private ISelectEventAssingmentTargetPresenter _presenter;

      public SelectEventAssignmentTargetView(IObjectBaseDTOToSpatialStructureNodeMapper spatialStructureNodeMapper, IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _treeView = new UxTreeView();
         Controls.Add(_treeView);
         initTreeView(imageListRetriever);
         _treeView.NodeClick += onNodeClick;
         _spatialStructureNodeMapper = spatialStructureNodeMapper;
         _spatialStructureNodeMapper.Initialize((objectBase) => _presenter.GetChildren(objectBase));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.SelectChangedEntity;
      }

      public void BindTo(IEnumerable<IObjectBaseDTO> dtos)
      {
         _treeView.Clear();
         foreach (var dto in dtos)
         {
            if (dto.IsAnImplementationOf<SpatialStructureDTO>())
            {
               _treeView.AddNode(getSpatialStructureNode((SpatialStructureDTO) dto));
            }
            else
            {
               if (dto.IsAnImplementationOf<BuildingBlockDTO>())
               {
                  _treeView.AddNode(getBuildingBlockNode((BuildingBlockDTO) dto));
               }
               else
               {
                  _treeView.AddNode(_spatialStructureNodeMapper.MapFrom(dto));
               }
            }
         }
         SetOkButtonEnable();
      }

      public IObjectBaseDTO Selected => _treeView.SelectedNode?.TagAsObject as IObjectBaseDTO;

      public void AttachPresenter(ISelectEventAssingmentTargetPresenter presenter)
      {
         _presenter = presenter;
      }

      public override bool HasError => !_presenter.IsValidSelection(Selected);

      private void onNodeClick(MouseEventArgs arg1, ITreeNode arg2)
      {
         SetOkButtonEnable();
      }

      private void initTreeView(IImageListRetriever imageListRetriever)
      {
         _treeView.ShouldExpandAddedNode = false;
         _treeView.UseLazyLoading = true;
         _treeView.StateImageList = imageListRetriever.AllImages16x16;
         panelControl1.Controls.Add(_treeView);
         _treeView.Dock = DockStyle.Fill;
      }

      private HierarchicalStructureNode getBuildingBlockNode(BuildingBlockDTO dto)
      {
         var buildingBlockNode = _spatialStructureNodeMapper.MapFrom(dto);
         foreach (var dtoBuilder in dto.Builder)
         {
            buildingBlockNode.AddChild(_spatialStructureNodeMapper.MapFrom(dtoBuilder));
         }
         return buildingBlockNode;
      }

      private HierarchicalStructureNode getSpatialStructureNode(SpatialStructureDTO dtoSpatialStructure)
      {
         var spatialStructureNode = _spatialStructureNodeMapper.MapFrom(dtoSpatialStructure);

         if (dtoSpatialStructure.MoleculeProperties != null)
            spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(dtoSpatialStructure.MoleculeProperties));

         if (dtoSpatialStructure.TopContainer != null && dtoSpatialStructure.TopContainer.Any())
            dtoSpatialStructure.TopContainer.Each(dto => spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(dto)));

         if (dtoSpatialStructure.Neighborhoods != null)
            spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(dtoSpatialStructure.Neighborhoods));

         return spatialStructureNode;
      }

      public ITreeNode GetNode(string id)
      {
         return _treeView.NodeById(id);
      }
   }
}