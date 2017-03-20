using System;
using System.Collections.Generic;
using System.Linq;
using MoBi.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter.BasePresenter;
using MoBi.Presentation.Views;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public interface IExportSelectedQuantitiesPresenter : IDisposablePresenter
   {
      /// <summary>
      ///    Starts the selection process. Returns a tuple containg the path as first argument and the selected data column as
      ///    second argument.
      ///    Returns null if the selection was canceled
      /// </summary>
      Tuple<string, IEnumerable<DataColumn>> ExportQuantities(IMoBiSimulation simulation, DataRepository dataRepository);

      void SelectReportFile();
   }

   public class ExportSelectedQuantitiesPresenter : MoBiDisposablePresenter<IExportSelectedQuantitiesView, IExportSelectedQuantitiesPresenter>, IExportSelectedQuantitiesPresenter
   {
      private readonly IQuantitySelectionPresenter _quantitySelectionPresenter;
      private readonly IDialogCreator _dialogCreator;
      private readonly ExportQuantitiesSelectionDTO _exportQuantitiesSelectionDTO;

      public ExportSelectedQuantitiesPresenter(IExportSelectedQuantitiesView view, IQuantitySelectionPresenter quantitySelectionPresenter, IDialogCreator dialogCreator) : base(view)
      {
         _quantitySelectionPresenter = quantitySelectionPresenter;
         _dialogCreator = dialogCreator;
         AddSubPresenters(_quantitySelectionPresenter);
         View.AddSelectionView(_quantitySelectionPresenter.BaseView);
         _quantitySelectionPresenter.Description = AppConstants.Captions.ExportSelectedObservedDataDescription;
         _exportQuantitiesSelectionDTO = new ExportQuantitiesSelectionDTO();
         _view.BindTo(_exportQuantitiesSelectionDTO);
         ViewChanged();
      }

      public override void ViewChanged()
      {
         int numberOfSelectedMolecules = _quantitySelectionPresenter.NumberOfSelectedQuantities;
         _quantitySelectionPresenter.Info = AppConstants.Captions.NumberOfExportedCurveIs(numberOfSelectedMolecules);
         View.OkEnabled = CanClose;
      }

      public override bool CanClose
      {
         get { return base.CanClose && _quantitySelectionPresenter.HasSelection; }
      }

      public Tuple<string, IEnumerable<DataColumn>> ExportQuantities(IMoBiSimulation simulation, DataRepository dataRepository)
      {
         _quantitySelectionPresenter.Edit(simulation.Model.Root);

         View.Display();
         if (View.Canceled)
            return null;

         return new Tuple<string, IEnumerable<DataColumn>>(_exportQuantitiesSelectionDTO.ReportFile, selectedCurves(dataRepository));
      }

      private IEnumerable<DataColumn> selectedCurves(DataRepository dataRepository)
      {
         var selectedQuantities = new Cache<string, QuantitySelection>(x => x.Path);
         selectedQuantities.AddRange(_quantitySelectionPresenter.SelectedQuantities());

         foreach (var dataColumn in dataRepository.Where(c => c.QuantityInfo != null))
         {
            var path = dataColumn.QuantityInfo.PathAsString;
            if (selectedQuantities.Contains(path))
               yield return dataColumn;
         }
      }

      public void SelectReportFile()
      {
         var reportFile = _dialogCreator.AskForFileToSave(AppConstants.Dialog.ExportSimulationResultsToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(reportFile)) return;
         _exportQuantitiesSelectionDTO.ReportFile = reportFile;
      }
   }
}