using System.Collections.Generic;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class TransportBuildersTEXBuilder : OSPSuiteTeXBuilder<IEnumerable<TransportBuilder>>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public TransportBuildersTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IEnumerable<TransportBuilder> transportBuilders, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         foreach (var transport in transportBuilders)
            listToReport.Add(transport);

         _builderRepository.Report(listToReport, buildTracker);
      }

   }
}
