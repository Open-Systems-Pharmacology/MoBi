using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   internal interface IContextMenuSpecifictaionFactoryForEventGroupBuilder : IContextMenuSpecificationFactory<IViewItem>
   {

   }
   class ContextMenuSpecifictaionFactoryForEventGroupBuilder : IContextMenuSpecifictaionFactoryForEventGroupBuilder
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         IContextMenuFor contextMenu;
         if (viewItem==null||!viewItem.IsAnImplementationOf<ApplicationBuilderDTO>())
         {
            contextMenu = IoC.Resolve<ContextMenuForEventGroupBuilder>();
         }
         else
         {
            contextMenu = IoC.Resolve<ContextMenuForApplicationBuilder>();
         }


         return contextMenu.InitializeWith(viewItem as EventGroupBuilderDTO, presenter);
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         if (objectRequestingContextMenu == null && presenter.IsAnImplementationOf<IEventGroupListPresenter>())
         {
            return true;
         }
         if (objectRequestingContextMenu.IsAnImplementationOf<EventGroupBuilderDTO>())
         {
            return true;
         }
         return false;
      }
   }
}