using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.UICommands;

namespace MoBi.Presentation.UICommand
{
   public class ExportModelPartsToExcelUICommand : ObjectUICommand<IMoBiSimulation>
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IModelPartsToExcelExporterTask _modelPartsToExcelExporterTask;
      private readonly IDialogCreator _dialogCreator;

      public ExportModelPartsToExcelUICommand(IMoBiProjectRetriever projectRetriever, IModelPartsToExcelExporterTask modelPartsToExcelExporterTask, IDialogCreator dialogCreator)
      {
         _projectRetriever = projectRetriever;
         _modelPartsToExcelExporterTask = modelPartsToExcelExporterTask;
         _dialogCreator = dialogCreator;
      }

      protected override void PerformExecute()
      {
         var currentProject = _projectRetriever.CurrentProject;
         var projectName = currentProject.Name;
         if (string.IsNullOrEmpty(projectName))
            projectName = AppConstants.Undefined;

         var defaultFileName = AppConstants.DefaultFileNameForModelPartsExport(projectName, Subject.Name);
         var excelFileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.ExportModelAsTables, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.MODEL_PART,  defaultFileName);
         if (excelFileName.IsNullOrEmpty())
            return;

         _modelPartsToExcelExporterTask.ExportModelPartsToExcelFile(excelFileName, Subject, openExcel: true);
      }
   }
}