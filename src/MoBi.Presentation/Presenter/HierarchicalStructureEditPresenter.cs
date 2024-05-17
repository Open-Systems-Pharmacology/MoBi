using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Nodes;
using ITreeNodeFactory = MoBi.Presentation.Nodes.ITreeNodeFactory;

namespace MoBi.Presentation.Presenter
{
   public abstract class HierarchicalStructureEditPresenter : HierarchicalStructurePresenter
   {

      protected ITreeNode _favoritesNode;
      protected ITreeNode _userDefinedNode;

      protected HierarchicalStructureEditPresenter(
         IHierarchicalStructureView view, 
         IMoBiContext context,
         IObjectBaseToObjectBaseDTOMapper objectBaseMapper, 
         ITreeNodeFactory treeNodeFactory)
         : base(view, context, objectBaseMapper)
      {
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

      protected virtual void RaiseEntitySelectedEvent(ObjectBaseDTO objectBaseDTO)
      {
         // This presenter cannot handle the Neighbor selection, but a derived presenter can
         if(!(objectBaseDTO is NeighborDTO))
            _context.PublishEvent(new EntitySelectedEvent(objectBaseDTO.ObjectBase, this));
      }

      protected abstract void RaiseFavoritesSelectedEvent();

      protected abstract void RaiseUserDefinedSelectedEvent();
   }
}