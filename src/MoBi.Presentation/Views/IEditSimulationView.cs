using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditSimulationView : IMdiChildView<IEditSimulationPresenter>
   {
      void SetEditView(IView view);
      void SetTreeView(IView view);
      void SetModelDiagram(ISimulationDiagramView subView);

      /// <summary>
      ///    Indicates whether the current view is an analysis tab
      /// </summary>
      bool ShowsResults { get; }

      /// <summary>
      ///    Selects the first analysis tab if one exists
      /// </summary>
      void ShowResultsTab();

      /// <summary>
      ///    Sets the outputMappingView to the corresponding tab
      /// </summary>
      void SetDataView(ISimulationOutputMappingView view);

      void ShowChangesTab();
      void SetChangesView(ISimulationChangesView view);
      void SetParametersTabEnabled(bool enabled);

      /// <summary>
      ///    Adds a new analysis tab for the given presenter
      /// </summary>
      void AddAnalysis(ISimulationAnalysisPresenter analysisPresenter);

      /// <summary>
      ///    Removes the analysis tab for the given presenter
      /// </summary>
      void RemoveAnalysis(ISimulationAnalysisPresenter analysisPresenter);
   }
}