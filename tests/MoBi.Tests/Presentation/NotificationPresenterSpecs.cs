using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Core.Commands;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using MoBi.Core.Services;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Views;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Regions;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;

namespace MoBi.Presentation
{
   public abstract class concern_for_NotificationPresenter : ContextSpecification<INotificationPresenter>
   {
      protected INotificationView _view;
      private IRegionResolver _regionResolver;
      protected IRegion _region;
      protected IUserSettings _userSettings;
      protected NotificationType _settingsToUse = NotificationType.Error | NotificationType.Warning;
      protected IEnumerable<NotificationMessageDTO> _allNotifications;
      protected INotificationMessageMapper _notificationMessageMapper;
      protected IBuildingBlockRetriever _buildingBlockRetriever;
      private IViewItemContextMenuFactory _viewItemContextMenuFactory;
      protected IMoBiApplicationController _applicationController;
      protected IMoBiContext _context;
      protected IBuildingBlock _buildingBlock;
      protected IDialogCreator _dialogCreator;

      protected override void Context()
      {
         _view = A.Fake<INotificationView>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         _region = A.Fake<IRegion>();
         _userSettings = A.Fake<IUserSettings>();
         _viewItemContextMenuFactory = A.Fake<IViewItemContextMenuFactory>();
         _userSettings.VisibleNotification = _settingsToUse;
         _regionResolver = A.Fake<IRegionResolver>();
         _buildingBlockRetriever = A.Fake<IBuildingBlockRetriever>();
         _applicationController = A.Fake<IMoBiApplicationController>();
         _context = A.Fake<IMoBiContext>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _notificationMessageMapper = new NotificationMessageMapper(new ObjectTypeResolver(), _buildingBlockRetriever);
         A.CallTo(() => _regionResolver.RegionWithName(RegionNames.NotificationList)).Returns(_region);
         sut = new NotificationPresenter(_view, _regionResolver, _userSettings, _notificationMessageMapper, _viewItemContextMenuFactory, _applicationController, _context, _dialogCreator);
         A.CallTo(() => _view.BindTo(A<NotifyList<NotificationMessageDTO>>._))
            .Invokes(x => _allNotifications = x.GetArgument<IEnumerable<NotificationMessageDTO>>(0));

         sut.Initialize();
      }
   }

   public class When_initializing_the_notification_list_presenter : concern_for_NotificationPresenter
   {
      protected override void Context()
      {
         _settingsToUse = NotificationType.Error | NotificationType.Info;
         base.Context();
      }

