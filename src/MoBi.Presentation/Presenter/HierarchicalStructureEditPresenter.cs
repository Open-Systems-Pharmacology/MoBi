using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public abstract class HierarchicalStructureEditPresenter : HierarchicalStructurePresenter
   {
      protected readonly INeighborhoodToNeighborDTOMapper _neighborhoodToNeighborDTOMapper;

      protected ITreeNode _favoritesNode;
      protected ITreeNode _userDefinedNode;

      protected HierarchicalStructureEditPresenter(
         IHierarchicalStructureView view,
         IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper,
         ITreeNodeFactory treeNodeFactory,
         INeighborhoodToNeighborDTOMapper neighborhoodToNeighborDTOMapper)
         : base(view, context, objectBaseMapper)
      {
         _neighborhoodToNeighborDTOMapper = neighborhoodToNeighborDTOMapper;
         _favoritesNode = treeNodeFactory.CreateForFavorites();
         _userDefinedNode = treeNodeFactory.CreateForUserDefined();
      }

      public virtual void Select(ObjectBaseDTO objectBaseDTO)
      {
         if (objectBaseDTO == _favoritesNode.TagAsObject)
            RaiseFavoritesSelectedEvent();

         else if (objectBaseDTO == _userDefinedNode.TagAsObject)
            RaiseUserDefinedSelectedEvent();

         else
            RaiseEntitySelectedEvent(objectBaseDTO);
      }

      protected void RaiseEntitySelectedEvent(ObjectBaseDTO objectBaseDTO)
      {
         var entity = GetSelectedObject(objectBaseDTO);
         _context.PublishEvent(new EntitySelectedEvent(entity, this));
      }

      protected IObjectBase GetSelectedObject(ObjectBaseDTO dto)
      {
         if (dto is NeighborDTO neighborDTO)
         {
            return GetEntityForNeighbor(neighborDTO);
         }

         return dto.ObjectBase;
      }

      protected abstract IEntity GetEntityForNeighbor(NeighborDTO neighborDTO);

      protected abstract void RaiseFavoritesSelectedEvent();

      protected abstract void RaiseUserDefinedSelectedEvent();
   }
}