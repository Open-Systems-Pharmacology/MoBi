using System.Collections.Generic;
using System.Windows.Forms;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using MoBi.Assets;
using MoBi.Core;
using MoBi.Presentation.Presenter.Main;
using MoBi.Presentation.Views;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using IToolTipCreator = MoBi.UI.Services.IToolTipCreator;

namespace MoBi.UI.Views
{
   public partial class NotificationView : BaseUserControl, INotificationView, IViewWithPopup
   {
      private readonly IToolTipCreator _toolTipCreator;
      private INotificationPresenter _presenter;
      private readonly ICache<NotificationType, BarButtonItem> _buttonCache;
      private readonly GridViewBinder<NotificationMessageDTO> _gridViewBinder;
      private readonly RepositoryItemPictureEdit _statusIconRepository;
      private readonly BarManager _popupBarManager;
      private readonly IStartOptions _runOptions;

      public NotificationView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator, IStartOptions runOptions)
      {
         _toolTipCreator = toolTipCreator;
         _runOptions = runOptions;
         InitializeComponent();
         _barManager.Images = imageListRetriever.AllImages16x16;
         _popupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImages16x16};
         _buttonCache = new Cache<NotificationType, BarButtonItem>();
         _gridViewBinder = new GridViewBinder<NotificationMessageDTO>(gridViewMessages);
         gridViewMessages.CustomRowFilter += customRowFilter;
         _statusIconRepository = new RepositoryItemPictureEdit();
         var toolTipController = new ToolTipController {ImageList = imageListRetriever.AllImages16x16};
         toolTipController.AutoPopDelay = AppConstants.NotificationToolTipDelay;
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;
         gridMessages.ToolTipController = toolTipController;
         gridViewMessages.MouseDown += (o, e) => this.DoWithinExceptionHandler(() => onGridViewMouseDown(e));
         gridViewMessages.DoubleClick += (o, e) => this.DoWithinExceptionHandler(onDoubleClick);
         gridViewMessages.ShouldUseColorForDisabledCell = false;
      }

      private void customRowFilter(object sender, RowFilterEventArgs e)
      {
         var notification = _gridViewBinder.SourceElementAt(e.ListSourceRow);
         if (notification == null) return;
         e.Visible = _presenter.ShouldShow(notification);

         //handle filter only if we explicitly hide the record
         if (!e.Visible)
            e.Handled = true;
      }

      private void onGridViewMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right) return;

         var rowHandle = gridViewMessages.RowHandleAt(e);
         _presenter.ShowContextMenu(_gridViewBinder.ElementAt(rowHandle), e.Location);
      }

      public void AttachPresenter(INotificationPresenter presenter)
      {
         _presenter = presenter;
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != gridViewMessages.GridControl) return;

         var hi = gridViewMessages.CalcHitInfo(e.ControlMousePosition);
         if (hi.Column == null) return;
         if (!hi.InRowCell) return;

         var notification = _gridViewBinder.ElementAt(hi.RowHandle);
         if (notification == null) return;

         //check if subclass want to display a tool tip as well
         var superToolTip = _toolTipCreator.ToolTipFor(notification.NotificationMessage);
         if (superToolTip == null)
            return;

         //An object that uniquely identifies a row cell
         e.Info = new ToolTipControlInfo(notification, string.Empty) {SuperTip = superToolTip, ToolTipType = ToolTipType.SuperTip};
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.Image)
            .WithCaption(AppConstants.Captions.EmptyColumn)
            .WithRepository(x => _statusIconRepository)
            .WithFixedWidth(OSPSuite.UI.UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _gridViewBinder.AutoBind(x => x.ObjectType)
            .WithCaption(AppConstants.Captions.ObjectType)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.ObjectName)
            .WithCaption(AppConstants.Captions.ObjectName)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.BuildingBlockType)
            .WithCaption(AppConstants.Captions.BuildingBlockType)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.BuildingBlockName)
            .WithCaption(AppConstants.Captions.BuildingBlockName)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.NotificationMessage)
            .WithCaption(AppConstants.Captions.Message)
            .AsReadOnly();
         _gridViewBinder.AutoBind(x => x.MessageOrigin)
            .WithCaption(AppConstants.Captions.MessageOrigin)
            .AsReadOnly();
      }

      public void Show(NotificationType type, bool visible)
      {
         _buttonCache[type].Down = visible;
      }

      public void BindTo(NotifyList<NotificationMessageDTO> notifications)
      {
         _gridViewBinder.BindToSource(notifications);
         gridViewMessages.BestFitColumns();
      }

      public void SetNotificationCount(NotificationType notificationType, int count)
      {
         var button = _buttonCache[notificationType];
         button.Caption = string.Format("{0} {1}", count, button.Tag);
      }

      private void onDoubleClick()
      {
         this.DoWithinExceptionHandler(() => _presenter.GoToObject(_gridViewBinder.FocusedElement));
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         initializeButton(btnErrors, NotificationType.Error, ApplicationIcons.Error, AppConstants.Captions.Errors);
         initializeButton(btnMessages, NotificationType.Info, ApplicationIcons.Info, AppConstants.Captions.Messages);
         initializeButton(btnWarnings, NotificationType.Warning, ApplicationIcons.Warning, AppConstants.Captions.Warnings);
         initializeButton(btnDebug, NotificationType.Debug, ApplicationIcons.Debug, AppConstants.Captions.Debug);

         if (!_runOptions.IsDeveloperMode)
            btnDebug.Visibility = BarItemVisibility.Never;

         btnExportToFile.ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.Save);
         btnExportToFile.PaintStyle = BarItemPaintStyle.CaptionGlyph;
         btnExportToFile.Caption = AppConstants.Captions.SaveToFile;
         btnExportToFile.ItemClick += (o, e) => this.DoWithinExceptionHandler(() => _presenter.ExportToFile());
      }

      private void initializeButton(BarButtonItem barButtonItem, NotificationType notificationType, ApplicationIcon icon, string caption)
      {
         barButtonItem.Tag = caption;
         barButtonItem.ImageIndex = ApplicationIcons.IconIndex(icon);
         barButtonItem.PaintStyle = BarItemPaintStyle.CaptionGlyph;
         barButtonItem.ButtonStyle = BarButtonStyle.Check;
         barButtonItem.ItemClick += (o, e) => OnEvent(() => toggleView(notificationType));
         _buttonCache.Add(notificationType, barButtonItem);
         SetNotificationCount(notificationType, 0);
      }

      private void toggleView(NotificationType notificationType)
      {
         _presenter.Toggle(notificationType);
         gridViewMessages.RefreshData();
      }

      public BarManager PopupBarManager
      {
         get { return _popupBarManager; }
      }
   }
}