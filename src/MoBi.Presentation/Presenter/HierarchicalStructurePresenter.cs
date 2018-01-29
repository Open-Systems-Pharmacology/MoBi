using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public interface IHierarchicalStructurePresenter : IPresenterWithContextMenu<IViewItem>
   {
      IReadOnlyList<IObjectBaseDTO> GetChildObjects(IObjectBaseDTO dto, Func<IEntity, bool> predicate);
      void Select(IObjectBaseDTO objectBaseDTO);
      void Clear();
   }

   public abstract class HierarchicalStructurePresenter : AbstractCommandCollectorPresenter<IHierarchicalStructureView, IHierarchicalStructurePresenter>
   {
      protected readonly IMoBiContext _context;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseMapper;
      protected ITreeNode _favoritesNode;
      protected ITreeNode _userDefinedNode;

      protected HierarchicalStructurePresenter(IHierarchicalStructureView view, IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper, ITreeNodeFactory treeNodeFactory)
         : base(view)
      {
         _context = context;
         _objectBaseMapper = objectBaseMapper;
         _favoritesNode = treeNodeFactory.CreateForFavorites();
         _userDefinedNode = treeNodeFactory.CreateForUserDefined();
      }

      public virtual IReadOnlyList<IObjectBaseDTO> GetChildObjects(IObjectBaseDTO dto, Func<IEntity, bool> predicate)
      {
         var container = _context.Get<IContainer>(dto.Id);
         return container == null ? new List<IObjectBaseDTO>() : GetChildrenSorted(container, predicate);
      }

      protected virtual IReadOnlyList<IObjectBaseDTO> GetChildrenSorted(IContainer container,
         Func<IEntity, bool> predicate)
      {
         return container.GetChildren(predicate)
            .OrderBy(groupingTypeFor)
            .ThenBy(x => x.Name)
            .MapAllUsing(_objectBaseMapper);
      }

      private ContainerType groupingTypeFor(IEntity entity)
      {
         var container = entity as IContainer;
         return container?.ContainerType ?? ContainerType.Other;
      }

      public virtual void Select(IObjectBaseDTO objectBaseDTO)
      {
         if (objectBaseDTO ==_favoritesNode.TagAsObject)
            RaiseFavoritesSelectedEvent();

         else if(objectBaseDTO == _userDefinedNode.TagAsObject)
            RaiseFavoritesSelectedEvent();
    
         else
            raiseEntitySelectedEvent(objectBaseDTO);
      }

      private void raiseEntitySelectedEvent(IObjectBaseDTO dtoObjectBase)
      {
         var objectBase = _context.Get<IObjectBase>(dtoObjectBase.Id);
         _context.PublishEvent(new EntitySelectedEvent(objectBase, this));
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         base.ReleaseFrom(eventPublisher);
         Clear();
      }

      public void Clear()
      {
         _view.Clear();
      }

      protected abstract void RaiseFavoritesSelectedEvent();

      protected abstract void RaiseUserDefinedSelectedEvent();

      public abstract void ShowContextMenu(IViewItem objectRequestingPopup, Point popupLocation);
   }
}