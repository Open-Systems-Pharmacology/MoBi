using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using MoBi.Core.Reporting.Items;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class LogNormalDistributionFormulaTEXBuilder : OSPSuiteTeXBuilder<LogNormalDistributionFormula>
   {
         private readonly ITeXBuilderRepository _builderRepository;

         public LogNormalDistributionFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
         {
            _builderRepository = builderRepository;
         }

         public override void Build(LogNormalDistributionFormula logNormalDistributionFormula, OSPSuiteTracker tracker)
         {
            if (!logNormalDistributionFormula.ObjectPaths.Any()) return;

            var listToReport = new List<object>
                               {
                                  new DistributionFormulaTextBox(Constants.LOG_NORMAL_DISTRIBUTION_FORMULA, logNormalDistributionFormula)
                               };

            _builderRepository.Report(listToReport, tracker);
         }
   }
}
