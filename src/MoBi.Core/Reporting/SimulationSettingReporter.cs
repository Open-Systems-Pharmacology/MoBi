using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using MoBi.Assets;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   internal class SimulationSettingReporter : OSPSuiteTeXReporter<SimulationSettings>
   {
      public override IReadOnlyCollection<object> Report(SimulationSettings simulationSettings, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();

         if (noChapterCreatedYet(buildTracker))
            listToReport.Add(new Chapter(Constants.SIMULATION_SETTINGS));

         listToReport.Add(new Section(string.Format(Constants.BUILDING_BLOCK_PROMPT_FORMAT, Constants.SIMULATION_SETTINGS, simulationSettings.Name)));
         listToReport.AddRange(this.ReportDescription(simulationSettings, buildTracker));

         listToReport.Add(new SubSection(AppConstants.Captions.SolverSettings));
         listToReport.Add(tableFor(simulationSettings.Solver));
         listToReport.Add(new SubSection(AppConstants.Captions.OutputIntervals));
         listToReport.Add(tableFor(simulationSettings.OutputSchema));
         return listToReport;
      }

      private static bool noChapterCreatedYet(OSPSuiteTracker buildTracker)
      {
         return !buildTracker.TrackedObjects.Any(s => s.IsAnImplementationOf<Chapter>());
      }

      private DataTable tableFor(SolverSettings solver)
      {
         var dt = new DataTable(AppConstants.Captions.SolverSettings);
         dt.AddColumn(AppConstants.Captions.Name);
         dt.AddColumn(AppConstants.Captions.Value);

         dt.BeginLoadData();

         addRowFor(dt, OSPSuite.Core.Domain.Constants.Parameters.ABS_TOL, solver.AbsTol.ToString(CultureInfo.InvariantCulture));
         addRowFor(dt, OSPSuite.Core.Domain.Constants.Parameters.REL_TOL, solver.RelTol.ToString(CultureInfo.InvariantCulture));
         addRowFor(dt, OSPSuite.Core.Domain.Constants.Parameters.H0, solver.H0.ToString(CultureInfo.InvariantCulture));
         addRowFor(dt, OSPSuite.Core.Domain.Constants.Parameters.H_MIN, solver.HMin.ToString(CultureInfo.InvariantCulture));
         addRowFor(dt, OSPSuite.Core.Domain.Constants.Parameters.H_MAX, solver.HMax.ToString(CultureInfo.InvariantCulture));
         addRowFor(dt, OSPSuite.Core.Domain.Constants.Parameters.MX_STEP, solver.MxStep.ToString(CultureInfo.InvariantCulture));
         addRowFor(dt, OSPSuite.Core.Domain.Constants.Parameters.USE_JACOBIAN, solver.UseJacobian.ToString());

         
         dt.EndLoadData();
         dt.AcceptChanges();

         return dt;
      }

      private void addRowFor(DataTable dt, string name, string value)
      {
         var row = dt.NewRow();
         row[AppConstants.Captions.Name] = name;
         row[AppConstants.Captions.Value] = value;
         dt.Rows.Add(row);
      }

      private DataTable tableFor(OutputSchema schema)
      {
         var dt = new DataTable(AppConstants.Captions.OutputIntervals);

         dt.AddColumn(AppConstants.Captions.StartTime);
         dt.AddColumn(AppConstants.Captions.EndTime);
         dt.AddColumn(AppConstants.Captions.Resolution);

         dt.BeginLoadData();
         foreach (var interval in schema.Intervals)
         {
            var row = dt.NewRow();
            row[AppConstants.Captions.StartTime] = formatValue(interval.StartTime);
            row[AppConstants.Captions.EndTime] = formatValue(interval.EndTime);
            row[AppConstants.Captions.Resolution] = formatValue(interval.Resolution);
            dt.Rows.Add(row);
         }
         dt.EndLoadData();
         dt.AcceptChanges();
         return dt;
      }

      private string formatValue(IParameter parameter)
      {
         return formatValue(parameter.ValueInDisplayUnit, parameter.DisplayUnit.Name);
      }

      private string formatValue(double value, string unit)
      {
         return string.Format("{0} {1}", value, unit);
      }
   
   }
}
