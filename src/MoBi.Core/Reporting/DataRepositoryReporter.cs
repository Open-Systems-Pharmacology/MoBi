using System.Collections.Generic;
using OSPSuite.TeXReporting.Items;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting
{
   internal class DataRepositoryReporter : OSPSuiteTeXReporter<DataRepository>
   {
      public override IReadOnlyCollection<object> Report(DataRepository dataRepository, OSPSuiteTracker buildTracker)
      {
         return new List<object> { new Chapter(Constants.DATA), dataRepository };
      }
   }
}

