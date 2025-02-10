using System.Collections.Generic;
using System.Data;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using MoBi.Presentation.DTO;
using OSPSuite.Infrastructure.Import.Services;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IImportQuantitiesPresenter : IDisposablePresenter
   {
      /// <summary>
      /// Begins the reading of the file and importing of related quantities
      /// </summary>
      void StartImport();

      /// <summary>
      /// Sets the file path that will be used for import
      /// </summary>
      void SelectFile();

      /// <summary>
      /// Issues commands to add quantities to the project
      /// </summary>
      void TransferImportedQuantities();
   }

   public abstract class AbstractQuantitiesImporterPresenter : AbstractDisposableCommandCollectorPresenter<IImportQuantityView, IImportQuantitiesPresenter>, IImportQuantitiesPresenter
   {
      protected QuantityImporterDTO _quantityImporterDTO;
      protected ImportExcelSheetSelectionDTO _importExcelSheetSelectionDTO;
      protected IDialogCreator _dialogCreator;
      protected IImportFromExcelTask _excelTask;
      protected INotifyList<ImportedQuantityDTO> _quantityDTOs;

      protected AbstractQuantitiesImporterPresenter(IImportQuantityView view, IDialogCreator dialogCreator, IImportFromExcelTask excelTask)
         : base(view)
      {
         _dialogCreator = dialogCreator;
         _excelTask = excelTask;
      }

      public void SelectFile()
      {
         var file = _dialogCreator.AskForFileToOpen(AppConstants.BrowseForFile, Constants.Filter.EXCEL_OPEN_FILE_FILTER, Constants.DirectoryKey.XLS_IMPORT);
         if (string.IsNullOrEmpty(file)) return;

         updateDTOFromSelectedFile(file);
      }

      private void updateDTOFromSelectedFile(string file)
      {
         _importExcelSheetSelectionDTO.FilePath = file;
         var sheets = _excelTask.RetrieveExcelSheets(_importExcelSheetSelectionDTO.FilePath, excludeEmptySheets: true).ToList();
         _importExcelSheetSelectionDTO.AllSheetNames = sheets;
         _importExcelSheetSelectionDTO.SelectedSheet = sheets.FirstOrDefault();

         if (!sheets.Any())
            _importExcelSheetSelectionDTO.Messages = new List<string> { AppConstants.Exceptions.CannotImportFromExcelFile(_importExcelSheetSelectionDTO.FilePath) };

         _view.UpdateSheetList();
      }

      protected void UpdateLog(IEnumerable<string> importLog)
      {
         _importExcelSheetSelectionDTO.Messages = importLog;
      }

      private DataTable importedDataTableFromFile()
      {
         return _excelTask.GetDataTables(_importExcelSheetSelectionDTO.FilePath, _importExcelSheetSelectionDTO.SelectedSheet, firstRowAsCaption: true);
      }

      protected abstract QuantityImporterDTO ConvertTableToImportedQuantities(DataTable table);

      public void StartImport()
      {
         var tables = importedDataTableFromFile();
         _quantityImporterDTO = ConvertTableToImportedQuantities(tables);

         _view.BindTo(_quantityImporterDTO);
         UpdateLog(_quantityImporterDTO.Log);

         _quantityDTOs = GetQuantitiesFromDTO(_quantityImporterDTO.DowncastTo<QuantityImporterDTO>());
      }

      protected NotifyList<ImportedQuantityDTO> GetQuantitiesFromDTO(QuantityImporterDTO quantityImporterDTO)
      {
         return quantityImporterDTO.QuantityDTOs;
      }

      public abstract void TransferImportedQuantities();
   }
}