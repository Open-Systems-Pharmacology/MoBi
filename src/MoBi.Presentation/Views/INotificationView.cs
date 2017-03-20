using System.Collections.Generic;
using MoBi.Core;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Collections;

namespace MoBi.Presentation.Views
{
   public interface INotificationView : IView<INotificationPresenter>
   {
      void Show(NotificationType type, bool visible);
      void BindTo(NotifyList<NotificationMessageDTO> notifications);
      void SetNotificationCount(NotificationType notificationType, int count);
   }
}