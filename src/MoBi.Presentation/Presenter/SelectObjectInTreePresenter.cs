using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
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
      public ObjectPath Path { get; }

      public SelectedEntityChangedArgs(IEntity entity, ObjectPath path)
      {
         Entity = entity;
         Path = path;
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
      private readonly IMoBiContext _context;
      public event EventHandler<SelectedEntityChangedArgs> OnSelectedEntityChanged = delegate { };
      private readonly IObjectBaseDTOToSpatialStructureNodeMapper _spatialStructureNodeMapper;

      public SelectEntityInTreePresenter(ISelectEntityInTreeView view, IObjectPathFactory objectPathFactory, IMoBiContext context, IObjectBaseDTOToSpatialStructureNodeMapper spatialStructureNodeMapper) : base(view)
      {
         _objectPathFactory = objectPathFactory;
         _context = context;
         _spatialStructureNodeMapper = spatialStructureNodeMapper;
         _spatialStructureNodeMapper.Initialize(objectBase => GetChildren(objectBase));
      }

      protected IEntity EntityFrom(ObjectBaseDTO dto) => _context.Get<IEntity>(dto.Id);

      public ObjectPath SelectedEntityPath => SelectedEntity != null ? _objectPathFactory.CreateAbsoluteObjectPath(SelectedEntity) : null;

      public virtual void SelectObjectBaseDTO(ObjectBaseDTO dto)
      {
         var entity = EntityFrom(dto);

         // if the selected node is not an entity, then use null as the selected entity to indicate
         // that no entity is selected. This happens when a module node is clicked for example.
         OnSelectedEntityChanged(this, new SelectedEntityChangedArgs(entity, entity == null ? null : _objectPathFactory.CreateAbsoluteObjectPath(entity)));
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
               return _spatialStructureNodeMapper.MapFrom(dto);
         }
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

         if (spatialStructureDTO.TopContainers != null && spatialStructureDTO.TopContainers.Any())
            spatialStructureDTO.TopContainers.Each(dto => spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(dto)));

         if (spatialStructureDTO.Neighborhoods != null)
            spatialStructureNode.AddChild(_spatialStructureNodeMapper.MapFrom(spatialStructureDTO.Neighborhoods));

         return spatialStructureNode;
      }

      public IEntity SelectedEntity => EntityFrom(_view.Selected);

      public ObjectBaseDTO SelectedDTO => _view.Selected;

      public ITreeNode TreeNodeFor(ObjectBaseDTO dto) => _view.GetNode(dto.Id);
   }
}