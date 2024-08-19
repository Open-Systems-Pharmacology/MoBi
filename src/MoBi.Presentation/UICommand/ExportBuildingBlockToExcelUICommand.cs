using System.Collections.Generic;
using System.Data;
using MoBi.Assets;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Export;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace MoBi.Presentation.UICommand
{
   public abstract class ExportBuildingBlockToExcelUICommand<T> : ObjectUICommand<T> where T : class, IBuildingBlock
   {
      private readonly IMoBiProjectRetriever _projectRetriever;
      private readonly IDialogCreator _dialogCreator;
      private readonly IMapper<T, List<DataTable>> _mapper;

      protected ExportBuildingBlockToExcelUICommand(
         IMoBiProjectRetriever projectRetriever,
         IDialogCreator dialogCreator,
         IMapper<T, List<DataTable>> mapper)
      {
         _projectRetriever = projectRetriever;
         _dialogCreator = dialogCreator;
         _mapper = mapper;
      }

      protected override void PerformExecute()
      {
         var currentProject = _projectRetriever.CurrentProject;
         var projectName = currentProject.Name;
         if (string.IsNullOrEmpty(projectName))
            projectName = AppConstants.Undefined;

         var defaultFileName = AppConstants.DefaultFileNameForBuildingBlockExport(projectName, Subject.Name);
         var excelFileName = _dialogCreator.AskForFileToSave(AppConstants.Captions.ExportToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, defaultFileName);

         if (excelFileName.IsNullOrEmpty())
            return;

         var mappedValues = _mapper.MapFrom(Subject);
         ExportToExcelTask.ExportDataTablesToExcel(mappedValues, excelFileName, openExcel: true);
      }
   }
}