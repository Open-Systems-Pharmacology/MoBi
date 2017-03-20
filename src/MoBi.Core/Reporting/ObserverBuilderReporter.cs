using System.Collections.Generic;
using MoBi.Assets;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   public class ObserverBuilderReporter : OSPSuiteTeXReporter<ObserverBuilder>
   {
      public override IReadOnlyCollection<object> Report(ObserverBuilder objectToReport, OSPSuiteTracker buildTracker)
      {
         return new List<object>
                   {
                      new Chapter(AppConstants.Captions.Observers),
                      new Section(objectToReport.Name),
                      objectToReport
                   };
      }
   }
}
