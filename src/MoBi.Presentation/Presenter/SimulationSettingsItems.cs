using System.Collections.Generic;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace MoBi.Presentation.Presenter
{
   public static class SimulationSettingsItems
   {
      public static SimulationSettingsItems<IEditSolverSettingsPresenter> Solver = new SimulationSettingsItems<IEditSolverSettingsPresenter>();
      public static SimulationSettingsItems<IEditOutputSchemaPresenter> OutputSchema = new SimulationSettingsItems<IEditOutputSchemaPresenter>();
      public static SimulationSettingsItems<IEditOutputSelectionsPresenter> OutputSelections = new SimulationSettingsItems<IEditOutputSelectionsPresenter>();
      public static SimulationSettingsItems<IEditChartTemplateManagerPresenter> ChartTemplates = new SimulationSettingsItems<IEditChartTemplateManagerPresenter>();

      public static readonly IReadOnlyList<ISubPresenterItem> All = new List<ISubPresenterItem> {OutputSchema, Solver, OutputSelections, ChartTemplates};
   }

   public class SimulationSettingsItems<TSimulationSettingsItemPresenter> : SubPresenterItem<TSimulationSettingsItemPresenter> where TSimulationSettingsItemPresenter : ISimulationSettingsItemPresenter
   {
   }
}