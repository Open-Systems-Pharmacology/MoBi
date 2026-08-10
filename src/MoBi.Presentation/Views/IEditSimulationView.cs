using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views.BaseDiagram;
using OSPSuite.Core.Domain;
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
      ///    Adds a new tab for the given analysis showing <paramref name="analysisView" />
      /// </summary>
      void AddAnalysis(ISimulationAnalysis analysis, IView analysisView);

      /// <summary>
      ///    Removes the tab of the given analysis
      /// </summary>
      void RemoveAnalysis(ISimulationAnalysis analysis);

      /// <summary>
      ///    Selects the tab of the given analysis
      /// </summary>
      void SelectAnalysis(ISimulationAnalysis analysis);
   }
}