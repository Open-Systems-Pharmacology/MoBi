using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using MoBi.Core.Domain.Model;
using MoBi.Core.Services;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   internal class SimulationReporter : OSPSuiteTeXReporter<IMoBiSimulation>
   {
      private readonly IDiagramModelToImageTask _diagramModelToImageTask;
      private readonly SimulationConfigurationReporter _simulationConfigurationReporter;

      public SimulationReporter(IDiagramModelToImageTask diagramModelToImageTask, SimulationConfigurationReporter simulationConfigurationReporter)
      {
         _diagramModelToImageTask = diagramModelToImageTask;
         _simulationConfigurationReporter = simulationConfigurationReporter;
      }

      public override IReadOnlyCollection<object> Report(IMoBiSimulation simulation, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         var simulationChapter = new Chapter(simulation.Name);
         listToReport.Add(simulationChapter);
         buildTracker.Track(simulationChapter);
         listToReport.AddRange(this.ReportDescription(simulation, buildTracker));

         if (buildTracker.Settings.Verbose)
         {
            //model image
            var fileName = $"{simulation.Name}_{Guid.NewGuid()}.png";
            var figure = Figure.ForCreation($"Diagram of simulation  {simulation.Name}", fileName, buildTracker);
            listToReport.Add(new Text("The {0} shows the diagram of the simulation.", new Reference(figure)));
            listToReport.Add(figure);
            _diagramModelToImageTask.ExportTo(simulation, figure.FullPath);
         }

         listToReport.AddRange(_simulationConfigurationReporter.Report(simulation.Configuration, buildTracker));

         if (simulationHasChartWithCurves(simulation))
         {
            listToReport.Add(new Section(Constants.RESULTS));
            listToReport.Add(new SubSection(Constants.CHART));
            listToReport.Add(simulation.Chart);
         }
         return listToReport;
      }

      private bool simulationHasChartWithCurves(IMoBiSimulation simulation)
      {
         return simulation.Chart != null && simulation.Chart.Curves.Any();
      }
   }
}