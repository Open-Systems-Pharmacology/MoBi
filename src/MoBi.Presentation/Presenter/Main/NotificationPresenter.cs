using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Presentation.Settings;
using MoBi.Presentation.Mappers;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;

namespace MoBi.Presentation.Presenter.Main
{
   public interface INotificationPresenter : IMainViewItemPresenter,
      IPresenterWithContextMenu<IViewItem>,
      IListener<ShowValidationResultsEvent>,
      IListener<FormulaValidEvent>,
      IListener<FormulaInvalidEvent>,
      IListener<ProjectClosedEvent>,
      IListener<ShowNotificationsEvent>,
      IListener<RemovedEvent>
   {
      void Toggle(NotificationType typeToToggle);
      NotificationType VisibleNotification { get; }
      bool ShouldShow(NotificationMessageDTO notification);
      void ClearNotifications(MessageOrigin messageOrigin);
      void ClearAllNotification();

      /// <summary>
      ///    Select the underlying object that was used to create the notification
      /// </summary>
      void GoToObject(NotificationMessageDTO notificationMessage);

      void ExportToFile();
   }

   public class NotificationPresenter : AbstractPresenter<INotificationView, INotificationPresenter>, INotificationPresenter
   {
      private readonly IRegionResolver _regionResolver;
      private readonly IUserSettings _userSettings;
      private readonly INotificationMessageMapper _notificationMessageMapper;
      private IRegion _region;
      private NotifyList<NotificationMessageDTO> _allNotifications;
      public NotificationType VisibleNotification { get; private set; }
      private readonly IViewItemContextMenuFactory _viewItemContextMenuFactory;
      private readonly IMoBiApplicationController _applicationController;
      private readonly IMoBiContext _context;
      private readonly IDialogCreator _dialogCreator;

      public NotificationPresenter(INotificationView view, IRegionResolver regionResolver, IUserSettings userSettings,
         INotificationMessageMapper notificationMessageMapper, IViewItemContextMenuFactory viewItemContextMenuFactory,
         IMoBiApplicationController applicationController, IMoBiContext context, IDialogCreator dialogCreator)
         : base(view)
      {
         _regionResolver = regionResolver;
         _userSettings = userSettings;
         _notificationMessageMapper = notificationMessageMapper;
         _viewItemContextMenuFactory = viewItemContextMenuFactory;
         _applicationController = applicationController;
         _context = context;
         _dialogCreator = dialogCreator;
         VisibleNotification = _userSettings.VisibleNotification;
      }

      public void ToggleVisibility()
      {
         _region.ToggleVisibility();
      }

      public override void Initialize()
      {
         _region = _regionResolver.RegionWithName(RegionNames.NotificationList);
         _region.Add(_view);
         new[] {NotificationType.Error, NotificationType.Info, NotificationType.Warning}.Each(t => _view.Show(t, isShowing(t)));
         _allNotifications = new NotifyList<NotificationMessageDTO>();
         bindToView();
      }

      private void bindToView()
      {
         _view.BindTo(_allNotifications);
         updateNotificationCount();
      }

      private void updateNotificationCount()
      {
         updateCountFor(NotificationType.Error);
         updateCountFor(NotificationType.Warning);
         updateCountFor(NotificationType.Info);
      }

      private void updateCountFor(NotificationType notificationType)
      {
         _view.SetNotificationCount(notificationType, _allNotifications.Count(x => x.Type == notificationType));
      }

      public bool ShouldShow(NotificationMessageDTO notification)
      {
         return notification.Type.Is(VisibleNotification);
      }

      private bool isPKSimObserverMessage(NotificationMessageDTO notification)
      {
         if (!notification.Object.IsAnImplementationOf<IObserverBuilder>())
            return false;

         if (AppConstants.DefaultNames.PKSimStaticObservers.Contains(notification.ObjectName))
            return true;

         return AppConstants.DefaultNames.PKSimDynamicObservers.Any(notification.ObjectName.StartsWith);
      }

      public void ClearNotifications(MessageOrigin messageOrigin)
      {
         var newNotifications = new NotifyList<NotificationMessageDTO>(_allNotifications);
         _allNotifications.Where(n => n.MessageOrigin.Is(messageOrigin)).Each(n => newNotifications.Remove(n));
         _allNotifications = newNotifications;
         bindToView();
      }

      public void ClearAllNotification()
      {
         ClearNotifications(MessageOrigin.All);
      }

      public void GoToObject(NotificationMessageDTO notificationMessage)
      {
         if (notificationMessage == null) return;
         _applicationController.Select(notificationMessage.Object, notificationMessage.BuildingBlock, _context.HistoryManager);
      }

