using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.Presenter
{
   public interface IHierarchicalSimulationPresenter : IHierarchicalStructurePresenter, IEditPresenter<IMoBiSimulation>,
      IListener<EntitySelectedEvent>
   {
      Action ShowSolverSettings { set; get; }
      Action ShowOutputSchema { get; set; }
      Func<IEnumerable<IParameter>> SimulationFavorites { get; set; }
   }

   internal class HierarchicalSimulationPresenter : HierarchicalStructurePresenter, IHierarchicalSimulationPresenter
   {
      private IMoBiSimulation _simulation;
      private readonly ISimulationSettingsToObjectBaseDTOMapper _simulationSettingsMapper;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;
      public Action ShowSolverSettings { set; get; }
      public Action ShowOutputSchema { get; set; }

      public Func<IEnumerable<IParameter>> SimulationFavorites { get; set; } = () => new List<IParameter>();

      public HierarchicalSimulationPresenter(IHierarchicalStructureView view, IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper,
         ISimulationSettingsToObjectBaseDTOMapper simulationSettingsMapper, ITreeNodeFactory treeNodeFactory, IViewItemContextMenuFactory contextMenuFactory)
         : base(view, context, objectBaseMapper, treeNodeFactory)
      {
         _simulationSettingsMapper = simulationSettingsMapper;
         _contextMenuFactory = contextMenuFactory;
      }

      protected override void RaiseFavoritesSelectedEvent()
      {
         _context.PublishEvent(new FavoritesSelectedEvent(_simulation));
      }

      protected override void RaiseUserDefinedSelectedEvent()
      {
         _context.PublishEvent(new UserDefinedSelectedEvent(_simulation));
      }

      public override void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         if (!SimulationFavorites().Any())
            return;

         var contextMenu = _contextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Edit(IMoBiSimulation simulation)
      {
         _simulation = simulation;
         _view.AddNode(_favoritesNode);
         _view.AddNode(_userDefinedNode);

         var roots = new List<IObjectBaseDTO> {_simulationSettingsMapper.MapFrom(simulation.Settings)};
         roots.AddRange(rootContainers());
         _view.Show(roots);

         ShowOutputSchema();
      }

      private IEnumerable<IObjectBaseDTO> rootContainers()
      {
         return GetChildrenSorted(_simulation.Model.Root, x => true);
      }

      public object Subject => _simulation;

      public void Edit(object objectToEdit)
      {
         Edit(objectToEdit.DowncastTo<IMoBiSimulation>());
      }

      public override IReadOnlyList<IObjectBaseDTO> GetChildObjects(IObjectBaseDTO dto, Func<IEntity, bool> predicate)
      {
         if (string.Equals(dto.Id, AppConstants.SimulationSettingsId))
            return new[]
            {
               _simulationSettingsMapper.MapFrom(_simulation.OutputSchema),
               _simulationSettingsMapper.MapFrom(_simulation.Solver),
            };

         if (dto.Id.IsOneOf(AppConstants.OutputIntervalId, AppConstants.SolverSettingsId))
            return new List<IObjectBaseDTO>();

         return base.GetChildObjects(dto, predicate);
      }

      public override void Select(IObjectBaseDTO objectBaseDTO)
      {
         if (string.Equals(objectBaseDTO.Id, AppConstants.OutputIntervalId))
            ShowOutputSchema();
         else if (string.Equals(objectBaseDTO.Id, AppConstants.SolverSettingsId))
            ShowSolverSettings();
         else if (string.Equals(objectBaseDTO.Id, AppConstants.SimulationSettingsId))
            return;
         else
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
         _view.Select(entityToSelect.Id);
      }

      private bool selectedEntityIsInSimulation(IEntity entity)
      {
         return _simulation.Model.Root.Equals(entity.RootContainer) ||
                _simulation.Model.Neighborhoods.Equals(entity.RootContainer);
      }
   }
}