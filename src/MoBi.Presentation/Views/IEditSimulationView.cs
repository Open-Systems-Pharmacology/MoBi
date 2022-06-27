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
      /// Indicates whether or not the current view is the results view
      /// </summary>
      bool ShowsResults { get; }

      /// <summary>
      /// Changes the displayed view to the results view
      /// </summary>
      void ShowResultsTab();

      void SetDataView(ISimulationOutputMappingView view);
   }
}