      [Observation]
      public void should_set_the_view_in_the_error_list_region()
      {
         A.CallTo(() => _region.Add(_view)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_available_notification_to_the_view()
      {
         A.CallTo(() => _view.BindTo(A<NotifyList<NotificationMessageDTO>>._)).MustHaveHappened();
      }

      [Observation]
      public void the_visible_info_should_be_set_to_the_one_defined_in_the_user_settings()
      {
         sut.VisibleNotification.Is(NotificationType.Error).ShouldBeTrue();
         sut.VisibleNotification.Is(NotificationType.Info).ShouldBeTrue();
         sut.VisibleNotification.Is(NotificationType.Warning).ShouldBeFalse();
      }

      [Observation]
      public void should_set_the_check_state_for_the_button_in_the_view()
      {
         A.CallTo(() => _view.Show(NotificationType.Error, true)).MustHaveHappened();
         A.CallTo(() => _view.Show(NotificationType.Info, true)).MustHaveHappened();
         A.CallTo(() => _view.Show(NotificationType.Warning, false)).MustHaveHappened();
      }
   }

   public class When_toggling_the_notification_display : concern_for_NotificationPresenter
   {
      protected override void Because()
      {
         sut.Toggle(NotificationType.Error);
      }

      [Observation]
      public void should_not_display_errors_anymore()
      {
         sut.VisibleNotification.Is(NotificationType.Error).ShouldBeFalse();
      }
   }

   public class When_toggling_the_warning_display : concern_for_NotificationPresenter
   {
      protected override void Because()
      {
         sut.Toggle(NotificationType.Warning);
      }

      [Observation]
      public void should_not_display_warnings_anymore()
      {
         sut.VisibleNotification.Is(NotificationType.Warning).ShouldBeFalse();
      }
   }

   public class When_toggling_the_message_display : concern_for_NotificationPresenter
   {
      protected override void Because()
      {
         sut.Toggle(NotificationType.Info);
      }

      [Observation]
      public void should_display_messages()
      {
         sut.VisibleNotification.Is(NotificationType.Info).ShouldBeTrue();
      }
   }

   public class When_toggling_the_visibility_of_the_entire_view : concern_for_NotificationPresenter
   {
      protected override void Because()
      {
         sut.ToggleVisibility();
      }

      [Observation]
      public void should_delegate_to_the_underlying_region()
      {
         A.CallTo(() => _region.ToggleVisibility()).MustHaveHappened();
      }
   }

   public class When_the_notification_list_presenter_is_being_notified_that_some_validations_should_be_displayed : concern_for_NotificationPresenter
   {
      private ValidationResult _validationResult;

      protected override void Context()
      {
         base.Context();

         _validationResult = new ValidationResult();
         _validationResult.AddMessage(NotificationType.Error, A.Fake<IObjectBase>().WithId("1"), string.Empty);
         _validationResult.AddMessage(NotificationType.Warning, A.Fake<IObjectBase>().WithId("2"), string.Empty);
      }

      protected override void Because()
      {
         sut.Handle(new ShowValidationResultsEvent(_validationResult));
      }

      [Observation]
      public void should_add_the_new_items_to_the_view()
      {
         _allNotifications.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_update_the_count_for_all_notifications()
      {
         A.CallTo(() => _view.SetNotificationCount(NotificationType.Error, 1)).MustHaveHappened();
         A.CallTo(() => _view.SetNotificationCount(NotificationType.Warning, 1)).MustHaveHappened();
         A.CallTo(() => _view.SetNotificationCount(NotificationType.Info, 0)).MustHaveHappened();
      }
   }

   public class When_the_notification_list_presenter_is_being_notified_that_some_validations_should_be_displayed_but_some_of_these_validations_are_hidden : concern_for_NotificationPresenter
   {
      private ValidationResult _validationResult;

      protected override void Context()
      {
         base.Context();
         _userSettings.ShowPKSimObserverMessages = false;
         _userSettings.ShowUnresolvedEndosomeMessagesForInitialConditions = false;
         _validationResult = new ValidationResult();

         var staticObserver = A.Fake<ObserverBuilder>().WithId("1").WithName(AppConstants.DefaultNames.PKSimStaticObservers[0]);
         var dynamicObserver = A.Fake<ObserverBuilder>().WithId("2").WithName($"{AppConstants.DefaultNames.PKSimDynamicObservers[0]}-HELLO");

         _validationResult.AddMessage(NotificationType.Error, staticObserver, string.Empty);
         _validationResult.AddMessage(NotificationType.Error, dynamicObserver, string.Empty);
         _validationResult.AddMessage(NotificationType.Warning, A.Fake<IObjectBase>().WithId("2"), string.Empty);
         _validationResult.AddMessage(NotificationType.Warning, A.Fake<IObjectBase>().WithId("2"), string.Empty);
         var initialCondition = new InitialCondition
         {
            Name = "moleculeName",
            ContainerPath = new ObjectPath("Endosome"),
            Id = "3"
         };
         // this message should be hidden by ShowUnresolvedEndosomeMessagesForInitialConditions
         _validationResult.AddMessage(NotificationType.Warning, initialCondition, Validation.StartValueDefinedForContainerThatCannotBeResolved("moleculeName", "Endosome"));
         // this message should not be hidden by ShowUnresolvedEndosomeMessagesForInitialConditions because it a different type of warning for the same initial condition
         initialCondition = new InitialCondition
         {
            Name = "molecule2Name",
            ContainerPath = new ObjectPath("Endosome"),
            Id = "4"
         };
         _validationResult.AddMessage(NotificationType.Warning, initialCondition, string.Empty);
      }

      protected override void Because()
      {
         sut.Handle(new ShowValidationResultsEvent(_validationResult));
      }

      [Observation]
      public void should_only_add_visible_notification()
      {
         _allNotifications.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_update_the_count_for_all_notifications()
      {
         A.CallTo(() => _view.SetNotificationCount(NotificationType.Error, 0)).MustHaveHappened();
         A.CallTo(() => _view.SetNotificationCount(NotificationType.Warning, 2)).MustHaveHappened();
         A.CallTo(() => _view.SetNotificationCount(NotificationType.Info, 0)).MustHaveHappened();
      }
   }

   public class When_the_notification_presenter_is_asked_if_a_notification_should_be_displayed : concern_for_NotificationPresenter
   {
      private NotificationMessageDTO _visibleNotification;
      private NotificationMessageDTO _hiddenNotification;

      protected override void Context()
      {
         _settingsToUse = NotificationType.Error | NotificationType.Info;
         base.Context();
         _visibleNotification = new NotificationMessageDTO(new NotificationMessage(A.Fake<IObjectBase>(), MessageOrigin.Simulation, A.Fake<IBuildingBlock>(), NotificationType.Error));
         _hiddenNotification = new NotificationMessageDTO(new NotificationMessage(A.Fake<IObjectBase>(), MessageOrigin.Simulation, A.Fake<IBuildingBlock>(), NotificationType.Warning));
      }

      [Observation]
      public void should_return_true_if_the_notification_type_is_selected()
      {
         sut.ShouldShow(_visibleNotification).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_notification_type_is_not_selected()
      {
         sut.ShouldShow(_hiddenNotification).ShouldBeFalse();
      }
   }

   public class When_the_notification_presenter_is_being_notified_that_a_formula_is_invalid : concern_for_NotificationPresenter
   {
      private IFormula _newInvalidFormula;
      private IFormula _existingInvalidFormula;

      protected override void Context()
      {
         base.Context();
         _existingInvalidFormula = A.Fake<IFormula>().WithId("EXISTING");
         _newInvalidFormula = A.Fake<IFormula>().WithId("NEW");
         sut.Handle(new FormulaInvalidEvent(_existingInvalidFormula, _buildingBlock, "EXISTING OLD"));
      }

      protected override void Because()
      {
         sut.Handle(new FormulaInvalidEvent(_newInvalidFormula, _buildingBlock, "NEW"));
         sut.Handle(new FormulaInvalidEvent(_existingInvalidFormula, _buildingBlock, "EXISTING NEW"));
      }

      [Observation]
      public void should_add_a_notification_to_the_view_if_the_formula_was_not_already_displayed()
      {
         _allNotifications.Count().ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_update_the_notification_to_the_view_if_the_formula_was_already_displayed()
      {
         _allNotifications.Select(x => x.NotificationMessage.Message).ShouldOnlyContain("NEW", "EXISTING NEW");
      }
   }

   public class When_the_notification_presenter_is_being_notified_that_a_formula_is_valid : concern_for_NotificationPresenter
   {
      private IFormula _validFormula;
      private IFormula _invalidFormula;

      protected override void Context()
      {
         base.Context();
         _validFormula = A.Fake<IFormula>().WithId("VALID");
         _invalidFormula = A.Fake<IFormula>().WithId("INVALID");
         sut.Handle(new FormulaInvalidEvent(_validFormula, _buildingBlock, "VALID_MESSAGE"));
         sut.Handle(new FormulaInvalidEvent(_invalidFormula, _buildingBlock, "INVALID_MESSAGE"));
      }

      protected override void Because()
      {
         sut.Handle(new FormulaValidEvent(_validFormula, _buildingBlock));
      }

      [Observation]
      public void should_remove_any_previous_notification_if_a_warning_for_the_formula_was_being_displayed()
      {
         _allNotifications.Count().ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_have_kept_the_other_formula_notification()
      {
         _allNotifications.Select(x => x.NotificationMessage.Message).ShouldOnlyContain("INVALID_MESSAGE");
      }
   }

   public class When_the_notification_presenter_is_told_to_clear_the_notifications_of_a_given_origin : concern_for_NotificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         var validationResult = new ValidationResult();
         validationResult.AddMessage(NotificationType.Error, A.Fake<IObjectBase>(), "TOTO");
         sut.Handle(new ShowValidationResultsEvent(validationResult));
         sut.Handle(new FormulaInvalidEvent(A.Fake<IFormula>().WithId("INVALID"), _buildingBlock, "INVALID_MESSAGE"));
      }

      protected override void Because()
      {
         sut.ClearNotifications(MessageOrigin.Simulation);
      }

      [Observation]
      public void should_remove_the_expected_notification()
      {
         _allNotifications.Count().ShouldBeEqualTo(1);
         _allNotifications.ElementAt(0).MessageOrigin.ShouldBeEqualTo(MessageOrigin.Formula);
      }
   }

   public class When_the_notification_presenter_is_told_to_clear_all_notifications : concern_for_NotificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         var validationResult = new ValidationResult();
         validationResult.AddMessage(NotificationType.Error, A.Fake<IObjectBase>(), "TOTO");
         sut.Handle(new ShowValidationResultsEvent(validationResult));
         sut.Handle(new FormulaInvalidEvent(A.Fake<IFormula>().WithId("INVALID"), _buildingBlock, "INVALID_MESSAGE"));
      }

      protected override void Because()
      {
         sut.ClearAllNotification();
      }

      [Observation]
      public void should_remove_the_expected_notification()
      {
         _allNotifications.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_the_notification_presenter_is_notified_that_the_project_is_closed : concern_for_NotificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         var validationResult = new ValidationResult();
         validationResult.AddMessage(NotificationType.Error, A.Fake<IObjectBase>(), "TOTO");
         sut.Handle(new ShowValidationResultsEvent(validationResult));
         sut.Handle(new FormulaInvalidEvent(A.Fake<IFormula>().WithId("INVALID"), _buildingBlock, "INVALID_MESSAGE"));
      }

      protected override void Because()
      {
         sut.Handle(new ProjectClosedEvent());
      }

      [Observation]
      public void should_remove_all_notifications()
      {
         _allNotifications.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_the_notification_presenter_is_notified_that_an_building_block_was_removed_for_which_a_notification_was_available : concern_for_NotificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Handle(new FormulaInvalidEvent(A.Fake<IFormula>().WithId("INVALID"), _buildingBlock, "INVALID_MESSAGE"));
      }

      protected override void Because()
      {
         sut.Handle(new RemovedEvent(_buildingBlock, A.Fake<MoBiProject>()));
      }

      [Observation]
      public void should_remove_the_notification()
      {
         _allNotifications.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_the_notification_presenter_is_notified_that_an_entity_in_a_building_block_was_removed_for_which_a_notification_was_available : concern_for_NotificationPresenter
   {
      private IFormula _formula;

      protected override void Context()
      {
         base.Context();
         _formula = A.Fake<IFormula>().WithId("INVALID");
         sut.Handle(new FormulaInvalidEvent(_formula, _buildingBlock, "INVALID_MESSAGE"));
      }

      protected override void Because()
      {
         sut.Handle(new RemovedEvent(_formula, A.Fake<IObjectBase>()));
      }

      [Observation]
      public void should_remove_the_notification()
      {
         _allNotifications.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_the_notification_presenter_is_asked_to_select_the_item_corresponding_to_a_notification : concern_for_NotificationPresenter
   {
      private NotificationMessageDTO _notificationMessage;
      private IObjectBase _objectBase;

      protected override void Context()
      {
         base.Context();
         _objectBase = A.Fake<IObjectBase>();
         _buildingBlock = A.Fake<IBuildingBlock>();
         A.CallTo(() => _context.HistoryManager).Returns(A.Fake<IMoBiHistoryManager>());
         _notificationMessage = new NotificationMessageDTO(new NotificationMessage(_objectBase, MessageOrigin.Simulation, _buildingBlock, NotificationType.Error));
      }

      protected override void Because()
      {
         sut.GoToObject(_notificationMessage);
      }

      [Observation]
      public void should_select_the_item_in_the_view()
      {
         A.CallTo(() => _applicationController.Select(_objectBase, _buildingBlock, _context.HistoryManager)).MustHaveHappened();
      }
   }

   public class When_exporting_the_notification_to_a_log_file_and_the_user_cancels : concern_for_NotificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.ExportToFile();
      }

      [Observation]
      public void should_not_export_anything()
      {
      }
   }

   public class When_exporting_the_notification_to_a_log_file_and_the_user_confirms_the_export : concern_for_NotificationPresenter
   {
      private string _fileToExport;

      protected override void Context()
      {
         base.Context();
         _fileToExport = FileHelper.GenerateTemporaryFileName();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_fileToExport);
      }

      protected override void Because()
      {
         sut.ExportToFile();
      }

      [Observation]
      public void should_export_the_log_to_a_file()
      {
         FileHelper.FileExists(_fileToExport).ShouldBeTrue();
      }

      public override void Cleanup()
      {
         FileHelper.DeleteFile(_fileToExport);
      }
   }
}