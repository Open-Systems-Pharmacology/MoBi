using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.UI;
using OSPSuite.Assets;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class SelectLocalisationView : UxImageTreeView, ISelectLocalisationView
   {
      private readonly IObjectBaseDTOToReferenceNodeMapper _nodeMapper;
      private ISelectLocalisationPresenter _presenter;

      public SelectLocalisationView(IObjectBaseDTOToReferenceNodeMapper nodeMapper, IImageListRetriever imageListRetriever)
      {
         _nodeMapper = nodeMapper;
         InitializeComponent();
         StateImageList = imageListRetriever.AllImages16x16;
         NodeClick += (args, node) => _presenter.ViewChanged();
         ShouldExpandAddedNode = false;
         UseLazyLoading = true;
      }

      public void Show(IEnumerable<SpatialStructureDTO> dtoSpatialStructures)
      {
         _nodeMapper.Initialize(x => _presenter.GetChildObjects(x.Id));
         foreach (var dtoSpatialStructure in dtoSpatialStructures)
         {
            var spatialStructureNode = getSpatialStructureNode(dtoSpatialStructure);
            AddNode(spatialStructureNode);
         }
      }

      public ObjectBaseDTO Selected
      {
         get { return SelectedNode.TagAsObject as ObjectBaseDTO; }
      }

      public void Show(List<ObjectBaseDTO> dtos)
      {
         foreach (var dto in dtos)
         {
            if (dto.IsAnImplementationOf<SpatialStructureDTO>())
            {
               AddNode(getSpatialStructureNode((SpatialStructureDTO) dto));
            }
            else
            {
               if (dto.IsAnImplementationOf<BuildingBlockDTO>())
               {
                  AddNode(getBuildingBlockNode((BuildingBlockDTO) dto));
               }
            }
         }
      }

      private ITreeNode getBuildingBlockNode(BuildingBlockDTO dto)
      {
         var buildingBlockNode = _nodeMapper.MapFrom(dto);
         foreach (var dtoBuilder in dto.Builder)
         {
            buildingBlockNode.AddChild(_nodeMapper.MapFrom(dtoBuilder));
         }
         return buildingBlockNode;
      }

      private ITreeNode getSpatialStructureNode(SpatialStructureDTO dtoSpatialStructure)
      {
         var spatialStructureNode = _nodeMapper.MapFrom(dtoSpatialStructure);

         if (dtoSpatialStructure.MoleculeProperties != null)
         {
            spatialStructureNode.AddChild(_nodeMapper.MapFrom(dtoSpatialStructure.MoleculeProperties));
         }
         if (dtoSpatialStructure.TopContainer != null && dtoSpatialStructure.TopContainer.Any())
         {
            dtoSpatialStructure.TopContainer.Each(dto => spatialStructureNode.AddChild(_nodeMapper.MapFrom(dto)));
         }
         if (dtoSpatialStructure.Neighborhoods != null)
         {
            spatialStructureNode.AddChild(_nodeMapper.MapFrom(dtoSpatialStructure.Neighborhoods));
         }
         return spatialStructureNode;
      }

      public void AttachPresenter(ISelectLocalisationPresenter presenter)
      {
         _presenter = presenter;
      }

      public void InitializeBinding()
      {
         /*nothing to do*/
      }

      public void InitializeResources()
      {
         /*nothing to do*/
      }

      public void AttachPresenter(IPresenter presenter)
      {
         /*nothing to do*/
      }

      public ApplicationIcon ApplicationIcon { get; set; }

      public bool HasError
      {
         get
         {
            return !_presenter.SelectionIsValid(Selected);
         }
      }

      public event EventHandler CaptionChanged = delegate { };
   }
}