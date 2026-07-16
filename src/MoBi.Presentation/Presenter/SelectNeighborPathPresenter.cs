using System.Linq;
using MoBi.Core.Domain.Repository;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface ISelectNeighborPathPresenter : IPresenter<ISelectNeighborPathView>
   {
      void Init(string label, NeighborhoodObjectPathDTO selectionDTO);
      ObjectPath NeighborPath { get; }
      void RefreshView();
   }

   public class SelectNeighborPathPresenter : AbstractPresenter<ISelectNeighborPathView, ISelectNeighborPathPresenter>, ISelectNeighborPathPresenter
   {
      private readonly ISelectContainerInTreePresenter _selectContainerInTreePresenter;
      private readonly IModuleToModuleAndSpatialStructureDTOMapper _moduleToModuleDTOMapper;
      private readonly IBuildingBlockRepository _buildingBlockRepository;
      private readonly IObjectPathFactory _objectPathFactory;
      private NeighborhoodObjectPathDTO _selectedPathDTO;

      public SelectNeighborPathPresenter(
         ISelectNeighborPathView view,
         ISelectContainerInTreePresenter selectContainerInTreePresenter,
         IModuleToModuleAndSpatialStructureDTOMapper moduleToModuleDTOMapper,
         IBuildingBlockRepository buildingBlockRepository,
         IObjectPathFactory objectPathFactory) : base(view)
      {
         _selectContainerInTreePresenter = selectContainerInTreePresenter;
         _moduleToModuleDTOMapper = moduleToModuleDTOMapper;
         _buildingBlockRepository = buildingBlockRepository;
         _objectPathFactory = objectPathFactory;
         AddSubPresenters(_selectContainerInTreePresenter);
         _view.AddContainerCriteriaView(_selectContainerInTreePresenter.BaseView);
         _selectContainerInTreePresenter.OnSelectedEntityChanged += (o, e) => onSelectedContainerPathChanged(e.Entity);
      }

      private void onSelectedContainerPathChanged(IEntity entity)
      {
         if (entity is IContainer container)
         {
            _selectedPathDTO.Mode = container.Mode;
            _selectedPathDTO.Path = _objectPathFactory.CreateAbsoluteObjectPath(container).PathAsString;
         }

         ViewChanged();
      }

      public void Init(string label, NeighborhoodObjectPathDTO selectionDTO)
      {
         _view.Label = label;

         var modules = _buildingBlockRepository.SpatialStructureCollection.Select(x => mapModuleDTO(x.Module)).ToList();

         //no organism found, nothing to do?
         if (!modules.Any())
            return;

         _selectContainerInTreePresenter.InitTreeStructure(modules);
         _selectedPathDTO = selectionDTO;
         _view.BindTo(_selectedPathDTO);
         ViewChanged();
      }

      private ObjectBaseDTO mapModuleDTO(Module module)
      {
         return _moduleToModuleDTOMapper.MapFrom(module);
      }

      public ObjectPath NeighborPath => new ObjectPath(_selectedPathDTO.Path.ToPathArray());


      public void RefreshView() => _view.ValidateNeighborhood();

      public override bool CanClose => !_view.HasError;
   }
}