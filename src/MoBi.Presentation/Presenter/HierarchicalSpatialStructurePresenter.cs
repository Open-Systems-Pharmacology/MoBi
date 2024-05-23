using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Nodes;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.Presenter
{
   public interface IHierarchicalSpatialStructurePresenter : IEditPresenter<SpatialStructure>,
      IHierarchicalStructurePresenter,
      IListener<AddedEvent>,
      IListener<RemovedEvent>,
      IListener<EntitySelectedEvent>
   {
      void Select(IEntity entity);
      void Refresh(IEntity entity);
   }

   public class HierarchicalSpatialStructurePresenter : HierarchicalStructureEditPresenter, IHierarchicalSpatialStructurePresenter
   {
      private SpatialStructure _spatialStructure;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;

      public HierarchicalSpatialStructurePresenter(
         IHierarchicalStructureView view,
         IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper,
         IViewItemContextMenuFactory contextMenuFactory,
         ITreeNodeFactory treeNodeFactory, INeighborhoodToNeighborDTOMapper neighborhoodToNeighborDTOMapper)
         : base(view, context, objectBaseMapper, treeNodeFactory, neighborhoodToNeighborDTOMapper)
      {
         _contextMenuFactory = contextMenuFactory;
      }

      public void InitializeWith(SpatialStructure spatialStructure)
      {
         _spatialStructure = spatialStructure;

         _view.AddNode(_favoritesNode);
         _view.AddNode(_userDefinedNode);

         var roots = new List<ObjectBaseDTO> { _objectBaseMapper.MapFrom(spatialStructure.GlobalMoleculeDependentProperties) };
         spatialStructure.TopContainers.Each(x => roots.Add(_objectBaseMapper.MapFrom(x)));

         var neighborhood = _objectBaseMapper.MapFrom(spatialStructure.NeighborhoodsContainer);
         neighborhood.Description = ToolTips.BuildingBlockSpatialStructure.HowToCreateNeighborhood;

         roots.Add(neighborhood);

         _view.Show(roots);
      }

      public void Edit(SpatialStructure spatialStructure)
      {
         InitializeWith(spatialStructure);
      }

      public object Subject => _spatialStructure;

      public void Edit(object objectToEdit) => Edit(objectToEdit.DowncastTo<SpatialStructure>());

      protected override void RaiseFavoritesSelectedEvent()
      {
         _context.PublishEvent(new FavoritesSelectedEvent(_spatialStructure));
      }

      protected override void RaiseUserDefinedSelectedEvent()
      {
         _context.PublishEvent(new UserDefinedSelectedEvent(_spatialStructure));
      }

      public override void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation)
      {
         var contextMenu = _contextMenuFactory.CreateFor(objectRequestingPopup ?? new SpatialStructureRootItem(), this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Handle(AddedEvent eventToHandle)
      {
         if (_spatialStructure == null)
            return;

         var entity = eventToHandle.AddedObject as IContainer;

         if (entity == null)
            return;

         if (entity.IsAnImplementationOf<IDistributedParameter>())
            return;

         //not in the spatial structure? nothing to do here in the first place
         if (!entityIsInSpatialStructure(entity))
            return;

         var dto = _objectBaseMapper.MapFrom(entity);

         // A root container in a spatial structure should be added to the root of the tree view
         // The root container will have ParentContainer == null
         if (entityIsRootContainer(entity))
            _view.AddRoot(dto);
         else
            _view.Add(dto, _objectBaseMapper.MapFrom(entity.ParentContainer));
      }

      private bool entityIsRootContainer(IContainer entity) => entity.RootContainer == entity;

      public void Handle(RemovedEvent eventToHandle)
      {
         if (_spatialStructure == null) return;
         eventToHandle.RemovedObjects.OfType<IEntity>().Each(remove);
      }

      public void Select(IEntity entity) => _view.Select(entity);

      public void Refresh(IEntity entity)
      {
         var dto = _objectBaseMapper.MapFrom(entity);
         _view.Refresh(dto);
      }

      private void remove(IEntity entity) => _view.Remove(entity);

      /// <summary>
      ///    Entity is in spatial structure when its root container is either one of top container or the neighborhood container
      /// </summary>
      private bool entityIsInSpatialStructure(IEntity entity)
      {
         var rootContainer = entity.RootContainer;
         return _spatialStructure.TopContainers.Contains(rootContainer) ||
                Equals(_spatialStructure.NeighborhoodsContainer, rootContainer);
      }

      public void Handle(EntitySelectedEvent eventToHandle)
      {
         if (eventToHandle.Sender == this)
            return;

         var entity = eventToHandle.ObjectBase as IEntity;
         if (entity == null || _spatialStructure == null)
            return;

         if (!entityIsInSpatialStructure(entity))
            return;

         var entityToSelect = entity.IsAnImplementationOf<IParameter>() ? entity.ParentContainer : entity;
         _view.Select(entityToSelect);
      }

      protected override IEntity GetEntityForNeighbor(NeighborDTO neighborDTO)
      {
         return _spatialStructure.TopContainers.Select(x => neighborDTO.Path.Resolve<IEntity>(x)).FirstOrDefault(x => x != default);
      }

      protected override IReadOnlyList<ObjectBaseDTO> GetChildrenSorted(IContainer container, Func<IEntity, bool> predicate)
      {
         if (container is NeighborhoodBuilder neighborhood)
         {
            return neighborsOf(neighborhood).Union(base.GetChildrenSorted(container, predicate)).ToList();
         }

         return base.GetChildrenSorted(container, predicate);
      }

      private IEnumerable<ObjectBaseDTO> neighborsOf(NeighborhoodBuilder neighborhoodBuilder)
      {
         return _neighborhoodToNeighborDTOMapper.MapFrom(neighborhoodBuilder);
      }
   }
}