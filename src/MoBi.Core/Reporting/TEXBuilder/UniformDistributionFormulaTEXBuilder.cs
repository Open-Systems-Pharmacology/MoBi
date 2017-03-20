using System.Collections.Generic;
using System.Linq;
using OSPSuite.TeXReporting.Builder;
using MoBi.Core.Reporting.Items;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Infrastructure.Reporting;

namespace MoBi.Core.Reporting.TEXBuilder
{
   class UniformDistributionFormulaTEXBuilder : OSPSuiteTeXBuilder<UniformDistributionFormula>
   {
      private readonly ITeXBuilderRepository _builderRepository;

      public UniformDistributionFormulaTEXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(UniformDistributionFormula uniformDistributionFormula, OSPSuiteTracker tracker)
      {
         if (!uniformDistributionFormula.ObjectPaths.Any()) return;

         var listToReport = new List<object>
                               {
                                  new DistributionFormulaTextBox(Constants.UNIFORM_DISTRIBUTION_FORMULA, uniformDistributionFormula)
                               };

         _builderRepository.Report(listToReport, tracker);
      }
   }
}
