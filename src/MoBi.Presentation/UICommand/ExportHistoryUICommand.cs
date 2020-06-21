using System.IO;
using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace MoBi.Presentation.UICommand
{
  public class ExportHistoryUICommand : IUICommand
   {
      private readonly IMoBiContext _context;
      private readonly IHistoryExportTask _reportTask;
      private readonly IDialogCreator _dialogCreator;

      public ExportHistoryUICommand(IMoBiContext context, IHistoryExportTask reportTask, IDialogCreator dialogCreator)
      {
         _context = context;
         _reportTask = reportTask;
         _dialogCreator = dialogCreator;
      }

      public void Execute()
      {
         var projectName = Path.GetFileNameWithoutExtension(_context.CurrentProject.FilePath);
         if (string.IsNullOrEmpty(projectName))
            projectName = AppConstants.Undefined;

         var reportFileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.ExportHistory, Constants.Filter.HISTORY_FILE_FILTER, Constants.DirectoryKey.REPORT,  projectName);
         if (reportFileName.IsNullOrEmpty()) return;

         var reportOptions = new ReportOptions { ReportFullPath = reportFileName, SheetName = projectName, OpenReport = true };
         _reportTask.CreateReport(_context.HistoryManager, reportOptions);
      }
   }
}