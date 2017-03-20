using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public abstract class ContextMenuForObjectBaseDTOSpecificationFactory<TEntityType> : IContextMenuSpecificationFactory<IViewItem> where TEntityType : IObjectBase
   {
      private readonly IMoBiContext _context;

      protected ContextMenuForObjectBaseDTOSpecificationFactory(IMoBiContext context)
      {
         _context = context;
      }

      public virtual IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return CreateFor(objectRequestingContextMenu.DowncastTo<IObjectBaseDTO>(), presenter);
      }

      public abstract IContextMenu CreateFor(IObjectBaseDTO objectBaseDTO, IPresenterWithContextMenu<IViewItem> presenter);

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var objectBaseDTO = objectRequestingContextMenu as IObjectBaseDTO;
         if (objectBaseDTO == null) 
            return false;

         var entity = _context.Get<IEntity>(objectBaseDTO.Id);
         return IsSatisfiedBy(entity, presenter);
      }

      protected virtual bool IsSatisfiedBy(IEntity entity, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return entity.IsAnImplementationOf<TEntityType>();
      }
   }
}