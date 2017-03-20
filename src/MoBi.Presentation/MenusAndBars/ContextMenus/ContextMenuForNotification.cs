using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class ContextMenuForNotification : ContextMenuBase
   {
      private readonly NotificationMessageDTO _notificationMessage;
      private readonly INotificationPresenter _presenter;

      public ContextMenuForNotification(NotificationMessageDTO notificationMessage, INotificationPresenter presenter)
      {
         _notificationMessage = notificationMessage;
         _presenter = presenter;
      }

      public override IEnumerable<IMenuBarItem> AllMenuItems()
      {
         yield return CreateMenuButton.WithCaption(AppConstants.MenuNames.GoTo)
                   .WithActionCommand(() => _presenter.GoToObject(_notificationMessage))
                   .WithIcon(ApplicationIcons.GoTo);
      }
   }

   public class ContextMenuForNotificationFactory : IContextMenuSpecificationFactory<IViewItem>
   {
      public IContextMenu CreateFor(IViewItem viewItem, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return new ContextMenuForNotification(viewItem.DowncastTo<NotificationMessageDTO>(), presenter.DowncastTo<INotificationPresenter>());
      }

      public bool IsSatisfiedBy(IViewItem objectRequestingContextMenu, IPresenterWithContextMenu<IViewItem> presenter)
      {
         return objectRequestingContextMenu.IsAnImplementationOf<NotificationMessage>() &&
                presenter.IsAnImplementationOf<INotificationPresenter>();
      }
   }
}