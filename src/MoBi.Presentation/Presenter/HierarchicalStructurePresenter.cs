using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public interface IHierarchicalStructurePresenter : IPresenterWithContextMenu<IViewItem>
   {
      IReadOnlyList<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto, Func<IEntity, bool> predicate);
      void Select(ObjectBaseDTO objectBaseDTO);
      void Clear();
   }

   public abstract class HierarchicalStructurePresenter : AbstractCommandCollectorPresenter<IHierarchicalStructureView, IHierarchicalStructurePresenter>
   {
      protected readonly IMoBiContext _context;
      protected IObjectBaseToObjectBaseDTOMapper _objectBaseMapper;
      protected ITreeNode _favoritesNode;
      protected ITreeNode _userDefinedNode;

      protected HierarchicalStructurePresenter(
         IHierarchicalStructureView view, 
         IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper, 
         ITreeNodeFactory treeNodeFactory)
         : base(view)
      {
         _context = context;
         _objectBaseMapper = objectBaseMapper;
         _favoritesNode = treeNodeFactory.CreateForFavorites();
         _userDefinedNode = treeNodeFactory.CreateForUserDefined();
      }

      public virtual IReadOnlyList<ObjectBaseDTO> GetChildObjects(ObjectBaseDTO dto, Func<IEntity, bool> predicate)
      {
         return (dto.ObjectBase is IContainer container) ? GetChildrenSorted(container, predicate) : Array.Empty<ObjectBaseDTO>();
      }

      protected virtual IReadOnlyList<ObjectBaseDTO> GetChildrenSorted(IContainer container, Func<IEntity, bool> predicate)
      {
         IReadOnlyList<ObjectBaseDTO> allChildrenDTO()
         {
            return container.GetChildren(predicate)
               .OrderBy(groupingTypeFor)
               .ThenBy(x => x.Name)
               .MapAllUsing(_objectBaseMapper);
         }

         switch (container)
         {
            case NeighborhoodBuilder neighborhood:
               return neighborsOf(neighborhood).Union(allChildrenDTO()).ToList();
            default:
               return allChildrenDTO();

         }
      }

      private IEnumerable<ObjectBaseDTO> neighborsOf(NeighborhoodBuilder neighborhoodBuilder)
      {
         if(neighborhoodBuilder.FirstNeighborPath!=null)
            yield return new ObjectBaseDTO{Name = neighborhoodBuilder.FirstNeighborPath, Icon = ApplicationIcons.Neighbor, Id = createNeighborhoodId(neighborhoodBuilder.FirstNeighbor, neighborhoodBuilder.SecondNeighbor)};

         if (neighborhoodBuilder.SecondNeighborPath != null)
            yield return new ObjectBaseDTO { Name = neighborhoodBuilder.SecondNeighborPath, Icon = ApplicationIcons.Neighbor, Id = createNeighborhoodId(neighborhoodBuilder.SecondNeighbor, neighborhoodBuilder.FirstNeighbor) };
      }

      private string createNeighborhoodId(IWithId me, IWithId myNeighbor)
      {
         return $"{me.Id}-{myNeighbor.Id}";
      }

      private ContainerType groupingTypeFor(IEntity entity)
      {
         var container = entity as IContainer;
         return container?.ContainerType ?? ContainerType.Other;
      }

      public virtual void Select(ObjectBaseDTO objectBaseDTO)
      {
         if (objectBaseDTO == _favoritesNode.TagAsObject)
            RaiseFavoritesSelectedEvent();

         else if (objectBaseDTO == _userDefinedNode.TagAsObject)
            RaiseUserDefinedSelectedEvent();

         else
            raiseEntitySelectedEvent(objectBaseDTO);
      }

      private void raiseEntitySelectedEvent(ObjectBaseDTO objectBaseDTO)
      {
         _context.PublishEvent(new EntitySelectedEvent(objectBaseDTO.ObjectBase, this));
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