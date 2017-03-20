using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuFactoryForContainerInEvents : IContextMenuSpecificationFactory<IViewItem>
   {
      private readonly IMoBiContext _context;

      public ContextMenuFactoryForContainerInEvents(IMoBiContext context)
      {
         _context = context;
      }

      public IContextMenu CreateFor(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var contextMenu = IoC.Resolve<IContextMenuForContainerInEventGroups>();
         var dto = objectRequestingContextMenu as IObjectBaseDTO;
         return contextMenu.InitializeWith(dto, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         var dto = objectRequestingContextMenu as IObjectBaseDTO;
         if (dto == null) return false;
         if (!presenter.IsAnImplementationOf<IEventGroupListPresenter>()) return false;
         var entity = _context.Get<IObjectBase>(dto.Id);
         return isContainerWithSpecialContextMenu(entity);
      }

      private static bool isContainerWithSpecialContextMenu(IObjectBase entity)
      {
         return entity.IsAnImplementationOf<IContainer>() && 
            !entity.IsAnImplementationOf<ITransportBuilder>() && 
            !entity.IsAnImplementationOf<IEventGroupBuilder>() && 
            !entity.IsAnImplementationOf<IEventBuilder>();
      }
   }
}