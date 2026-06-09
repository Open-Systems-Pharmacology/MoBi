using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public class SelectedEntityChangedArgs : EventArgs
   {
      public IEntity Entity { get; }

      public SelectedEntityChangedArgs(IEntity entity)
      {
         Entity = entity;
      }
   }

   public interface ISelectEntityInTreePresenter : IPresenter<ISelectEntityInTreeView>
   {
      Func<ObjectBaseDTO, IReadOnlyList<ObjectBaseDTO>> GetChildren { get; set; }
      bool IsValidSelection(ObjectBaseDTO selectedDTO);
      void InitTreeStructure(IReadOnlyList<ObjectBaseDTO> entityDTOs);
      IEntity SelectedEntity { get; }
      ObjectBaseDTO SelectedDTO { get; }
      ITreeNode TreeNodeFor(ObjectBaseDTO dto);
      ObjectPath SelectedEntityPath { get; }
      void SelectObjectBaseDTO(ObjectBaseDTO dto);
      event EventHandler<SelectedEntityChangedArgs> OnSelectedEntityChanged;
      void SelectById(string id);
   }

   public class SelectEntityInTreePresenter : AbstractPresenter<ISelectEntityInTreeView, ISelectEntityInTreePresenter>, ISelectEntityInTreePresenter
   {
      private readonly IObjectPathFactory _objectPathFactory;
      public event EventHandler<SelectedEntityChangedArgs> OnSelectedEntityChanged = delegate { };
      private readonly IObjectBaseDTOToReferenceNodeMapper _referenceNodeMapper;

      public SelectEntityInTreePresenter(ISelectEntityInTreeView view, IObjectPathFactory objectPathFactory, IObjectBaseDTOToReferenceNodeMapper referenceNodeMapper) : base(view)
      {
         _objectPathFactory = objectPathFactory;
         _referenceNodeMapper = referenceNodeMapper;
         _referenceNodeMapper.Initialize(objectBase => GetChildren(objectBase));
      }

      protected IEntity EntityFrom(ObjectBaseDTO dto) => dto.ObjectBase as IEntity;

      public virtual ObjectPath SelectedEntityPath => SelectedEntity != null ? _objectPathFactory.CreateAbsoluteObjectPath(SelectedEntity) : null;

      public virtual void SelectObjectBaseDTO(ObjectBaseDTO dto)
      {
         var entity = EntityFrom(dto);

         OnSelectedEntityChanged(this, new SelectedEntityChangedArgs(entity));
      }

      public void SelectById(string id) => _view.SelectNodeById(id);

      public Func<ObjectBaseDTO, IReadOnlyList<ObjectBaseDTO>> GetChildren { get; set; }

      public bool IsValidSelection(ObjectBaseDTO selectedDTO)
      {
         return selectedDTO != null;
      }

      public virtual void InitTreeStructure(IReadOnlyList<ObjectBaseDTO> entityDTOs)
      {
         _view.Display(entityDTOs.Select(mapToNode).ToList());
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
               return _referenceNodeMapper.MapFrom(dto);
         }
      }

      private ITreeNode getBuildingBlockNode(BuildingBlockDTO buildingBlockDTO)
      {
         var buildingBlockNode = _referenceNodeMapper.MapFrom(buildingBlockDTO);

         buildingBlockDTO.Builder.MapAllUsing<ObjectBaseDTO, ITreeNode>(_referenceNodeMapper).Each(buildingBlockNode.AddChild);
         return buildingBlockNode;
      }

      private ITreeNode getSpatialStructureNode(SpatialStructureDTO spatialStructureDTO)
      {
         var spatialStructureNode = _referenceNodeMapper.MapFrom(spatialStructureDTO);

         if (spatialStructureDTO.MoleculeProperties != null)
            spatialStructureNode.AddChild(_referenceNodeMapper.MapFrom(spatialStructureDTO.MoleculeProperties));

         if (spatialStructureDTO.TopContainers != null && spatialStructureDTO.TopContainers.Any())
            spatialStructureDTO.TopContainers.Each(dto => spatialStructureNode.AddChild(_referenceNodeMapper.MapFrom(dto)));

         if (spatialStructureDTO.Neighborhoods != null)
            spatialStructureNode.AddChild(_referenceNodeMapper.MapFrom(spatialStructureDTO.Neighborhoods));

         return spatialStructureNode;
      }

      public IEntity SelectedEntity => EntityFrom(_view.Selected);

      public ObjectBaseDTO SelectedDTO => _view.Selected;

      public ITreeNode TreeNodeFor(ObjectBaseDTO dto) => _view.GetNode(dto.Id);
   }
}