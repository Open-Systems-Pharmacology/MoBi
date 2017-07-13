using FakeItEasy;
using MoBi.Core;
using MoBi.Presentation.MenusAndBars.ContextMenus;
using MoBi.Presentation.Presenter.Main;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation
{
   public abstract class concern_for_ContextMenuForNotificationFactory : ContextSpecification<ContextMenuForNotificationFactory>
   {
      protected override void Context()
      {
         sut = new ContextMenuForNotificationFactory();
      }
   }

   public class When_the_context_for_notification_factory_is_checking_if_an_object_is_satisfying_the_condition_to_create_the_context_menu : concern_for_ContextMenuForNotificationFactory
   {
      [Observation]
      public void should_return_true_if_the_presenter_is_a_notification_presenter_and_the_object_is_a_notification_item_dto()
      {
         var notificationItemDTO = new NotificationMessageDTO(A.Fake<NotificationMessage>());
         var notifcationPresenter = A.Fake<INotificationPresenter>();
         sut.IsSatisfiedBy(notificationItemDTO, notifcationPresenter).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         var notificationItemDTO = new NotificationMessageDTO(A.Fake<NotificationMessage>());
         var notifcationPresenter = A.Fake<INotificationPresenter>();
         sut.IsSatisfiedBy(A.Fake<IViewItem>() , notifcationPresenter).ShouldBeFalse();
         sut.IsSatisfiedBy(notificationItemDTO, A.Fake<IPresenterWithContextMenu<IViewItem>>()).ShouldBeFalse();
      }
   }
}	