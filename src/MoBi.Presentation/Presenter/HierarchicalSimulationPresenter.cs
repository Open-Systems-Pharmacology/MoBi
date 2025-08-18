using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MoBi.Presentation.Presenter
{
   public interface IHierarchicalSimulationPresenter :
      IHierarchicalStructurePresenter,
      IEditPresenter<IMoBiSimulation>,
      IListener<EntitySelectedEvent>
   {
      Action ShowSolverSettings { set; get; }
      Action ShowOutputSchema { get; set; }
      Func<IEnumerable<IParameter>> SimulationFavorites { get; set; }
      IMoBiSimulation Simulation { get; }
      void CopyCurrentPathToClipBoard(IEntity entity);

   }

   internal class HierarchicalSimulationPresenter : HierarchicalStructureEditPresenter, IHierarchicalSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private readonly ISimulationSettingsToObjectBaseDTOMapper _simulationSettingsMapper;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;
      private readonly IEntityPathResolver _entityPathResolver;
      public Action ShowSolverSettings { set; get; }
      public Action ShowOutputSchema { get; set; }

      public Func<IEnumerable<IParameter>> SimulationFavorites { get; set; } = () => new List<IParameter>();

      public IMoBiSimulation Simulation => _simulation;

      public HierarchicalSimulationPresenter(IHierarchicalStructureView view, IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper,
         ISimulationSettingsToObjectBaseDTOMapper simulationSettingsMapper, ITreeNodeFactory treeNodeFactory, IViewItemContextMenuFactory contextMenuFactory, INeighborhoodToNeighborDTOMapper neighborhoodToNeighborDTOMapper, IEntityPathResolver entityPathResolver)
         : base(view, context, objectBaseMapper, treeNodeFactory, neighborhoodToNeighborDTOMapper)
      {
         _simulationSettingsMapper = simulationSettingsMapper;
         _contextMenuFactory = contextMenuFactory;
         _entityPathResolver = entityPathResolver;
      }

      protected override void RaiseFavoritesSelectedEvent() => _context.PublishEvent(new FavoritesSelectedEvent(_simulation));

      protected override void RaiseUserDefinedSelectedEvent() => _context.PublishEvent(new UserDefinedSelectedEvent(_simulation));

      public override void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _contextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _view.AddNode(_favoritesNode);
         _view.AddNode(_userDefinedNode);

         var roots = new List<ObjectBaseDTO> { _simulationSettingsMapper.MapFrom(simulation.Settings) };
         roots.AddRange(rootContainers());
         _view.Show(roots);

         ShowOutputSchema();
      }

      private IEnumerable<ObjectBaseDTO> rootContainers() => GetChildrenSorted(_simulation.Model.Root, x => true);

      public object Subject => _simulation;

      public void Edit(object objectToEdit) => Edit(objectToEdit.DowncastTo<IMoBiSimulation>());

      public override IReadOnlyList<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto, Func<IEntity, bool> predicate)
      {
         if (string.Equals(dto.Id, AppConstants.SimulationSettingsId))
            return new[]
            {
               _simulationSettingsMapper.MapFrom(_simulation.OutputSchema),
               _simulationSettingsMapper.MapFrom(_simulation.Solver),
            };

         if (dto.Id.IsOneOf(AppConstants.OutputIntervalId, AppConstants.SolverSettingsId))
            return new List<ObjectBaseDTO>();

         return base.GetChildObjects(dto, predicate);
      }

      public override void Select(ObjectBaseDTO objectBaseDTO)
      {
         if (string.Equals(objectBaseDTO.Id, AppConstants.OutputIntervalId))
            ShowOutputSchema();
         else if (string.Equals(objectBaseDTO.Id, AppConstants.SolverSettingsId))
            ShowSolverSettings();
         else if (!string.Equals(objectBaseDTO.Id, AppConstants.SimulationSettingsId))
            base.Select(objectBaseDTO);
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         _simulation = null;
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         if (eventToHandle.Sender == this)
            return;

         var entity = eventToHandle.ObjectBase as IEntity;
         if (entity == null)
            return;

         if (!selectedEntityIsInSimulation(entity))
            return;

         var entityToSelect = entity.IsAnImplementationOf<IParameter>() ? entity.ParentContainer : entity;
         _view.Select(entityToSelect);
      }

      private bool selectedEntityIsInSimulation(IEntity entity) =>
         _simulation.Model.Root.Equals(entity.RootContainer) ||
         _simulation.Model.Neighborhoods.Equals(entity.RootContainer);

      protected override IReadOnlyList<ObjectBaseDTO> GetChildrenSorted(IContainer container, Func<IEntity, bool> predicate)
      {
         if (container is Neighborhood neighborhood)
         {
            return neighborsOf(neighborhood).Union(base.GetChildrenSorted(container, predicate)).ToList();
         }

         return base.GetChildrenSorted(container, predicate);
      }

      private IEnumerable<ObjectBaseDTO> neighborsOf(Neighborhood neighborhood)
      {
         var neighbors = _neighborhoodToNeighborDTOMapper.MapFrom(neighborhood).ToList();
         neighbors.Each(x =>
         {
            x.Path.Remove(_simulation.Name);
            x.Name = x.Path;
         });
         return neighbors;
      }

      protected override IEntity GetEntityForNeighbor(NeighborDTO neighborDTO) => neighborDTO.Path.Resolve<IEntity>(_simulation.Model.Root);

      public void CopyCurrentPathToClipBoard(IEntity entity) => _view.CopyToClipBoard(_entityPathResolver.PathFor(entity));
   }
}