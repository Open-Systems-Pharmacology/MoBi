using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   public class ObservedDataReporter : OSPSuiteTeXReporter<IReadOnlyCollection<DataRepository>>
   {
      public override IReadOnlyCollection<object> Report(IReadOnlyCollection<DataRepository> observedDataRepositories, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         if (!observedDataRepositories.Any())
            return listToReport;
         listToReport.Add(new Chapter(Constants.OBSERVED_DATA));
         listToReport.AddRange(observedDataRepositories);

         return listToReport;
      }
   }
}
