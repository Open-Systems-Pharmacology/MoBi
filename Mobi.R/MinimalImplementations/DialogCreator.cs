using System;
using System.Collections.Generic;
using OSPSuite.Core.Services;

namespace MoBi.R.MinimalImplementations
{
   public class DialogCreator : IDialogCreator
   {
      public void MessageBoxError(string message) => Console.WriteLine(message);

      public ViewResult MessageBoxYesNoCancel(string message, ViewResult defaultButton) => ViewResult.Yes;

      public ViewResult MessageBoxYesNoCancel(string message, string yes, string no, string cancel, ViewResult defaultButton) => ViewResult.Yes;

      public ViewResult MessageBoxYesNo(string message, ViewResult defaultButton) => ViewResult.Yes;
      public ViewResult MessageBoxConfirm(string message, Action doNotShowAgain, ViewResult defaultButton = ViewResult.Yes) => ViewResult.Yes;

      public ViewResult MessageBoxYesNo(string message, string yes, string no, ViewResult defaultButton) => ViewResult.Yes;

      public void MessageBoxInfo(string message) => Console.WriteLine(message);

      public string AskForFileToOpen(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null)
      {
         return string.Empty;
      }

      public string AskForFileToSave(string title, string filter, string directoryKey, string defaultFileName = null, string defaultDirectory = null)
      {
         return string.Empty;
      }

      public string AskForFolder(string title, string directoryKey, string defaultDirectory = null)
      {
         return string.Empty;
      }

      public string AskForInput(string caption, string text, string defaultValue = null, IEnumerable<string> forbiddenValues = null, IEnumerable<string> predefinedValues = null, string iconName = null)
      {
         return string.Empty;
      }
   }
}