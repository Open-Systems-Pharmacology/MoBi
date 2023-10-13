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

   public class HierarchicalSpatialStructurePresenter : HierarchicalStructurePresenter, IHierarchicalSpatialStructurePresenter
   {
      private SpatialStructure _spatialStructure;
      private readonly IViewItemContextMenuFactory _contextMenuFactory;

      public HierarchicalSpatialStructurePresenter(
         IHierarchicalStructureView view,
         IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper,
         IViewItemContextMenuFactory contextMenuFactory,
         ITreeNodeFactory treeNodeFactory)
         : base(view, context, objectBaseMapper, treeNodeFactory)
      {
         _contextMenuFactory = contextMenuFactory;
      }

      public void InitializeWith(SpatialStructure spatialStructure)
      {
         _spatialStructure = spatialStructure;

         _view.AddNode(_favoritesNode);
         _view.AddNode(_userDefinedNode);

         var roots = new List<ObjectBaseDTO> {_objectBaseMapper.MapFrom(spatialStructure.GlobalMoleculeDependentProperties)};
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


         var dto = _objectBaseMapper.MapFrom(entity);

         if (entityIsInSpatialStructure(entity))
            _view.Add(dto, _objectBaseMapper.MapFrom(entity.ParentContainer));
         else
         {
            //the object that has changed does not belong into the spatial structure. Nothing do to
            if (!Equals(eventToHandle.Parent, _spatialStructure))
               return;

            _view.AddRoot(dto);
         }
      }

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

      private bool entityIsInSpatialStructure(IEntity entity) => _spatialStructure.TopContainers.Contains(entity.RootContainer);

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
   }
}