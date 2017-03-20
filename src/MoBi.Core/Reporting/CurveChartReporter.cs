using System;
using System.Collections.Generic;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Chart;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   internal class CurveChartReporter : OSPSuiteTeXReporter<CurveChart>
   {
      public override IReadOnlyCollection<object> Report(CurveChart chart, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();

         if (chart.Curves.Count == 0) return listToReport;

         listToReport.Add(new Chapter(Constants.CHART));
         if (!String.IsNullOrEmpty(chart.Name)) 
            listToReport.Add(new Section(chart.Name));
         listToReport.Add(chart);

         return listToReport;
      }
   }
}