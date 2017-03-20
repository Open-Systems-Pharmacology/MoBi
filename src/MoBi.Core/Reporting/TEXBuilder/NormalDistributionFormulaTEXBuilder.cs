using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using MoBi.Core.Reporting.Items;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   internal class NormalDistributionFormulaTEXBuilder : OSPSuiteTeXBuilder<NormalDistributionFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public NormalDistributionFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(NormalDistributionFormula normalDistributionFormula, OSPSuiteTracker tracker)
      {
         if (!normalDistributionFormula.ObjectPaths.Any()) return;

         var listToReport = new List<object>
                               {
                                  new DistributionFormulaTextBox(Constants.NORMAL_DISTRIBUTION_FORMULA, normalDistributionFormula)
                               };

         _builderRepository.Report(listToReport, tracker);
      }
   }
}