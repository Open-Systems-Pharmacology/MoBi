using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IImportQuantityView : IModalView<IImportQuantitiesPresenter>
   {
      /// <summary>
      /// Binds the view to the DTO
      /// </summary>
      /// <param name="importFileSelectionDTO">The file selection DTO to be bound</param>
      void BindTo(ImportExcelSheetSelectionDTO importFileSelectionDTO);

      /// <summary>
      /// Sets the window caption
      /// </summary>
      string Text { get; set; }

      /// <summary>
      /// Sets the hint label for the window
      /// </summary>
      string HintLabel { get; set; }

      void BindTo(QuantityImporterDTO quantityImporterDTO);

      /// <summary>
      /// Updates the list of possible worksheets shown by the view
      /// </summary>
      void UpdateSheetList();
   }
}
