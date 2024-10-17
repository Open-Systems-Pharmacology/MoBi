using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditSimulationView : IMdiChildView<IEditSimulationPresenter>
   {
      void SetEditView(IView view);
      void SetTreeView(IView view);
      void SetChartView(IChartView chartView);
      void SetModelDiagram(ISimulationDiagramView subView);

      /// <summary>
      ///    Indicates whether the current view is the results view
      /// </summary>
      bool ShowsResults { get; }

      /// <summary>
      ///    Changes the displayed view to the results view
      /// </summary>
      void ShowResultsTab();

      /// <summary>
      ///    Sets the outputMappingView to the corresponding tab
      /// </summary>
      void SetDataView(ISimulationOutputMappingView view);

      void SetPredictedVsObservedView(ISimulationVsObservedDataView view);
      void SetResidualsVsTimeView(ISimulationVsObservedDataView view);
      void ShowChangesTab();
      void SetChangesView(ISimulationChangesView view);
   }
}