      public void ExportToFile()
      {
         var file = _dialogCreator.AskForFileToSave(AppConstants.Captions.SaveLogToFile, Constants.Filter.CSV_FILE_FILTER, Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(file)) return;

         var dataTable = new DataTable();

         dataTable.Columns.Add(AppConstants.Captions.Type, typeof (string));
         dataTable.Columns.Add(AppConstants.Captions.ObjectType, typeof (string));
         dataTable.Columns.Add(AppConstants.Captions.ObjectName, typeof (string));
         dataTable.Columns.Add(AppConstants.Captions.BuildingBlockType, typeof (string));
         dataTable.Columns.Add(AppConstants.Captions.BuildingBlockName, typeof (string));
         dataTable.Columns.Add(AppConstants.Captions.Message, typeof (string));
         dataTable.Columns.Add(AppConstants.Captions.MessageOrigin, typeof(string));


         foreach (var message in _allNotifications.Where(x => isShowing(x.Type)))
         {
            var newRow = dataTable.NewRow();
            newRow[AppConstants.Captions.Type] = message.Type;
            newRow[AppConstants.Captions.ObjectType] = message.ObjectType;
            newRow[AppConstants.Captions.ObjectName] = message.ObjectName;
            newRow[AppConstants.Captions.BuildingBlockType] = message.BuildingBlockType;
            newRow[AppConstants.Captions.BuildingBlockName] = message.BuildingBlockName;
            newRow[AppConstants.Captions.Message] = message.NotificationMessage + Environment.NewLine + message.Details.ToString(Environment.NewLine);
            newRow[AppConstants.Captions.MessageOrigin] = Enum.GetName(typeof(MessageOrigin), message.MessageOrigin);
            dataTable.Rows.Add(newRow);
         }

         dataTable.ExportToCSV(file);
      }

      private bool isShowing(NotificationType notificationType)
      {
         return VisibleNotification.Is(notificationType);
      }

      public void Toggle(NotificationType typeToToggle)
      {
         VisibleNotification = VisibleNotification ^ typeToToggle;
      }

      public void Handle(ShowValidationResultsEvent validationResultsEvent)
      {
         updateNotificationWith(validationResultsEvent.ValidationResult.Messages.Select(m => _notificationMessageMapper.MapFrom(m)));
      }

      private void updateNotificationWith(IEnumerable<NotificationMessageDTO> notificationMessages)
      {
         _allNotifications = new NotifyList<NotificationMessageDTO>(_allNotifications);

     
         notificationMessages.Where(notificationIsViible).Each(m =>
         {
            if (!_allNotifications.Contains(m))
               _allNotifications.Add(m);
         });

         bindToView();
      }

      private bool notificationIsViible(NotificationMessageDTO message)
      {
         return _userSettings.ShowPKSimObserverMessages || !isPKSimObserverMessage(message);
      }

      public void Handle(FormulaValidEvent eventToHandle)
      {
         var notificationMessage = _notificationMessageMapper.MapFrom(eventToHandle.Formula, eventToHandle.BuildingBlock);
         removeNotification(notificationMessage);
         updateNotificationCount();
      }

      private void removeNotification(NotificationMessageDTO notificationMessage)
      {
         if (_allNotifications.Contains(notificationMessage))
            _allNotifications.Remove(notificationMessage);
      }

      public void Handle(FormulaInvalidEvent eventToHandle)
      {
         var notificationMessageDTO = _notificationMessageMapper.MapFrom(eventToHandle.Formula, eventToHandle.BuildingBlock);
         notificationMessageDTO.NotificationMessage.Message = eventToHandle.Message;
         if (_allNotifications.Contains(notificationMessageDTO))
            _allNotifications[_allNotifications.IndexOf(notificationMessageDTO)].NotificationMessage.Message = eventToHandle.Message;
         else
            _allNotifications.Add(notificationMessageDTO);

         updateNotificationCount();
      }

      public void Handle(ProjectClosedEvent eventToHandle)
      {
         ClearAllNotification();
      }

      public void ShowContextMenu(IViewItem notificationItem, Point popupLocation)
      {
         var contextMenu = _viewItemContextMenuFactory.CreateFor(notificationItem, this);
         contextMenu.Show(_view, popupLocation);
      }

      public void Handle(ShowNotificationsEvent eventToHandle)
      {
         updateNotificationWith(eventToHandle.NotificationMessages.Select(x => new NotificationMessageDTO(x)));
      }

      public void Handle(RemovedEvent eventToHandle)
      {
         notificationsReferencing(eventToHandle.RemovedObjects.ToList()).Each(removeNotification);
         updateNotificationCount();
      }

      private IEnumerable<NotificationMessageDTO> notificationsReferencing(IReadOnlyList<IObjectBase> possibleObjects)
      {
         return _allNotifications.Where(n => possibleObjects.Contains(n.Object) || possibleObjects.Contains(n.BuildingBlock)).ToList();
      }
   }

   public class NotificationMessageDTO : IViewItem
   {
      private readonly NotificationMessage _notificationMessage;

      public NotificationMessageDTO(NotificationMessage notificationMessage)
      {
         _notificationMessage = notificationMessage;
      }

      public Image Image => _notificationMessage.Image;
      public NotificationType Type => _notificationMessage.Type;
      public NotificationMessage NotificationMessage => _notificationMessage;
      public IObjectBase Object => _notificationMessage.Object;
      public IObjectBase BuildingBlock => _notificationMessage.BuildingBlock;
      public string ObjectType => _notificationMessage.ObjectType;
      public string ObjectName => _notificationMessage.ObjectName;
      public string BuildingBlockType => _notificationMessage.BuildingBlockType;
      public string BuildingBlockName => _notificationMessage.BuildingBlockName;
      public MessageOrigin MessageOrigin => _notificationMessage.MessageOrigin;
      public IEnumerable<string> Details => _notificationMessage.Details;

      public override int GetHashCode()
      {
         return NotificationMessage.GetHashCode();
      }

      public override bool Equals(object obj)
      {
         var otherDTO = obj as NotificationMessageDTO;
         if (otherDTO == null)
            return false;

         return otherDTO.NotificationMessage.Equals(NotificationMessage);
      }
   }
}