using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectNeighborPathPresenter : IPresenter<ISelectNeighborPathView>
   {
      void Init(string label, string defaultSelection = null);
      ObjectPath NeighborPath { get; }
   }

   public class SelectNeighborPathPresenter : AbstractPresenter<ISelectNeighborPathView, ISelectNeighborPathPresenter>, ISelectNeighborPathPresenter
   {
      private readonly ISelectContainerInTreePresenter _selectContainerInTreePresenter;
      private readonly ISpatialStructureToSpatialStructureDTOMapper _spatialStructureToSpatialStructureDTOMapper;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly ObjectPathDTO _selectedPathDTO = new ObjectPathDTO();

      public SelectNeighborPathPresenter(
         ISelectNeighborPathView view,
         ISelectContainerInTreePresenter selectContainerInTreePresenter,
         ISpatialStructureToSpatialStructureDTOMapper spatialStructureToSpatialStructureDTOMapper,
         IBuildingBlockRepository buildingBlockRepository) : base(view)
      {
         _selectContainerInTreePresenter = selectContainerInTreePresenter;
         _spatialStructureToSpatialStructureDTOMapper = spatialStructureToSpatialStructureDTOMapper;
         _buildingBlockRepository = buildingBlockRepository;
         AddSubPresenters(_selectContainerInTreePresenter);
         _view.AddContainerCriteriaView(_selectContainerInTreePresenter.BaseView);
         _view.BindTo(_selectedPathDTO);
         _selectContainerInTreePresenter.OnSelectedEntityChanged += (o, e) => onSelectedContainerPathChanged(e.Entity, e.Path);
      }

      private void onSelectedContainerPathChanged(IEntity entity, ObjectPath containerObjectPath)
      {
         if (!(entity is IContainer container))
            return;

         //Only physical containers can be selected as neighbors
         if (container.Mode != ContainerMode.Physical) 
            return;

         var parentPath = container.RootContainer.ParentPath?.Clone<ObjectPath>() ?? new ObjectPath();
         containerObjectPath.Each(parentPath.Add);
         _selectedPathDTO.Path = parentPath.PathAsString;
         ViewChanged();
      }

      public void Init(string label, string defaultSelection = null)
      {
         _view.Label = label;
         
         var topContainers = _buildingBlockRepository.SpatialStructureCollection.SelectMany(mapTopContainerDTOs).ToList();
         
         //no organism found, nothing to do?
         if (!topContainers.Any())
            return;

         _selectContainerInTreePresenter.InitTreeStructure(topContainers);
         _selectedPathDTO.Path = defaultSelection ?? string.Empty;
      }

      private IReadOnlyList<ObjectBaseDTO> mapTopContainerDTOs(SpatialStructure spatialStructure)
      {
         return _spatialStructureToSpatialStructureDTOMapper.MapFrom(spatialStructure).TopContainers.ToList();
      }

      public ObjectPath NeighborPath => new ObjectPath(_selectedPathDTO.Path.ToPathArray());
   }
}