using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using ISimulationPersistableUpdater = MoBi.Core.Services.ISimulationPersistableUpdater;

namespace MoBi.Presentation.Presenter
{
   public interface IHierarchicalQuantitySelectionPresenter : IHierarchicalStructurePresenter
   {
      /// <summary>
      /// Selects the quantity from the <paramref name="quantityPath"/> in the tree view
      /// </summary>
      void SelectQuantityFromPath(ObjectPath quantityPath);

      /// <summary>
      /// The path of the quantity that was selected in the tree view
      /// </summary>
      ObjectPath SelectedPath { get; }


      void SelectPathFrom(IReadOnlyList<ISimulation> simulations);
   }

   public class HierarchicalQuantitySelectionPresenter : HierarchicalStructurePresenter, IHierarchicalQuantitySelectionPresenter
   {
      private readonly ISimulationPersistableUpdater _simulationPersistableUpdater;
      private readonly IEntityPathResolver _entityPathResolver;
      private IReadOnlyList<ISimulation> _simulations;
      private ObjectBaseDTO _selection;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;

      public HierarchicalQuantitySelectionPresenter(IHierarchicalStructureView view,
         IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper,
         ISimulationPersistableUpdater simulationPersistableUpdater,
         IEntityPathResolver entityPathResolver, IViewItemContextMenuFactory contextMenuFactory) :
         base(view, context, objectBaseMapper)
      {
         _simulationPersistableUpdater = simulationPersistableUpdater;
         _entityPathResolver = entityPathResolver;
         _contextMenuFactory = contextMenuFactory;
      }

      public void SelectPathFrom(IReadOnlyList<ISimulation> simulations)
      {
         _simulations = simulations;
         var simulationNodes = new List<ObjectBaseDTO>();
         simulationNodes.AddRange(_simulations.Select(x => _objectBaseMapper.MapFrom(x)));
         _view.Show(simulationNodes);
      }

      public override IReadOnlyList<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto, Func<IEntity, bool> predicate)
      {
         if (dto.ObjectBase is ISimulation simulation)
            return GetChildrenSorted(simulation.Model.Root, hasSelectable);

         return base.GetChildObjects(dto, predicate);
      }

      protected override IReadOnlyList<ObjectBaseDTO> GetChildrenSorted(IContainer container, Func<IEntity, bool> predicate)
      {
         // Use hasPersistable because we want to include any container that contains a persistable quantity
         // as well as the quantities themselves.
         return base.GetChildrenSorted(container, x => hasSelectable(x) && predicate(x));
      }

      public override void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _contextMenuFactory.CreateFor(objectRequestingPopup, this);
         contextMenu.Show(_view, popupLocation);
      }

      private bool hasSelectable(IEntity entity)
      {
         if (entityIsSelectableQuantity(entity))
            return true;

         if (entity is IContainer container)
            return container.Any(hasSelectable);

         return false;
      }

      private bool entityIsSelectableQuantity(IObjectBase entity)
      {
         if (entity is IQuantity quantity)
            return _simulationPersistableUpdater.QuantityIsSelectable(quantity, forceAmountToBeSelectable: true);

         return false;
      }

      public void SelectQuantityFromPath(ObjectPath quantityPath)
      {
         if (quantityPath == null)
            return;

         var entity = _simulations.Select(x => x.Model.Root.EntityAt<IEntity>(quantityPath)).FirstOrDefault();
         if (entity == null)
            return;

         _view.Select(entity);
      }

      public ObjectPath SelectedPath => _selection.ObjectBase is IEntity entity ? _entityPathResolver.ObjectPathFor(entity) : null;

      public object Subject => _simulations;

      public void Select(ObjectBaseDTO objectBaseDTO)
      {
         _selection = objectBaseDTO;
         ViewChanged();
      }

      public override bool CanClose => entityIsSelectableQuantity(_selection?.ObjectBase);
   }
}