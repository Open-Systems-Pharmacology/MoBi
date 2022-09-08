using System.Collections.Generic;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using MoBi.Assets;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Core.Domain;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;
using IValidationMessagesView = MoBi.Presentation.Views.IValidationMessagesView;

namespace MoBi.UI.Views
{
   public partial class ValidationMessagesView : BaseModalView, IValidationMessagesView
   {
      private readonly IImageListRetriever _imageListRetriever;
      private IValidationMessagesPresenter _presenter;
      private readonly GridViewBinder<ValidationMessage> _gridViewBinder;
      private readonly ScreenBinder<LogFilterDTO> _screenBinder;
      private readonly UxRepositoryItemImageComboBox _statusRepository;

      public ValidationMessagesView(IImageListRetriever imageListRetriever)
      {
         _imageListRetriever = imageListRetriever;
         InitializeComponent();
         gridView.AllowsFiltering = false;
         gridView.ShowRowIndicator = false;
         gridView.ShouldUseColorForDisabledCell = false;
         _screenBinder = new ScreenBinder<LogFilterDTO>();
         _gridViewBinder = new GridViewBinder<ValidationMessage>(gridView);
         _statusRepository = new UxRepositoryItemImageComboBox(gridView, _imageListRetriever);
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.NotificationType)
            .WithRepository(configureStatusRepository)
            .WithCaption("Type")
            .WithFixedWidth(UIConstants.STATUS_WIDTH)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Text)
            .WithCaption("Message")
            .AsReadOnly();

         _screenBinder.Bind(x => x.Error).To(chkError);
         _screenBinder.Bind(x => x.Warning).To(chkWarning);

         btnSaveLog.Click += (o, e) => this.DoWithinExceptionHandler(() => _presenter.SaveLogToFile());
         _screenBinder.Changed += () => this.DoWithinExceptionHandler(() => _presenter.FilterChanged());
      }

      private RepositoryItem configureStatusRepository(ValidationMessage validationMessage)
      {
         _statusRepository.Items.Clear();
         _statusRepository.Items.Add(new ImageComboBoxItem(validationMessage.NotificationType, _presenter.ImageIndexFor(validationMessage)));
         return _statusRepository;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Text = AppConstants.Captions.ValidationMessages;
         chkError.Text = AppConstants.Captions.Error;
         chkWarning.Text = AppConstants.Captions.Warning;
         btnSaveLog.Text = AppConstants.Captions.SaveLog;
         btnSaveLog.Image = ApplicationIcons.Save.ToImage(IconSizes.Size16x16);
         btnSaveLog.ImageLocation = ImageLocation.MiddleLeft;
         layoutItemSaveLog.AdjustButtonSize(layoutControl);
      }
         
      public void AttachPresenter(IValidationMessagesPresenter presenter)
      {
         _presenter = presenter;
      }

      public void ShowMessages(IEnumerable<ValidationMessage> messages)
      {
         _gridViewBinder.BindToSource(messages);
      }

      public void BindToFilter(LogFilterDTO logStatusFilter)
      {
         _screenBinder.BindToSource(logStatusFilter);
      }
   }
}