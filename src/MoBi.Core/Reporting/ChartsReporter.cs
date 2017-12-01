using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Infrastructure.Reporting;
using OSPSuite.TeXReporting.Items;

namespace MoBi.Core.Reporting
{
   internal class ChartsReporter : OSPSuiteTeXReporter<IEnumerable<CurveChart>>
   {
      public override IReadOnlyCollection<object> Report(IEnumerable<CurveChart> charts, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();

         if (charts == null || !charts.Any()) return listToReport;

         listToReport.Add(new Chapter(Constants.CHARTS));
         foreach (var chart in charts)
         {
            if (chart.Curves.Count == 0) continue;
            if (!string.IsNullOrEmpty(chart.Name))
               listToReport.Add(new Section(chart.Name));
            listToReport.Add(chart);
         }

         return listToReport;
      }
   }
}