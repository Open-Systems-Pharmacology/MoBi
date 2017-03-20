using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Assets;

namespace MoBi.Presentation.Presenter
{
   public interface IValidationMessagesPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Show validation message. Returns false if the user cancels the action otherwise true
      /// </summary>
      /// <param name="validationResult">Validation data</param>
      bool ShowMessages(ValidationResult validationResult);

      void SaveLogToFile();

      void FilterChanged();
      int ImageIndexFor(ValidationMessage validationMessage);
   }

   public class ValidationMessagesPresenter : AbstractDisposablePresenter<IValidationMessagesView, IValidationMessagesPresenter>, IValidationMessagesPresenter
   {
      private readonly IDialogCreator _dialogCreator;
      private readonly LogFilterDTO _logFilter;
      private ValidationResult _validationResult;

      public ValidationMessagesPresenter(IValidationMessagesView view,  IDialogCreator dialogCreator) : base(view)
      {
         _dialogCreator = dialogCreator;
         _logFilter = new LogFilterDTO();
      }

      public bool ShowMessages(ValidationResult validationResult)
      {
         _validationResult = validationResult;
         if (validationResult.ValidationState.Equals(ValidationState.Invalid))
         {
            _logFilter.Error = true;
            _view.OkEnabled = false;
         }
         _view.BindToFilter(_logFilter);
         updateView();
         _view.Display();
         return !_view.Canceled;
      }

      private void updateView()
      {
         _view.ShowMessages(messagesToDisplay());
      }

      private IEnumerable<ValidationMessage> messagesToDisplay()
      {
         var allMessages = _validationResult.Messages.OrderByDescending(message => message.NotificationType).ToList();
         var allMessagesToDisplay = new List<ValidationMessage>();
         if (_logFilter.Error)
            addMessageOfType(allMessagesToDisplay, allMessages, NotificationType.Error);

         if (_logFilter.Warning)
            addMessageOfType(allMessagesToDisplay, allMessages, NotificationType.Warning);

         return allMessagesToDisplay;
      }

      private void addMessageOfType(List<ValidationMessage> allMessagesToDisplay, IEnumerable<ValidationMessage> allMessages, NotificationType type)
      {
         allMessagesToDisplay.AddRange(allMessages.Where(m => m.NotificationType == type));
      }

      public void SaveLogToFile()
      {
         var file = _dialogCreator.AskForFileToSave(AppConstants.Captions.SaveLogToFile, Constants.Filter.CSV_FILE_FILTER, Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(file)) return;

         var dataTable = new DataTable();
         //Create one column for the parameter path
         dataTable.Columns.Add("Status", typeof (string));
         dataTable.Columns.Add("Message", typeof (string));
         foreach (var message in messagesToDisplay())
         {
            var newRow = dataTable.NewRow();
            newRow[0] = message.NotificationType;
            newRow[1] = message.Text;
            dataTable.Rows.Add(newRow);
         }
         try
         {
            dataTable.ExportToCSV(file);
         }
         catch (IOException exception)
         {
            _dialogCreator.MessageBoxError(exception.Message);
         }
      }

      public void FilterChanged()
      {
         updateView();
      }

      public int ImageIndexFor(ValidationMessage validationMessage)
      {
         if (validationMessage.NotificationType == NotificationType.Error)
            return ApplicationIcons.IconIndex(ApplicationIcons.Error);

         return ApplicationIcons.IconIndex(ApplicationIcons.Warning);
      }
   }
}