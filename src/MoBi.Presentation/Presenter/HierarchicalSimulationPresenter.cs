using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Formatters;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

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
      private readonly ISimulationSettingsToObjectBaseDTOMapper _simulationSettingsMapper;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly ICoreCalculationMethodRepository _calculationMethodRepository;
      public Action ShowSolverSettings { set; get; }
      public Action ShowOutputSchema { get; set; }

      public Func<IEnumerable<IParameter>> SimulationFavorites { get; set; } = () => new List<IParameter>();

      public IMoBiSimulation Simulation { get; private set; }

      public HierarchicalSimulationPresenter(IHierarchicalStructureView view, IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper,
         ISimulationSettingsToObjectBaseDTOMapper simulationSettingsMapper,
         ITreeNodeFactory treeNodeFactory,
         IViewItemContextMenuFactory contextMenuFactory,
         INeighborhoodToNeighborDTOMapper neighborhoodToNeighborDTOMapper,
         IEntityPathResolver entityPathResolver,
         ICoreCalculationMethodRepository calculationMethodRepository)
         : base(view, context, objectBaseMapper, treeNodeFactory, neighborhoodToNeighborDTOMapper)
      {
         _simulationSettingsMapper = simulationSettingsMapper;
         _contextMenuFactory = contextMenuFactory;
         _entityPathResolver = entityPathResolver;
         _calculationMethodRepository = calculationMethodRepository;
      }

      protected override void RaiseFavoritesSelectedEvent() => _context.PublishEvent(new FavoritesSelectedEvent(Simulation));

      protected override void RaiseUserDefinedSelectedEvent() => _context.PublishEvent(new UserDefinedSelectedEvent(Simulation));

      public override void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _contextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Edit(IMoBiSimulation simulation)
      {
         Simulation = simulation;
         _view.AddNode(_favoritesNode);
         _view.AddNode(_userDefinedNode);

         var roots = new List<ObjectBaseDTO> { _simulationSettingsMapper.MapFrom(simulation.Settings) };
         var objectBaseDTOs = rootContainers();
         addCalculationMethodDescriptions(simulation, moleculeRootContainerDTOs(objectBaseDTOs));
         roots.AddRange(objectBaseDTOs);
         _view.Show(roots);

         ShowOutputSchema();
      }

      private void addCalculationMethodDescriptions(IMoBiSimulation simulation, IReadOnlyList<ObjectBaseDTO> moleculeRootContainerDTOs) => 
         moleculeRootContainerDTOs.Each(x => addCalculationMethodDescription(simulation, x));

      private void addCalculationMethodDescription(IMoBiSimulation simulation, ObjectBaseDTO x)
      {
         var overrides = simulation.Configuration.CalculationMethodOverridesFor(x.Name);
         if (overrides.UsedCalculationMethods.Any())
            addCalculationMethodDescription(x, overrides, new UsedCalculationMethodCategoryFormatter());
      }

      private static IReadOnlyList<ObjectBaseDTO> moleculeRootContainerDTOs(IEnumerable<ObjectBaseDTO> objectBaseDTOs) => 
         objectBaseDTOs.Where(x => x.ObjectBase is Container { ContainerType: ContainerType.Molecule }).ToList();

      private void addCalculationMethodDescription(ObjectBaseDTO moleculeContainerDTO, MoleculeCalculationMethodOverride overriddenCalculationMethods, UsedCalculationMethodCategoryFormatter formatter) => 
         moleculeContainerDTO.Description = descriptionFor(overriddenCalculationMethods.UsedCalculationMethods.Where(shouldShow).ToList(), formatter);

      private string descriptionFor(IReadOnlyList<UsedCalculationMethod> usedMethods, UsedCalculationMethodCategoryFormatter formatter) => ToolTips.CalculationMethodDescription(usedMethods, formatter);

      private bool shouldShow(UsedCalculationMethod method)
      {
         return _calculationMethodRepository
            .GetAllCalculationMethodsFor(method.Category)
            .Count(x => !x.IsNamed(AppConstants.DefaultNames.EmptyCalculationMethod)) > 1;
      }

      private IReadOnlyList<ObjectBaseDTO> rootContainers() => GetChildrenSorted(Simulation.Model.Root, x => true);

      public object Subject => Simulation;

      public void Edit(object objectToEdit) => Edit(objectToEdit.DowncastTo<IMoBiSimulation>());

      public override IReadOnlyList<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto, Func<IEntity, bool> predicate)
      {
         if (string.Equals(dto.Id, AppConstants.SimulationSettingsId))
            return new[]
            {
               _simulationSettingsMapper.MapFrom(Simulation.OutputSchema),
               _simulationSettingsMapper.MapFrom(Simulation.Solver),
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
         Simulation = null;
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
         Simulation.Model.Root.Equals(entity.RootContainer) ||
         Simulation.Model.Neighborhoods.Equals(entity.RootContainer);

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
            x.Path.Remove(Simulation.Name);
            x.Name = x.Path;
         });
         return neighbors;
      }

      protected override IEntity GetEntityForNeighbor(NeighborDTO neighborDTO) => neighborDTO.Path.Resolve<IEntity>(Simulation.Model.Root);

      public void CopyCurrentPathToClipBoard(IEntity entity) => _view.CopyToClipBoard(_entityPathResolver.PathFor(entity));
   }
}