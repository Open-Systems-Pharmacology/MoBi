using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Chart;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   internal class ChartsReporter : OSPSuiteTeXReporter<IEnumerable<ICurveChart>>
   {
      public override IReadOnlyCollection<object> Report(IEnumerable<ICurveChart> charts, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();

         if (charts != null && charts.Any())
         {
            listToReport.Add(new Chapter(Constants.CHARTS));
            foreach (var chart in charts)
            {
               if (chart.Curves.Count == 0) continue;
               if (!String.IsNullOrEmpty(chart.Name))
                  listToReport.Add(new Section(chart.Name));
               listToReport.Add(chart);
            }
         }

         return listToReport;
      }
   }
}